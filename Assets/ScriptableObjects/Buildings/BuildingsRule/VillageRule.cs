using Buildings;
using UnityEngine;

namespace ScriptableObjects.Buildings.BuildingsRule
{
    [CreateAssetMenu(fileName = "VillageRule", menuName = "ScriptableObjects/Rules/VillageRule", order = 1)]
    public class VillageRule : BaseRule
    {
        public override bool Rule(PlaceHolder holder)
        {
            var located = false;
            var count = 0;
            if (holder.Type != PlaceHolderType.Node) return false;

            holder.Neighbours.ForEach(neighbour =>
            {
                neighbour.Neighbours.ForEach(other =>
                {
                    if (other.MainBuilding != null)
                    {
                        if (other != holder && (other.MainBuilding.MyType == BuildingsType.Village ||
                                                other.MainBuilding.MyType == BuildingsType.City))
                        {
                            located = true;
                        }
                    }
                    else
                    {
                        if (neighbour.MainBuilding != null && !located)
                        {
                            if (neighbour.MainBuilding.MyType == BuildingsType.Road &&
                                neighbour.Player.Id == GameManager.Instance.Player.Id)
                                count++;
                        }
                    }
                });
            });
            return !located && count > 0;
        }
    }
}