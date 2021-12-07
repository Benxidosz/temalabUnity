using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjects.CardObjects
{
    [CreateAssetMenu(fileName = "newCard", menuName = "ScriptableObjects/MakeCardObject", order = 1)]
    public class CardObject : ScriptableObject
    {
        public Sprite background;
        public int count;
        [Header("Title")] public new string name;
        public int titleSize = 20;
        [TextArea] public string description;
        public UnityEvent<string> action;
    }
}