using Unity.Netcode;
using UnityEngine;

public class NetworkMessageTest : NetworkBehaviour
{
    private string lastMessage = "No message received.";

    private void OnGUI()
    {
        if (!IsSpawned)
        {
            GUI.Label(
                new Rect(20, 20, 600, 30),
                "Waiting for network spawn..."
            );

            return;
        }

        GUI.Label(
            new Rect(20, 20, 700, 30),
            "Last Message: " + lastMessage
        );

        if (GUI.Button(
            new Rect(20, 70, 220, 50),
            "SEND HELLO TO SERVER"))
        {
            SendHelloServerRpc();
        }
    }

    [Rpc(SendTo.Server)]
    private void SendHelloServerRpc()
    {
        Debug.Log(
            "SERVER RECEIVED HELLO FROM CLIENT"
        );

        lastMessage =
            "Server received: Hello from Client";

        SendHelloClientRpc(
            "Hello from Server!"
        );
    }

    [Rpc(SendTo.NotServer)]
    private void SendHelloClientRpc(
        string message)
    {
        Debug.Log(
            "CLIENT RECEIVED: " + message
        );

        lastMessage =
            "Received: " + message;
    }
}