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
    public void Alkimist(PlayerController player) {
        player.PickDice();
    }

    public void IncreasePoint(PlayerController player) {
        ++player.Points;
    }

    public void AddTestCard(CardSO testCard) {
        GameManager.Instance.CurrentPlayer.AddTestCard(testCard);
    }
}
