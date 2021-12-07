using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private CardInventory _cardInventory;
    private UpgradeManager _upgradeManager;
    private GameManager _gameManager;
    
    void Start() {
        _gameManager = GameManager.Instance;
        _gameManager.RegisterPlayer(this);
        
        _cardInventory = GetComponent<CardInventory>();
        _upgradeManager = GetComponent<UpgradeManager>();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown("i")) {
            _upgradeManager.Disable();
            _cardInventory.SwitchUiState();
        }

        if (Input.GetKeyDown("c")) {
            _cardInventory.Disable();
            _upgradeManager.SwitchUiState();
        }
    }
    public void DrawActionCard(ActionDice action) {
        switch (action) {
            case ActionDice.blue: {
                if (DiceRoller.Instance.RedDice <= _upgradeManager.BlueCounter)
                    _cardInventory.AddCard(CardDealer.Instance.NextBlueCard);
                break;
            }
            case ActionDice.green: {
                if (DiceRoller.Instance.RedDice <= _upgradeManager.GreenCounter)
                    _cardInventory.AddCard(CardDealer.Instance.NextGreenCard);
                break;
            }
            case ActionDice.yellow: {
                if (DiceRoller.Instance.RedDice <= _upgradeManager.YellowCounter)
                    _cardInventory.AddCard(CardDealer.Instance.NextYellowCard);
                break;
            }
        }
    }
}