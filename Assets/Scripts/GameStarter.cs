using Map;
using Unity.Netcode;
using Unity.Netcode.Transports.UNET;
using UnityEngine;

public class GameStarter : MonoBehaviour
{
    [SerializeField] private RandomBoardGenerator boardGenerator;

    private const int Port = 80085;
    
    private enum ClientType
    {
        None,
        Host,
        Client
    }

    private string _ip = "127.0.0.1";
    private ClientType _clientType = ClientType.None;
    private bool _mapGenerated = false;
    

    private void OnGUI()
    {
        if (_clientType == ClientType.None)
        {
            var transport = (UNetTransport) NetworkManager.Singleton.NetworkConfig.NetworkTransport;

            _ip = GUI.TextField(new Rect(10, 10, 150, 30), _ip);
            transport.ConnectAddress = _ip;
            transport.ConnectPort = Port;
            transport.ServerListenPort = Port;

            if (GUI.Button(new Rect(10, 50, 150, 30), "Start as Host"))
            {
                NetworkManager.Singleton.StartHost();
                _clientType = ClientType.Host;
            }

            if (GUI.Button(new Rect(10, 90, 150, 30), "Connect to server"))
            {
                NetworkManager.Singleton.StartClient();
                _clientType = ClientType.Client;
            }
        }
        else if (_clientType == ClientType.Host)
        {
            if (!_mapGenerated && GUI.Button(new Rect(10, 10, 150, 20), "Generate map"))
            {
                boardGenerator.CreateRandomBoard();
                _mapGenerated = true;
            }
        }
    }
}