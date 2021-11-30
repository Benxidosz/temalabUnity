using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBoardGenerator : MonoBehaviour
{
    public GameObject hexagonPrefab;
    public Texture2D bricksTile;
    public Texture2D oresTile;
    public Texture2D wheetsTile;
    public Texture2D woolsTile;
    public Texture2D woodsTile;

    //TODO: sivatag
    int noBricks = 3;
    int noOres = 4;//3
    int noWheets = 4;
    int noWools = 4;
    int noWoods = 4;

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
        int rand = Random.Range(0, 5);
        Texture2D temp;
        temp = bricksTile;
        bool run = true;
        while(run)
        {
            switch (rand)
            {
                case 0:
                    if (noBricks == 0)
                        break;
                    else
                    {
                        temp = bricksTile;
                        noBricks--;
                        run = false;
                        break;
                    }
                case 1:
                    if (noOres == 0)
                        break;
                    else
                    {
                        temp = oresTile;
                        noOres--;
                        run = false;
                        break;
                    }
                case 2:
                    if (noWheets == 0)
                        break;
                    else
                    {
                        temp = wheetsTile;
                        noWheets--;
                        run = false;
                        break;
                    }
                case 3:
                    if (noWoods == 0)
                        break;
                    else
                    {
                        temp = woodsTile;
                        noWoods--;
                        run = false;
                        break;
                    }
                case 4:
                    if (noWools == 0)
                        break;
                    else
                    {
                        temp = woolsTile;
                        noWools--;
                        run = false;
                        break;
                    }
            }

            rand = Random.Range(0, 5);
        }

        return temp;
    }
}
