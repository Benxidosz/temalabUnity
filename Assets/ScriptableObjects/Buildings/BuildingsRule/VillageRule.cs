using UnityEngine;

[CreateAssetMenu(fileName = "VillageRule", menuName = "ScriptableObjects/Rules/VillageRule", order = 1)]
public class VillageRule : BaseRule{
    public override bool Rule(PlaceHolder holder){
        var re = true;
        if (holder.Type != PlaceHolderType.NODE) return false;
        holder.Neighbours.ForEach(nb => {
            nb.Neighbours.ForEach(nnb => {
                if (nnb.MainBuilding != null)
                    if (nnb != holder && (nnb.MainBuilding.MyType == BuildingsType.VILLAGE ||
                                          nnb.MainBuilding.MyType == BuildingsType.CITY)){
                        re = false;
                    }
            });
        });
        
        return re;
    }
}
