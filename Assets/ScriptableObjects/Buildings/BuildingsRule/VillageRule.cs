using System;
using Buildings;
using UnityEngine;

[CreateAssetMenu(fileName = "VillageRule", menuName = "ScriptableObjects/Rules/VillageRule", order = 1)]
public class VillageRule : BaseRule{
    public override bool Rule(PlaceHolder holder){
        var re = true;
        var loc = false;
        var cou = 0;
        if (holder.Type != PlaceHolderType.Node) return false;
        holder.Neighbours.ForEach(nb => {
            nb.Neighbours.ForEach(nnb => {
                if (nnb.MainBuilding != null){
                    if (nnb != holder && (nnb.MainBuilding.MyType == BuildingsType.Village ||
                                          nnb.MainBuilding.MyType == BuildingsType.City)){
                        re = false;
                        loc = true;
                    }
                }
                else{
                    if (nb.MainBuilding != null && !loc){
                        if(nb.MainBuilding.MyType == BuildingsType.Road)
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
