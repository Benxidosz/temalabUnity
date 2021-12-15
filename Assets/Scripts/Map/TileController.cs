using System;
using System.Collections.Generic;
using Buildings;
using Unity.Netcode;
using UnityEngine;

namespace Map
{
    public class TileController : NetworkBehaviour
    {
        public MaterialType MyType { get; set; }
        public int MyNumber { get; set; }

        public bool Block
        {
            private get { return _blocked.Value; }
            set { _blocked.Value = value; }
        }

        private NetworkVariable<bool> _blocked = new NetworkVariable<bool>(false);

        [Header("Placeholders")] [SerializeField]
        private List<PlaceHolder> placeHolders = new List<PlaceHolder>();

        public void AddPlaceHolders(List<PlaceHolder> placeHolder)
        {
            placeHolders = placeHolder;
        }

        public void Harvest()
        {
            if (Block) return;
            placeHolders.ForEach(item =>
            {
                if (item.Type != PlaceHolderType.Node) return;
                switch (MyType)
                {
                    case MaterialType.Brick:
                    case MaterialType.Wheat:
                        item.Harvest(MyType, MyType);
                        break;
                    case MaterialType.Ore:
                        item.Harvest(MyType, MaterialType.Coin);
                        break;
                    case MaterialType.Wood:
                        item.Harvest(MyType, MaterialType.Paper);
                        break;
                    case MaterialType.Wool:
                        item.Harvest(MyType, MaterialType.Canvas);
                        break;
                    default:
                        return;
                }
            });
        }
    }
}