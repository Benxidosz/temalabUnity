using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RandomBoardGenerator : NetworkBehaviour
{
    public GameObject hexagonPrefab;
    public Texture2D bricksTile;
    public Texture2D oresTile;
    public Texture2D wheetsTile;
    public Texture2D woolsTile;
    public Texture2D woodsTile;

    //TODO: sivatag
    private int _noBricks = 3;
    private int _noOres = 4; //3
    private int _noWheets = 4;
    private int _noWools = 4;
    private int _noWoods = 4;

    private enum TileType
    {
        Bricks,
        Ore,
        Wheat,
        Wool,
        Wood
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

    private readonly Dictionary<TileType, Texture2D> _textures = new Dictionary<TileType, Texture2D>();

    float tileXOffset = 0.9f;
    float tileZOffset = 1.6f;

    private void Start()
    {
        _textures[TileType.Bricks] = bricksTile;
        _textures[TileType.Ore] = oresTile;
        _textures[TileType.Wheat] = wheetsTile;
        _textures[TileType.Wool] = woolsTile;
        _textures[TileType.Wood] = woodsTile;
    }

    public void CreateRandomBoard()
    {
        /*for(int x = 0; x < 5; x++)
        {
            for (int z = 0; z < 3; z++)
            {
                GameObject temp = Instantiate(hexagonPrefab);
                temp.transform.position = new Vector3(x * tileXOffset, 0, z * tileZOffset);
            }
        }*/

        /*for (int j = 0; j < 19; j++)
        {
            GameObject temp = Instantiate(hexagonPrefab);
            //temp.transform.position = new Vector3(j * tileXOffset, 0, j * tileZOffset);
        }*/
        GenerateTilesServerRPC();
    }

    private GameObject CreateHexagon(Vector3 position, TileType tileType)
    {
        var hexagon = Instantiate(hexagonPrefab);
        hexagon.transform.position = new Vector3(position.x * tileXOffset, 0, position.z * tileZOffset);
        hexagon.GetComponent<Renderer>().material.mainTexture = _textures[tileType];
        return hexagon;
    }

    [ServerRpc]
    private void GenerateTilesServerRPC()
    {
        var tileTypes = new TileType[_tileOffsets.Length];
        for (var i = 0; i < _tileOffsets.Length; i++)
        {
            tileTypes[i] = GetRandomTileType();
        }

        SpawnTilesClientRpc(tileTypes);
    }

    [ClientRpc]
    private void SpawnTilesClientRpc(TileType[] tileTypes)
    {
        var hexagonGOs = new List<GameObject>();
        CreateHexagons(hexagonGOs, tileTypes);
    }

    //TODO: algorithm
    private void CreateHexagons(List<GameObject> hexas, TileType[] tileTypes)
    {
        for (var i = 0; i < _tileOffsets.Length; i++)
        {
            hexas.Add(CreateHexagon(_tileOffsets[i], tileTypes[i]));
        }
    }

    private TileType GetRandomTileType()
    {
        var rand = Random.Range(0, 5);
        var tileType = TileType.Bricks;
        var run = true;
        while (run)
        {
            switch (rand)
            {
                case 0:
                    if (_noBricks == 0)
                        break;
                    else
                    {
                        tileType = TileType.Bricks;
                        _noBricks--;
                        run = false;
                        break;
                    }
                case 1:
                    if (_noOres == 0)
                        break;
                    else
                    {
                        tileType = TileType.Ore;
                        _noOres--;
                        run = false;
                        break;
                    }
                case 2:
                    if (_noWheets == 0)
                        break;
                    else
                    {
                        tileType = TileType.Wheat;
                        _noWheets--;
                        run = false;
                        break;
                    }
                case 3:
                    if (_noWoods == 0)
                        break;
                    else
                    {
                        tileType = TileType.Wood;
                        _noWoods--;
                        run = false;
                        break;
                    }
                case 4:
                    if (_noWools == 0)
                        break;
                    else
                    {
                        tileType = TileType.Wool;
                        _noWools--;
                        run = false;
                        break;
                    }
            }

            rand = Random.Range(0, 5);
        }

        return tileType;
    }
}