using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseRule : ScriptableObject
{
    public virtual bool Rule(PlaceHolder holder){
        return false;
    }
}
