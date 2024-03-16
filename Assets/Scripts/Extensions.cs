using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

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

    #region VectorExtensions

    public static Vector3 WithX(this Vector3 v, float x) => new(x, v.y, v.z);

    public static Vector3 WithY(this Vector3 v, float y) => new(v.x, y, v.z);

    public static Vector3 WithZ(this Vector3 v, float z) => new(v.x, v.y, z);

    public static Vector3 AddX(this Vector3 v, float x) => new(v.x + x, v.y, v.z);

    public static Vector3 AddY(this Vector3 v, float y) => new(v.x, v.y + y, v.z);

    public static Vector3 AddZ(this Vector3 v, float z) => new(v.x, v.y, v.z + z);

    public static Vector2 WithX(this Vector2 v, float x) => new(x, v.y);

    public static Vector2 WithY(this Vector2 v, float y) => new(v.x, y);

    public static Vector2 AddX(this Vector2 v, float y) => new(v.x, v.y + y);

    public static Vector2 AddY(this Vector2 v, float x) => new(v.x + x, v.y);

    #endregion
}

