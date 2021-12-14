using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialPicker : MonoBehaviour {
    [SerializeField] private MaterialType type;
    public MaterialType Value => type;
    
    public event Action<MaterialPicker> ButtonClicked;
    private Button _button;
    public Button Button => _button;
    // Start is called before the first frame update
    void Start() {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(() => {
            ButtonClicked?.Invoke(this);
        });
    }
}