﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using GameManagerSpace.Game.HunterGame;
using PlayerSpace.Game;

namespace GameManagerSpace.Game
{
    public class Control : MonoBehaviour
    {
        Model model = null;
        View view = null;

        Action<string> changeGameStateAction = null;
        int activePlayerCounts = 0;
        int playerGotStartItemCounts = 0;

        public void Init(Action<string> changeGameStateCallback)
        {
            changeGameStateAction = changeGameStateCallback;
        }

        # region Game Listener
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
            CoreModel.GetCaughtRoles.Add(role);
        }
        public void GetGoal(PlayerCharacter role)
        {

        }
        # endregion

        # region Game Setting
        public IEnumerator RandomRooms()
        {
            int index = 0;
            List<GameObject> blocks = new List<GameObject>();
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
        }

        public IEnumerator SpawnPlayers()
        {
            List<GameObject> _roles = new List<GameObject>();

            for (int i = 0; i < CoreModel.RolePrefabs.Count; i++)
            {
                GameObject go = Instantiate(CoreModel.RolePrefabs[i]);
                go.GetComponentInChildren<PlayerCharacter>().AssignController(i);
                _roles.Add(go);
            }

            model.roles = _roles;

            activePlayerCounts = _roles.Count;

            yield return null;
        }

        public IEnumerator RandomPlayerAvatars()
        {
            model.hunter = model.roles.Random();
            model.hunterPlayer = CoreModel.ActivePlayers.Find(x => CoreModel.RolePrefabs[x.id] == model.hunter);
            model.escapers = model.roles.FindAll(x => (x != model.hunter));
            model.escaperPlayers = CoreModel.ActivePlayers.FindAll(x => x != model.hunterPlayer);

            // ------------------------------

            model.hunter.transform.position = model.hunterSpawn.position;
            model.escapers.ForEach(x => x.transform.position = model.escaperSpawn.position);

            List<System.Action<PlayerCharacter>> actions = new List<System.Action<PlayerCharacter>>();
            actions.Add(GetStartItemCallback);
            actions.Add(GetCaught);
            actions.Add(GetGoal);

            model.hunter.GetComponentInChildren<PlayerCharacter>().AssignTeam(1, actions);
            model.escapers.ForEach(x => x.GetComponentInChildren<PlayerCharacter>().AssignTeam(0, actions));

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
                model.blocks[i] = Instantiate(model.blocks[i]);
            }
            for (int i = 0; i < model.blocks.Count; i++)
            {
                Vector2 pos = (i == 0) ?
                    model.startRoom.GetChild(0).position :
                    model.blocks[i - 1].transform.localPosition + model.blocks[i - 1].transform.GetChild(0).localPosition;
                model.blocks[i].transform.position = pos;
            }
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
        void OpenDoors()
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
        public IEnumerator InitPlayerGame()
        {
            yield return null;
        }
        #endregion

        #region Game scoring
        public IEnumerator Scoring()
        {
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