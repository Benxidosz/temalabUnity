using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class BuildingController : MonoBehaviour{ 
    [SerializeField] private RaycastController raycastController;
    [FormerlySerializedAs("PlaceHolders")] [SerializeField] private List<PlaceHolder> placeHolders;
    [SerializeField] private int FreeVillage = 0;

    [Header("Buildings")] 
    public Building Village;
    public Building City;
    public Building Road;

    private void Start(){
        placeHolders = new List<PlaceHolder>();
        foreach (var o in GameObject.FindGameObjectsWithTag("PlaceHolder")){
            placeHolders.Add(o.GetComponent<PlaceHolder>());
        }
    }

    public void BuildVillage(){
        if (FreeVillage > 0 && raycastController.FocusedPlaceHolder.Type == PlaceHolderType.NODE){
            raycastController.FocusedPlaceHolder.PlaceNew(Village);
            raycastController.SetFocusNull();
            FreeVillage--;
            return;
        }
        
        if (raycastController.FocusedObj is null || !Village.MyRule.Rule(raycastController.FocusedPlaceHolder)) return;
        raycastController.FocusedPlaceHolder.PlaceNew(Village);
        raycastController.SetFocusNull();
    }

    public void BuildCity(){
        if (raycastController.FocusedObj is null || !City.MyRule.Rule(raycastController.FocusedPlaceHolder)) return;
        raycastController.FocusedPlaceHolder.PlaceNew(City);
        raycastController.SetFocusNull();
    }

    public void BuildRoad(){
        if (raycastController.FocusedObj is null || !Road.MyRule.Rule(raycastController.FocusedPlaceHolder)) return;
        raycastController.FocusedPlaceHolder.PlaceNew(Road);
        raycastController.SetFocusNull();
    }
}

public enum BuildingsType{
    VILLAGE, CITY, ROAD
}
