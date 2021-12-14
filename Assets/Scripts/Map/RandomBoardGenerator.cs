using System;
using System.Collections.Generic;
using System.Linq;
using Buildings;
using Unity.Netcode;
using UnityEngine;

namespace Map
{
    public class RandomBoardGenerator : NetworkBehaviour
    {
        private enum TileType
        {
            Bricks,
            Ore,
            Wheat,
            Wool,
            Wood,
            Desert,
            Ocean
        }

        private enum PortType
        {
            Brick,
            Ore,
            Wheat,
            Wool,
            Wood,
            Any
        }

        private readonly Vector3[] _tileOffsets = new[]
        {
            new Vector3(0, 0, 0),
            new Vector3(1, 0, 1),
            new Vector3(-1, 0, -1),
            new Vector3(2, 0, 2),
            new Vector3(-2, 0, -2),
            new Vector3(-1, 0, 1),
            new Vector3(1, 0, -1),
            new Vector3(-2, 0, 0),
            new Vector3(2, 0, 0),
            new Vector3(0, 0, 2),
            new Vector3(0, 0, -2),
            new Vector3(-3, 0, -1),
            new Vector3(3, 0, 1),
            new Vector3(-2, 0, 2),
            new Vector3(2, 0, -2),
            new Vector3(4, 0, 0),
            new Vector3(-4, 0, 0),
            new Vector3(-3, 0, 1),
            new Vector3(3, 0, -1)
        };

        private readonly Vector3[] _oceanTileOffsets = new[]
        {
            new Vector3(-3, 0, -3),
            new Vector3(-4, 0, -2),
            new Vector3(-5, 0, -1),
            new Vector3(-6, 0, 0),
            new Vector3(-5, 0, 1),
            new Vector3(-4, 0, 2),
            new Vector3(-3, 0, 3),
            new Vector3(-1, 0, 3),
            new Vector3(1, 0, 3),
            new Vector3(3, 0, 3),
            new Vector3(4, 0, 2),
            new Vector3(5, 0, 1),
            new Vector3(6, 0, 0),
            new Vector3(5, 0, -1),
            new Vector3(4, 0, -2),
            new Vector3(3, 0, -3),
            new Vector3(1, 0, -3),
            new Vector3(-1, 0, -3)
        };

        private readonly Dictionary<Vector3, string> _placeHoldersOffset = new Dictionary<Vector3, string>()
        {
            {new Vector3(-0.87f, 0, 0), "E"},
            {new Vector3(-0.87f, 0, 0.5f), "N"},
            {new Vector3(-0.44f, 0, 0.75f), "E"},
            {new Vector3(0, 0, 1), "N"},
            {new Vector3(0.44f, 0, 0.75f), "E"},
            {new Vector3(0.87f, 0, 0.5f), "N"},
            {new Vector3(0.87f, 0, 0), "E"},
            {new Vector3(0.87f, 0, -0.5f), "N"},
            {new Vector3(0.44f, 0, -0.75f), "E"},
            {new Vector3(0, 0, -1), "N"},
            {new Vector3(-0.44f, 0, -0.75f), "E"},
            {new Vector3(-0.87f, 0, -0.5f), "N"},
        };

        [SerializeField] private GameObject hexagonPrefab;
        [SerializeField] private GameObject diskPrefab;
        [SerializeField] private GameObject portPrefab;
        [SerializeField] private GameObject placeholderPrefab;

        [SerializeField] private Tile brick;
        [SerializeField] private Tile ore;
        [SerializeField] private Tile wood;
        [SerializeField] private Tile wheat;
        [SerializeField] private Tile wool;
        [SerializeField] private Tile desert;
        [SerializeField] private Tile ocean;

        [SerializeField] private Port brickPort;
        [SerializeField] private Port orePort;
        [SerializeField] private Port woodPort;
        [SerializeField] private Port wheatPort;
        [SerializeField] private Port woolPort;
        [SerializeField] private Port anyPort;

        [SerializeField] private GameObject brickPrefab;
        [SerializeField] private GameObject orePrefab;
        [SerializeField] private GameObject woodPrefab;
        [SerializeField] private GameObject wheatPrefab;
        [SerializeField] private GameObject woolPrefab;
        [SerializeField] private GameObject desertPrefab;
        [SerializeField] private GameObject oceanPrefab;

        [SerializeField] private Disk[] numberDisks;

        [SerializeField] private GameObject[] diskObjects;
        
        private readonly List<GameObject> _hexagonGameObjects = new List<GameObject>();
        private readonly List<GameObject> _disksGameObjects = new List<GameObject>();
        private readonly List<GameObject> _portGameObjects = new List<GameObject>();

        private readonly Dictionary<Vector3, GameObject>
            _placeHolderGameObjects = new Dictionary<Vector3, GameObject>();

        [SerializeField] private Vector2 tileOffset = new Vector2(0.9f, 1.6f);

        private readonly Dictionary<TileType, Tile> _tiles = new Dictionary<TileType, Tile>();
        private readonly Dictionary<int, Disk> _disks = new Dictionary<int, Disk>();
        private readonly Dictionary<PortType, Port> _ports = new Dictionary<PortType, Port>();

        private void Start()
        {
            _tiles[TileType.Bricks] = brick;
            _tiles[TileType.Ore] = ore;
            _tiles[TileType.Wheat] = wheat;
            _tiles[TileType.Wool] = wool;
            _tiles[TileType.Wood] = wood;
            _tiles[TileType.Desert] = desert;
            _tiles[TileType.Ocean] = ocean;

            _ports[PortType.Brick] = brickPort;
            _ports[PortType.Ore] = orePort;
            _ports[PortType.Wheat] = wheatPort;
            _ports[PortType.Wool] = woolPort;
            _ports[PortType.Wood] = woodPort;
            _ports[PortType.Any] = anyPort;

            _disks[2] = numberDisks[0];
            _disks[3] = numberDisks[1];
            _disks[4] = numberDisks[2];
            _disks[5] = numberDisks[3];
            _disks[6] = numberDisks[4];
            _disks[8] = numberDisks[5];
            _disks[9] = numberDisks[6];
            _disks[10] = numberDisks[7];
            _disks[11] = numberDisks[8];
            _disks[12] = numberDisks[9];
        }

        public void CreateRandomBoard()
        {
            var types = new List<TileType>();

            foreach (var type in (TileType[]) Enum.GetValues(typeof(TileType)))
            {
                if (type != TileType.Ocean)
                {
                    for (var i = 0; i < _tiles[type].Quantity; i++)
                    {
                        types.Add(type);
                    }
                }
            }

            Utilities.ShuffleList(types);

            var numbers = new List<int>();
            foreach (var number in numberDisks)
            {
                for (var i = 0; i < number.quantity; i++)
                {
                    numbers.Add(number.value);
                }
            }

            Utilities.ShuffleList(numbers);

            var ports = new List<PortType>();
            foreach (var port in (PortType[]) Enum.GetValues(typeof(PortType)))
            {
                for (var i = 0; i < _ports[port].quantity; i++)
                {
                    ports.Add(port);
                }
            }

            Utilities.ShuffleList(ports);

            CreateHexagons(types.ToArray(), numbers.ToArray(), ports.ToArray());
            GameManager.Instance.CurrentPlayer.BuildingController.LoadPlaceHolders();
        }

        private GameObject CreateHexagon(Vector3 position, TileType tileType)
        {
            MaterialType materialType;
            GameObject prefab;
            switch (tileType)
            {
                case TileType.Bricks:
                    materialType = MaterialType.Brick;
                    prefab = brickPrefab;
                    break;
                case TileType.Ore:
                    materialType = MaterialType.Ore;
                    prefab = orePrefab;
                    break;
                case TileType.Wheat:
                    materialType = MaterialType.Wheat;
                    prefab = wheatPrefab;
                    break;
                case TileType.Wool:
                    materialType = MaterialType.Wool;
                    prefab = woolPrefab;
                    break;
                case TileType.Wood:
                    materialType = MaterialType.Wood;
                    prefab = woodPrefab;
                    break;
                case TileType.Ocean:
                    materialType = MaterialType.Default;
                    prefab = oceanPrefab;
                    break;
                default:
                    materialType = MaterialType.Default;
                    prefab = desertPrefab;
                    break;
            }

            var hexagon = Instantiate(prefab);
            hexagon.GetComponent<NetworkObject>().Spawn();
            var controller = hexagon.GetComponent<TileController>();
            controller.MyType = materialType;
            var pos = new Vector3(position.x * tileOffset.x, 0, position.z * tileOffset.y);
            hexagon.transform.position = pos;

            if (tileType != TileType.Ocean)
            {
                var placeHolders = new List<PlaceHolder>();
                foreach (var offset in _placeHoldersOffset)
                {
                    var placeHolderPos = new Vector3(0, 0.05f, 0) + pos + offset.Key;
                    // var roundPos = new Vector3((float) (Math.Floor((pos.x + offset.x) * 100) / 100), (float) (Math.Floor((0.1f + offset.y) * 100) / 100),  (float) (Math.Floor((pos.z + offset.z) * 100) / 100));
                    PlaceHolder tmpPlaceHolder = null;
                    var has = false;
                    foreach (var tmpObj in _placeHolderGameObjects.Where(tmpObj =>
                        (tmpObj.Key - placeHolderPos).sqrMagnitude <= 0.001))
                    {
                        has = true;
                        tmpPlaceHolder = tmpObj.Value.GetComponent<PlaceHolder>();
                        break;
                    }

                    if (!has)
                    {
                        var tmp = Instantiate(placeholderPrefab, placeHolderPos, hexagon.transform.rotation,
                            hexagon.transform);
                        tmp.GetComponent<NetworkObject>().Spawn();
                        tmp.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                        tmp.GetComponent<PlaceHolder>().Type =
                            offset.Value.Equals("E") ? PlaceHolderType.Edge : PlaceHolderType.Node;
                        tmp.transform.rotation = Quaternion.LookRotation(offset.Key);
                        tmpPlaceHolder = tmp.GetComponent<PlaceHolder>();
                        _placeHolderGameObjects.Add(placeHolderPos, tmp);
                    }

                    placeHolders.Add(tmpPlaceHolder);
                }

                controller.AddPlaceHolders(placeHolders);
                for (var i = 0; i < placeHolders.Count; i++)
                {
                    var nei1 = i - 1;
                    var nei2 = i + 1;
                    if (nei1 < 0)
                        nei1 += placeHolders.Count;
                    if (nei2 >= placeHolders.Count)
                        nei2 -= placeHolders.Count;
                    placeHolders[i].AddNeighbour(placeHolders[nei1]);
                    placeHolders[i].AddNeighbour(placeHolders[nei2]);
                }
            }

            return hexagon;
        }

        private GameObject CreateDisk(Vector3 position, Disk disk, TileController tile)
        {
            var id = disk.value;
            if (id > 7)
                id--;
            id -= 2;
            var numberDisk = Instantiate(diskObjects[id]);
            numberDisk.GetComponent<NetworkObject>().Spawn();
            numberDisk.transform.position = new Vector3(position.x * tileOffset.x, 0.05f, position.z * tileOffset.y);
            tile.MyNumber = disk.value;

            return numberDisk;
        }

        private GameObject CreatePort(Vector3 position, PortType portType, int rotation)
        {
            var port = Instantiate(portPrefab);
            port.GetComponent<NetworkObject>().Spawn();
            port.transform.position = new Vector3(position.x * tileOffset.x, 0.075f, position.z * tileOffset.y);
            port.transform.rotation = Quaternion.Euler(-90, 0, rotation);
            port.transform.position += 0.45f * port.transform.up;

            port.GetComponent<Renderer>().material.mainTexture = _ports[portType].texture;
            return null;
        }

        private void CreateHexagons(TileType[] tileTypes, int[] numbers, PortType[] portTypes)
        {
            var j = 0;
            for (var i = 0; i < _tileOffsets.Length; i++)
            {
                var hexagon = CreateHexagon(_tileOffsets[i], tileTypes[i]);
                _hexagonGameObjects.Add(hexagon);
                if (tileTypes[i] != TileType.Desert)
                {
                    _disksGameObjects.Add(CreateDisk(_tileOffsets[i], _disks[numbers[j]],
                        hexagon.GetComponent<TileController>()));
                    j++;
                }
            }

            var rot = -150;
            for (var i = 0; i < _oceanTileOffsets.Length; i++)
            {
                _hexagonGameObjects.Add(CreateHexagon(_oceanTileOffsets[i], TileType.Ocean));
                if (i % 2 == 0)
                {
                    if (i == 12)
                        rot += 60;

                    _portGameObjects.Add(CreatePort(_oceanTileOffsets[i], portTypes[i / 2], rot));

                    if (i % 4 == 0)
                        rot += 60;
                }
            }
        }
    }
}

