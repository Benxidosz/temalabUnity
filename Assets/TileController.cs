using System;
using System.Collections;
using System.Collections.Generic;
using Buildings;
using UnityEngine;

public class TileController : MonoBehaviour{
    
    public MaterialType myType;
    public int myNumber;

    [Header("Placeholders")] [SerializeField]
    private List<PlaceHolder> placeHolders = new List<PlaceHolder>();

    public void AddPlaceHolder(PlaceHolder placeHolder){
        placeHolders.Add(placeHolder);
    }
    
    public void Harvest(){
        placeHolders.ForEach(item => {
            if (item.Type != PlaceHolderType.Node) return;
            switch (myType){
                case MaterialType.Brick:
                case MaterialType.Wheat:
                    item.Harvest( myType,myType);
                    break;
                case MaterialType.Ore:
                    item.Harvest( myType,MaterialType.Coin);
                    break;
                case MaterialType.Wood:
                    item.Harvest( myType,MaterialType.Paper);
                    break;
                case MaterialType.Wool:
                    item.Harvest( myType,MaterialType.Canvas);
                    break;
                default:
                    return;
            }
        });
    }
}
