using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Rewired;
using PlayerSpace.Game;

namespace GameManagerSpace.Hall
{
    public class HallManager : MonoBehaviour
    {
        // How many players required to start game.
        [SerializeField] int requiredPlayers = 1;
        System.Action<string, bool> loadSceneAction = null;
        System.Action audioAction = null;

        // Handle UI view evenets.
        View view = null;
        // Data storage.
        Model model = new Model();
        // Handle finding rewired players unassigned.
        JoinHandler join = new JoinHandler();

        public void Init(System.Action<string, bool> callback, System.Action audioCallback)
        {
            loadSceneAction = callback;
            audioAction = audioCallback;
            model.ableToJoin = true;
        }

        #region Controller func.
        public void JoinDetection()
        {
            if (ReInput.players.GetSystemPlayer().GetButtonDown("JoinGame"))
            {
                if (model.ableToJoin == false) return;
                (Player player, InputActionSourceData source) = join.AssignNextPlayer(model.activeController);
                if (player != null) AssignController(player, source);
            }
        }

        public void StateBackDection()
        {
            for (int i = 0; i < ReInput.players.playerCount; i++)
            {
                if (model.activePlayers.Count <= 0)
                {
                    if (ReInput.players.SystemPlayer.GetButtonDown("StateBack"))
                    {
                        loadSceneAction("MenuScene", false);
                    }
                }
                if (ReInput.players.GetPlayer(i).GetButtonDown("StateBack"))
                {
                    StateBack(i);
                }
            }
        }

        public void AssignController(Player player, InputActionSourceData source)
        {
            if (player == null) return;

            var controller = source.controller;

            model.activeController.Add(source);
            model.activePlayers.Add(player);

            player.controllers.AddController(controller, true);
            player.isPlaying = true;

            AssignPlayer(player.id);

            if (model.activePlayers.Count >= 4) model.ableToJoin = false;
        }

        public void AssignPlayer(int id)
        {
            PlayerContainer _c = new PlayerContainer();
            _c.id = id;
            _c.currentMapIndex = 0;
            _c.currentRoleIndex = 0;
            _c.selfSelectState = SelectState.OnChoosingRole;
            model.containers.Add(_c);
            view.UpdateRoleContainer(id, _c.currentRoleIndex, true);
        }

        public void DeassignController(Player player, InputActionSourceData source)
        {
            player.controllers.RemoveController(source.controller);
            player.isPlaying = false;
            DeassignPlayer(player.id);
        }

        public void DeassignPlayer(int id)
        {
            model.containers.Remove(model.containers.GetID(id));
            ReInput.players.SystemPlayer.controllers.AddController(
                model.activeController[id].controller,
                true
            );
            model.activeController.Remove(model.activeController[id]);
            model.activePlayers.Remove(model.activePlayers[id]);
        }

        public void SelectMap()
        {
            for (int i = 0; i < model.activePlayers.Count; i++)
            {
                if (!model.containers[i].selfSelectState.IsState("OnChoosingMap")) continue;
                int id = i;
                int rows = 2;

                // Passing values to handler 
                System.Action<int, int, bool> action = view.UpdateMapContainer;
                int containerLength = view.GetMapLength;
                ref int currentIndex = ref model.containers.GetID(id).currentMapIndex;
                int index = 0;
                // Passing values to handler

                if (ReInput.players.GetPlayer(i).GetButtonDown("Choose"))
                {
                    view.UpdateMapContainer(id, currentIndex, false);
                    view.MapContainerEffect(currentIndex, true);
                    model.containers.GetID(id).choosenMap = view.GetMapName(currentIndex);
                    model.containers.GetID(id).selfSelectState++;
                }
                // Update index to change direction.
                else if (ReInput.players.GetPlayer(i).GetButtonDown("SelectorL"))
                {
                    index = 1;
                    model.containers.GetID(id).UpdateUIHandler(action, containerLength, ref currentIndex, index);
                }
                else if (ReInput.players.GetPlayer(i).GetButtonDown("SelectorR"))
                {
                    index = -1;
                    model.containers.GetID(id).UpdateUIHandler(action, containerLength, ref currentIndex, index);
                }
                else if (ReInput.players.GetPlayer(i).GetButtonDown("SelectorU"))
                {
                    index = -rows;
                    model.containers.GetID(id).UpdateUIHandler(action, containerLength, ref currentIndex, index);
                }
                else if (ReInput.players.GetPlayer(i).GetButtonDown("SelectorD"))
                {
                    index = rows;
                    model.containers.GetID(id).UpdateUIHandler(action, containerLength, ref currentIndex, index);
                }
            }
        }

        public void SelectRole()
        {
            for (int i = 0; i < model.activePlayers.Count; i++)
            {
                if (!model.containers[i].selfSelectState.IsState("OnChoosingRole")) continue;
                int id = i;
                int rows = 4;

                // Passing values to handler 
                System.Action<int, int, bool> action = view.UpdateRoleContainer;
                int containerLength = view.GetRoleLength;
                ref int currentIndex = ref model.containers.GetID(id).currentRoleIndex;
                int index = 0;
                // Passing values to handler

                if (ReInput.players.GetPlayer(i).GetButtonDown("Choose"))
                {
                    view.UpdateRoleContainer(id, currentIndex, false);
                    view.UpdateMapContainer(id, model.containers.GetID(id).currentMapIndex, true);

                    string path = "Game/Roles/";
                    model.containers.GetID(id).roleModel = Resources.Load<GameObject>(path + view.GetRoleName(currentIndex));
                    model.containers.GetID(id).selfSelectState++;

                }
                // Update index to change direction.
                else if (ReInput.players.GetPlayer(i).GetButtonDown("SelectorR"))
                {
                    index = 1;
                    model.containers.GetID(id).UpdateUIHandler(action, containerLength, ref currentIndex, index);
                }
                else if (ReInput.players.GetPlayer(i).GetButtonDown("SelectorL"))
                {
                    index = -1;
                    model.containers.GetID(id).UpdateUIHandler(action, containerLength, ref currentIndex, index);
                }
                else if (ReInput.players.GetPlayer(i).GetButtonDown("SelectorU"))
                {
                    index = rows;
                    model.containers.GetID(id).UpdateUIHandler(action, containerLength, ref currentIndex, index);
                }
                else if (ReInput.players.GetPlayer(i).GetButtonDown("SelectorD"))
                {
                    index = -rows;
                    model.containers.GetID(id).UpdateUIHandler(action, containerLength, ref currentIndex, index);
                }
            }
        }

        public void StateBack(int id)
        {
            switch (model.containers.GetID(id).selfSelectState)
            {
                case SelectState.OnChoosingRole:
                    view.UpdateRoleContainer(id, model.containers.GetID(id).currentRoleIndex, false);
                    DeassignController(
                        model.activePlayers[id],
                        model.activeController[id]
                    );
                    break;
                case SelectState.OnChoosingMap:
                    view.UpdateMapContainer(id, model.containers.GetID(id).currentMapIndex, false);
                    view.UpdateRoleContainer(id, model.containers.GetID(id).currentRoleIndex, true);
                    this.AbleToDo(
                        0.1f,
                        () => model.containers.GetID(id).selfSelectState--
                    );
                    break;
                case SelectState.OnWaiting:
                    view.MapContainerEffect(id, false);
                    view.UpdateMapContainer(id, model.containers.GetID(id).currentMapIndex, true);
                    this.AbleToDo(
                        0.1f,
                        () => model.containers.GetID(id).selfSelectState--
                    );
                    break;
            }
        }
        #endregion

        #region Game control func.
        public void GameStart()
        {
            if (model.containers.Count < requiredPlayers || model.isStarting) return;
            if (model.AbleToStart)
            {
                model.isStarting = true;
                CoreModel.activePlayersCount = model.activePlayers.Count;

                List<GameObject> playerPrefabs = new List<GameObject>();
                for (int i = 0; i < CoreModel.activePlayersCount; i++)
                {
                    playerPrefabs.Add(model.containers[i].roleModel);
                    playerPrefabs[i].GetComponentInChildren<PlayerCharacter>().AssignController(0);
                }
                CoreModel.RoleAvatars = playerPrefabs;
                CoreModel.ActivePlayers = model.activePlayers;
                CoreModel.ActiveController = model.activeController;

                InitCoreModelDatas();

                StartCoroutine(GameStartCoroutine());
            }
        }

        /// <summary>
        /// Except some variables, all of the values can be assigned here.
        /// Exception: RolePrefabs
        /// </summary>
        /// <param name="CoreModel">Init all CoreModel datas here.</param>
        void InitCoreModelDatas()
        {
            CoreModel.WinnerAvatars = new List<GameObject>();
            CoreModel.TotalScores = new List<int>();
            for (int i = 0; i < CoreModel.activePlayersCount; i++)
            {
                CoreModel.TotalScores.Add(0);
            }
        }

        IEnumerator GameStartCoroutine()
        {
            float duration = 0.1f;
            float counter = duration;

            yield return StartCoroutine(MapPolling());
            CoreModel.choosenMapName = model.mapName;

            while (counter >= 0)
            {
                yield return null;
                counter -= Time.unscaledDeltaTime;
            }

            if (model.AbleToStart) loadSceneAction(CoreModel.choosenMapName, true);
            else model.isStarting = false;
        }

        IEnumerator MapPolling()
        {
            var polls = new Dictionary<string, int>();
            foreach (PlayerContainer c in model.containers)
            {
                string key = c.choosenMap;
                if (polls.ContainsKey(key)) continue;
                polls.Add(key, model.containers.FindAll(x => x.choosenMap == key).Count);
            }
            model.mapName = polls.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
            yield return null;
        }
        #endregion

        void Update()
        {
            JoinDetection();
            StateBackDection();
            SelectMap();
            SelectRole();
            GameStart();
        }

        private void Awake()
        {
            view = GetComponent<View>();
            model = new Model();
            join = new JoinHandler();
            Reset();
        }

        void Reset()
        {
            ReInput.players.AssignAllPlayersToSystemPlayer();
        }
    }
}