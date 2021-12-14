using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace ScriptableObjects.CardObjects
{
    [CreateAssetMenu(fileName = "newCard", menuName = "ScriptableObjects/MakeCardObject", order = 1)]
    public class CardObject : ScriptableObject, ICloneable {
        public Sprite background;
        public int count;
        [Header("Title")]
        public new string name;
        public int titleSize = 20;
        [TextArea]
        public string description;
        [FormerlySerializedAs("Action")] public UnityEvent<PlayerController> action;
        public object Clone() {
            var clone = CreateInstance<CardObject>();
            clone.background = background;
            clone.count = count;
            clone.description = description; 
            clone.name = name;
            clone.action = action;
            clone.titleSize = titleSize;
            return clone;
        }
    }
}