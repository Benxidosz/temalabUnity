using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CardScript : MonoBehaviour {
    [SerializeField] private CardSO backend;
    public CardSO Backend {
        get => backend;
        set {
            backend = value;
            RefreshUI();
        }
    }
    private Text[] _texts;
    private Image _img;

    private void Awake() {
        _texts = GetComponentsInChildren<Text>();
        _img = GetComponent<Image>();
    }

    private void RefreshUI() {
        _texts[0].text = backend.name;
        _texts[0].fontSize = backend.titleSize;
        _texts[1].text = backend.description;
        _img.sprite = backend.background;
    }

    public void onClick() {
        Backend.Action?.Invoke(GameManager.Instance.CurrentPlayer);
    }
}