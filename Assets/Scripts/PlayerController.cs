using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private CardInventory _cardInventory;
    private UpgradeManager _upgradeManager;
    private GameManager _gameManager;
    private MaterialController _materialController;
    private BuildingController _buildingController;
    
    public long id{ get; private set; }
    public BuildingController BuildingController => _buildingController;
    public MaterialController MaterialController => _materialController;
   


    private bool _uiActive = false;

    private Canvas _dicePicker;
    private SubmitButton _dicePickerSubmit;

    public bool DiceSet { get; set; }
    public int WhiteDice { get; private set; }
    public int RedDice { get; private set; }

    private int _points = 0;
    public int Points {
        get => _points;
        set {
            if (value < _points)
                throw new ArgumentException("New Point cannot be lower!");
            _points = value;
            RefreshPoints();
        }
    }

    [SerializeField] private TextMeshProUGUI points;

    void Start() {
        _gameManager = GameManager.Instance;
        _gameManager.RegisterPlayer(this);
        _cardInventory = GetComponent<CardInventory>();
        _upgradeManager = GetComponent<UpgradeManager>();

        _materialController = GetComponent<MaterialController>();
        _buildingController = GetComponent<BuildingController>();
        id = GetInstanceID();

        _dicePicker = _gameManager.UIs[GameManager.UIKeys.dicePicker];

        RefreshPoints();
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

    private void RefreshPoints() {
        points.text = $"Points: {Points}";
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