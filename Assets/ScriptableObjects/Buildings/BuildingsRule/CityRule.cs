using UnityEngine;

[CreateAssetMenu(fileName = "CityRule", menuName = "ScriptableObjects/Rules/CityRule", order = 2)]
public class CityRule : BaseRule{
    public override bool Rule(PlaceHolder holder){
        return holder.MainBuilding.MyType == BuildingsType.VILLAGE;
    }
}