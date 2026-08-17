using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkMessageTest : NetworkBehaviour
{
    private string lastMessage = "No message received.";

    private ulong assignedPlayerId;

    private Vector3 currentPosition;

    private readonly Dictionary<ulong, Vector3> latestPlayerStates =
        new Dictionary<ulong, Vector3>();

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        assignedPlayerId =
            NetworkManager.Singleton.LocalClientId;

        currentPosition = transform.position;

        Debug.Log(
            "NETWORK SPAWNED - My Client ID: " +
            assignedPlayerId
        );

        if (IsClient)
        {
            RequestPlayerIdServerRpc();
        }
    }

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
            "My Client ID: " +
            assignedPlayerId
        );

        GUI.Label(
            new Rect(20, 60, 700, 30),
            "Position: " +
            currentPosition
        );

        GUI.Label(
            new Rect(20, 100, 700, 30),
            "Last Message: " +
            lastMessage
        );

        if (GUI.Button(
            new Rect(20, 150, 240, 50),
            "SEND POSITION"))
        {
            SendPositionServerRpc(
                currentPosition
            );
        }
    }

    [Rpc(SendTo.Server)]
    private void RequestPlayerIdServerRpc(
        RpcParams rpcParams = default)
    {
        ulong clientId =
            rpcParams.Receive.SenderClientId;

        Debug.Log(
            "SERVER: Client connected with ID: " +
            clientId
        );

        SendPlayerIdClientRpc(
            clientId,
            RpcTarget.Single(
                clientId,
                RpcTargetUse.Temp
            )
        );
    }

    [Rpc(SendTo.SpecifiedInParams)]
    private void SendPlayerIdClientRpc(
        ulong playerId,
        RpcParams rpcParams = default)
    {
        assignedPlayerId = playerId;

        lastMessage =
            "Server assigned ID: " +
            playerId;

        Debug.Log(
            "CLIENT: Server assigned my ID = " +
            playerId
        );
    }

    [Rpc(SendTo.Server)]
    private void SendPositionServerRpc(
        Vector3 position,
        RpcParams rpcParams = default)
    {
        ulong clientId =
            rpcParams.Receive.SenderClientId;

        latestPlayerStates[clientId] =
            position;

        Debug.Log(
            "SERVER STATE UPDATE | " +
            "Client ID: " +
            clientId +
            " | Position: " +
            position
        );

        Debug.Log(
            "SERVER STORED STATES: " +
            latestPlayerStates.Count
        );

        foreach (
            KeyValuePair<ulong, Vector3> state
            in latestPlayerStates)
        {
            Debug.Log(
                "  Client " +
                state.Key +
                " → Position " +
                state.Value
            );
        }

        SendStateReceivedClientRpc(
            clientId,
            position,
            RpcTarget.Single(
                clientId,
                RpcTargetUse.Temp
            )
        );
    }

    [Rpc(SendTo.SpecifiedInParams)]
    private void SendStateReceivedClientRpc(
        ulong clientId,
        Vector3 position,
        RpcParams rpcParams = default)
    {
        lastMessage =
            "Server received state for Client " +
            clientId +
            " at " +
            position;

        Debug.Log(
            "CLIENT: Server confirmed position = " +
            position
        );
    }
}