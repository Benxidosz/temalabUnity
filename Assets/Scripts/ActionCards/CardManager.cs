using System.Linq;
using Buildings;
using ScriptableObjects.CardObjects;
using UnityEngine;

namespace ActionCards {
    public class CardManager : MonoBehaviour {
        public static CardManager Instance { get; set; }

        public void Awake() {
            if (Instance == null)
                Instance = this;
            else {
                Destroy(this);
            }
        }

        public void Alkimist(PlayerController player) {
            player.PickDice();
        }

        public void IncreasePoint(PlayerController player) {
            ++player.Points;
        }

        public void AddTestCard(CardObject testCard) {
            GameManager.Instance.Player.AddTestCard(testCard);
        }

        private void Monopoly(MaterialType material, PlayerController playerController) {
            var sum = 0;
            GameManager.Instance.Players.ForEach(otherPlayer => {
                sum += otherPlayer.MaterialController.Decrease(material, 2);
            });
            playerController.MaterialController.Increase(material, sum);
        }

        public void CommonMonopoly(PlayerController player) {
            GameManager.Instance.ShowPickMaterial(
                () => GameManager.Instance.UIs[GameManager.UIKeys.MaterialPicker]
                    .GetComponentInChildren<MaterialPickerUIController>().ShowCommon(),
                material => Monopoly((MaterialType) material, player));
        }

        public void TradingMonopoly(PlayerController player) {
            GameManager.Instance.ShowPickMaterial(
                () => GameManager.Instance.UIs[GameManager.UIKeys.MaterialPicker]
                    .GetComponentInChildren<MaterialPickerUIController>().ShowTrading(),
                material => Monopoly((MaterialType) material, player));
        }

        public void TradingFleet(PlayerController player) {
            GameManager.Instance.ShowPickMaterial(
                () => GameManager.Instance.UIs[GameManager.UIKeys.MaterialPicker]
                    .GetComponentInChildren<MaterialPickerUIController>().ShowAll(), material => {
                    if (material != null)
                        player.SetTemporaryNeed((MaterialType) material, 2);
                });
        }

        public void Mining(PlayerController player) {
            var tiles = GameManager.Instance.GetTiles(MaterialType.Ore);
            var placeHolders = player.BuildingController
                .PlaceHolders.Where(p => p.Type == PlaceHolderType.Node &&
                                         p.MainBuilding != null &&
                                         p.MainBuilding.MyType != BuildingsType.Road);
            int count = 0;
            foreach (var tile in tiles) {
                foreach (var placeHolder in placeHolders) {
                    if (tile.PlaceHolders.Contains(placeHolder)) {
                        ++count;
                        break;
                    }
                }
            }
            player.MaterialController.Increase(MaterialType.Ore, count * 2);
        }
        public void Watering(PlayerController player) {
            var tiles = GameManager.Instance.GetTiles(MaterialType.Wheat);
            var placeHolders = player.BuildingController
                .PlaceHolders.Where(p => p.Type == PlaceHolderType.Node &&
                                         p.MainBuilding != null &&
                                         p.MainBuilding.MyType != BuildingsType.Road);
            int count = 0;
            foreach (var tile in tiles) {
                foreach (var placeHolder in placeHolders) {
                    if (tile.PlaceHolders.Contains(placeHolder)) {
                        ++count;
                        break;
                    }
                }
            }
            player.MaterialController.Increase(MaterialType.Wheat, count * 2);
        }

        public void Builder(PlayerController player) {
            player.UpgradeManager.CostReduction++;
        }

        public void RoadBuilding(PlayerController player) {
            player.BuildingController.FreeRoad += 2;
        }

        public void Medicine(PlayerController player) {
            player.BuildingController.ReducedCity++;
        }

        public void Bishop(PlayerController player) {
            GameManager.Instance.RobberMovable();
        }
    }
}
