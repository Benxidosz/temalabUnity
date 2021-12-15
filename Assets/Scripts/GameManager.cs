using System;
using System.Collections.Generic;
using System.Linq;
using Map;
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
        BeforeRoll,
        Rolled
    }

    public enum UIKeys {
        DicePicker,
        MaterialPicker,
        AlertDialog
    }

    public static GameManager Instance { get; private set; }

    [FormerlySerializedAs("_players")] [SerializeField]
    private List<PlayerController> players = new List<PlayerController>();
    
    public PlayerController CurrentPlayer { get; private set; }
    public TurnState CurrentTurnState { get; private set; }

    public Dictionary<UIKeys, Canvas> UIs;
    public List<PlayerController> Players => players;
    
    [SerializeField] private List<TileController> _tileControllers = new List<TileController>();
    private int _currentPlayerIdx = 0;

    [SerializeField] private TextMeshProUGUI barbarianText;
    
    [SerializeField] private Sprite emptyCard;
    public Sprite EmptyCard => emptyCard;
    private int _barbarianTurn = 7;
    public Robber Robber{ private get; set; }
    private bool _robberMovable;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            CurrentTurnState = TurnState.BeforeRoll;
            UIs = new Dictionary<UIKeys, Canvas>();
            foreach (var ui in namedUis) {
                UIs[ui.key] = ui.ui;
                ui.ui.enabled = false;
            }
        } else {
            Destroy(this);
        }
    }
    
    public void AddTileController(TileController controller){
        _tileControllers.Add(controller);
    }
    
    public void RegisterPlayer(PlayerController player) {
        if (players.Count == 0) {
            CurrentPlayer = player;
        } else {
            player.PointsSwitchState();
        }

        players.Add(player);
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

    public void Rolled(int sum) {
        _robberMovable = sum == 7;
        CurrentTurnState = TurnState.Rolled;
        foreach (var controller in _tileControllers.Where(oc => oc.MyNumber == sum)){
            controller.Harvest();
        }
    }

    public void MoveRobber(GameObject tile){
        if (_robberMovable == false) return;
        Robber.ChangeTile(tile);
        _robberMovable = false;
    }

    public void EndTurn() {
        print(CurrentTurnState);
        if (CurrentTurnState == TurnState.Rolled) {
            CurrentPlayer.ResetTemporaryNeed();
            _currentPlayerIdx++;
            if (_currentPlayerIdx >= players.Count)
                _currentPlayerIdx = 0;
            CurrentPlayer.PointsSwitchState();
            CurrentPlayer = players[_currentPlayerIdx];
            CurrentPlayer.PointsSwitchState();
            CurrentTurnState = TurnState.BeforeRoll;
        } else {
            UIs[UIKeys.AlertDialog].GetComponent<AlertDialog>().ShowDialog("You have not rolled yet!");
        }
    }

    public void Village() {
        CurrentPlayer.BuildingController.BuildVillage();
    }

    public void City() {
        CurrentPlayer.BuildingController.BuildCity();
    }

    public void Road() {
        CurrentPlayer.BuildingController.BuildRoad();
    }

    public void UpdatePanel() {
        CurrentPlayer.MaterialController.UpdatePanel();
    }

    public void ShowPickMaterial(Action showUI, Action<MaterialType?> callBack) {
        showUI();
        UIs[UIKeys.MaterialPicker].GetComponentInChildren<MaterialSubmitButton>().OnClick = callBack;
    }

    public IEnumerable<TileController> GetTiles(MaterialType type) {
        List<TileController> res = new List<TileController>();
        _tileControllers.ForEach(tile => {
            if (tile.MyType == type)
                res.Add(tile);
        });
        return res;
    }
}