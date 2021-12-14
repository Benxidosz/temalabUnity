using Buildings;
using UnityEngine;

namespace ScriptableObjects.Buildings.BuildingsRule
{
    [CreateAssetMenu(fileName = "RoadRule", menuName = "ScriptableObjects/Rules/RoadRule", order = 3)]
    public class RoadRule : BaseRule
    {
        public override bool Rule(PlaceHolder holder)
        {
            var result = false;
            if (holder.Type != PlaceHolderType.Edge) return false;
            holder.Neighbours.ForEach(neighbour =>
            {
                neighbour.Neighbours.ForEach(other =>
                {
                    if (other.MainBuilding != null)
                    {
                        // Kapcsolodik e uthoz
                        if (other != holder &&
                            other.MainBuilding.MyType == BuildingsType.Road &&
                            other.Player.Id == GameManager.Instance.CurrentPlayer.Id)
                        {
                            result = true;
                        }
                    }
                    else
                    {
                        if (neighbour.MainBuilding != null)
                        {
                            // Szomszedos e varossal
                            if ((neighbour.MainBuilding.MyType == BuildingsType.City ||
                                 neighbour.MainBuilding.MyType == BuildingsType.Village) &&
                                neighbour.Player.Id == GameManager.Instance.CurrentPlayer.Id)
                                result = true;
                        }
                    }
                });
            });

            return result;
        }
    }
}