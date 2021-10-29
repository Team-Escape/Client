using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using GameManagerSpace.Game.HunterGame;
using GameManagerSpace.Score;
using PlayerSpace.Gameplayer;
using Rewired;

namespace GameManagerSpace.Game
{
    public class Control : MonoBehaviour
    {
        GameManagerSpace.Game.Model model = null;
        GameManagerSpace.Game.View view = null;

        Action<string> changeGameStateAction = null;
        int activePlayerCounts = 0;
        int playerGotStartItemCounts = 0;
        bool isStarted = false;
        bool isGoaled = false;

        public void Init(Action<string> changeGameStateCallback)
        {
            changeGameStateAction = changeGameStateCallback;
            CoreModel.WinnerAvatars = new List<GameObject>();
            model.GetCaughtRoles = new List<Gameplayer>();
            model.GoalRoles = new List<Gameplayer>();
        }

        # region Game Listener
        public void ItemTeleportNext(Gameplayer role, CinemachineConfiner confiner)
        {
            role.currentRoomID--;
            MapObjectData m_data = model.blocks[role.currentRoomID].GetComponent<MapObjectData>();
            confiner.m_BoundingShape2D = m_data.polygonCollider2D;
            role.transform.position = m_data.entrance.position;
        }
        public void TeleportNext(Gameplayer role, CinemachineConfiner confiner)
        {
            role.currentRoomID++;
            MapObjectData m_data = model.blocks[role.currentRoomID].GetComponent<MapObjectData>();
            confiner.m_BoundingShape2D = m_data.polygonCollider2D;
            role.transform.position = m_data.entrance.position;
        }
        public void TeleportPrev(Gameplayer role, CinemachineConfiner confiner)
        {
            role.currentRoomID--;
            MapObjectData m_data = model.blocks[role.currentRoomID].GetComponent<MapObjectData>();
            confiner.m_BoundingShape2D = m_data.polygonCollider2D;
            role.transform.position = m_data.exit.position;
        }
        public void GetStartItemCallback(Gameplayer role)
        {
            if (isStarted) return;
            playerGotStartItemCounts++;
            if (playerGotStartItemCounts >= activePlayerCounts - 1)
            {
                changeGameStateAction("Starting");
            }
        }
        public void GetCaught(Gameplayer role)
        {
            model.GetCaughtRoles.Add(role);
            if (model.GetCaughtRoles.Count >= activePlayerCounts - 1)
            {
                changeGameStateAction("Scoring");
            }
            //else model.hunter.hunterDebuff(activePlayerCounts - 1);
        }
        public void GetGoal(Gameplayer role)
        {
            if (model.GoalRoles.Any(x => x.playerID == role.playerID) == false)
                model.GoalRoles.Add(role);
            if (isGoaled) return;
            StartCoroutine(CountDown());
        }

        IEnumerator CountDown()
        {
            isGoaled = true;
            float timer = CoreModel.goalCountDownDuration;
            view.StartCount();
            while (timer >= 0)
            {
                view.CountDown(timer);
                //Debug.Log("Time left: " + timer);
                timer -= Time.deltaTime;
                yield return null;
            }
            view.EndCount();
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

            GameObject go1 = null;
            string name1 = blocks[index].name;
            switch (name1.Split(',')[2])
            {
                case "left":
                    go1 = model.destinationRoomRight.gameObject;
                    break;
                case "right":
                    go1 = model.destinationRoomLeft.gameObject;
                    break;
                case "up":
                    go1 = model.destinationRoomDown.gameObject;
                    break;
                case "down":
                    go1 = model.destinationRoomUp.gameObject;
                    break;
            }
            blocks.Add(go1);

            model.blocks = blocks;

            for (int i = 1; i < model.blocks.Count; i++)
            {
                model.blocks[i] = Instantiate(model.blocks[i]);
            }
        }

        public IEnumerator SpawnPlayers()
        {
            List<Gameplayer> _roles = new List<Gameplayer>();
            List<Camera> cameras = new List<Camera>();
            Debug.Log("Got N Ava"+CoreModel.RoleAvatars.Count);
            for (int i = 0; i < CoreModel.RoleAvatars.Count; i++)
            {
                GameObject go = Instantiate(CoreModel.RoleAvatars[i]);
                Debug.Log(go.name);
                go.GetComponentInChildren<Gameplayer>().AssignController(i);
                go.GetComponentInChildren<CinemachineConfiner>().m_BoundingShape2D = model.startRoom.GetComponent<MapObjectData>().polygonCollider2D;
                cameras.Add(go.GetComponentInChildren<Camera>());
                _roles.Add(go.GetComponentInChildren<Gameplayer>());
            }

            Debug.Log("C size: " +cameras.Count);

            model.roles = _roles;

            activePlayerCounts = _roles.Count;

            cameras.Resize();

            yield return null;
        }

        public IEnumerator RandomPlayerAvatars()
        {
            model.hunter = model.roles.Random();
            model.hunterPlayer = ReInput.players.GetPlayer(model.hunter.GetComponent<Gameplayer>().playerID);
            model.escapers = model.roles.FindAll(x => (x != model.hunter));
            model.escaperPlayers = CoreModel.ActivePlayers.FindAll(x => x != model.hunterPlayer);

            // ------------------------------

            model.hunter.transform.position = model.hunterSpawn.position;
            model.escapers.ForEach(x => x.transform.position = model.escaperSpawn.position);

            List<System.Action<Gameplayer>> actions = new List<System.Action<Gameplayer>>();
            actions.Add(GetStartItemCallback);
            actions.Add(GetCaught);
            actions.Add(GetGoal);

            List<System.Action<Gameplayer, CinemachineConfiner>> changeLevelActions = new List<Action<Gameplayer, CinemachineConfiner>>();
            changeLevelActions.Add(TeleportNext);
            changeLevelActions.Add(TeleportPrev);
            changeLevelActions.Add(ItemTeleportNext);

            model.hunter.GetComponent<Gameplayer>().AssignTeam(1, actions, changeLevelActions);
            model.escapers.ForEach(x => x.GetComponent<Gameplayer>().AssignTeam(0, actions, changeLevelActions));

            //model.mainCam.enabled = false;

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
                model.blocks[i].GetComponent<MapObjectData>().id = i;
                if (i == 0) continue;
                Vector2 pos = model.blocks[i - 1].GetComponent<MapObjectData>().endpoint.position + new Vector3(100, 100, 0);
                model.blocks[i].transform.position = pos;
            }
            yield return null;
        }
        #endregion

        #region Game starting

        public IEnumerator HunterGameSetup()
        {
            isStarted = true;
            HunterGameSetup hunterGameSetup = model.hunter.transform.parent.GetComponentInChildren<HunterGameSetup>();
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
        #endregion

        #region Game scoring
        public IEnumerator StopHunterGame()
        {
            model.hunter.transform.parent.GetComponentInChildren<HunterGameSetup>().gameObject.SetActive(false);
            yield return null;
        }
        public IEnumerator BeforeScoring()
        {
            Camera[] cameras = FindObjectsOfType<Camera>();

            foreach (Camera cam in cameras)
            {
                cam.enabled = false;
            }

            model.mainCam.enabled = true;
            yield return null;
        }
        public IEnumerator Scoring()
        {
            yield return (SceneManager.LoadSceneAsync("ScoreScene", LoadSceneMode.Additive));
            ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
            scoreManager.Init(activePlayerCounts, CalculateScores, changeGameStateAction);

            yield return scoreManager.StartScoring();
        }

        public bool isScoreTest = false;

        List<int> CalculateScores
        {
            get
            {
                List<int> scores = new List<int>();
                for (int i = 0; i < CoreModel.activePlayersCount; i++)
                {
                    int score = 0;
                    Gameplayer role = model.roles[i];
                    if (role.teamID == 1)
                    {
                        if (model.GetCaughtRoles.Count == model.escaperPlayers.Count) score += CoreModel.goalScore;
                        else score += model.GetCaughtRoles.Count;
                    }
                    score += (model.GoalRoles.Any(x => x.playerID == role.playerID)) ? CoreModel.goalScore : 0;
                    scores.Add(score);
                }
                if (isScoreTest)
                {
                    scores[0] = 5;
                    isScoreTest = false;
                }
                return scores;
            }
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