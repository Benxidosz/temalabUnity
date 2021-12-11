using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "newCard", menuName = "ScriptableObjects/MakeCardObject", order = 1)]
public class CardSO : ScriptableObject, ICloneable {
    public Sprite background;
    public int count;
    [Header("Title")]
    public string name;
    public int titleSize = 20;
    [TextArea]
    public string description;
    public UnityEvent<PlayerController> Action;
    public object Clone() {
        var clone = CreateInstance<CardSO>();
        clone.background = background;
        clone.count = count;
        clone.description = description;
        clone.name = name;
        clone.Action = Action;
        clone.titleSize = titleSize;
        return clone;
    }
}