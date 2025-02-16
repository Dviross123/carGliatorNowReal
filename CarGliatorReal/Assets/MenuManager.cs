using Unity.Netcode;
using UnityEngine;

public class MenuManager : NetworkBehaviour
{
    int currentPlayerNum = 0;

    [SerializeField] GameObject playerBlock;
    [SerializeField] Transform parent; // "players grid" (Must have a NetworkObject)
    [SerializeField] Transform MapSelector; // UI Parent (MapSelector)

    private void Start()
    {
        currentPlayerNum = 0;
     //   NetworkManager.Singleton.OnClientConnectedCallback += OnPlayerConnected;
    }

    //private void OnPlayerConnected(ulong clientId)
    //{
    //    if (IsServer) // Only the server should spawn it
    //    {
    //        NetworkObject parentNetworkObject = parent.GetComponent<NetworkObject>();

    //        if (!parentNetworkObject.IsSpawned)
    //        {
    //            parentNetworkObject.Spawn(true);
    //            parent.SetParent(MapSelector, false);
    //        }

    //        NetworkObject mapSelectorNetworkObject = MapSelector.GetComponent<NetworkObject>();

    //        if (!parentNetworkObject.IsSpawned)
    //        {
    //            mapSelectorNetworkObject.Spawn(true);
    //            MapSelector.SetParent(MapSelector, false);
    //        }
    //    }
    //    AddPlayerToPlayersLstServerRpc();
    //}

    //[ServerRpc]
    //void AddPlayerToPlayersLstServerRpc()
    //{
    //    currentPlayerNum++;
    //    GameObject temp = Instantiate(playerBlock);
    //    NetworkObject tempNetworkObject = temp.GetComponent<NetworkObject>();
    //    tempNetworkObject.Spawn(true);

    //    // Check if parent is spawned before reparenting
    //    if (parent.TryGetComponent<NetworkObject>(out var parentNetworkObject) && parentNetworkObject.IsSpawned)
    //    {
    //        temp.transform.SetParent(parent, false);
    //    }
    //    else
    //    {
    //        Debug.LogError("Parent must have a NetworkObject and be spawned.");
    //    }
    //}
}
