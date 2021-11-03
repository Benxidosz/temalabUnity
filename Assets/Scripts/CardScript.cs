using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardScript : MonoBehaviour {
    public CardSO backend;

    void Update() {
        backend.Action?.Invoke("Hello");
    }
}