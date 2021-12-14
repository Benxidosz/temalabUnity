using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

namespace Buildings
{
    public class PlaceHolder : MonoBehaviour
    {
        [SerializeField] private List<PlaceHolder> neighbours;
        [SerializeField] private PlaceHolderType type;

        [FormerlySerializedAs("StartingObj")] [SerializeField]
        private GameObject startingObj;

        private List<GameObject> _buildings;

        public PlaceHolderType Type{
            get => type;
            set => type = value;
        }

        public List<PlaceHolder> Neighbours => neighbours;
        public Building MainBuilding { get; private set; }
        public PlayerController Player { get; private set; }

        private void Start()
        {
            _buildings = new List<GameObject>();
            var starting = Instantiate(startingObj, gameObject.transform.position, Quaternion.identity);
            starting.transform.parent = transform;
            _buildings.Add(starting);
            
        }

        public void PlaceNew(Building prefab, PlayerController player)
        {
            if (_buildings.Count > 1)
            {
                Destroy(_buildings[1]);
                _buildings.RemoveAt(1);
            }

            var tmpNew = Instantiate(prefab.Model, gameObject.transform.position, Quaternion.identity);
            tmpNew.transform.parent = transform;
            tmpNew.transform.rotation = transform.rotation;
            if (prefab.MyType != BuildingsType.Road){
                tmpNew.transform.Rotate(270f, 0, 0);
                tmpNew.transform.localScale = new Vector3(100, 100, 100);
            }
            else{
                
            }
            //tmpNew.GetComponent<NetworkObject>().Spawn();
            _buildings.Add(tmpNew);
            _buildings[0].SetActive(false);
            MainBuilding = prefab;
            Player = player;
        }

        public void Harvest(MaterialType itemMain, MaterialType itemSecondary){
            Player.MaterialController.Increase(itemMain, 1);
            if (MainBuilding.MyType == BuildingsType.City){
                Player.MaterialController.Increase(itemSecondary, 1);
            }
        }

        public void AddNeighbour(PlaceHolder holder) {
            if (!neighbours.Contains(holder))
                neighbours.Add(holder);
        }
    }

    public enum PlaceHolderType
    {
        Node,
        Edge,
        Tile
    }
}