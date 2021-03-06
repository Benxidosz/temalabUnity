using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

namespace Buildings
{
    public class BuildingController : NetworkBehaviour
    {
        [SerializeField] private RaycastController raycastController;
        [SerializeField] private MaterialController materialController; 
        [SerializeField] private PlayerController myPlayer;

        [FormerlySerializedAs("PlaceHolders")] [SerializeField]
        private List<PlaceHolder> placeHolders;

        public IEnumerable<PlaceHolder> PlaceHolders => placeHolders;

        [FormerlySerializedAs("FreeVillage")] [SerializeField] private int freeVillage;

        [FormerlySerializedAs("Village")] [Header("Buildings")] [SerializeField]
        private Building village;

        [FormerlySerializedAs("City")] [SerializeField]
        private Building city;

        [FormerlySerializedAs("Road")] [SerializeField]
        private Building road;

        [SerializeField] private Building cheapCity;

        public int FreeRoad { get; set; }
        public int ReducedCity { get; set; }
        
        private void Start(){
            materialController = GetComponent<MaterialController>();
            placeHolders = new List<PlaceHolder>();
            myPlayer = GetComponent<PlayerController>();
            foreach (var o in GameObject.FindGameObjectsWithTag("PlaceHolder"))
            {
                placeHolders.Add(o.GetComponent<PlaceHolder>());
            }
        }

        public void BuildVillage()
        {
            Debug.Log($"{raycastController.FocusedObj} {raycastController.FocusedPlaceHolder.Player}");
            if (raycastController.FocusedObj is null ||
                (raycastController.FocusedPlaceHolder.Player != null &&
                 raycastController.FocusedPlaceHolder.Player.Id != myPlayer.Id)) return;
        
            if (freeVillage > 0 && raycastController.FocusedPlaceHolder.Type == PlaceHolderType.Node){
                raycastController.FocusedPlaceHolder.PlaceNew(village, myPlayer);
                raycastController.SetFocusNull();
                freeVillage--;
                myPlayer.Points++;
                return;
            }

            if (!village.MyRule.Rule(raycastController.FocusedPlaceHolder) ||
                !materialController.TryToRemove(village)) return;
        
            raycastController.FocusedPlaceHolder.PlaceNew(village, myPlayer);
            myPlayer.Points++;
            raycastController.SetFocusNull();
        }

        public void BuildCity()
        {
            if (raycastController.FocusedObj is null||
                raycastController.FocusedPlaceHolder.Player != null && 
                raycastController.FocusedPlaceHolder.Player.Id != myPlayer.Id) return;
        
            if (!city.MyRule.Rule(raycastController.FocusedPlaceHolder) ||
                ReducedCity == 0 && !materialController.TryToRemove(city)) return;
            
            if (ReducedCity > 0 && !materialController.TryToRemove(cheapCity)) return;

            if (ReducedCity > 0)
                ReducedCity--;

            raycastController.FocusedPlaceHolder.PlaceNew(city, myPlayer);
            myPlayer.Points++;
            raycastController.SetFocusNull();
        }

        public void BuildRoad()
        {
            if (raycastController.FocusedObj is null||
                raycastController.FocusedPlaceHolder.Player != null && 
                raycastController.FocusedPlaceHolder.Player.Id != myPlayer.Id) return;

            if (!road.MyRule.Rule(raycastController.FocusedPlaceHolder) ||
                FreeRoad == 0 && !materialController.TryToRemove(road)) return;

            if (FreeRoad > 0)
                --FreeRoad;
        
            raycastController.FocusedPlaceHolder.PlaceNew(road, myPlayer);
            raycastController.SetFocusNull();
        }

        public void LoadPlaceHolders(){
            if (IsHost)
            {
                foreach (var o in GameObject.FindGameObjectsWithTag("PlaceHolder"))
                {
                    placeHolders.Add(o.GetComponent<PlaceHolder>());
                }
            }
        }
        
    }

    public enum BuildingsType
    {
        Village,
        City,
        Road
    }
}