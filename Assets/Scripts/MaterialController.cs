using System.Collections.Generic;
using System.Linq;
using Buildings;
using UnityEngine;

public delegate void SetTextDelegate(int count, MaterialType type);
public class MaterialController : MonoBehaviour{
    public event SetTextDelegate SetText;
    
    private Dictionary<MaterialType, int> _materials;

    private void Start(){
        _materials = new Dictionary<MaterialType, int>();
        var tmp = GameObject.FindGameObjectsWithTag("MaterialText");
        foreach (var m in tmp){
            var type = m.GetComponent<Material>().Type;
            _materials.Add(type, 0);
            Increase(type, 10);
        }
    }

    public void Increase(MaterialType type, int deltaCount){
        _materials[type] += deltaCount;
        SetText?.Invoke(_materials[type], type);
    }

    public void Decrease(MaterialType type, int deltaCount){
        _materials[type] -= deltaCount;
        SetText?.Invoke(_materials[type], type);
    }

    public void SetCount(MaterialType type, int count){
        _materials[type] = count;
        SetText?.Invoke(_materials[type], type);
    }

    public int GetMaterialCount(MaterialType type){
        return _materials[type];
    }

    public void UpdatePanel(){
        foreach (var material in _materials){
            SetText?.Invoke(material.Value, material.Key);
        }
    }

    public bool TryToRemove(Building building){
        if (!CheckMaterials(building)) return false;
        foreach (var mat in building.MyCost){
            _materials[mat.Key] -= mat.Value;
            SetText?.Invoke(_materials[mat.Key], mat.Key);
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
    Coin
}
