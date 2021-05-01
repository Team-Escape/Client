using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Rewired;

namespace GameManagerSpace.Hall
{
    public enum SelectState
    {
        OnChoosingRole, OnChoosingMap, OnWaiting
    }
    public class PlayerContainer
    {
        public int id = -1;
        public int currentMapIndex = 0;
        public int currentRoleIndex = 0;
        public SelectState selfSelectState = SelectState.OnChoosingRole;
        public GameObject roleModel = null;
        public string choosenMap = "";
    }
    public class HallManager : MonoBehaviour
    {
        // Player type variable of Rewired.
        List<Player> activePlayers = new List<Player>();

        // Controller type variable of Rewired.
        List<InputActionSourceData> activeController = new List<InputActionSourceData>();

        // Assemble variables handling selecting.
        List<PlayerContainer> containers = new List<PlayerContainer>();

        bool AbleToStart { get { return containers.FindAll(x => x.selfSelectState == SelectState.OnWaiting).Count == containers.Count; } }

        // Handle finding rewired players unassigned.
        JoinHandler join = new JoinHandler();

        // Handle UI view evenets.
        View view = null;
        // Enable players to join.
        bool ableToJoin = false;
        // Prevent game start againg when counting down.
        bool isStarting = false;
        // How many players required to start game.
        [SerializeField] int requiredPlayers = 1;
        // Nmae of game scene chosen by players. 
        string mapName = "";

        System.Action<string, bool> loadSceneAction = null;
        System.Action audioAction = null;
        public void Init(System.Action<string, bool> callback, System.Action audioCallback)
        {
            loadSceneAction = callback;
            audioAction = audioCallback;
            ableToJoin = true;
        }

        public void AssignController(Player player, InputActionSourceData source)
        {
            if (player == null) return;

            var controller = source.controller;

            activeController.Add(source);
            activePlayers.Add(player);

            player.controllers.AddController(controller, true);
            player.isPlaying = true;

            AssignPlayer(player.id);

            if (activePlayers.Count >= 4) ableToJoin = false;
        }

        public void AssignPlayer(int id)
        {
            PlayerContainer _c = new PlayerContainer();
            _c.id = id;
            _c.currentMapIndex = 0;
            _c.currentRoleIndex = 0;
            _c.selfSelectState = SelectState.OnChoosingRole;
            containers.Add(_c);
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
            containers.Remove(containers.GetID(id));
            ReInput.players.SystemPlayer.controllers.AddController(
                activeController[id].controller,
                true
            );
            activeController.Remove(activeController[id]);
            activePlayers.Remove(activePlayers[id]);
        }

        public void SelectMap()
        {
            for (int i = 0; i < activePlayers.Count; i++)
            {
                if (!containers[i].selfSelectState.IsState("OnChoosingMap")) continue;
                int id = i;
                int columns = 2;
                if (ReInput.players.GetPlayer(i).GetButtonDown("Choose"))
                {
                    view.UpdateMapContainer(id, containers.GetID(id).currentMapIndex, false);
                    containers.GetID(id).choosenMap = view.GetMapName(id);
                    containers.GetID(id).selfSelectState++;
                    view.MapContainerEffect(id, true);
                }
                else if (ReInput.players.GetPlayer(i).GetButtonDown("SelectorL"))
                {
                    if (containers.GetID(id).currentMapIndex + 1 < view.GetMapLength)
                    {
                        view.UpdateMapContainer(id, containers.GetID(id).currentMapIndex, false);
                        containers.GetID(id).currentMapIndex++;
                        view.UpdateMapContainer(id, containers.GetID(id).currentMapIndex);
                    }
                }
                else if (ReInput.players.GetPlayer(i).GetButtonDown("SelectorR"))
                {
                    if (containers.GetID(id).currentMapIndex - 1 >= 0)
                    {
                        view.UpdateMapContainer(id, containers.GetID(id).currentMapIndex, false);
                        containers.GetID(id).currentMapIndex--;
                        view.UpdateMapContainer(id, containers.GetID(id).currentMapIndex);
                    }
                }
                else if (ReInput.players.GetPlayer(i).GetButtonDown("SelectorU"))
                {
                    if (containers.GetID(id).currentMapIndex - columns >= 0)
                    {
                        view.UpdateMapContainer(id, containers.GetID(id).currentMapIndex, false);
                        containers.GetID(id).currentMapIndex -= columns;
                        view.UpdateMapContainer(id, containers.GetID(id).currentMapIndex);
                    }
                }
                else if (ReInput.players.GetPlayer(i).GetButtonDown("SelectorD"))
                {
                    if (containers.GetID(id).currentMapIndex + columns < view.GetMapLength)
                    {
                        view.UpdateMapContainer(id, containers.GetID(id).currentMapIndex, false);
                        containers.GetID(id).currentMapIndex += columns;
                        view.UpdateMapContainer(id, containers.GetID(id).currentMapIndex);
                    }
                }
            }
        }

        public void SelectRole()
        {
            for (int i = 0; i < activePlayers.Count; i++)
            {
                if (!containers[i].selfSelectState.IsState("OnChoosingRole")) continue;
                int id = i;
                int columns = 4;
                if (ReInput.players.GetPlayer(i).GetButtonDown("Choose"))
                {
                    view.UpdateRoleContainer(id, containers.GetID(id).currentRoleIndex, false);
                    view.UpdateMapContainer(id, containers.GetID(id).currentMapIndex, true);

                    string path = "Game/";
                    containers.GetID(id).roleModel = Resources.Load<GameObject>(path + view.GetRoleName(containers.GetID(id).currentRoleIndex));

                    // Delay the state changing for secs.
                    // Avoid directly dectecting by SelectMap func.
                    this.AbleToDo(
                        0.1f,
                        () => containers.GetID(id).selfSelectState++
                    );
                }
                else if (ReInput.players.GetPlayer(i).GetButtonDown("SelectorR"))
                {
                    if (containers.GetID(id).currentRoleIndex + 1 < view.GetRolesLength)
                    {
                        view.UpdateRoleContainer(id, containers.GetID(id).currentRoleIndex, false);
                        containers.GetID(id).currentRoleIndex++;
                        view.UpdateRoleContainer(id, containers.GetID(id).currentRoleIndex);
                    }
                }
                else if (ReInput.players.GetPlayer(i).GetButtonDown("SelectorL"))
                {
                    if (containers.GetID(id).currentRoleIndex - 1 >= 0)
                    {
                        view.UpdateRoleContainer(id, containers.GetID(id).currentRoleIndex, false);
                        containers.GetID(id).currentRoleIndex--;
                        view.UpdateRoleContainer(id, containers.GetID(id).currentRoleIndex);
                    }
                }
                else if (ReInput.players.GetPlayer(i).GetButtonDown("SelectorU"))
                {
                    if (containers.GetID(id).currentRoleIndex + columns < view.GetRolesLength)
                    {
                        view.UpdateRoleContainer(id, containers.GetID(id).currentRoleIndex, false);
                        containers.GetID(id).currentRoleIndex += columns;
                        view.UpdateRoleContainer(id, containers.GetID(id).currentRoleIndex);
                    }
                }
                else if (ReInput.players.GetPlayer(i).GetButtonDown("SelectorD"))
                {
                    if (containers.GetID(id).currentRoleIndex - columns >= 0)
                    {
                        view.UpdateRoleContainer(id, containers.GetID(id).currentRoleIndex, false);
                        containers.GetID(id).currentRoleIndex -= columns;
                        view.UpdateRoleContainer(id, containers.GetID(id).currentRoleIndex);
                    }
                }
            }
        }

        public void StateBack(int id)
        {
            switch (containers.GetID(id).selfSelectState)
            {
                case SelectState.OnChoosingRole:
                    view.UpdateRoleContainer(id, containers.GetID(id).currentRoleIndex, false);
                    DeassignController(
                        activePlayers[id],
                        activeController[id]
                    );
                    break;
                case SelectState.OnChoosingMap:
                    view.UpdateMapContainer(id, containers.GetID(id).currentMapIndex, false);
                    view.UpdateRoleContainer(id, containers.GetID(id).currentRoleIndex, true);
                    this.AbleToDo(
                        0.1f,
                        () => containers.GetID(id).selfSelectState--
                    );
                    break;
                case SelectState.OnWaiting:
                    view.MapContainerEffect(id, false);
                    view.UpdateMapContainer(id, containers.GetID(id).currentMapIndex, true);
                    this.AbleToDo(
                        0.1f,
                        () => containers.GetID(id).selfSelectState--
                    );
                    break;
            }
        }

        public void GameStart()
        {
            if (containers.Count < requiredPlayers || isStarting) return;
            if (AbleToStart)
            {
                isStarting = true;
                StartCoroutine(GameStartCoroutine());
            }
        }

        IEnumerator GameStartCoroutine()
        {
            float duration = 0.1f;
            float counter = duration;

            yield return StartCoroutine(MapPolling());

            while (counter >= 0)
            {
                yield return null;
                counter -= Time.unscaledDeltaTime;
            }

            if (AbleToStart) loadSceneAction(mapName, true);
            else isStarting = false;
        }

        IEnumerator MapPolling()
        {
            var polls = new Dictionary<string, int>();
            foreach (PlayerContainer c in containers)
            {
                string key = c.choosenMap;
                if (polls.ContainsKey(key)) continue;
                polls.Add(key, containers.FindAll(x => x.choosenMap == key).Count);
            }
            mapName = polls.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
            yield return null;
        }

        void Update()
        {
            if (ReInput.players.GetSystemPlayer().GetButtonDown("JoinGame"))
            {
                if (ableToJoin == false) return;
                (Player player, InputActionSourceData source) = join.AssignNextPlayer(activeController);
                if (player != null) AssignController(player, source);
            }
            for (int i = 0; i < ReInput.players.playerCount; i++)
            {
                if (ReInput.players.GetPlayer(i).GetButtonDown("StateBack"))
                {
                    StateBack(i);
                }
            }
            SelectRole();
            SelectMap();
            GameStart();
        }

        private void Awake()
        {
            view = GetComponent<View>();
        }
    }

    public static class HallManagerExtension
    {
        public static PlayerContainer GetID(this List<PlayerContainer> source, int id)
        {
            return source.Find(element => element.id == id);
        }
    }
}