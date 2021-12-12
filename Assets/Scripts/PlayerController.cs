using ActionCards;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private CardInventory _cardInventory;
    private UpgradeManager _upgradeManager;
    private GameManager _gameManager;

    private void Start() {
        _gameManager = GameManager.Instance;
        _gameManager.RegisterPlayer(this);
        
        _cardInventory = GetComponent<CardInventory>();
        _upgradeManager = GetComponent<UpgradeManager>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.I)) {
            _upgradeManager.Disable();
            _cardInventory.SwitchUiState();
        }

        if (Input.GetKeyDown(KeyCode.C)) {
            _cardInventory.Disable();
            _upgradeManager.SwitchUiState();
        }
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
}