using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

namespace GameManagerSpace.Hall
{
    #region Enum and Class container 
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
    #endregion

    #region Hall model
    public class Model
    {
        #region  Rewired
        public List<Player> activePlayers = new List<Player>();// Player type variable of Rewired.
        public List<InputActionSourceData> activeController = new List<InputActionSourceData>();// Controller type variable of Rewired.
        #endregion

        #region  Hall controller
        public bool AbleToStart { get { return containers.FindAll(x => x.selfSelectState == SelectState.OnWaiting).Count == containers.Count; } }
        public List<PlayerContainer> containers = new List<PlayerContainer>();// Assemble variables handling selecting.
        public bool ableToJoin = false; // Enable players to join.
        public bool isStarting = false; // Prevent game start againg when counting down.
        public string mapName = ""; // Nmae of game scene chosen by players. 
        #endregion
    }
    #endregion

    #region Extension
    public static class HallManagerExtension
    {
        public static PlayerContainer GetID(this List<PlayerContainer> source, int id)
        {
            return source.Find(element => element.id == id);
        }

        public static void UpdateUIHandler(this PlayerContainer source, System.Action<int, int, bool> callback, int containerLengrh, ref int currentIndex, int index, bool isActive = true)
        {
            if (index > 0 && currentIndex + index >= containerLengrh) return;
            else if (index < 0 && currentIndex + index < 0) return;

            int id = source.id;
            callback(id, currentIndex, !isActive);
            currentIndex += index;
            callback(id, currentIndex, isActive);
        }
    }
    #endregion
}