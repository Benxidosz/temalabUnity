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
        title[1].text = backend.description;
        Debug.Log(GetComponentInChildren<Image>().sprite = backend.background);
    }

    public void onClick() {
        backend.Action?.Invoke("Hello");
    }
}