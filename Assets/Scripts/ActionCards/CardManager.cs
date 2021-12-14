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
            GameManager.Instance.CurrentPlayer.AddTestCard(testCard);
        }

        private void Monopoly(MaterialType material, PlayerController playerController) {
            int sum = 0;
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
                    .GetComponentInChildren<MaterialPickerUIController>().ShowAll(), material => { print(material); });
        }
    }
}
