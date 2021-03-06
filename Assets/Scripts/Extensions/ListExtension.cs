using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListExtension
{
    public static T Random<T>(this List<T> source)
    {
        return source[UnityEngine.Random.Range(0, source.Count)];
    }

    /// <param name="RandomSeed">
    /// Random a list of seed. Length of n, and choose object in prefab pool randomly.
    ///</param>
    public static List<T> RandomSeed<T>(this List<T> source, int n)
    {
        List<int> seed = new List<int>();
        List<T> container = new List<T>();
        for (int i = 0; i < n; i++)
        {
            // int rnd = UnityEngine.Random.Range(0, max);
            int r = UnityEngine.Random.Range(0, source.Count);
            while (true)
            {
                if (seed.Contains(r) == false) break;
                r = UnityEngine.Random.Range(0, source.Count);
            }
            seed.Add(r);
            container.Add(source[r]);
        }
        return container;
    }
}