using Buildings;
using UnityEngine;

[CreateAssetMenu(fileName = "CityRule", menuName = "ScriptableObjects/Rules/CityRule", order = 2)]
public class CityRule : BaseRule{
    public override bool Rule(PlaceHolder holder){
        if (holder.MainBuilding == null) return false;
        return holder.MainBuilding.MyType == BuildingsType.Village;
    }
}