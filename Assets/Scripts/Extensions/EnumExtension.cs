using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameManagerSpace;
using GameManagerSpace.Audio;

public static class EnumExtension
{
    /// <summary>
    /// Compare Enum state by string name.
    /// </summary>
    /// <param name="CompareState">Enum usage</param>
    public static bool CompareState<T>(this T state, string index) where T : struct
    {
        var _tState = state.GetType();
        if (_tState == typeof(SceneState))
        {
            return _tState == (object)(SceneState)System.Enum.Parse(typeof(SceneState), index) ? true : false;
        }
        else if (_tState == typeof(AudioInScene))
        {
            return _tState == (object)(AudioInScene)System.Enum.Parse(typeof(AudioInScene), index) ? true : false;
        }
        else
        {
            Debug.Log("Data types must be enum like SceneState, GameState.\n You might want to regist it.");
            return false;
        }
    }

    /// <summary>
    /// Change Enum state by string name.
    /// </summary>
    /// <param name="ChangeState">Enum usage</param>
    public static void ChangeState<T>(this ref T state, string index) where T : struct
    {
        var _tState = state.GetType();
        if (_tState == typeof(AudioInScene))
        {
            state = (T)(object)(AudioInScene)System.Enum.Parse(typeof(AudioInScene), index);
        }
        else if (_tState == typeof(SceneState))
        {
            state = (T)(object)(SceneState)System.Enum.Parse(typeof(SceneState), index);
        }
        else
        {
            Debug.Log("Data types must be enum like SceneState, GameState.\n You might want to regist it.");
            return;
        }
    }
}