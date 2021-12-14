using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemTradeController : MonoBehaviour {
    [SerializeField] private MaterialPickerUIController sell;

    private MaterialPicker[] sells;

    public IEnumerable<MaterialPicker> Sells => sells;
    // Start is called before the first frame update
    void Start() {
        sells = sell.GetComponentsInChildren<MaterialPicker>();
    }
}