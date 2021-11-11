using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "newCard", menuName = "ScriptableObjects/MakeCardObject", order = 1)]
public class CardSO : ScriptableObject {
    public Sprite background;
    public int count;
    [Header("Title")]
    public string name;
    public int titleSize = 20;
    [TextArea]
    public string description;
    public UnityEvent<string> Action;
}