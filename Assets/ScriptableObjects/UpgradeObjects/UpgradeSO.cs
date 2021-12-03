using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "newUpgrade", menuName = "ScriptableObjects/MakeUpgradeObject", order = 2)]
public class UpgradeSO : ScriptableObject {
    [Header("Sprites")]
    public Sprite unDone;
    public Sprite done;
    [Header("Functionality")] 
    public int activationMax;
}