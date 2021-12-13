using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "newBuilding", menuName = "ScriptableObjects/MakeBuilding", order = 3)]
public class Building : ScriptableObject{
    [SerializeField] private GameObject model;
    [SerializeField] private BaseRule myRule;
    [SerializeField] private BuildingsType myType;

    [SerializeField] private List<MaterialType> cost;
    
    public GameObject Model => model;
    public BaseRule MyRule => myRule;
    public BuildingsType MyType => myType;

    public Dictionary<MaterialType, int> MyCost{
        get{
            var tmpDict = new Dictionary<MaterialType, int>();
            foreach (var mat in cost){
                if (tmpDict.ContainsKey(mat)){
                    tmpDict[mat] += 1;
                }
                else{
                    tmpDict.Add(mat, 1);
                }
            }

            return tmpDict;
        }
    }
}