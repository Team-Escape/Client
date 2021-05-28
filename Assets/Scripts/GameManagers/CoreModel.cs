using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using PlayerSpace.Game;
namespace GameManagerSpace
{
    public class CoreModel : MonoBehaviour
    {
        #region Rewired
        public static List<Player> ActivePlayers { get; set; }
        public static List<InputActionSourceData> ActiveController { get; set; }
        #endregion

        #region Game variables (passed by hallmanager)
        public static string choosenMapName = "";
        public static int activePlayersCount = 0;
        public static List<GameObject> RoleAvatars { get; set; }
        #endregion

        #region  Game judgementss (passed by different classes with different scenes)
        public static List<GameObject> WinnerAvatars { get; set; }
        public static int winningScore = 3;
        public static List<int> TotalScores { get; set; }
        #endregion
    }
    public enum SceneState
    {
        StartScene,
        MenuScene,
        SettingsScene,
        RemappingScene,
        HallScene,
        LabScene,
        ScoreScene,
        AwardScene,
    };
}