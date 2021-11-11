using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "newCard", menuName = "ScriptableObjects/MakeCardObject", order = 1)]
public class CardSO : ScriptableObject {
    public Sprite background;
    public string name;
    [TextArea]
    public string description;
    public UnityEvent<string> Action;
}