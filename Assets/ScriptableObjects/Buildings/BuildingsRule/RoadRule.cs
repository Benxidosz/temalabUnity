using UnityEngine;

[CreateAssetMenu(fileName = "RoadRule", menuName = "ScriptableObjects/Rules/RoadRule", order = 3)]
public class RoadRule : BaseRule{
    public override bool Rule(PlaceHolder holder){
        var re = false;
        if (holder.Type != PlaceHolderType.EDGE) return false;
        holder.Neighbours.ForEach(nb => {
            nb.Neighbours.ForEach(nnb => {
                if (nnb.MainBuilding != null){
                    //Kapcsolodik e uthoz
                    if (nnb != holder && nnb.MainBuilding.MyType == BuildingsType.ROAD){
                        re = true;
                    }
                }
                else{
                    if (nb.MainBuilding != null){
                        // szomszedos e varossal
                        if (nb.MainBuilding.MyType == BuildingsType.CITY ||
                            nb.MainBuilding.MyType == BuildingsType.VILLAGE)
                            re = true;
                    }    
                }
            });
        });
        
        return re;
    }
}
