using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameManagerSpace
{
    public class CoreModel : MonoBehaviour
    {
        public static string choosenMapName = "";
        public static int activePlayers = 0;
        public static List<GameObject> EscaperPrefabs { get; set; }
        public static List<GameObject> HunterPrefabs { get; set; }
        public static List<GameObject> WinnerPlayers { get; set; }

        // ------------------------------------

        public static int winningScore = 0;
        public static List<int> scores { get; set; }
        public static List<int> obtainScores { get; set; }
    }
    public enum SceneState
    {
        StartScene,
        MenuScene,
        SettingsScene,
        RemappingScene,
        HallScene,
        LabMapScene,
        ScoreScene,
        AwardScene,
    };
    public enum GameState
    {
        Setting, // On game scene and is setting.
        Starting, // On game scene and is starting.
        Playing, // On game scene and is playing.
        Scoring, // On score scene.
        GameOver,
        GameDraw,
    }
}