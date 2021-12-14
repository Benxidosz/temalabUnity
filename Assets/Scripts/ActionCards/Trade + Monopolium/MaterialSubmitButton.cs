using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialSubmitButton : MonoBehaviour {
    private Button _button;
    [SerializeField] private MaterialPickerUIController materialPickerUIController;

    public Action<MaterialType?> OnClick {
        set {
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(() => {
                materialPickerUIController.Canvas.enabled = false;
                value.Invoke(materialPickerUIController.Value);
            });
        }
    }
    // Start is called before the first frame update
    void Start() {
        _button = GetComponent<Button>();
    }

}