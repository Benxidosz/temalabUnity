using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MaterialPickerUIController : MonoBehaviour {
    [SerializeField] private GameObject commonGroup;
    [SerializeField] private GameObject tradingGroup;

    private MaterialPicker _lastMaterialPicker;
    private MaterialPicker[] _materialPickers;

    public MaterialType? Value {
        get {
            if (_lastMaterialPicker is null)
                return null;
            return _lastMaterialPicker.Value;
        }
    }

    private MaterialPickerUIController _materialPickerUIController;
    public Canvas Canvas { get; private set; }

    private void Awake() {
        _materialPickers = GetComponentsInChildren<MaterialPicker>();
        _materialPickerUIController = GetComponentInParent<MaterialPickerUIController>();
        Canvas = GetComponentInParent<Canvas>();
        _lastMaterialPicker = _materialPickers[0];
    }

    // Start is called before the first frame update
    void Start() {
        _lastMaterialPicker.Button.interactable = false;
        foreach (var materialPicker in _materialPickers) {
            materialPicker.ButtonClicked += ButtonClicked;
        }
    }

    public void PickMaterial(MaterialPicker materialPicker) {
        _lastMaterialPicker.Button.interactable = true;
        _lastMaterialPicker = materialPicker;
        _lastMaterialPicker.Button.interactable = false;
    }
    
    private void ButtonClicked(MaterialPicker materialPicker) {
        materialPicker.Button.interactable = false;
        _lastMaterialPicker.Button.interactable = true;
        _lastMaterialPicker = materialPicker;
    }

    public void ShowCommon() {
        _lastMaterialPicker.Button.interactable = true;
        _lastMaterialPicker = _materialPickers[0];
        commonGroup.SetActive(true);
        tradingGroup.SetActive(false);
        Canvas.enabled = true;
    }

    public void ShowTrading() {
        _lastMaterialPicker.Button.interactable = true;
        _lastMaterialPicker = _materialPickers[5];
        _lastMaterialPicker.Button.interactable = false;
        commonGroup.SetActive(false);
        tradingGroup.SetActive(true);
        Canvas.enabled = true;
    }

    public void ShowAll() {
        commonGroup.SetActive(true);
        tradingGroup.SetActive(true);
        Canvas.enabled = true;
    }

    public void SelectNone() {
        if (_lastMaterialPicker != null) {
            _lastMaterialPicker.Button.interactable = true;
            _lastMaterialPicker = null;
        }
    }
}