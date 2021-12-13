using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }
    public PlayerController CurrentPlayer { get => current; private set => current = value; }
    public PlayerController current;
    
    [SerializeField]private List<PlayerController> _players = new List<PlayerController>();

    private PlayerController tmpLastFramePlayer;
    
    public List<PlayerController> Players => _players;
    
    private void Awake() {
        if (Instance == null)
            Instance = this;
        else {
            Destroy(this);
        }
    }

    private void Update(){
        if (!(tmpLastFramePlayer is null) && tmpLastFramePlayer.id != current.id){
            UpdatePanel();
            tmpLastFramePlayer = CurrentPlayer;
        }
        else{
            tmpLastFramePlayer = CurrentPlayer;
        }
    }

    public void RegisterPlayer(PlayerController player) {
        if (_players.Count == 0) {
            CurrentPlayer = player;
            //UpdatePanel();
        }
        _players.Add(player);
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