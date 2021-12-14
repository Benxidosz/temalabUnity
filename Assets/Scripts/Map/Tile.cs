using UnityEngine;

namespace Map
{
    [CreateAssetMenu(fileName = "New Tile", menuName = "Tile")]
    public class Tile : ScriptableObject
    {
        [SerializeField] private new string name;
        [SerializeField] private Texture2D texture;
        [SerializeField] private int firstMaterial;
        [SerializeField] private int secondMaterial;
        [SerializeField] private int quantity;
        
        public Texture2D Texture => texture;
        public string Name => name;
        public int FirstMaterial => firstMaterial;
        public int SecondMaterial => secondMaterial;
        public int Quantity => quantity;
    }
}
