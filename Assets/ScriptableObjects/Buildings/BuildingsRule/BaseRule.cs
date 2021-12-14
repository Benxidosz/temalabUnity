using Buildings;
using UnityEngine;

namespace ScriptableObjects.Buildings.BuildingsRule
{
    public class BaseRule : ScriptableObject
    {
        public virtual bool Rule(PlaceHolder holder)
        {
            return false;
        }
    }
}