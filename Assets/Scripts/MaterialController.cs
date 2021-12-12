using System.Collections.Generic;
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
            Increase(type, 1);
        }
        
        print("");
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
