using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tile", menuName = "ScriptableObjects/Tile", order = 5)]
public class Tile : ScriptableObject
{
    public new string name;
    public Texture2D texture;

    public int firstMaterial;
    public int secondMaterial;

    public int quantity;
   
}
