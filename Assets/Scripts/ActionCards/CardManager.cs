using UnityEngine;

namespace ActionCards
{
    public class CardManager : MonoBehaviour {
        public static CardManager Instance { get; set; }

        public void Awake() {
            if (Instance == null)
                Instance = this;
            else {
                Destroy(this);
            }
        }

        public void TestAction(PlayerController player) {
            Debug.Log(player);
        }

        public void TestSokAction(PlayerController player) {
            Debug.Log(player);
        }
        public void Uj(PlayerController player) {
            Debug.Log(player);
        }
    }
}
