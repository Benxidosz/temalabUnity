using System;
using System.Collections;
using System.Collections.Generic;
using ActionCards;
using UnityEngine;
using UnityEngine.UI;

public class MaterialPicker : MonoBehaviour {
    [SerializeField] private MaterialType type;
    public MaterialType Value => type;
    
    public event Action<MaterialPicker> ButtonClicked;
    private Button _button;
    public Button Button => _button;

    private Sprite original;
    // Start is called before the first frame update
    void Start() {
        _button = GetComponent<Button>();
        original = _button.image.sprite;
        _button.onClick.AddListener(() => {
            ButtonClicked?.Invoke(this);
        });
    }

    public void Disable() {
        _button.image.sprite = GameManager.Instance.EmptyCard;
        _button.onClick.RemoveAllListeners();
    }
    public void Enable() {
        _button.image.sprite = original;
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(() => {
            ButtonClicked?.Invoke(this);
        });
    }
}