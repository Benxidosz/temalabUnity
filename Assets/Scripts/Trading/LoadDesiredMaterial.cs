using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadDesiredMaterial : MonoBehaviour
{
    public Sprite brick;
    public Sprite ore;
    public Sprite wheat;
    public Sprite wool;
    public Sprite wood;
    public Sprite coin;
    public Sprite canvas;
    public Sprite paper;

    private MaterialType desiredMaterial;
    private int desiredAmount;
    private readonly Dictionary<MaterialType, Sprite> _materials = new Dictionary<MaterialType, Sprite>();
    private readonly List<Image> offerableMaterials = new List<Image>();

    // Start is called before the first frame update
    void Start()
    {
        desiredMaterial = MaterialType.Brick;
        desiredAmount = 2;

        _materials[MaterialType.Brick] = brick;
        _materials[MaterialType.Ore] = ore;
        _materials[MaterialType.Wheat] = wheat;
        _materials[MaterialType.Wool] = wool;
        _materials[MaterialType.Wood] = wood;
        _materials[MaterialType.Coin] = coin;
        _materials[MaterialType.Canvas] = canvas;
        _materials[MaterialType.Paper] = paper;

        for(var i = 0; i < 7; i++)
        {
            offerableMaterials.Add(GameObject.Find($"Material {i+1}").GetComponent<Image>());
        }

        var it = 0;
        foreach (var m in (MaterialType[]) Enum.GetValues(typeof(MaterialType)))
        {
            if(m == desiredMaterial)
            {
                GameObject.Find("Desired material").GetComponent<Image>().sprite = _materials[m];
            }
            else
            {
                offerableMaterials[it].sprite = _materials[m];
                it++;
            }
        }

        Text txt = GameObject.Find("Amount desired").GetComponent<Text>();
        txt.text = desiredAmount.ToString();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
