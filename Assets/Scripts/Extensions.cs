using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

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

    public static T GetRollFromWeights<T>(this IEnumerable<T> weights, System.Random random) where T : IWeighted
    {
        var totalWeight = weights.Sum(w => w.Weight);

        var roll = random.Next(1, totalWeight + 1);
        var tempWeight = 0f;
        foreach (var w in weights)
        {
            tempWeight += w.Weight;
            if (tempWeight >= roll) return w;
        }

        throw new Exception("No roll");
    }

    public static void OnAnyEvent(this InputAction inputAction, Action<InputAction.CallbackContext> action)
    {
        inputAction.started += action;
        inputAction.performed += action;
        inputAction.canceled += action;
    }

    public static bool Parallel(this Vector3 a, Vector3 b)
    {
        return Mathf.Approximately(Mathf.Abs(Vector3.Dot(a, b)), 1);
    }
}

