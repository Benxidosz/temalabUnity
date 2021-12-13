using System;
using UnityEngine;

[CreateAssetMenu(fileName = "VillageRule", menuName = "ScriptableObjects/Rules/VillageRule", order = 1)]
public class VillageRule : BaseRule{
    public override bool Rule(PlaceHolder holder){
        var re = true;
        var loc = false;
        var cou = 0;
        if (holder.Type != PlaceHolderType.NODE) return false;
        holder.Neighbours.ForEach(nb => {
            nb.Neighbours.ForEach(nnb => {
                if (nnb.MainBuilding != null){
                    if (nnb != holder && (nnb.MainBuilding.MyType == BuildingsType.VILLAGE ||
                                          nnb.MainBuilding.MyType == BuildingsType.CITY)){
                        re = false;
                        loc = true;
                    }
                }
                else{
                    if (nb.MainBuilding != null && !loc){
                        if(nb.MainBuilding.MyType == BuildingsType.ROAD && 
                           nb.Player.id == GameManager.Instance.CurrentPlayer.id)
                            cou++;
                    }
                }
            });
        });
        if (!re)
            return false;
        if (re && cou > 0)
            return true;
        return false;
    }
}
