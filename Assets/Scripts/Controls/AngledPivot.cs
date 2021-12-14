using UnityEngine;

namespace Controls
{
    public class AngledPivot : MonoBehaviour
    {
        public Vector3 offset = new Vector3(0, 4, -6);
        public float angleSteps = 90;
        public float pivotSpeed = 0.1f;
        public Vector3 center = Vector3.zero;
        public float minZoom = 0.2f;
        public float maxZoom = 1;
        public float zoomStep = 0.2f;
        public float zoomSpeed = 0.7f;

        private bool _leftPivot;
        private bool _rightPivot;
        private float _scroll;

        private float _targetYaw;
        private float _yaw;
        private float _zoom;
        private float _targetZoom;

        private Vector3 _lastMousePosition = Vector3.zero;

        private Camera _camera;

        private void Awake()
        {
            _targetZoom = _zoom = (Mathf.Sqrt(minZoom) + Mathf.Sqrt(maxZoom)) / 2;

            _camera = GetComponent<Camera>();
        }

        private void Update()
        {
            _leftPivot = _leftPivot || Input.GetKeyDown(KeyCode.Q);
            _rightPivot = _rightPivot || Input.GetKeyDown(KeyCode.E);
            _scroll += Input.GetAxis("Mouse ScrollWheel");
        }

        private void FixedUpdate()
        {
            _targetZoom = Mathf.Clamp(
                _targetZoom + _scroll * zoomStep,
                Mathf.Sqrt(minZoom),
                Mathf.Sqrt(maxZoom)
            );
            _scroll = 0;
            if (_leftPivot)
            {
                _targetYaw += angleSteps;
                _leftPivot = false;
            }

            if (_rightPivot)
            {
                _targetYaw -= angleSteps;
                _rightPivot = false;
            }

            // Zoom
            _zoom = Mathf.Pow(Mathf.Lerp(Mathf.Sqrt(_zoom), Mathf.Sqrt(_targetZoom), zoomSpeed), 2);
            _zoom = Mathf.Clamp(_zoom, Mathf.Sqrt(minZoom), Mathf.Sqrt(maxZoom));
            var zoomedOffset = offset / (_zoom * _zoom);

            // Pivot
            _yaw = Mathf.Lerp(_yaw, _targetYaw, pivotSpeed);
            var rot = Quaternion.Euler(0, _yaw, 0);
            var off = rot * zoomedOffset;
            var lookAt = Quaternion.LookRotation(-off, Vector3.up);

            var cameraTransform = transform;
            var cameraPos = center + off;
            cameraTransform.rotation = lookAt;

            // Drag 
            var mouse = Input.mousePosition;
            if (Input.GetMouseButtonDown(1))
            {
                _lastMousePosition = mouse;
            }

            if (Input.GetMouseButton(1))
            {
                var lastRay = _camera.ScreenPointToRay(_lastMousePosition);
                var currentRay = _camera.ScreenPointToRay(mouse);

                var dist1 = lastRay.origin.y / lastRay.direction.y;
                var dist2 = currentRay.origin.y / currentRay.direction.y;

                var lastGroundPos = cameraPos + lastRay.direction * dist1;
                var currentGroundPos = cameraPos + currentRay.direction * dist2;

                center += currentGroundPos - lastGroundPos;
            }

            _lastMousePosition = mouse;

            cameraTransform.position = cameraPos;
        }
    }
}