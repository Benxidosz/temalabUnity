using UnityEngine;

namespace Buildings
{
    public class RaycastController : MonoBehaviour{
        [SerializeField] private UnityEngine.Material focusMaterial;
        [SerializeField] private UnityEngine.Material basicMaterial;
    
        private Ray _ray;
        private RaycastHit _hit;
        private Camera _camera;

        public GameObject FocusedObj{ get; private set; }
        public PlaceHolder FocusedPlaceHolder{ get; private set; }

        private void Start(){
            _camera = Camera.main;
        }

        private void Update(){
            if (_camera is{ }) _ray = _camera.ScreenPointToRay(Input.mousePosition);
            
            if (!Physics.Raycast(_ray, out _hit)) return;
            
            if(Input.GetMouseButtonDown(0)){
                if (_hit.collider.gameObject.tag.Equals("Tile")){
                    GameManager.Instance.MoveRobber(_hit.collider.gameObject);
                    return;
                }
                
                if (!(FocusedObj is null)) FocusedObj.GetComponent<MeshRenderer>().material = basicMaterial;
                FocusedObj = _hit.collider.gameObject;
                FocusedPlaceHolder = _hit.collider.gameObject.GetComponentInParent<PlaceHolder>();
                FocusedObj.GetComponent<MeshRenderer>().material = focusMaterial;
            }
        }

        public void SetFocusNull(){
            FocusedObj = null;
            FocusedPlaceHolder = null;
        }
    }
}
