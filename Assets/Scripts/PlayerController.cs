using ActionCards;
using Buildings;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private CardInventory _cardInventory;
    private UpgradeManager _upgradeManager;
    private GameManager _gameManager;

    private MaterialController _materialController;
    private BuildingController _buildingController;
    
    public long Id{ get; private set; }
    public BuildingController BuildingController => _buildingController;
    public MaterialController MaterialController => _materialController;
   


    private bool _uiActive = false;

    private Canvas _dicePicker;
    private DiceSubmitButton _dicePickerDiceSubmit;

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

    private void Awake(){
        _gameManager = GameManager.Instance;
        _gameManager.RegisterPlayer(this);
        _cardInventory = GetComponent<CardInventory>();
        _upgradeManager = GetComponent<UpgradeManager>();

        _materialController = GetComponent<MaterialController>();
        _buildingController = GetComponent<BuildingController>();
        _dicePicker = _gameManager.UIs[GameManager.UIKeys.dicePicker];

        RefreshPoints();
        Id = GetInstanceID();
    }

    private void Update() {
        if (!_uiActive && this == _gameManager.CurrentPlayer) {
            if (Input.GetKeyDown(KeyCode.I)) {
                _upgradeManager.Disable();
                _cardInventory.SwitchUiState();
            }

            if (Input.GetKeyDown(KeyCode.C)) {
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
            case ActionDice.Blue: {
                if (DiceRoller.Instance.RedDice <= _upgradeManager.BlueCounter)
                    _cardInventory.AddCard(CardDealer.Instance.NextBlueCard);
                break;
            }
            case ActionDice.Green: {
                if (DiceRoller.Instance.RedDice <= _upgradeManager.GreenCounter)
                    _cardInventory.AddCard(CardDealer.Instance.NextGreenCard);
                break;
            }
            case ActionDice.Yellow: {
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
        _dicePickerDiceSubmit = _dicePicker.GetComponentInChildren<DiceSubmitButton>();
        _dicePickerDiceSubmit.Player = this;
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

    public void PointsSwitchState() {
        points.enabled = !points.enabled;
    }
}