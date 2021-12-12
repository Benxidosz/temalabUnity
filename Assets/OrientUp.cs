using UnityEngine;

public class OrientUp : MonoBehaviour
{
    [SerializeField] private float yAngleOffset;
    [SerializeField] private float alignmentSpeed = 0.1f;

    private Camera _camera;
    private float _angle;

    private float GetCurrentAngle()
    {
        var forward = _camera.transform.forward;
        forward.y = 0;
        return Vector3.SignedAngle(Vector3.right, forward, Vector3.up) + yAngleOffset;
    }

    private void Start()
    {
        _camera = Camera.main;
        _angle = GetCurrentAngle();
    }

    private void Update()
    {
        var newAngle = GetCurrentAngle();
        var currentAngle = transform.rotation.eulerAngles;
        _angle = Mathf.LerpAngle(_angle, newAngle, alignmentSpeed);
        transform.rotation = Quaternion.Euler(
            currentAngle.x,
            _angle,
            currentAngle.z
        );
    }
}