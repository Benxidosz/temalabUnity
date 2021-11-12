using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void SetTextDelegate(int count, MaterialType type);
public class MaterialController : MonoBehaviour{
    public event SetTextDelegate SetText;
    
    private Dictionary<MaterialType, int> materials;

    private void Start(){
        materials = new Dictionary<MaterialType, int>();
        var tmp = GameObject.FindGameObjectsWithTag("MaterialText");
        foreach (var m in tmp){
            materials.Add(m.GetComponent<Material>().Type, 2);
           // SetText?.Invoke(materials[m.GetComponent<Material>().Type],m.GetComponent<Material>().Type);
        }
        print("");
    }

    public void Increase(MaterialType type, int deltaCount){
        materials[type] += deltaCount;
        SetText?.Invoke(materials[type], type);
    }

    public void Decrease(MaterialType type, int deltaCount){
        materials[type] -= deltaCount;
        SetText?.Invoke(materials[type], type);
    }

    public int GetMaterialCount(MaterialType type){
        return materials[type];
    }
}

public enum MaterialType{
    Wood,
    Brick,
    Wool,
    Wheat,
    Ore,
    Paper,
    Canvas,
    Coin
}
