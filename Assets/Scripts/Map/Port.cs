using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Port", menuName = "ScriptableObjects/Port", order = 4)]
public class Port : ScriptableObject
{
    public new string name;
    public Texture2D texture;

    public int quantity;
}
