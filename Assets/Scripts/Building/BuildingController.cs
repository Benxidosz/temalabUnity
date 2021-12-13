using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class BuildingController : MonoBehaviour{ 
    [SerializeField] private RaycastController raycastController; 
    [SerializeField] private MaterialController materialController; 
    [SerializeField] private PlayerController myPlayer;
    [FormerlySerializedAs("PlaceHolders")] [SerializeField] private List<PlaceHolder> placeHolders;
    [SerializeField] private int FreeVillage;

    [Header("Buildings")] 
    public Building Village;
    public Building City;
    public Building Road;

    private void Start(){
        placeHolders = new List<PlaceHolder>();
        myPlayer = GetComponent<PlayerController>();
        materialController = GetComponent<MaterialController>();
        raycastController = GetComponent<RaycastController>();
            
        foreach (var o in GameObject.FindGameObjectsWithTag("PlaceHolder")){
            placeHolders.Add(o.GetComponent<PlaceHolder>());
        }
    }

    public void BuildVillage(){
        if (raycastController.FocusedObj is null||
            (raycastController.FocusedPlaceHolder.Player != null &&
             raycastController.FocusedPlaceHolder.Player.id != myPlayer.id)) return;
        
        if (FreeVillage > 0 && raycastController.FocusedPlaceHolder.Type == PlaceHolderType.NODE){
            raycastController.FocusedPlaceHolder.PlaceNew(Village, myPlayer);
            raycastController.SetFocusNull();
            FreeVillage--;
            return;
        }

        if (!Village.MyRule.Rule(raycastController.FocusedPlaceHolder) ||
            !materialController.TryToRemove(Village)) return;
        
        raycastController.FocusedPlaceHolder.PlaceNew(Village, myPlayer);
        raycastController.SetFocusNull();
    }

    public void BuildCity(){
        if (raycastController.FocusedObj is null||
            raycastController.FocusedPlaceHolder.Player != null && 
            raycastController.FocusedPlaceHolder.Player.id != myPlayer.id) return;
        
        if (!City.MyRule.Rule(raycastController.FocusedPlaceHolder) ||
            !materialController.TryToRemove(City)) return;
        
        raycastController.FocusedPlaceHolder.PlaceNew(City, myPlayer);
        raycastController.SetFocusNull();
    }

    public void BuildRoad(){
        if (raycastController.FocusedObj is null||
            raycastController.FocusedPlaceHolder.Player != null && 
            raycastController.FocusedPlaceHolder.Player.id != myPlayer.id) return;
        
        if (!Road.MyRule.Rule(raycastController.FocusedPlaceHolder) ||
            !materialController.TryToRemove(Road)) return;
        
        raycastController.FocusedPlaceHolder.PlaceNew(Road, myPlayer);
        raycastController.SetFocusNull();
    }

    
}

public enum BuildingsType{
    VILLAGE, CITY, ROAD
}
