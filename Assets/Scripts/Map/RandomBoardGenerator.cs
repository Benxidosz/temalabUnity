using System;using System.Collections.Generic;using System.Linq;using Buildings;using TMPro.EditorUtilities;using Unity.Netcode;using UnityEngine;namespace Map {    public class RandomBoardGenerator : NetworkBehaviour {        private enum TileType {            Bricks,            Ore,            Wheat,            Wool,            Wood,            Desert,            Ocean        }        private readonly Vector3[] _tileOffsets = new[] {            new Vector3(0, 0, 0),            new Vector3(1, 0, 1),            new Vector3(-1, 0, -1),            new Vector3(2, 0, 2),            new Vector3(-2, 0, -2),            new Vector3(-1, 0, 1),            new Vector3(1, 0, -1),            new Vector3(-2, 0, 0),            new Vector3(2, 0, 0),            new Vector3(0, 0, 2),            new Vector3(0, 0, -2),            new Vector3(-3, 0, -1),            new Vector3(3, 0, 1),            new Vector3(-2, 0, 2),            new Vector3(2, 0, -2),            new Vector3(4, 0, 0),            new Vector3(-4, 0, 0),            new Vector3(-3, 0, 1),            new Vector3(3, 0, -1)        };        private readonly Vector3[] _oceanTileOffsets = new[] {            new Vector3(3, 0, 3),            new Vector3(-3, 0, -3),            new Vector3(-4, 0, -2),            new Vector3(-5, 0, -1),            new Vector3(-6, 0, 0),            new Vector3(-5, 0, 1),            new Vector3(-4, 0, 2),            new Vector3(-3, 0, 3),            new Vector3(-1, 0, 3),            new Vector3(1, 0, 3),            new Vector3(4, 0, 2),            new Vector3(5, 0, 1),            new Vector3(6, 0, 0),            new Vector3(5, 0, -1),            new Vector3(4, 0, -2),            new Vector3(3, 0, -3),            new Vector3(1, 0, -3),            new Vector3(-1, 0, -3)        };        private readonly Dictionary<Vector3, string> _placeHoldersOffset = new Dictionary<Vector3, string>() {            {new Vector3(-0.87f, 0, 0), "E"},            {new Vector3(0.87f, 0, 0), "E"},            {new Vector3(0, 0, 1), "N"},            {new Vector3(0, 0, -1), "N"},            {new Vector3(-0.44f, 0, -0.75f), "E"},            {new Vector3(0.44f, 0, -0.75f), "E"},            {new Vector3(-0.44f, 0, 0.75f), "E"},            {new Vector3(0.44f, 0, 0.75f), "E"},            {new Vector3(-0.87f, 0, -0.5f), "N"},            {new Vector3(-0.87f, 0, 0.5f), "N"},            {new Vector3(0.87f, 0, 0.5f), "N"},            {new Vector3(0.87f, 0, -0.5f), "N"}        };        [SerializeField] private GameObject hexagonPrefab;        [SerializeField] private GameObject diskPrefab;        [SerializeField] private GameObject placeholderPrefab;        [SerializeField] private Tile brick;        [SerializeField] private Tile ore;        [SerializeField] private Tile wood;        [SerializeField] private Tile wheat;        [SerializeField] private Tile wool;        [SerializeField] private Tile desert;        [SerializeField] private Tile ocean;        [SerializeField] private Disk[] numberDisks;        private readonly List<GameObject> _hexagonGameObjects = new List<GameObject>();        private readonly List<GameObject> _disksGameObjects = new List<GameObject>();        private readonly Dictionary<Vector3, GameObject> _placeHolderObjects = new Dictionary<Vector3, GameObject>();        [SerializeField] private Vector2 tileOffset = new Vector2(0.9f, 1.6f);        private readonly Dictionary<TileType, Tile> _tiles = new Dictionary<TileType, Tile>();        private readonly Dictionary<int, Disk> _disks = new Dictionary<int, Disk>();        private void Start() {            _tiles[TileType.Bricks] = brick;            _tiles[TileType.Ore] = ore;            _tiles[TileType.Wheat] = wheat;            _tiles[TileType.Wool] = wool;            _tiles[TileType.Wood] = wood;            _tiles[TileType.Desert] = desert;            _tiles[TileType.Ocean] = ocean;            _disks[2] = numberDisks[0];            _disks[3] = numberDisks[1];            _disks[4] = numberDisks[2];            _disks[5] = numberDisks[3];            _disks[6] = numberDisks[4];            _disks[8] = numberDisks[5];            _disks[9] = numberDisks[6];            _disks[10] = numberDisks[7];            _disks[11] = numberDisks[8];            _disks[12] = numberDisks[9];        }        public void CreateRandomBoard() {            GenerateTilesServerRPC();        }        private GameObject CreateHexagon(Vector3 position, TileType tileType) {            var hexagon = Instantiate(hexagonPrefab);            hexagon.transform.position = new Vector3(position.x * tileOffset.x, 0, position.z * tileOffset.y);            var pos = hexagon.transform.position;            if (tileType != TileType.Ocean) {                var placeHolders = new List<PlaceHolder>();                foreach (var offset in _placeHoldersOffset) {                    var placeHolderPos = new Vector3(pos.x + offset.Key.x, 0.05f + offset.Key.y, pos.z + offset.Key.z);                    // var roundPos = new Vector3((float) (Math.Floor((pos.x + offset.x) * 100) / 100), (float) (Math.Floor((0.1f + offset.y) * 100) / 100),  (float) (Math.Floor((pos.z + offset.z) * 100) / 100));                    PlaceHolder tmpPlaceHolder = null;                    bool has = false;                    foreach (var tmpObj in _placeHolderObjects) {                        if ((tmpObj.Key - placeHolderPos).sqrMagnitude <= 0.01) {                            has = true;                            tmpPlaceHolder = tmpObj.Value.GetComponent<PlaceHolder>();                            break;                        }                    }                    if (!has) {                        var tmp = Instantiate(placeholderPrefab, placeHolderPos, hexagon.transform.rotation);                        tmp.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);                        if (offset.Value.Equals("E")) {                            tmp.GetComponent<PlaceHolder>().Type = PlaceHolderType.Edge;                        } else {                            tmp.GetComponent<PlaceHolder>().Type = PlaceHolderType.Node;                        }                        tmpPlaceHolder = tmp.GetComponent<PlaceHolder>();                        _placeHolderObjects.Add(placeHolderPos, tmp);                    }                    placeHolders.Add(tmpPlaceHolder);                }                for (int i = 0; i < placeHolders.Count; i++) {                    int nei1 = i - 1;                    int nei2 = i + 1;                    if (nei1 < 0)                        nei1 += placeHolders.Count;                    if (nei2 >= placeHolders.Count)                        nei2 -= placeHolders.Count;                    placeHolders[i].AddNeighbour(placeHolders[nei1]);                    placeHolders[i].AddNeighbour(placeHolders[nei2]);                }            }            hexagon.GetComponent<Renderer>().material.mainTexture = _tiles[tileType].Texture;            return hexagon;        }        private GameObject CreateDisk(Vector3 position, Disk disk) {            var numberDisk = Instantiate(diskPrefab);            numberDisk.transform.position = new Vector3(position.x * tileOffset.x, 0.05f, position.z * tileOffset.y);            numberDisk.GetComponent<Renderer>().material.mainTexture = disk.texture;            return numberDisk;        }        [ServerRpc]        private void GenerateTilesServerRPC() {            var types = new List<TileType>();            foreach (var type in (TileType[]) Enum.GetValues(typeof(TileType))) {                if (type != TileType.Ocean) {                    for (var i = 0; i < _tiles[type].Quantity; i++) {                        types.Add(type);                    }                }            }            Utilities.ShuffleList(types);            var numbers = new List<int>();            foreach (var number in numberDisks) {                for (var i = 0; i < number.quantity; i++) {                    numbers.Add(number.value);                }            }            Utilities.ShuffleList(numbers);            SpawnTilesClientRpc(types.ToArray(), numbers.ToArray());        }        [ClientRpc]        private void SpawnTilesClientRpc(TileType[] tileTypes, int[] numbers) {            CreateHexagons(tileTypes, numbers);        }        private void CreateHexagons(TileType[] tileTypes, int[] numbers) {            var j = 0;            for (var i = 0; i < _tileOffsets.Length; i++) {                var hex = CreateHexagon(_tileOffsets[i], tileTypes[i]);                _hexagonGameObjects.Add(hex);                if (tileTypes[i] != TileType.Desert) {                    _disksGameObjects.Add(CreateDisk(_tileOffsets[i], _disks[numbers[j]]));                    j++;                }            }            foreach (var oceanTileOffset in _oceanTileOffsets) {                _hexagonGameObjects.Add(CreateHexagon(oceanTileOffset, TileType.Ocean));            }        }    }}