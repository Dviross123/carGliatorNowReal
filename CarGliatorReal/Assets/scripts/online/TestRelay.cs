 using System.Collections;
using System.Collections.Generic;
using Unity.Services.Core;
using Unity.Netcode;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Services.Authentication;
using UnityEngine;
using Unity.Networking.Transport.Relay;
using Unity.Netcode.Transports.UTP;
using UnityEngine.UI;
using TMPro;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using System.Threading.Tasks;

public class TestRelay : MonoBehaviour
{
    //continue from gpt blocking calls
    [SerializeField] private Button HostBtn;
    [SerializeField] private Image bg;
    [SerializeField] private Button ClientBtn;
    [SerializeField] private TextMeshProUGUI debugger;
    [SerializeField] private TextMeshProUGUI joinCodeString;
    [SerializeField] private GameObject field;

    Lobby currentLobby;

    void CloseUi()
    {
        bg.gameObject.SetActive(false);
        HostBtn.gameObject.SetActive(false);
        ClientBtn.gameObject.SetActive(false);
        debugger.gameObject.SetActive(false);
        field.SetActive(false);
    }

    private async void Start()
    {
        await UnityServices.InitializeAsync();
        await Task.Delay(1000);

        currentLobby = await Lobbies.Instance.GetLobbyAsync(TestLobby.lobbyId);

        if (AuthenticationService.Instance.PlayerId == currentLobby.HostId)
        {
            CreateRelay();
        }
        else
        {
            JoinRelay();
        }
    }



    private async void CreateRelay()
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);
            string relayJoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            Debug.Log("Relay join code: " + relayJoinCode);

            // Update the lobby with the relay join code
            await Lobbies.Instance.UpdateLobbyAsync(currentLobby.Id, new UpdateLobbyOptions
            {
                Data = new Dictionary<string, DataObject>
            {
                { "RelayJoinCode", new DataObject(DataObject.VisibilityOptions.Member, relayJoinCode) }
            }
            });

            // Set up Relay transport and start hosting
            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartHost();

            // Update UI
            debugger.text = relayJoinCode;
        }
        catch (RelayServiceException e)
        {
            Debug.LogError(e);
        }
    }


    private async void JoinRelay()
    {
        try
        {
            // Get the relay join code from the lobby data
            string relayJoinCode = currentLobby.Data["RelayJoinCode"].Value;
            Debug.Log("Joining relay with code: " + relayJoinCode);

            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(relayJoinCode);
            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartClient();
        }
        catch (RelayServiceException e)
        {
            Debug.LogError(e);
        }
    }

}

