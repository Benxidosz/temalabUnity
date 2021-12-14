using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTurnButton : MonoBehaviour {
    void Start() {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(GameManager.Instance.EndTurn);
    }
}