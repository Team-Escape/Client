using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Rewired;
using PlayerSpace.Game;

namespace GameManagerSpace.Game
{
    public enum GameState
    {
        Setting, // On game scene and is setting.
        Starting, // On game scene and is starting.
        Playing, // On game scene and is playing.
        Scoring, // On score scene.
        NewGame,
        GameOver,
        GameDraw,
    }
    public enum GameListenerState
    {
        Caught,
        Arrival,
    }
    public class Model : MonoBehaviour
    {
        public Camera mainCam = null;

        #region  Game Level : Rooms
        #region StartRoomStuff
        [Header("Start room")]
        public Transform startRoom = null;
        public Tilemap startRoomTilemap = null;
        public Vector3Int[] wallDestoryInEscape = null;
        public Vector3Int[] wallDestoryInHunter = null;
        #endregion

        #region Destination
        [Header("Destination")]
        public Transform destinationRoomUp = null;
        public Transform destinationRoomDown = null;
        public Transform destinationRoomRight = null;
        public Transform destinationRoomLeft = null;
        #endregion        

        #region Rooms
        public List<GameObject> blocks { get; set; }
        [Header("Room block")]
        public int roomSize = 5;
        public BlockContainer blockContainer = null;
        #endregion
        #endregion

        #region Game Logic
        #endregion

        #region PlayerStuff
        [Header("Player variable")]
        public Transform hunterSpawn = null;
        public Transform escaperSpawn = null;
        [HideInInspector]
        public List<PlayerCharacter> roles = null;
        [HideInInspector]
        public PlayerCharacter hunter = null;
        [HideInInspector]
        public List<PlayerCharacter> escapers = new List<PlayerCharacter>();
        [HideInInspector]
        public Player hunterPlayer = null;
        [HideInInspector]
        public List<Player> escaperPlayers = null;
        public List<PlayerCharacter> GetCaughtRoles { get; set; }
        public List<PlayerCharacter> GoalRoles { get; set; }
        #endregion

        #region StartItem
        [Header("Start item")]
        public List<GameObject> startItemGameObjects = new List<GameObject>();
        public List<StartItemContainer> startItemContainers = new List<StartItemContainer>();
        #endregion

    }

    [System.Serializable]
    public class StartItemContainer
    {
        public Sprite display = null;
        public string name = "";
    }

    [System.Serializable]
    public class BlockContainer
    {
        public List<GameObject> left = new List<GameObject>();
        public List<GameObject> right = new List<GameObject>();
        public List<GameObject> up = new List<GameObject>();
        public List<GameObject> down = new List<GameObject>();
    }
}