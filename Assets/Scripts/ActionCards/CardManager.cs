using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Dynamic;
using System.Linq;
using UnityEngine;

public class CardManager : MonoBehaviour {
    public static CardManager Instance { get; set; }

    public void Awake() {
        if (Instance == null)
            Instance = this;
        else {
            Destroy(this);
        }
    }

    public void testAction(string player) {
        Debug.Log(player);
    }

    public void testSokAction(string player) {
        Debug.Log(player.ToUpper());
    }
    public void uj(string player) {
        Debug.Log(player.ToUpper());
    }
}
