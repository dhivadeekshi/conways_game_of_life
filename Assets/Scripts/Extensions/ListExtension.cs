using System.Collections.Generic;
using UnityEngine;

public static class ListExtension {

    public static void PrintAllElements<T>(this List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
            Debug.Log("Element at " + i + ": " + list[i]);
    }
}
