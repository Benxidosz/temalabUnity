using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class Material : MonoBehaviour{
    [FormerlySerializedAs("_type")] [SerializeField] private MaterialType type;
    [SerializeField] private TextMeshProUGUI tmpText;
    private MaterialController _myController;
    public MaterialType Type => type;

    private void Start(){
        tmpText = GetComponent<TextMeshProUGUI>();
        _myController = GameObject.FindWithTag("InventoryController").GetComponent<MaterialController>();
        _myController.SetText += SetText;
        foreach (var player in GameManager.Instance.Players){
            player.MaterialController.SetText += SetText;
        }
    }

    private void SetText(int count, MaterialType materialType){
        if (materialType.Equals(type)) tmpText.text = count.ToString();
    }

}
