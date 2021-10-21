using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "newCard", menuName = "ScriptableObjects/MakeCardObject", order = 1)]
public class CardSO : ScriptableObject {
    public Sprite background;
    [TextArea]
    public string text;
    public UnityEvent<string> Action;
}