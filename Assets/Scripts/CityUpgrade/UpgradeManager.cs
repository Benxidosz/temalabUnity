using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour {
    [SerializeField] private Canvas ui;
    private UpgradeTile[] _yellowUpgrades;
    private UpgradeTile[] _blueUpgrades;
    private UpgradeTile[] _greenUpgrades;

    private int _yellowCounter = 0;
    private int _blueCounter = 0;
    private int _greenCounter = 0;
    
    void Start() {
        var tmpUpgrades = ui.GetComponentsInChildren<UpgradeTile>();
        _yellowUpgrades = new UpgradeTile[5];
        _blueUpgrades = new UpgradeTile[5];
        _greenUpgrades = new UpgradeTile[5];

        for (int i = 0; i < 5; i++) {
            _yellowUpgrades[i] = tmpUpgrades[i];
            _blueUpgrades[i] = tmpUpgrades[i + 5];
            _greenUpgrades[i] = tmpUpgrades[i + 10];
        }
    }

    public void UpgradeYellow() {
        if (_yellowCounter < _yellowUpgrades.Length)
            _yellowUpgrades[_yellowCounter++].Build();
    }
    public void UpgradeBlue() {
        if (_blueCounter < _blueUpgrades.Length) 
            _blueUpgrades[_blueCounter++].Build();
    }
    public void UpgradeGreen() {
        if (_greenCounter < _greenUpgrades.Length) 
            _greenUpgrades[_greenCounter++].Build();
    }
}