using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "newBuilding", menuName = "ScriptableObjects/MakeBuilding", order = 3)]
public class Building : ScriptableObject{
    [SerializeField] private GameObject model;
    [SerializeField] private BaseRule myRule;
    [SerializeField] private BuildingsType myType;
    
    public GameObject Model => model;
    public BaseRule MyRule => myRule;
    public BuildingsType MyType => myType;

    public void ChangeMaterial(UnityEngine.Material material){
        model.GetComponent<MeshRenderer>().material = material;
    }
}