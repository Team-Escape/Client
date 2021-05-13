using System;
using System.Collections;
using UnityEngine;

namespace GameManagerSpace.Game
{
    public class Control : MonoBehaviour
    {
        Action<string> changeGameStateAction = null;
        int activePlayers = 0;
        int gotStartItemPlayers = 0;

        public void Init(Action<string> changeGameStateCallback)
        {
            changeGameStateAction = changeGameStateCallback;
        }

        # region Game Setting
        public IEnumerator RandomRooms()
        {
            yield return null;
        }

        public IEnumerator RandomPlayerAvatars()
        {
            yield return null;
        }

        public IEnumerator RandomStartItem()
        {
            yield return null;
        }

        public IEnumerator SpawnPlayers()
        {

            yield return null;
        }

        public IEnumerator SpawnRooms()
        {
            yield return null;
        }

        public IEnumerator SpawnStartItems()
        {
            yield return null;
        }
        #endregion

        #region Game starting
        public IEnumerator InitGameObstacle()
        {
            yield return null;
        }
        public IEnumerator HunterGameSetup()
        {
            yield return null;
        }
        public IEnumerator OpenEscaperRoomsDoor()
        {
            yield return null;
        }
        public IEnumerator OpenHunterRoomsDoor()
        {
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
        #endregion

        #region Game over
        #endregion

        #region Game draw
        #endregion
    }
}