using Unity.Netcode;
using UnityEngine;

public class NetworkConnectionTest : MonoBehaviour
{
    [SerializeField]
    private NetworkObject networkMessagePrefab;

    private NetworkObject spawnedMessageObject;

    private void Start()
    {
        if (NetworkManager.Singleton == null)
        {
            Debug.LogError("NetworkManager not found!");
            return;
        }

        NetworkManager.Singleton.OnServerStarted += OnServerStarted;
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnServerStarted -= OnServerStarted;
        }
    }

    private void OnServerStarted()
    {
        if (!NetworkManager.Singleton.IsHost)
            return;

        if (networkMessagePrefab == null)
        {
            Debug.LogError(
                "NetworkMessageTest prefab is not assigned!"
            );

            return;
        }

        spawnedMessageObject =
            Instantiate(networkMessagePrefab);

        spawnedMessageObject.Spawn();

        Debug.Log(
            "NetworkMessageTest spawned by host."
        );
    }

    private void OnGUI()
    {
        if (NetworkManager.Singleton == null)
        {
            GUI.Label(
                new Rect(20, 20, 500, 30),
                "NetworkManager not found!"
            );

            return;
        }

        if (!NetworkManager.Singleton.IsListening)
        {
            if (GUI.Button(
                new Rect(20, 60, 200, 50),
                "START HOST"))
            {
                NetworkManager.Singleton.StartHost();
            }

            if (GUI.Button(
                new Rect(20, 120, 200, 50),
                "START CLIENT"))
            {
                NetworkManager.Singleton.StartClient();
            }

            return;
        }

        string status;

        if (NetworkManager.Singleton.IsHost)
            status = "HOST";
        else if (NetworkManager.Singleton.IsClient)
            status = "CLIENT";
        else
            status = "CONNECTED";

        GUI.Label(
            new Rect(20, 20, 500, 30),
            "Status: " + status
        );

        if (spawnedMessageObject != null &&
            NetworkManager.Singleton.IsHost)
        {
            GUI.Label(
                new Rect(20, 180, 600, 30),
                "Network Message Object Spawned"
            );
        }
    }
}