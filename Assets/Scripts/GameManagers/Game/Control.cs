using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using GameManagerSpace.Game.HunterGame;
using PlayerSpace.Game;
using Rewired;

namespace GameManagerSpace.Game
{
    public class Control : MonoBehaviour
    {
        Model model = null;
        View view = null;

        Action<string> changeGameStateAction = null;
        int activePlayerCounts = 0;
        int playerGotStartItemCounts = 0;
        bool isGoaled = false;

        public void Init(Action<string> changeGameStateCallback)
        {
            changeGameStateAction = changeGameStateCallback;
            for (int i = 0; i < activePlayerCounts; i++)
            {
                CoreModel.WinnerAvatars = new List<GameObject>();
                model.GetCaughtRoles = new List<PlayerCharacter>();
                model.GoalRoles = new List<PlayerCharacter>();
            }
        }

        # region Game Listener
        public void TeleportNext(PlayerCharacter role, CinemachineConfiner confiner)
        {
            role.currentRoomId++;
            confiner.m_BoundingShape2D = model.blocks[role.currentRoomId].GetComponent<MapObjectData>().polygonCollider2D;
            role.transform.position = model.blocks[role.currentRoomId].GetComponent<MapObjectData>().entrance.position;
        }
        public void TeleportPrev(PlayerCharacter role, CinemachineConfiner confiner)
        {
            role.currentRoomId--;
            confiner.m_BoundingShape2D = model.blocks[role.currentRoomId].GetComponent<MapObjectData>().polygonCollider2D;
            role.transform.position = model.blocks[role.currentRoomId].GetComponent<MapObjectData>().exit.position;
        }
        public void GetStartItemCallback(PlayerCharacter role)
        {
            playerGotStartItemCounts++;
            if (playerGotStartItemCounts >= activePlayerCounts - 1)
            {
                changeGameStateAction("Starting");
            }
        }
        public void GetCaught(PlayerCharacter role)
        {
            model.GetCaughtRoles.Add(role);
            if (model.GetCaughtRoles.Count >= activePlayerCounts - 1)
            {
                changeGameStateAction("Scoring");
            }
        }
        public void GetGoal(PlayerCharacter role)
        {
            model.GoalRoles.Add(role);
            if (isGoaled) return;
            StartCoroutine(CountDown());
        }
        IEnumerator CountDown()
        {
            isGoaled = true;
            float timer = CoreModel.goalCountDownDuration;
            while (timer >= 0)
            {
                timer -= Time.deltaTime;
                yield return null;
            }
            changeGameStateAction("Scoring");
        }
        # endregion

        # region Game Setting
        public IEnumerator RandomRooms()
        {
            int index = 1;
            List<GameObject> blocks = new List<GameObject>();
            blocks.Add(model.startRoom.gameObject);
            blocks.Add(model.blockContainer.left.Random());
            while (true)
            {
                if (index > model.roomSize - 1) break;
                GameObject go = null;
                string name = blocks[index].name;
                switch (name.Split(',')[2])
                {
                    case "left":
                        go = model.blockContainer.right.Random();
                        break;
                    case "right":
                        go = model.blockContainer.left.Random();
                        break;
                    case "up":
                        go = model.blockContainer.down.Random();
                        break;
                    case "down":
                        go = model.blockContainer.up.Random();
                        break;
                }
                blocks.Add(go);
                index++;
                yield return null;
            }

            string name1 = blocks[blocks.Count - 1].name;
            switch (name1.Split(',')[2])
            {
                case "left":
                    blocks.Add(model.destinationRoomRight.gameObject);
                    break;
                case "right":
                    blocks.Add(model.destinationRoomLeft.gameObject);
                    break;
                case "up":
                    blocks.Add(model.destinationRoomDown.gameObject);
                    break;
                case "down":
                    blocks.Add(model.destinationRoomUp.gameObject);
                    break;
            }

            model.blocks = blocks;

            for (int i = 1; i < model.blocks.Count; i++)
            {
                model.blocks[i] = Instantiate(model.blocks[i]);
            }
        }

        public IEnumerator SpawnPlayers()
        {
            List<GameObject> _roles = new List<GameObject>();

            for (int i = 0; i < CoreModel.RoleAvatars.Count; i++)
            {
                GameObject go = Instantiate(CoreModel.RoleAvatars[i]);
                go.GetComponentInChildren<PlayerCharacter>().AssignController(i);
                go.GetComponentInChildren<CinemachineConfiner>().m_BoundingShape2D = model.startRoom.GetComponent<MapObjectData>().polygonCollider2D;
                _roles.Add(go);
            }

            model.roles = _roles;

            activePlayerCounts = _roles.Count;

            yield return null;
        }

        public IEnumerator RandomPlayerAvatars()
        {
            model.hunter = model.roles.Random();
            model.hunterPlayer = ReInput.players.GetPlayer(model.hunter.GetComponentInChildren<PlayerCharacter>().playerId);
            model.escapers = model.roles.FindAll(x => (x != model.hunter));
            model.escaperPlayers = CoreModel.ActivePlayers.FindAll(x => x != model.hunterPlayer);

            // ------------------------------

            model.hunter.transform.position = model.hunterSpawn.position;
            model.escapers.ForEach(x => x.transform.position = model.escaperSpawn.position);

            List<System.Action<PlayerCharacter>> actions = new List<System.Action<PlayerCharacter>>();
            actions.Add(GetStartItemCallback);
            actions.Add(GetCaught);
            actions.Add(GetGoal);

            List<System.Action<PlayerCharacter, CinemachineConfiner>> changeLevelActions = new List<Action<PlayerCharacter, CinemachineConfiner>>();
            changeLevelActions.Add(TeleportNext);
            changeLevelActions.Add(TeleportPrev);

            model.hunter.GetComponentInChildren<PlayerCharacter>().AssignTeam(1, actions, changeLevelActions);
            model.escapers.ForEach(x => x.GetComponentInChildren<PlayerCharacter>().AssignTeam(0, actions, changeLevelActions));

            model.mainCam.enabled = false;

            yield return null;
        }

        public IEnumerator RandomStartItem()
        {
            List<StartItemContainer> containers = model.startItemContainers.RandomSeed(3);
            for (int i = 0; i < model.startItemGameObjects.Count; i++)
            {
                model.startItemGameObjects[i].GetComponent<SpriteRenderer>().sprite = containers[i].display;
                model.startItemGameObjects[i].name = containers[i].name;
            }
            yield return null;
        }

        public IEnumerator SpawnRooms()
        {
            for (int i = 0; i < model.blocks.Count; i++)
            {
                if (i < model.blocks.Count - 1)
                    model.blocks[i].GetComponent<MapObjectData>().id = i;
                if (i == 0) continue;
                Vector2 pos = model.blocks[i - 1].GetComponent<MapObjectData>().endpoint.position + (i < model.blocks.Count - 1 ? new Vector3(30, 0, 0) : Vector3.zero);
                model.blocks[i].transform.position = pos;
            }
            model.blocks[model.blocks.Count - 2].GetComponent<MapObjectData>().exit.gameObject.SetActive(false);
            yield return null;
        }
        #endregion

        #region Game starting
        public IEnumerator InitGameObstacle()
        {
            foreach (var go in FindObjectsOfType<MapObjectCore>())
            {
                go.Init();
            }
            yield return null;
        }
        public IEnumerator HunterGameSetup()
        {
            HunterGameSetup hunterGameSetup = FindObjectOfType<HunterGameSetup>();
            hunterGameSetup.Generator(model.hunterPlayer, OpenDoors);
            yield return null;
        }
        public void OpenDoors()
        {
            StartCoroutine(OpenEscaperRoomsDoor());
            StartCoroutine(OpenHunterRoomsDoor());
        }
        public IEnumerator OpenEscaperRoomsDoor()
        {
            foreach (Vector3Int pos in model.wallDestoryInEscape)
            {
                yield return new WaitForSeconds(0.05f);
                model.startRoomTilemap.SetTile(pos, null);
            }
            yield return null;
        }
        public IEnumerator OpenHunterRoomsDoor()
        {
            foreach (Vector3Int pos in model.wallDestoryInHunter)
            {
                yield return new WaitForSeconds(0.05f);
                model.startRoomTilemap.SetTile(pos, null);
            }
            yield return null;
        }
        #endregion

        #region Game playing
        public IEnumerator InitGame()
        {
            foreach (var s in FindObjectsOfType<MapObjectCore>())
            {
                s.Init();
            }
            yield return null;
        }
        #endregion

        #region Game scoring
        public IEnumerator Scoring()
        {
            for (int i = 0; i < activePlayerCounts; i++)
            {
                int score = 0;
                GameObject go = CoreModel.RoleAvatars[i];
                if (model.hunter == go)
                {
                    score += model.GetCaughtRoles.Count;
                    score += (model.GoalRoles.Contains(go.GetComponentInChildren<PlayerCharacter>())) ? CoreModel.goalScore : 0;
                }
                else if (model.escapers.Contains(go))
                {
                    score += (model.GoalRoles.Contains(go.GetComponentInChildren<PlayerCharacter>())) ? CoreModel.goalScore : 0;
                }
                CoreModel.TotalScores[i] += score;
                if (CoreModel.TotalScores[i] >= CoreModel.winningScore) CoreModel.WinnerAvatars.Add(go);
            }
            yield return null;
        }
        public IEnumerator GameJudge()
        {
            if (CoreModel.WinnerAvatars.Count > 1)
            {
                changeGameStateAction("GameDraw");
            }
            else if (CoreModel.WinnerAvatars.Count > 0)
            {
                changeGameStateAction("GameOver");
            }
            else
            {
                changeGameStateAction("NewGame");
            }
            yield return null;
        }
        #endregion

        #region Game over
        #endregion

        #region Game draw
        #endregion

        private void Awake()
        {
            model = GetComponent<Model>();
            view = GetComponent<View>();
        }
    }
}