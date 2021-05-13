﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameManagerSpace.Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] string awardScene = "AwardScene";
        [SerializeField] string scoreScene = "ScoreScene";

        System.Action<string, bool> loadSceneAction = null;
        System.Action loadedAction = null;

        private GameState gameState = new GameState();
        private Control control = null;

        public void Init(System.Action<string, bool> loadSceneActionCallback, System.Action loadedCallback)
        {
            loadSceneAction = loadSceneActionCallback;
            loadedAction = loadedCallback;
        }

        public void GameListener(string mode)
        {
            GameListenerState state = mode.ToEnum<GameListenerState>();
            switch (state)
            {
                case GameListenerState.Caught:
                    break;
                case GameListenerState.Arrival:
                    break;
            }
        }

        public void GameFlow(string name)
        {
            gameState.Change(name);
            switch (gameState)
            {
                case GameState.Setting:
                    StartCoroutine(Setting());
                    break;
                case GameState.Starting:
                    StartCoroutine(Starting());
                    break;
                case GameState.Playing:
                    StartCoroutine(Playing());
                    break;
                case GameState.Scoring:
                    StartCoroutine(Scoring());
                    break;
                case GameState.GameOver:
                    StartCoroutine(GameOver());
                    break;
                case GameState.GameDraw:
                    StartCoroutine(GameDraw());
                    break;
            }
        }

        IEnumerator Setting()
        {
            yield return StartCoroutine(control.RandomRooms());
            yield return StartCoroutine(control.RandomStartItem());
            yield return StartCoroutine(control.RandomPlayerAvatars());
            yield return StartCoroutine(control.SpawnRooms());
            yield return StartCoroutine(control.SpawnStartItems());
            yield return StartCoroutine(control.SpawnPlayers());
            loadedAction(); // Call Scene Transition
        }

        IEnumerator Starting()
        {
            yield return StartCoroutine(control.InitGameObstacle());
            yield return StartCoroutine(control.HunterGameSetup());
            yield return StartCoroutine(control.OpenEscaperRoomsDoor());
            yield return StartCoroutine(control.OpenHunterRoomsDoor());
        }

        IEnumerator Playing()
        {
            // Init player actions.
            yield return null;
        }

        IEnumerator Scoring()
        {
            yield return null;
        }

        IEnumerator GameOver()
        {
            yield return null;
        }

        IEnumerator GameDraw()
        {
            yield return null;
        }

        void Awake()
        {
            gameState.Change("Setting");
            control = GetComponent<Control>();
            control.Init(
                (string name) => GameFlow(name)
            );
        }
    }
}