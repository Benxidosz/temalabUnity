using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastController : MonoBehaviour{
    [SerializeField] private UnityEngine.Material focusMaterial;
    [SerializeField] private UnityEngine.Material basicMaterial;
    
    Ray ray;
    RaycastHit hit;
    private Camera _camera;

    public GameObject FocusedObj{ get; private set; }
    public PlaceHolder FocusedPlaceHolder{ get; private set; }

    private void Start(){
        _camera = Camera.main;
    }

    void Update(){
        if (_camera is{ }) ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out hit)) return;
        if(Input.GetMouseButtonDown(0)){
            print(hit.collider.name);
            if (!(FocusedObj is null)) FocusedObj.GetComponent<MeshRenderer>().material = basicMaterial;
            FocusedObj = hit.collider.gameObject;
            FocusedPlaceHolder = hit.collider.gameObject.GetComponentInParent<PlaceHolder>();
            FocusedObj.GetComponent<MeshRenderer>().material = focusMaterial;
        }
    }

    public void SetFocusNull(){
        FocusedObj = null;
        FocusedPlaceHolder = null;
    }
}
