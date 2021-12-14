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
        private GameObject starting;

        public PlaceHolderType Type => type;
        public List<PlaceHolder> Neighbours => neighbours;
        public Building MainBuilding { get; private set; }

        public PlayerController Player { get; private set; }

        private void Start()
        {
            _buildings = new List<GameObject>();
            starting = Instantiate(startingObj, gameObject.transform.position, Quaternion.identity);
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
            tmpNew.GetComponent<NetworkObject>().Spawn();
            _buildings.Add(tmpNew);
            _buildings[0].SetActive(false);
            MainBuilding = prefab;
            Player = player;
        }
    }

    public enum PlaceHolderType
    {
        Node,
        Edge,
        Tile
    }
}