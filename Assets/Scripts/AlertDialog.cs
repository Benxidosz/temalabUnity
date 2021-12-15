using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AlertDialog : MonoBehaviour {
    private Canvas ui;
    [SerializeField] private TextMeshProUGUI message;

    void Start() {
        ui = GetComponent<Canvas>();
    }

    public void Ok() {
        ui.enabled = false;
    }

    public void ShowDialog(string msg) {
        message.text = msg;
        ui.enabled = true;
    }
}