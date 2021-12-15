using Buildings;
using UnityEngine;

namespace ScriptableObjects.Buildings.BuildingsRule
{
    [CreateAssetMenu(fileName = "CityRule", menuName = "ScriptableObjects/Rules/CityRule", order = 2)]
    public class CityRule : BaseRule
    {
        public override bool Rule(PlaceHolder holder)
        {
            if (holder.MainBuilding == null ||
                holder.MainBuilding.MyType != BuildingsType.Village ||
                holder.Player.Id != GameManager.Instance.Player.Id) return false;
            return holder.MainBuilding.MyType == BuildingsType.Village;
        }
    }
}