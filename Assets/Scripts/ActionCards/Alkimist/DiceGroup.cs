using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceGroup : MonoBehaviour {
    private DiceChooser lastDiceChooser;
    private DiceChooser[] dices;
    public int Value => lastDiceChooser.Value;

    private void Awake() {
        dices = GetComponentsInChildren<DiceChooser>();
        lastDiceChooser = dices[0];
    }

    void Start() {
        lastDiceChooser.Button.interactable = false;
        foreach (var dice in dices) {
            dice.ButtonClicked += ButtonClicked;
        }
    }

    private void ButtonClicked(DiceChooser dice) {
        dice.Button.interactable = false;
        lastDiceChooser.Button.interactable = true;
        lastDiceChooser = dice;
    }

    public void Clear() {
        lastDiceChooser.Button.interactable = true;
        lastDiceChooser = dices[0];
        lastDiceChooser.Button.interactable = false;
    }
}