using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardScript : MonoBehaviour {
    public CardSO backend;
    private Text[] title;

    private void Start() {
        title = GetComponentsInChildren<Text>();
        title[0].text = backend.name;
        title[0].fontSize = backend.titleSize;
        title[1].text = backend.description;
    }

    public void onClick() {
        backend.Action?.Invoke("Hello");
    }
}