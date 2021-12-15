using System;
using System.Collections;
using System.Collections.Generic;
using Map;
using UnityEngine;

public class Robber : MonoBehaviour
{
   public Vector3 position{ get; set; }
   public TileController tile{ get; set; }

   private void Start(){
      position = transform.position;
   }

   public void ChangeTile(GameObject newTile){
      transform.position = newTile.transform.position;
      if (tile != null){
         tile.GetComponent<TileController>().Block = false;
      }
      var ctr =  newTile.GetComponent<TileController>();
      ctr.Block = true;
      tile = ctr;
      position = transform.position;
   }
}
