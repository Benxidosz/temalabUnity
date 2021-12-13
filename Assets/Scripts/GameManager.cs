using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    [Serializable]
    public struct NamedUI {
        public UIKeys key;
        public Canvas ui;
    }
    [SerializeField] private NamedUI[] namedUis;
    public enum TurnState {
        beforeRoll, rolled
    }

    public enum UIKeys {
        dicePicker
    }
    public static GameManager Instance { get; private set; }
    public PlayerController CurrentPlayer { get; private set; }
    public TurnState CurrentTurnState { get; private set; }

    public Dictionary<UIKeys, Canvas> UIs;

    private List<PlayerController> _players = new List<PlayerController>();
    private int _currentPlayerIdx = 0;
    private void Awake() {
        if (Instance == null) {
            Instance = this;
            CurrentTurnState = TurnState.beforeRoll;
            UIs = new Dictionary<UIKeys, Canvas>();
            foreach (var ui in namedUis) {
                UIs[ui.key] = ui.ui;
                ui.ui.enabled = false;
            }
        } else {
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
    public void Rolled() {
        CurrentTurnState = TurnState.rolled;
    }
    public void EndTurn() {
        if (CurrentTurnState == TurnState.rolled) {
            _currentPlayerIdx++;
            if (_currentPlayerIdx > _players.Count)
                _currentPlayerIdx = 0;
            CurrentPlayer = _players[_currentPlayerIdx];
            CurrentTurnState = TurnState.beforeRoll;
        }
    }

    public void Village(){
        CurrentPlayer.BuildingController.BuildVillage();
    }

    public void City(){
        CurrentPlayer.BuildingController.BuildCity();
    }

    public void Road(){
        CurrentPlayer.BuildingController.BuildRoad();
    }

    public void UpdatePanel(){
        CurrentPlayer.MaterialController.UpdatePanel();
    }
}