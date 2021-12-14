using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemTradeManager : MonoBehaviour {
    [SerializeField] private Canvas ui;
    private PlayerController myPlayer;
    private SystemTradeController _systemTradeController;
    [SerializeField] private MaterialPickerUIController materialPickerUIController;

    private void Awake() {
        myPlayer = GetComponent<PlayerController>();
        _systemTradeController = ui.GetComponent<SystemTradeController>();
        ui.enabled = false;
    }
    
    private void EmptyUI() {
        foreach (var materialPicker in _systemTradeController.Sells) {
            materialPicker.Enable();
        }
    }

    private void RefillUI() {
        bool first = true;
        foreach (var materialPicker in _systemTradeController.Sells) {
            MaterialType type = materialPicker.Value;
            if (myPlayer.MaterialController.GetMaterialCount(type) >= myPlayer.TradingNeeds[type]) {
                if (first) {
                    materialPickerUIController.PickMaterial(materialPicker);
                    first = false;
                }
                materialPicker.Enable();
                
            } else {
                materialPicker.Disable();
            }
        }
        if (first)
            materialPickerUIController.SelectNone();
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