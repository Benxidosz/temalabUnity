using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Buildings
{
    public class BuildingController : MonoBehaviour
    {
        [SerializeField] private RaycastController raycastController;

        [FormerlySerializedAs("PlaceHolders")] [SerializeField]
        private List<PlaceHolder> placeHolders;

        [FormerlySerializedAs("FreeVillage")] [SerializeField] private int freeVillage = 0;

        [FormerlySerializedAs("Village")] [Header("Buildings")] [SerializeField]
        private Building village;

        [FormerlySerializedAs("City")] [SerializeField]
        private Building city;

        [FormerlySerializedAs("Road")] [SerializeField]
        private Building road;

        private void Start()
        {
            placeHolders = new List<PlaceHolder>();
            foreach (var o in GameObject.FindGameObjectsWithTag("PlaceHolder"))
            {
                placeHolders.Add(o.GetComponent<PlaceHolder>());
            }
        }

        public void BuildVillage()
        {
            if (freeVillage > 0 && raycastController.FocusedPlaceHolder.Type == PlaceHolderType.Node)
            {
                raycastController.FocusedPlaceHolder.PlaceNew(village);
                raycastController.SetFocusNull();
                freeVillage--;
                return;
            }

            if (raycastController.FocusedObj is null || !village.MyRule.Rule(raycastController.FocusedPlaceHolder)) return;
            raycastController.FocusedPlaceHolder.PlaceNew(village);
            raycastController.SetFocusNull();
        }

        public void BuildCity()
        {
            if (raycastController.FocusedObj is null || !city.MyRule.Rule(raycastController.FocusedPlaceHolder)) return;
            raycastController.FocusedPlaceHolder.PlaceNew(city);
            raycastController.SetFocusNull();
        }

        public void BuildRoad()
        {
            if (raycastController.FocusedObj is null || !road.MyRule.Rule(raycastController.FocusedPlaceHolder)) return;
            raycastController.FocusedPlaceHolder.PlaceNew(road);
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