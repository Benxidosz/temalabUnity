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
            var type = m.GetComponent<Material>().Type;
            materials.Add(type, 0);
            Increase(type, 1);
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

    public void SetCount(MaterialType type, int count){
        materials[type] = count;
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
