using System;
using UnityEngine;
using GameManagerSpace;
using GameManagerSpace.Hall;
using GameManagerSpace.Audio;

public static class EnumExtension
{
    /// <summary>
    /// Compare Enum state by string name.
    /// </summary>
    /// <param name="CompareState">Enum usage</param>
    public static bool IsState<T>(this T state, string index) where T : struct
    {
        Enum s1 = Enum.Parse(typeof(T), state.ToString()) as Enum;
        int x1 = Convert.ToInt32(s1);

        Enum s2 = Enum.Parse(typeof(T), index.ToEnum<T>().ToString()) as Enum;
        int x2 = Convert.ToInt32(s2);

        return (x1 == x2) ? true : false;
    }

    public static T ToEnum<T>(this string value)
    {
        return (T)(object)System.Enum.Parse(typeof(T), value, true);
    }

    /// <summary>
    /// Change Enum state by string name.
    /// </summary>
    /// <param name="ChangeState">Enum usage</param>
    public static void Change<T>(this ref T state, string index) where T : struct
    {
        state = index.ToEnum<T>();
    }
}