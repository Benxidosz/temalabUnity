using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Buildings
{
    public class BuildingController : MonoBehaviour
    {
        [SerializeField] private RaycastController raycastController;
        [SerializeField] private MaterialController materialController; 
        [SerializeField] private PlayerController myPlayer;

        [FormerlySerializedAs("PlaceHolders")] [SerializeField]
        private List<PlaceHolder> placeHolders;

        [FormerlySerializedAs("FreeVillage")] [SerializeField] private int freeVillage;

        [FormerlySerializedAs("Village")] [Header("Buildings")] [SerializeField]
        private Building village;

        [FormerlySerializedAs("City")] [SerializeField]
        private Building city;

        [FormerlySerializedAs("Road")] [SerializeField]
        private Building road;

        private void Start()
        {
            placeHolders = new List<PlaceHolder>();
            myPlayer = GetComponent<PlayerController>();
            foreach (var o in GameObject.FindGameObjectsWithTag("PlaceHolder"))
            {
                placeHolders.Add(o.GetComponent<PlaceHolder>());
            }
        }

        public void BuildVillage()
        {
            if (raycastController.FocusedObj is null||
                (raycastController.FocusedPlaceHolder.Player != null &&
                 raycastController.FocusedPlaceHolder.Player.Id != myPlayer.Id)) return;
        
            if (freeVillage > 0 && raycastController.FocusedPlaceHolder.Type == PlaceHolderType.Node){
                raycastController.FocusedPlaceHolder.PlaceNew(village, myPlayer);
                raycastController.SetFocusNull();
                freeVillage--;
                return;
            }

            if (!village.MyRule.Rule(raycastController.FocusedPlaceHolder) ||
                !materialController.TryToRemove(village)) return;
        
            raycastController.FocusedPlaceHolder.PlaceNew(village, myPlayer);
            raycastController.SetFocusNull();
        }

        public void BuildCity()
        {
            if (raycastController.FocusedObj is null||
                raycastController.FocusedPlaceHolder.Player != null && 
                raycastController.FocusedPlaceHolder.Player.Id != myPlayer.Id) return;
        
            if (!city.MyRule.Rule(raycastController.FocusedPlaceHolder) ||
                !materialController.TryToRemove(city)) return;
        
            raycastController.FocusedPlaceHolder.PlaceNew(city, myPlayer);
            raycastController.SetFocusNull();
        }

        public void BuildRoad()
        {
            if (raycastController.FocusedObj is null||
                raycastController.FocusedPlaceHolder.Player != null && 
                raycastController.FocusedPlaceHolder.Player.Id != myPlayer.Id) return;
        
            if (!road.MyRule.Rule(raycastController.FocusedPlaceHolder) ||
                !materialController.TryToRemove(road)) return;
        
            raycastController.FocusedPlaceHolder.PlaceNew(road, myPlayer);
            raycastController.SetFocusNull();
        }
    }

    public enum BuildingsType
    {
        Village,
        City,
        Road
    }
}