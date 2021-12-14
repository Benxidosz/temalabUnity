using System.Collections.Generic;
using System.Linq;
using Buildings;
using UnityEngine;

public delegate void SetTextDelegate(int count, MaterialType type);
public class MaterialController : MonoBehaviour{
    public event SetTextDelegate SetText;
    
    private Dictionary<MaterialType, int> materials;

    public static readonly MaterialType[] MaterialTypes = new[] {
        MaterialType.Brick,
        MaterialType.Canvas,
        MaterialType.Coin,
        MaterialType.Ore,
        MaterialType.Wheat,
        MaterialType.Wood,
        MaterialType.Wool,
        MaterialType.Paper
    };

    private void Start(){
        materials = new Dictionary<MaterialType, int>();
        var tmp = GameObject.FindGameObjectsWithTag("MaterialText");
        foreach (var m in MaterialTypes){
            materials.Add(m, 0);
            Increase(m, 3);
        }
        Increase(MaterialType.Brick, 2);
        Increase(MaterialType.Coin, 2);
    }

    public void Increase(MaterialType type, int deltaCount){
        materials[type] += deltaCount;
        SetText?.Invoke(materials[type], type);
    }

    public int Decrease(MaterialType type, int deltaCount) {
        int tmpCounter = materials[type];
        if (materials[type] - deltaCount >= 0) {
            materials[type] -= deltaCount;
            SetText?.Invoke(materials[type], type);
        }
        return tmpCounter - materials[type];
    }

    public void SetCount(MaterialType type, int count){
        materials[type] = count;
        SetText?.Invoke(materials[type], type);
    }

    public int GetMaterialCount(MaterialType type){
        return materials[type];
    }

    public void UpdatePanel(){
        foreach (var material in materials){
            SetText?.Invoke(material.Value, material.Key);
        }
    }

    public bool TryToRemove(Building building){
        if (!CheckMaterials(building)) return false;
        foreach (var mat in building.MyCost){
            materials[mat.Key] -= mat.Value;
            SetText?.Invoke(materials[mat.Key], mat.Key);
        }

        return true;
    }
    private bool CheckMaterials(Building building){
        return building.MyCost.All(mat => GetMaterialCount(mat.Key) >= mat.Value);
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
    Coin,
    Default
}
