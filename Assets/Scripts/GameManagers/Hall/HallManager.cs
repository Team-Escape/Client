using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Rewired;

namespace GameManagerSpace.Hall
{
    public enum SelectState
    {
        OnJoining, OnChoosingRole, OnChoosingMap, OnWaiting
    }
    public class PlayerContainer
    {
        public int id = -1;
        public int currentMapIndex = 0;
        public int currentRoleIndex = 0;
        public SelectState selfSelectState = SelectState.OnJoining;
        public GameObject roleModel = null;
        public string choosenMap = "";
    }
    public class HallManager : MonoBehaviour
    {
        View view = null;
        List<InputActionSourceData> activeController = new List<InputActionSourceData>();
        List<PlayerContainer> containers = new List<PlayerContainer>();
        List<Player> activePlayers = new List<Player>();
        JoinHandler join = new JoinHandler();
        bool ableToJoin = false;

        System.Action<string, bool> loadSceneAction = null;
        System.Action audioAction = null;
        public void Init(System.Action<string, bool> callback, System.Action audioCallback)
        {
            loadSceneAction = callback;
            audioAction = audioCallback;
            ableToJoin = true;
        }

        public void StateBack(int id)
        {
            ref SelectState state = ref containers.GetID(id).selfSelectState;
            switch (state)
            {
                case SelectState.OnJoining:
                    return;
                case SelectState.OnChoosingRole:
                    view.UpdateRoleContainer(id, containers.GetID(id).currentRoleIndex, false);
                    DeassignController(
                        activePlayers[id],
                        activeController[id]
                    );
                    state--;
                    break;
                case SelectState.OnChoosingMap:
                    view.UpdateMapContainer(id, containers.GetID(id).currentRoleIndex, true);
                    view.UpdateRoleContainer(id, containers.GetID(id).currentMapIndex, false);
                    state--;
                    break;
                case SelectState.OnWaiting:
                    view.UpdateMapContainer(id, containers.GetID(id).currentRoleIndex, true);
                    state--;
                    break;
            }
        }

        public void SelectMap(int id)
        {
            for (int i = 0; i < ReInput.players.playerCount; i++)
            {
                if (!containers[i].selfSelectState.IsState("OnChoosingMap")) continue;
                if (ReInput.players.GetPlayer(i).GetButtonDown("Choose"))
                {
                    view.UpdateMapContainer(id, containers.GetID(id).currentMapIndex, false);
                    containers.GetID(id).selfSelectState++;
                }
                if (ReInput.players.GetPlayer(i).GetButtonDown("CursorMoveX"))
                {
                    int nextIndex = (ReInput.players.GetPlayer(i).GetAxis("CursorMoveX") > 0) ? 1 : -1;
                    PlayerContainer _p = containers.GetID(id);

                    if (nextIndex > 0 && containers.GetID(id).currentMapIndex + nextIndex < view.GetRolesLength)
                    {
                        containers.GetID(id).currentMapIndex += nextIndex;
                        view.UpdateRoleContainer(id, containers.GetID(id).currentMapIndex);
                    }
                    if (nextIndex < 0 && containers.GetID(id).currentMapIndex + nextIndex >= 0)
                    {
                        containers.GetID(id).currentMapIndex += nextIndex;
                        view.UpdateRoleContainer(id, containers.GetID(id).currentMapIndex);
                    }
                }
            }
        }

        public void SelectRole(int id)
        {
            for (int i = 0; i < ReInput.players.playerCount; i++)
            {
                if (!containers[i].selfSelectState.IsState("OnChoosingRole")) continue;
                if (ReInput.players.GetPlayer(i).GetButtonDown("Choose"))
                {
                    string path = "Game/";
                    view.UpdateRoleContainer(id, containers.GetID(id).currentRoleIndex, false);
                    view.UpdateMapContainer(id, containers.GetID(id).currentMapIndex, true);
                    containers.GetID(id).roleModel = Resources.Load<GameObject>(path + view.GetRoleName(containers.GetID(id).currentRoleIndex));
                    containers.GetID(id).selfSelectState++;
                }
                if (ReInput.players.GetPlayer(i).GetButtonDown("CursorMoveX"))
                {
                    int nextIndex = (ReInput.players.GetPlayer(i).GetAxis("CursorMoveX") > 0) ? 1 : -1;

                    if (nextIndex > 0 && containers.GetID(id).currentRoleIndex + nextIndex < view.GetRolesLength)
                    {
                        containers.GetID(id).currentRoleIndex += nextIndex;
                        view.UpdateRoleContainer(id, containers.GetID(id).currentRoleIndex);
                    }
                    if (nextIndex < 0 && containers.GetID(id).currentRoleIndex + nextIndex >= 0)
                    {
                        containers.GetID(id).currentRoleIndex += nextIndex;
                        view.UpdateRoleContainer(id, containers.GetID(id).currentRoleIndex);
                    }
                }
            }
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
            activeController.Remove(activeController[id]);
            activePlayers.Remove(activePlayers[id]);
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