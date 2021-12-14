using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    
    public static void ShuffleList<T>(List<T> list)
    {
        for (var i = list.Count - 1; i >= 1; i--) {
            var j = Random.Range(0, i);
            var temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
    
}
