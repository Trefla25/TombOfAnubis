using System.Collections.Generic;
using UnityEngine;

public static class ListExtensions
{
    public static T Draw<T>(this List<T> list)
    {
        if (list.Count == 0) return default;
        var r = Random.Range(0, list.Count);
        var t = list[r];
        list.Remove(t);
        return t;
    }
}
