using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private CardInventory _cardInventory;
    private UpgradeManager _upgradeManager;
    private GameManager _gameManager;

    private bool _uiActive = false;

    private Canvas _dicePicker;
    private SubmitButton _dicePickerSubmit;

    public bool DiceSet { get; set; }
    public int WhiteDice { get; private set; }
    public int RedDice { get; private set; }

    void Start() {
        _gameManager = GameManager.Instance;
        _gameManager.RegisterPlayer(this);

        _cardInventory = GetComponent<CardInventory>();
        _upgradeManager = GetComponent<UpgradeManager>();

        _dicePicker = _gameManager.UIs[GameManager.UIKeys.dicePicker];
    }

    // Update is called once per frame
    void Update() {
        if (!_uiActive) {
            if (Input.GetKeyDown("i")) {
                _upgradeManager.Disable();
                _cardInventory.SwitchUiState();
            }

            if (Input.GetKeyDown("c")) {
                _cardInventory.Disable();
                _upgradeManager.SwitchUiState();
            }
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

    public void AddTestCard(CardSO testCard) {
        _cardInventory.AddCard(testCard);
    }

    public void PickDice() {
        _dicePicker.enabled = true;
        _dicePickerSubmit = _dicePicker.GetComponentInChildren<SubmitButton>();
        _dicePickerSubmit.Player = this;
        _uiActive = true;
        _cardInventory.Disable();
        _upgradeManager.Disable();
    }

    public void SubmitPick(int whiteDice, int redDice) {
        _dicePicker.enabled = false;
        _uiActive = false;
        WhiteDice = whiteDice;
        RedDice = redDice;
        DiceSet = true;
        foreach (var group in _dicePicker.GetComponentsInChildren<DiceGroup>()) {
            group.Clear();
        }
    }
}