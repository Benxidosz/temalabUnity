using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBoardGenerator : MonoBehaviour
{
    /*public Tile brick;
    public Tile ore;
    public Tile wood;
    public Tile wheat;
    public Tile wool;
    public Tile desert;*/

    /*public GameObject hex1;
    public GameObject hex2;
    public GameObject hex3;
    public GameObject hex4;
    public GameObject hex5;
    public GameObject hex6;
    public GameObject hex7;
    public GameObject hex8;
    public GameObject hex9;
    public GameObject hex10;
    public GameObject hex11;
    public GameObject hex12;
    public GameObject hex13;
    public GameObject hex14;
    public GameObject hex15;
    public GameObject hex16;
    public GameObject hex17;
    public GameObject hex18;
    public GameObject hex19;

    private void Start()
    {
        RandomizeMap();
    }

    void RandomizeMap()
    {
        
    }*/


    //Old solution
    public GameObject hexagonPrefab;
    public Tile brick;
    public Tile ore;
    public Tile wood;
    public Tile wheat;
    public Tile wool;
    public Tile desert;

    float tileXOffset = 0.9f;
    float tileZOffset = 1.6f;

    // Start is called before the first frame update
    void Start()
    {
        CreateRandomBoard();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void CreateRandomBoard()
    {
        for (int j = 0; j < 19; j++)
        {
            GameObject temp = Instantiate(hexagonPrefab);
            //temp.transform.position = new Vector3(j * tileXOffset, 0, j * tileZOffset);
        }
        List<GameObject> hexagonGOs = new List<GameObject>();
        CreateHexagons(hexagonGOs);
        SetTileTexture(hexagonGOs);
    }

    //TODO: algorithm
    void CreateHexagons(List<GameObject> hexas)
    {
        GameObject temp;

        //1.
        temp = Instantiate(hexagonPrefab);
        temp.transform.position = new Vector3(0, 0, 0);
        hexas.Add(temp);
        temp = Instantiate(hexagonPrefab);
        temp.transform.position = new Vector3(1 * tileXOffset, 0, 1 * tileZOffset);
        hexas.Add(temp);
        temp = Instantiate(hexagonPrefab);
        temp.transform.position = new Vector3(-1 * tileXOffset, 0, -1 * tileZOffset);
        hexas.Add(temp);
        temp = Instantiate(hexagonPrefab);
        temp.transform.position = new Vector3(2 * tileXOffset, 0, 2 * tileZOffset);
        hexas.Add(temp);
        temp = Instantiate(hexagonPrefab);
        temp.transform.position = new Vector3(-2 * tileXOffset, 0, -2 * tileZOffset);
        hexas.Add(temp);

        //2.
        temp = Instantiate(hexagonPrefab);
        temp.transform.position = new Vector3(-1 * tileXOffset, 0, 1 * tileZOffset);
        hexas.Add(temp);
        temp = Instantiate(hexagonPrefab);
        temp.transform.position = new Vector3(1 * tileXOffset, 0, -1 * tileZOffset);
        hexas.Add(temp);
        temp = Instantiate(hexagonPrefab);
        temp.transform.position = new Vector3(-2 * tileXOffset, 0, 0 * tileZOffset);
        hexas.Add(temp);
        temp = Instantiate(hexagonPrefab);
        temp.transform.position = new Vector3(2 * tileXOffset, 0, 0 * tileZOffset);
        hexas.Add(temp);
        temp = Instantiate(hexagonPrefab);
        temp.transform.position = new Vector3(0 * tileXOffset, 0, 2 * tileZOffset);
        hexas.Add(temp);
        temp = Instantiate(hexagonPrefab);
        temp.transform.position = new Vector3(0 * tileXOffset, 0, -2 * tileZOffset);
        hexas.Add(temp);
        temp = Instantiate(hexagonPrefab);
        temp.transform.position = new Vector3(-3 * tileXOffset, 0, -1 * tileZOffset);
        hexas.Add(temp);
        temp = Instantiate(hexagonPrefab);
        temp.transform.position = new Vector3(3 * tileXOffset, 0, 1 * tileZOffset);
        hexas.Add(temp);

        //3.
        temp = Instantiate(hexagonPrefab);
        temp.transform.position = new Vector3(-2 * tileXOffset, 0, 2 * tileZOffset);
        hexas.Add(temp);
        temp = Instantiate(hexagonPrefab);
        temp.transform.position = new Vector3(2 * tileXOffset, 0, -2 * tileZOffset);
        hexas.Add(temp);
        temp = Instantiate(hexagonPrefab);
        temp.transform.position = new Vector3(4 * tileXOffset, 0, 0 * tileZOffset);
        hexas.Add(temp);
        temp = Instantiate(hexagonPrefab);
        temp.transform.position = new Vector3(-4 * tileXOffset, 0, 0 * tileZOffset);
        hexas.Add(temp);
        temp = Instantiate(hexagonPrefab);
        temp.transform.position = new Vector3(-3 * tileXOffset, 0, 1 * tileZOffset);
        hexas.Add(temp);
        temp = Instantiate(hexagonPrefab);
        temp.transform.position = new Vector3(3 * tileXOffset, 0, -1 * tileZOffset);
        hexas.Add(temp);
    }

    void SetTileTexture(List<GameObject> hexas)
    {
        foreach(GameObject g in hexas)
        {
            g.GetComponent<Renderer>().material.mainTexture = RandomTexture();
        }
    }

    Texture2D RandomTexture()
    {
        int rand = Random.Range(0, 6);
        Texture2D temp = brick.texture;
        bool run = true;
        while(run)
        {
            switch (rand)
            {
                case 0:
                    if (brick.quantity == 0)
                        break;
                    else
                    {
                        temp = brick.texture;
                        brick.quantity--;
                        run = false;
                        break;
                    }
                case 1:
                    if (ore.quantity == 0)
                        break;
                    else
                    {
                        temp = ore.texture;
                        ore.quantity--;
                        run = false;
                        break;
                    }
                case 2:
                    if (wheat.quantity == 0)
                        break;
                    else
                    {
                        temp = wheat.texture;
                        wheat.quantity--;
                        run = false;
                        break;
                    }
                case 3:
                    if (wood.quantity == 0)
                        break;
                    else
                    {
                        temp = wood.texture;
                        wood.quantity--;
                        run = false;
                        break;
                    }
                case 4:
                    if (wool.quantity == 0)
                        break;
                    else
                    {
                        temp = wool.texture;
                        wool.quantity--;
                        run = false;
                        break;
                    }
                case 5:
                    if (desert.quantity == 0)
                        break;
                    else
                    {
                        temp = desert.texture;
                        desert.quantity--;
                        run = false;
                        break;
                    }
            }

            rand = Random.Range(0, 6);
        }

        return temp;
    }
}
