using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameManagerSpace.Game
{
    public enum GameState
    {
        Setting, // On game scene and is setting.
        Starting, // On game scene and is starting.
        Playing, // On game scene and is playing.
        Scoring, // On score scene.
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

    }
}