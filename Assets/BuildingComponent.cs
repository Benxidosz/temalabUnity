using Unity.Netcode;
using UnityEngine;

public class BuildingComponent : NetworkBehaviour
{
    private NetworkVariable<float> _red = new NetworkVariable<float>(0f);
    private NetworkVariable<float> _green = new NetworkVariable<float>(0f);
    private NetworkVariable<float> _blue = new NetworkVariable<float>(0f);

    private Renderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    [ServerRpc]
    private void SetColorServerRPC(float red, float green, float blue)
    {
        _red.Value = red;
        _green.Value = green;
        _blue.Value = blue;
    }

    public void SetColor(float red, float green, float blue)
    {
        SetColorServerRPC(red, green, blue);
    }

    private void Update()
    {
        _renderer.material.color = new Color(_red.Value, _green.Value, _blue.Value);
    }
}