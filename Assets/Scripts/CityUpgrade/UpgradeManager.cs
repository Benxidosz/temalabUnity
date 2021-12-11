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

    public int YellowCounter { get; private set; }
    public int BlueCounter { get; private set; }
    public int GreenCounter { get; private set; }
    
    void Start() {
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

    public void UpgradeYellow() {
        if (YellowCounter < _yellowUpgrades.Length)
            _yellowUpgrades[YellowCounter++].Build();
    }
    public void UpgradeBlue() {
        if (BlueCounter < _blueUpgrades.Length) 
            _blueUpgrades[BlueCounter++].Build();
    }
    public void UpgradeGreen() {
        if (GreenCounter < _greenUpgrades.Length) 
            _greenUpgrades[GreenCounter++].Build();
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