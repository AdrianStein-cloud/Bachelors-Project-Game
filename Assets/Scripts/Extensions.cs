using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
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

    public static bool Opposite(this Vector3 a, Vector3 b)
    {
        return Mathf.Approximately(Vector3.Dot(a.normalized, b.normalized), -1);
    }

    public static float DistanceAlongDirection(this Vector3 v, Vector3 direction)
    {
        return Vector3.Dot(v, direction.normalized);
    }

    public static int GetChance(this Rarity rarity)
    {
        return UpgradeConstants.Instance.chance
            .Select(t => new { rarity = t.Key, chacne = t.Value })
            .Where(t => t.rarity == rarity)
            .First()
            .chacne;
    }

    public static int GetPrice(this Rarity rarity)
    {
        return UpgradeConstants.Instance.price
            .Select(t => new { rarity = t.Key, price = t.Value })
            .Where(t => t.rarity == rarity)
            .First()
            .price;
    }

    public static Color GetColor(this Rarity rarity)
    {
        return UpgradeConstants.Instance.colors
            .Select(t => new { rarity = t.Key, color = t.Value })
            .Where(t => t.rarity == rarity)
            .First()
            .color;
    }

    public static List<Enum> GetFlags(this Enum e)
    {
        return Enum.GetValues(e.GetType())
            .Cast<Enum>()
            .Where(e.HasFlag)
            .ToList();
    }

    public static Vector3 RoundToNearestInt(this Vector3 v)
    {
        return new Vector3(Mathf.Round(v.x), Mathf.Round(v.y), Mathf.Round(v.z));
    }

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

    public static bool ApproxEquals(this Vector3 a, Vector3 b, float tolerance)
    {
        bool x = Mathf.Abs(a.x - b.x) < tolerance;
        bool y = Mathf.Abs(a.y - b.y) < tolerance;
        bool z = Mathf.Abs(a.z - b.z) < tolerance;
        Debug.Log($"{a} == {b} is {x & y & z}");
        return x & y & z;
    }

    public static IEnumerable<Tuple<T, T>> GetPairs<T>(this IEnumerable<T> enumerable, Func<T, T, bool> pairSeletor)
    {
        var list = enumerable.ToList();

        for (int i = 0; i < list.Count; i++)
        {
            for (int j = i + 1; j < list.Count; j++)
            {
                if (pairSeletor(list[i], list[j]))
                {
                    yield return new Tuple<T, T>(list[i], list[j]);
                }
            }
        }
    }

}

public class VectorComparer : IEqualityComparer<Vector3>
{
    public bool Equals(Vector3 a, Vector3 b)
    {
        bool equal = Mathf.Approximately(a.x, b.x)
            && Mathf.Approximately(a.y, b.y)
            && Mathf.Approximately(a.z, b.z);
        Debug.Log($"{a} == {b}: {equal.ToString()}");
        return equal;
    }

    public int GetHashCode(Vector3 obj)
    {
        return obj.GetHashCode();
    }
}

