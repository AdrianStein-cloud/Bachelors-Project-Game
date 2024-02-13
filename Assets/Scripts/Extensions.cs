using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static void Shuffle<T>(this IList<T> list, System.Random random)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(0, n + 1);
            (list[n], list[k]) = (list[k], list[n]);
        }
    }
}
