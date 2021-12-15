using System.Collections.Generic;
using Buildings;
using UnityEngine;

namespace Map
{
    public class TileController : MonoBehaviour{
    
        public MaterialType MyType{ get; set; }
        public int MyNumber{ get; set; }

        [Header("Placeholders")] [SerializeField]
        private List<PlaceHolder> placeHolders = new List<PlaceHolder>();

        public IEnumerable<PlaceHolder> PlaceHolders => placeHolders;

        public void AddPlaceHolders(List<PlaceHolder> placeHolder){
            placeHolders = placeHolder;
        }
    
        public void Harvest(){
            placeHolders.ForEach(item => {
                if (item.Type != PlaceHolderType.Node) return;
                switch (MyType){
                    case MaterialType.Brick:
                    case MaterialType.Wheat:
                        item.Harvest( MyType,MyType);
                        break;
                    case MaterialType.Ore:
                        item.Harvest( MyType,MaterialType.Coin);
                        break;
                    case MaterialType.Wood:
                        item.Harvest( MyType,MaterialType.Paper);
                        break;
                    case MaterialType.Wool:
                        item.Harvest( MyType,MaterialType.Canvas);
                        break;
                    default:
                        return;
                }
            });
        }
    }
}