using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RandomBoardGenerator : NetworkBehaviour
{
    public GameObject hexagonPrefab;
    public GameObject diskPrefab;
    public GameObject portPrefab;

    public Tile brick;
    public Tile ore;
    public Tile wood;
    public Tile wheat;
    public Tile wool;
    public Tile desert;
    public Tile ocean;

    public Port brickPort;
    public Port orePort;
    public Port woodPort;
    public Port wheatPort;
    public Port woolPort;
    public Port anyPort;

    public Disk[] numberDisks;

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

    private readonly Dictionary<TileType, Tile> _tiles = new Dictionary<TileType, Tile>();
    private readonly Dictionary<int, Disk> _disks = new Dictionary<int, Disk>();
    private readonly Dictionary<PortType, Port> _ports = new Dictionary<PortType, Port>();

    [SerializeField] private Vector2 tileOffset = new Vector2(0.9f, 1.6f);

    private void Start()
    {
        _tiles[TileType.Bricks] = brick;
        _tiles[TileType.Ore] = ore;
        _tiles[TileType.Wheat] = wheat;
        _tiles[TileType.Wool] = wool;
        _tiles[TileType.Wood] = wood;
        _tiles[TileType.Desert] = desert;
        _tiles[TileType.Ocean] = ocean;

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

        _ports[PortType.Brick] = brickPort;
        _ports[PortType.Ore] = orePort;
        _ports[PortType.Wheat] = wheatPort;
        _ports[PortType.Wool] = woolPort;
        _ports[PortType.Wood] = woodPort;
        _ports[PortType.Any] = anyPort;
    }

    public void CreateRandomBoard()
    {
        GenerateTilesServerRPC();
    }

    private GameObject CreateHexagon(Vector3 position, TileType tileType)
    {
        var hexagon = Instantiate(hexagonPrefab);
        hexagon.transform.position = new Vector3(position.x * tileOffset.x, 0, position.z * tileOffset.y);
        hexagon.GetComponent<Renderer>().material.mainTexture = _tiles[tileType].texture;
        return hexagon;
    }

    private GameObject CreateDisk(Vector3 position, Disk disk)
    {
        var numberDisk = Instantiate(diskPrefab);
        numberDisk.transform.position = new Vector3(position.x * tileOffset.x, 0.05f, position.z * tileOffset.y);
        numberDisk.GetComponent<Renderer>().material.mainTexture = disk.texture;
        return numberDisk;
    }

    private GameObject CreatePort(Vector3 position, PortType portType, int rotation)
    {
        var port = Instantiate(portPrefab);
        port.transform.position = new Vector3(position.x * tileOffset.x, 0.05f, position.z * tileOffset.y);
        port.transform.rotation = Quaternion.Euler(-90, 0, rotation);
        port.GetComponent<Renderer>().material.mainTexture = _ports[portType].texture;
        return null;
    }

    [ServerRpc]
    private void GenerateTilesServerRPC()
    {
        var types = new List<TileType>();

        foreach (var type in (TileType[]) Enum.GetValues(typeof(TileType)))
        {
            if(type != TileType.Ocean) 
            {
                for (var i = 0; i < _tiles[type].quantity; i++)
                {
                    types.Add(type);
                }
            }
            
        }
        Utilities.ShuffleList(types);

        var ports = new List<PortType>();
        foreach (var port in (PortType[])Enum.GetValues(typeof(PortType)))
        {
            for (var i = 0; i < _ports[port].quantity; i++)
            {
                ports.Add(port);
            }
        }
        Utilities.ShuffleList(ports);

        SpawnTilesClientRpc(types.ToArray(), ports.ToArray());
    }

    [ClientRpc]
    private void SpawnTilesClientRpc(TileType[] tileTypes, PortType[] portTypes)
    {
        var hexagonGOs = new List<GameObject>();
        var diskGOs = new List<GameObject>();
        var portGOs = new List<GameObject>();
        CreateHexagons(hexagonGOs, tileTypes, diskGOs, portGOs, portTypes);
    }

    private void CreateHexagons(List<GameObject> hexagons, TileType[] tileTypes, List<GameObject> disks, List<GameObject> ports, PortType[] portTypes)
    {

        var numbers = new List<int>();
        foreach (Disk number in numberDisks)
        {
            for (var i = 0; i < number.quantity; i++)
            {
                numbers.Add(number.value);
            }
        }
        Utilities.ShuffleList(numbers);

        var j = 0;
        for (var i = 0; i < _tileOffsets.Length; i++)
        {
            hexagons.Add(CreateHexagon(_tileOffsets[i], tileTypes[i]));
            if(tileTypes[i] != TileType.Desert)
            {
                disks.Add(CreateDisk(_tileOffsets[i], _disks[numbers[j]]));
                j++;
            }

        }

        var rot = -150;
        for (var i = 0; i < _oceanTileOffsets.Length; i++)
        {
            hexagons.Add(CreateHexagon(_oceanTileOffsets[i], TileType.Ocean));
            if (i % 2 == 0)
            {
                if (i == 12)
                    rot += 60;

                ports.Add(CreatePort(_oceanTileOffsets[i], portTypes[i/2], rot));

                if (i % 4 == 0)
                    rot += 60;
            }
        }
    }
}