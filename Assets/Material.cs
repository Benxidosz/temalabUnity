using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Material : MonoBehaviour{
    [SerializeField] private MaterialType _type;
    [SerializeField] private TextMeshProUGUI tmpText;
    private MaterialController _myController;
    public MaterialType Type => _type;

    private void Start(){
        tmpText = GetComponent<TextMeshProUGUI>();
        _myController = GameObject.FindWithTag("InventoryController").GetComponent<MaterialController>();
        _myController.SetText += setText;
    }

    private void setText(int count, MaterialType type){
        if (type.Equals(_type)) tmpText.text = count.ToString();
    }

}
