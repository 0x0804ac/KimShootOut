using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;


public class NgoTestLauncher : MonoBehaviour
{
    [SerializeField]
    private string address = "127.0.0.1";

    [SerializeField]
    private ushort port = 7777;

    private void StartNetwork(bool host)
    {
        NetworkManager manager = NetworkManager.Singleton;
        UnityTransport transport = manager.GetComponent<UnityTransport>();

        if (transport == null )
        {
            Debug.LogError("UnityTransport is missing.");
            return;
        }

        transport.SetConnectionData(
            address,
            port,
            host ? "0.0.0.0" : null);

        if (host)
        {
            manager.StartHost();
        }
        else
        {
            manager.StartClient();
        }
    }

    private void OnGUI()
    {
        NetworkManager manager = NetworkManager.Singleton;

        if (manager == null )
        {
            return;
        }

        GUILayout.BeginArea(
            new Rect(10, 10, 240, 160),
            GUI.skin.box
            );
        
        GUILayout.Label("NGO Network Test");

        if(!manager.IsListening)
        {
            if (GUILayout.Button("Start Host"))
            { 
                StartNetwork(true);
            }
            if (GUILayout.Button("Start Client"))
            {
                StartNetwork(false);    
            }
        }
        else
        {
            GUILayout.Label(manager.IsHost ? "Host" : "Client");

            if (manager.IsServer)
            {
                GUILayout.Label($"Players: {manager.ConnectedClients.Count}");
            }

            if (GUILayout.Button("Disconnect"))
            {
                manager.Shutdown();
            }
        }

        GUILayout.EndArea();
    }
}
