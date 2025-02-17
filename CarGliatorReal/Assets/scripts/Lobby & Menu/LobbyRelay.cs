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
public class LobbyRelay : MonoBehaviour
{
    [SerializeField] private Button HostBtn;
    [SerializeField] private Button ClientBtn;
    [SerializeField] private TextMeshProUGUI codeText;
    [SerializeField] private GameObject mapSelectObj;
    private TextMeshProUGUI joinCodeString;
    string joinCode;
    bool isHost;

    private void Awake()
    {
        HostBtn.onClick.AddListener(() =>
        {
            CreateRelay();
        });

        ClientBtn.onClick.AddListener(() =>
        {
            joinCode = joinCodeString.text;
            JoinRelay(joinCode);
        });

    }

    private async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () => {
            Debug.Log("sign in:" + AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    private async void CreateRelay()
    {
        try
        {
            //max players not containing the host
            Allocation allcation = await RelayService.Instance.CreateAllocationAsync(3);
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allcation.AllocationId);
            Debug.Log("join code:" + joinCode);


            RelayServerData relayServerData = new RelayServerData(allcation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartHost();
            mapSelectObj.SetActive(true);
            string tempStr = joinCode.Substring(0, 6);
            codeText.text = "CODE: " + tempStr;

        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }

    }

    private async void JoinRelay(string joinCode)
    {
        try
        {
            joinCode = joinCode.Substring(0, 6);
            Debug.Log("joined relay with:" + joinCode);
            JoinAllocation joinAlloaction = await RelayService.Instance.JoinAllocationAsync(joinCode.ToString());
            RelayServerData relayServerData = new RelayServerData(joinAlloaction, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartClient();
            mapSelectObj.SetActive(true);

        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }

    }


}