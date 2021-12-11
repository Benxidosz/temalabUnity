using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }
    public PlayerController CurrentPlayer { get; private set; }
    
    private List<PlayerController> _players = new List<PlayerController>();
    private void Awake() {
        if (Instance == null)
            Instance = this;
        else {
            Destroy(this);
        }
    }
    public void RegisterPlayer(PlayerController player) {
        if (_players.Count == 0) {
            CurrentPlayer = player;
        }
        _players.Add(player);
    }

    public void DrawActionCard(ActionDice action) {
        CurrentPlayer.DrawActionCard(action);
    }
}