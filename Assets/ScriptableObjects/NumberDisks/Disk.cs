using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Disk", menuName = "ScriptableObjects/Disk", order = 3)]
public class Disk : ScriptableObject {
    public new string name;
    public Texture2D texture;

    public int quantity;
    public int value;
}