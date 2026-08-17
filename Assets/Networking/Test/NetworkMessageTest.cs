using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class NetworkMessageTest : NetworkBehaviour
{
    [Header("Movement Test")]
    [SerializeField] private float moveSpeed = 3f;

    [Header("Remote Player")]
    [SerializeField] private GameObject remotePlayerVisualPrefab;

    private string lastMessage = "No message received.";

    private ulong assignedPlayerId;

    private Vector3 currentPosition;

    private readonly Dictionary<ulong, Vector3> latestPlayerStates =
        new Dictionary<ulong, Vector3>();

    private readonly Dictionary<ulong, GameObject> remotePlayers =
        new Dictionary<ulong, GameObject>();

    // SERVER-SIDE PLAYER LIST
    private readonly HashSet<ulong> connectedPlayers =
        new HashSet<ulong>();

    private void Update()
    {
        if (!IsSpawned || !IsOwner)
            return;

        HandleLocalMovement();

        currentPosition = transform.position;

        if (IsClient)
        {
            SendPositionServerRpc(currentPosition);
        }
    }

    private void HandleLocalMovement()
    {
        if (Keyboard.current == null)
            return;

        Vector2 input = Vector2.zero;

        if (Keyboard.current.aKey.isPressed)
            input.x -= 1f;

        if (Keyboard.current.dKey.isPressed)
            input.x += 1f;

        if (Keyboard.current.sKey.isPressed)
            input.y -= 1f;

        if (Keyboard.current.wKey.isPressed)
            input.y += 1f;

        Vector3 movement = new Vector3(
            input.x,
            0f,
            input.y
        ).normalized;

        transform.position +=
            movement * moveSpeed * Time.deltaTime;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        assignedPlayerId =
            NetworkManager.Singleton.LocalClientId;

        currentPosition =
            transform.position;

        Debug.Log(
            "NETWORK SPAWNED - My Client ID: " +
            assignedPlayerId
        );

        if (IsServer)
        {
            RegisterPlayerServer(
                NetworkManager.Singleton.LocalClientId
            );

            NetworkManager.Singleton.OnClientConnectedCallback +=
                OnClientConnected;

            NetworkManager.Singleton.OnClientDisconnectCallback +=
                OnClientDisconnected;
        }

        if (IsClient)
        {
            RequestPlayerRegistrationServerRpc();
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        RegisterPlayerServer(clientId);

        Debug.Log(
            "SERVER: PLAYER JOINED | Client ID: " +
            clientId
        );
    }

    private void OnClientDisconnected(ulong clientId)
    {
        connectedPlayers.Remove(clientId);

        latestPlayerStates.Remove(clientId);

        Debug.Log(
            "SERVER: PLAYER LEFT | Client ID: " +
            clientId
        );

        Debug.Log(
            "SERVER PLAYER COUNT: " +
            connectedPlayers.Count
        );

        RemoveRemotePlayerClientRpc(clientId);
    }

    private void RegisterPlayerServer(ulong clientId)
    {
        if (connectedPlayers.Add(clientId))
        {
            Debug.Log(
                "SERVER: PLAYER REGISTERED | Client ID: " +
                clientId
            );

            Debug.Log(
                "SERVER PLAYER COUNT: " +
                connectedPlayers.Count
            );
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

        if (IsServer)
        {
            GUI.Label(
                new Rect(20, 140, 700, 30),
                "Server Player Count: " +
                connectedPlayers.Count
            );
        }
    }

    [Rpc(SendTo.Server)]
    private void RequestPlayerRegistrationServerRpc(
        RpcParams rpcParams = default)
    {
        ulong clientId =
            rpcParams.Receive.SenderClientId;

        RegisterPlayerServer(clientId);

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

        Vector3 approvedPosition =
            latestPlayerStates[clientId];

        Debug.Log(
            "SERVER APPROVED STATE | " +
            "Client ID: " +
            clientId +
            " | Position: " +
            approvedPosition
        );

        BroadcastApprovedStateClientRpc(
            clientId,
            approvedPosition
        );
    }

    [Rpc(SendTo.NotServer)]
    private void BroadcastApprovedStateClientRpc(
        ulong clientId,
        Vector3 approvedPosition)
    {
        if (clientId == assignedPlayerId)
            return;

        lastMessage =
            "Approved state | Client " +
            clientId +
            " | Position " +
            approvedPosition;

        UpdateRemotePlayer(
            clientId,
            approvedPosition
        );
    }

    private void UpdateRemotePlayer(
        ulong clientId,
        Vector3 position)
    {
        if (remotePlayerVisualPrefab == null)
        {
            Debug.LogError(
                "Remote Player Visual Prefab is not assigned!"
            );

            return;
        }

        if (!remotePlayers.TryGetValue(
            clientId,
            out GameObject remotePlayer))
        {
            remotePlayer =
                Instantiate(
                    remotePlayerVisualPrefab,
                    position,
                    Quaternion.identity
                );

            remotePlayers.Add(
                clientId,
                remotePlayer
            );

            Debug.Log(
                "Created remote player visual for Client " +
                clientId
            );
        }
        else
        {
            remotePlayer.transform.position =
                position;
        }
    }

    [Rpc(SendTo.NotServer)]
    private void RemoveRemotePlayerClientRpc(
        ulong clientId)
    {
        if (remotePlayers.TryGetValue(
            clientId,
            out GameObject remotePlayer))
        {
            if (remotePlayer != null)
            {
                Destroy(remotePlayer);
            }

            remotePlayers.Remove(clientId);

            Debug.Log(
                "CLIENT: Removed remote player visual for Client " +
                clientId
            );
        }
    }

    public override void OnNetworkDespawn()
    {
        if (IsServer &&
            NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -=
                OnClientConnected;

            NetworkManager.Singleton.OnClientDisconnectCallback -=
                OnClientDisconnected;
        }

        foreach (
            GameObject remotePlayer
            in remotePlayers.Values)
        {
            if (remotePlayer != null)
            {
                Destroy(remotePlayer);
            }
        }

        remotePlayers.Clear();

        base.OnNetworkDespawn();
    }
}