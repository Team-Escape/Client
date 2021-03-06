using System.Collections;
using UnityEngine;

namespace GameManagerSpace.Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] string awardScene = "AwardScene";

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
                    //StartCoroutine(Playing());
                    break;
                case GameState.Scoring:
                    StartCoroutine(Scoring());
                    break;
                case GameState.NewGame:
                    StartCoroutine(NewGame());
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
            yield return StartCoroutine(control.SpawnRooms());
            yield return StartCoroutine(control.RandomStartItem());
            yield return StartCoroutine(control.SpawnPlayers());
            yield return StartCoroutine(control.RandomPlayerAvatars());

            loadedAction(); // Call Scene Transition
        }

        IEnumerator Starting()
        {
            yield return StartCoroutine(control.HunterGameSetup());
            yield return StartCoroutine(control.OpenEscaperRoomsDoor());
            yield return StartCoroutine(control.OpenHunterRoomsDoor());
        }

        IEnumerator Scoring()
        {
            yield return StartCoroutine(control.StopHunterGame());
            yield return StartCoroutine(control.BeforeScoring());
            yield return StartCoroutine(control.Scoring());
        }

        IEnumerator GameOver()
        {
            loadSceneAction(awardScene, false);
            yield return null;
        }

        IEnumerator NewGame()
        {
            loadSceneAction(CoreModel.choosenMapName, false);
            yield return null;
        }

        IEnumerator GameDraw()
        {
            loadSceneAction(awardScene, false);
            // Draw feature
            yield return null;
        }

        private void Awake()
        {
            control = GetComponent<Control>();
            control.Init(
                (string name) => GameFlow(name)
            );
        }

        private void Start()
        {
            GameFlow("Setting");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                GameFlow("Starting");
            }
            else if (Input.GetKeyDown(KeyCode.O))
            {
                control.OpenDoors();
            }
            else if (Input.GetKeyDown(KeyCode.I))
            {
                control.isScoreTest = true;
                GameFlow("Scoring");
            }
        }
    }
}