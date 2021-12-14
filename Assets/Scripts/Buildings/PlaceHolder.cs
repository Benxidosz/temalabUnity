using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

namespace Buildings
{
    public class PlaceHolder : NetworkBehaviour
    {
        [SerializeField] private List<PlaceHolder> neighbours;
        [SerializeField] private PlaceHolderType type;

        [FormerlySerializedAs("StartingObj")] [SerializeField]
        private GameObject startingObj;

        [SerializeField] private GameObject settlement;
        [SerializeField] private GameObject city;
        [SerializeField] private GameObject road;

        private List<GameObject> _buildings;

        public PlaceHolderType Type
        {
            get => type;
            set => type = value;
        }

        public List<PlaceHolder> Neighbours => neighbours;
        public Building MainBuilding { get; private set; }
        public PlayerController Player { get; private set; }

        private void Start()
        {
            if (IsHost)
            {
                _buildings = new List<GameObject>();
                var starting = Instantiate(startingObj, gameObject.transform.position, Quaternion.identity);
                starting.GetComponent<NetworkObject>().Spawn();
                starting.transform.parent = transform;
                _buildings.Add(starting);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void PlaceNewBuildingServerRPC(BuildingsType buildingsType, Vector3 position)
        {
            if (_buildings.Count > 1)
            {
                Destroy(_buildings[1]);
                _buildings.RemoveAt(1);
            }
            
            GameObject prefab;
            switch (buildingsType)
            {
                case BuildingsType.City:
                    prefab = city;
                    break;
                case BuildingsType.Road:
                    prefab = road;
                    break;
                default:
                    prefab = settlement;
                    break;
            }
            var tmpNew = Instantiate(prefab, position, Quaternion.identity);
            tmpNew.transform.rotation = transform.rotation;
            tmpNew.GetComponent<NetworkObject>().Spawn();
            tmpNew.transform.parent = transform;
            if (buildingsType != BuildingsType.Road)
            {
                tmpNew.transform.Rotate(270f, 0, 0);
            }
            
            _buildings.Add(tmpNew);
            _buildings[0].SetActive(false);
        }
        
        public void PlaceNew(Building prefab, PlayerController player)
        {
            PlaceNewBuildingServerRPC(prefab.MyType, gameObject.transform.position);
            
            MainBuilding = prefab;
            Player = player;
        }

        public void Harvest(MaterialType itemMain, MaterialType itemSecondary)
        {
            Player.MaterialController.Increase(itemMain, 1);
            if (MainBuilding.MyType == BuildingsType.City)
            {
                Player.MaterialController.Increase(itemSecondary, 1);
            }
        }

        public void AddNeighbour(PlaceHolder holder)
        {
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