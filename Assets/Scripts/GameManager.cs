using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }
    public PlayerController CurrentPlayer { get; private set; }
    
    private readonly List<PlayerController> _players = new List<PlayerController>();

    [SerializeField] private TextMeshProUGUI barbarText;
    private int barbarTurn = 7;
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
    private void RefreshBarbarText() {
        barbarText.text = $"{barbarTurn} Black Rolls Until Barbars";
    }

    private void BarbarsComing() {
        Debug.Log("Barbars!");
    }
    public void BlackRolled() {
        barbarTurn--;
        if (barbarTurn == 0) {
            BarbarsComing();
            barbarTurn = 7;
        }
        RefreshBarbarText();
    }
    public void DrawActionCard(ActionDice action) {
        CurrentPlayer.DrawActionCard(action);
    }
}