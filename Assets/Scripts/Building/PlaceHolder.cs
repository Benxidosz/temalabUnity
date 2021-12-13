using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceHolder : MonoBehaviour{
    [SerializeField] private List<PlaceHolder> neighbours;
    [SerializeField] private PlaceHolderType type;
    [SerializeField] private GameObject StartingObj;
    
    private List<GameObject> Buildings;

    public PlaceHolderType Type => type;
    public List<PlaceHolder> Neighbours => neighbours;
    public Building MainBuilding{ get; private set; }
    public PlayerController Player{ get; private set; }

    private void Start(){
        Buildings = new List<GameObject>();
        var tmpNew = Instantiate(StartingObj, gameObject.transform.position, Quaternion.identity);
        tmpNew.transform.parent = this.transform;
        Buildings.Add(tmpNew);
    }

    public void PlaceNew(Building prefab, PlayerController player){
        if (Buildings.Count > 1){
            Destroy(Buildings[1]);
            Buildings.RemoveAt(1);
        }
        var tmpNew = Instantiate(prefab.Model, gameObject.transform.position, Quaternion.identity);
        tmpNew.transform.parent = this.transform;
        Buildings.Add(tmpNew);
        Buildings[0].SetActive(false);
        MainBuilding = prefab;
        Player = player;
    }
        
}

public enum PlaceHolderType{
    NODE, EDGE, TILE
}
