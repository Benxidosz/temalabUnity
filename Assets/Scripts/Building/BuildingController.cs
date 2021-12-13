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
        foreach (var o in GameObject.FindGameObjectsWithTag("PlaceHolder")){
            placeHolders.Add(o.GetComponent<PlaceHolder>());
        }
    }

    public void BuildVillage(){
        if (raycastController.FocusedPlaceHolder.Player != null && raycastController.FocusedPlaceHolder.Player.id != myPlayer.id) return;
        if (FreeVillage > 0 && raycastController.FocusedPlaceHolder.Type == PlaceHolderType.NODE){
            raycastController.FocusedPlaceHolder.PlaceNew(Village, myPlayer);
            raycastController.SetFocusNull();
            FreeVillage--;
            return;
        }

        if (raycastController.FocusedObj is null ||
            !Village.MyRule.Rule(raycastController.FocusedPlaceHolder) ||
            !materialController.TryToRemove(Village)) return;
        
        raycastController.FocusedPlaceHolder.PlaceNew(Village, myPlayer);
        raycastController.SetFocusNull();
    }

    public void BuildCity(){
        if (raycastController.FocusedPlaceHolder.Player != null && raycastController.FocusedPlaceHolder.Player.id != myPlayer.id) return;
        if (raycastController.FocusedObj is null || 
            !City.MyRule.Rule(raycastController.FocusedPlaceHolder) ||
            !materialController.TryToRemove(City)) return;
        
        raycastController.FocusedPlaceHolder.PlaceNew(City, myPlayer);
        raycastController.SetFocusNull();
    }

    public void BuildRoad(){
        if (raycastController.FocusedPlaceHolder.Player != null && raycastController.FocusedPlaceHolder.Player.id != myPlayer.id) return;
        if (raycastController.FocusedObj is null || 
            !Road.MyRule.Rule(raycastController.FocusedPlaceHolder) ||
            !materialController.TryToRemove(Road)) return;
        
        raycastController.FocusedPlaceHolder.PlaceNew(Road, myPlayer);
        raycastController.SetFocusNull();
    }

    
}

public enum BuildingsType{
    VILLAGE, CITY, ROAD
}
