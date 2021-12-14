using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubmitSystemTrade : MonoBehaviour {
    private Button _button;

    [SerializeField] private MaterialPickerUIController sell;
    [SerializeField] private MaterialPickerUIController buy;
    [SerializeField] private PlayerController player;
    // Start is called before the first frame update
    void Start() {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(() => {
            player.BuyFromSystem(sell.Value, (MaterialType) buy.Value);
        });
    }
}