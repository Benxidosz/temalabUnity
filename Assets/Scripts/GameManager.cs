using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }
    public PlayerController CurrentPlayer { get => current; private set => current = value; }
    public PlayerController current;
    
    [FormerlySerializedAs("_players")] [SerializeField]private List<PlayerController> players = new List<PlayerController>();

    private PlayerController _tmpLastFramePlayer;
    
    public List<PlayerController> Players => players;

    [FormerlySerializedAs("barbarText")] [SerializeField] private TextMeshProUGUI barbarianText;
    private readonly NetworkVariable<int> _barbarianTurn = new NetworkVariable<int>(7);
    
    private void Awake() {
        if (Instance == null)
            Instance = this;
        else {
            Destroy(this);
        }
    }

    private void Update(){
        if (!(_tmpLastFramePlayer is null) && _tmpLastFramePlayer.Id != current.Id){
            UpdatePanel();
            _tmpLastFramePlayer = CurrentPlayer;
        }
        else{
            _tmpLastFramePlayer = CurrentPlayer;
        }
    }

    public void RegisterPlayer(PlayerController player) {
        if (players.Count == 0) {
            CurrentPlayer = player;
            //UpdatePanel();
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
        _barbarianTurn.Value--;
        if (_barbarianTurn.Value == 0) {
            BarbariansComing();
            _barbarianTurn.Value = 7;
        }
        RefreshBarbarianText();
    }
    public void DrawActionCard(ActionDice action) {
        CurrentPlayer.DrawActionCard(action);
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