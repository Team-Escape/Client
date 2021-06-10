using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Rewired;
using GameManagerSpace.Audio;
using GameManagerSpace.Menu;
using GameManagerSpace.Hall;
using GameManagerSpace.Game;
using GameManagerSpace.Score;
using GameManagerSpace.Award;

namespace GameManagerSpace
{
    public class Core : MonoBehaviour, ICore
    {
        private SceneState currentScene = new SceneState();
        private List<Player> inputs = new List<Player>();
        AudioManager audioManager = null;
        CoreView coreView = null;

        /// <summary>
        /* Input Controller */
        /// <summary>
        public void AssignAllJoysticksToSystemPlayer(bool removeFromOtherPlayers)
        {
            foreach (var j in ReInput.controllers.Joysticks)
            {
                ReInput.players.GetSystemPlayer().controllers.AddController(j, removeFromOtherPlayers);
            }
        }
        public void ChangeInputMaps(string name)
        {
            inputs.SelectAllTheMap(name);
        }
        /// <summary>
        /* Scene Control */
        /// <summary>
        public void ChangeScene(string name)
        {
            SceneManager.LoadScene(name);
        }
        public void MaskChangeScene(string name, bool withLoading)
        {
            if (withLoading) coreView.MaskInWithLoading(() => SceneManager.LoadScene(name), () => coreView.UpdateLoadingUI(false));
            else coreView.MaskIn(() => SceneManager.LoadScene(name));
        }
        /// <summary>
        /// *List each game scene name here and SceneState enum.
        /// </summary>
        /// <param name="GameSceneRegister">Important ! Must remember to register game scene</param>
        void SceneStateManagement()
        {
            switch (currentScene)
            {
                case SceneState.StartScene:
                    ChangeInputMaps("Start");

                    var pressAnyButton = FindObjectOfType<PressAnyButton>();
                    pressAnyButton.SceneCallbackAction = ChangeScene;

                    break;
                case SceneState.MenuScene:
                    ChangeInputMaps("Menu");

                    var menu = FindObjectOfType<MenuManager>();
                    menu.Init(MaskChangeScene, () => audioManager.ChangeAudio("Menu"));

                    coreView.MaskOut();
                    break;
                case SceneState.HallScene:
                    ChangeInputMaps("Hall");

                    var hall = FindObjectOfType<HallManager>();
                    hall.Init(MaskChangeScene, coreView.MaskOut);

                    coreView.MaskOut();
                    break;
                case SceneState.LabScene:
                    if (FindObjectOfType<ScoreManager>())
                    {
                        Destroy(FindObjectOfType<ScoreManager>());
                    }
                    ChangeInputMaps("Game");

                    var lab = FindObjectOfType<GameManager>();
                    lab.Init(MaskChangeScene, () => audioManager.ChangeAudio("Game"));

                    coreView.MaskOut();
                    break;
                case SceneState.AwardScene:
                    ChangeInputMaps("Award");

                    var award = FindObjectOfType<AwardManager>();
                    award.Init(ChangeScene);

                    coreView.MaskOut();
                    break;
            }
        }

        // This function will be called when a controller is connected
        // You can get information about the controller that was connected via the args parameter
        void OnControllerConnected(ControllerStatusChangedEventArgs args)
        {
            if (args.controllerType != ControllerType.Joystick) return;

            // Check if this Joystick has already been assigned. If so, just let Auto-Assign do its job.
            foreach (var p in ReInput.players.GetPlayers())
            {
                if (p.controllers.ContainsController(args.controller)) return;
            }

            // Joystick hasn't ever been assigned before. Make sure it's assigned to the System Player until it's been explicitly assigned
            ReInput.players.GetSystemPlayer().controllers.AddController(
                args.controllerType,
                args.controllerId,
                true // remove any auto-assignments that might have happened
            );
        }
        // This function will be called when a controller is fully disconnected
        // You can get information about the controller that was disconnected via the args parameter
        void OnControllerDisconnected(ControllerStatusChangedEventArgs args)
        {
            Debug.Log("A controller was disconnected! Name = " + args.name + " Id = " + args.controllerId + " Type = " + args.controllerType);
        }

        // This function will be called when a controller is about to be disconnected
        // You can get information about the controller that is being disconnected via the args parameter
        // You can use this event to save the controller's maps before it's disconnected
        void OnControllerPreDisconnect(ControllerStatusChangedEventArgs args)
        {
            Debug.Log("A controller is being disconnected! Name = " + args.name + " Id = " + args.controllerId + " Type = " + args.controllerType);
        }

        /// <summary>
        /// Native APIs.
        /// </summary>
        /// <param name="Native">Unity native APIs</param>
        void Awake()
        {
            ReInput.ControllerConnectedEvent += OnControllerConnected;
            ReInput.ControllerDisconnectedEvent += OnControllerDisconnected;
            ReInput.ControllerPreDisconnectEvent += OnControllerPreDisconnect;

            inputs = inputs.FindAllPlayersWithJoystick();

            coreView = GetComponent<CoreView>();
            audioManager = GetComponentInChildren<AudioManager>();
            currentScene.Change(SceneManager.GetActiveScene().name);
        }
        void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            currentScene.Change(scene.name);
            SceneStateManagement();
        }
        void Start()
        {
            AssignAllJoysticksToSystemPlayer(true);
        }
        void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        void OnDestroy()
        {
            ReInput.ControllerConnectedEvent -= OnControllerConnected;
            ReInput.ControllerDisconnectedEvent -= OnControllerDisconnected;
            ReInput.ControllerPreDisconnectEvent -= OnControllerPreDisconnect;
        }
    }
}