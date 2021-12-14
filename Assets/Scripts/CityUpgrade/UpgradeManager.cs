using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour {
    [SerializeField] private Canvas ui;
    private UpgradeTile[] _yellowUpgrades;
    private UpgradeTile[] _blueUpgrades;
    private UpgradeTile[] _greenUpgrades;
    private PlayerController player;

    public int YellowCounter { get; private set; }
    public int BlueCounter { get; private set; }
    public int GreenCounter { get; private set; }
    
    void Start(){
        player = GameManager.Instance.CurrentPlayer;
        var tmpUpgrades = ui.GetComponentsInChildren<UpgradeTile>();
        _yellowUpgrades = new UpgradeTile[5];
        _blueUpgrades = new UpgradeTile[5];
        _greenUpgrades = new UpgradeTile[5];

        YellowCounter = 0;
        BlueCounter = 0;
        GreenCounter = 0;

        for (int i = 0; i < 5; i++) {
            _yellowUpgrades[i] = tmpUpgrades[i];
            _blueUpgrades[i] = tmpUpgrades[i + 5];
            _greenUpgrades[i] = tmpUpgrades[i + 10];
        }

        ui.enabled = false;
    }

    public void UpgradeYellow(){
        if (player.MaterialController.GetMaterialCount(MaterialType.Canvas) <= YellowCounter + 1) return;
        if (YellowCounter >= _yellowUpgrades.Length) return;
        _yellowUpgrades[YellowCounter++].Build();
        player.MaterialController.Decrease(MaterialType.Canvas, YellowCounter);
    }
    public void UpgradeBlue() {
        if (player.MaterialController.GetMaterialCount(MaterialType.Coin) <= BlueCounter + 1) return;
        if (BlueCounter >= _blueUpgrades.Length) return;
        _blueUpgrades[BlueCounter++].Build();
        player.MaterialController.Decrease(MaterialType.Coin, BlueCounter);
    }
    public void UpgradeGreen() {
        if (player.MaterialController.GetMaterialCount(MaterialType.Paper) <= GreenCounter + 1) return;
        if (GreenCounter >= _greenUpgrades.Length) return;
        _greenUpgrades[GreenCounter++].Build();
        player.MaterialController.Decrease(MaterialType.Paper, GreenCounter);
    }
    
    private void EmptyUI() {
        for (int i = 0; i < 5; i++) {
            _yellowUpgrades[i].Reset();
            _blueUpgrades[i].Reset();
            _greenUpgrades[i].Reset();
        }
    }

    private void RefillUI() {
        for (int i = 0; i < BlueCounter; i++) {
            _blueUpgrades[i].Build();
        }
        for (int i = 0; i < GreenCounter; i++) {
            _greenUpgrades[i].Build();
        }
        for (int i = 0; i < YellowCounter; i++) {
            _yellowUpgrades[i].Build();
        }
    }
    
    public void SwitchUiState() {
        ui.enabled = !ui.enabled;
        if (ui.enabled) {
            RefillUI();
        }
        else {
            EmptyUI();
        }
    }

    public void Disable() {
        ui.enabled = false;
        EmptyUI();
    }
}