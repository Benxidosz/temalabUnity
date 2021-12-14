using System;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

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
        dicePicker, materialPicker
    }
    public static GameManager Instance { get; private set; }
    
    [FormerlySerializedAs("_players")] [SerializeField]private List<PlayerController> _players = new List<PlayerController>();
    public PlayerController CurrentPlayer { get; private set; }
    public TurnState CurrentTurnState { get; private set; }

    public Dictionary<UIKeys, Canvas> UIs;
    public List<PlayerController> Players => _players;
    private int _currentPlayerIdx = 0;

    [SerializeField] private TextMeshProUGUI barbarianText;
    private int _barbarianTurn = 7;
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
        } else {
            player.PointsSwitchState();
        }
        _players.Add(player);
    }
    private void RefreshBarbarianText() {
        barbarianText.text = $"{_barbarianTurn} Black Rolls Until Barbarians";
    }

    private void BarbariansComing() {
        Debug.Log("Barbarians!");
    }
    public void BlackRolled() {
        _barbarianTurn--;
        if (_barbarianTurn == 0) {
            BarbariansComing();
            _barbarianTurn = 7;
        }
        RefreshBarbarianText();
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
            if (_currentPlayerIdx >= _players.Count)
                _currentPlayerIdx = 0;
            CurrentPlayer.PointsSwitchState();
            CurrentPlayer = _players[_currentPlayerIdx];
            CurrentPlayer.PointsSwitchState();
            CurrentTurnState = TurnState.beforeRoll;
        }
        _players.ForEach(player => {
            print(player.MaterialController.GetMaterialCount(MaterialType.Coin));
        });
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

    public void ShowPickMaterial(Action showUI, Action<MaterialType> callBack) {
        showUI();
        UIs[UIKeys.materialPicker].GetComponentInChildren<MaterialSubmitButton>().OnClick = callBack;
    }
}