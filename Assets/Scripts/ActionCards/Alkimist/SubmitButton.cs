using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubmitButton : MonoBehaviour {
    [Header("Disces")]
    [SerializeField] private DiceGroup whiteDice;
    [SerializeField] private DiceGroup redDice;

    public PlayerController Player { get; set; }
    void Start() {
        GetComponent<Button>().onClick.AddListener(() => {
            Player.SubmitPick(whiteDice.Value, redDice.Value);
        });
    }
}