using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceChooser : MonoBehaviour {
    [SerializeField] private int value = 0;
    private Button _button;
    public int Value => value;
    public event Action<DiceChooser> ButtonClicked;
    public Button Button => _button;
    void Awake() {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(() => {
            ButtonClicked?.Invoke(this);
        });
    }
}