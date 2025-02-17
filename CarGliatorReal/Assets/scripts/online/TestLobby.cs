using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class TestLobby : MonoBehaviour
{
    private Lobby hostLobby;
    private Lobby joinedLobby;
    private float heartBeatTimer;
    private float heartBeatTimerMax = 15f;

    private float lobbyUpdateTimer;
    private string playerName;

    [SerializeField] TextMeshProUGUI debugger;

    private async void Start()
    {
        await UnityServices.InitializeAsync();

        if (AuthenticationService.Instance.IsSignedIn) return;
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        playerName = "Player" + UnityEngine.Random.Range(10, 99);
        Debug.Log(playerName);     
    }

    private void Update()
    {
        HandleLobbyHeartBeat();
        HandleLobbyPollForUpdates();
    }


    public async void CreateLobby()
    {
        try
        {
            string lobbyName = "MyLobby";
            int maxPlayers = 4;
            CreateLobbyOptions op = new CreateLobbyOptions {
                IsPrivate = true,
                Player = GetPlayer()
            };

            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, op);

            hostLobby = lobby;
            joinedLobby = hostLobby;
            PrintPlayers(hostLobby);

            SceneManager.LoadScene("Lobby");
            Debug.Log("Lobby Created " + lobby.Name + " " + lobby.MaxPlayers + " " + lobby.LobbyCode);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void JoinLobbyByCode()
    {
        string code = UserData.codeEntered.Substring(0, 6);
        try
        {
            JoinLobbyByCodeOptions joinLobbyByCodeOptions = new JoinLobbyByCodeOptions
            {
                Player = GetPlayer()
            };

            Lobby lobby = await Lobbies.Instance.JoinLobbyByCodeAsync(code, joinLobbyByCodeOptions);
            joinedLobby = lobby;
            Debug.Log("joined lobby with code:" + code);
            PrintPlayers(joinedLobby);
            SceneManager.LoadScene("Lobby");
        }
        catch (LobbyServiceException e)
        {
            debugger.text = e.ToString();
        }
    }

    private async void HandleLobbyHeartBeat()
    {
        if (hostLobby != null)
        {
            heartBeatTimer -= Time.deltaTime;
            if (heartBeatTimer <= 0f)
            {
                heartBeatTimer = heartBeatTimerMax;
                await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
            }
        }
    }

    private async void HandleLobbyPollForUpdates()
    {
        if (joinedLobby != null)
        {
            lobbyUpdateTimer -= Time.deltaTime;
            if (lobbyUpdateTimer <= 0f)
            {
                lobbyUpdateTimer = 1.1f;
                Lobby lobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);
                joinedLobby = lobby;
            }
        }
    }

    private async void UpdatePlayerName(string newPlayerName)
    {
        try
        {
            playerName = newPlayerName;
            await LobbyService.Instance.UpdatePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId, new UpdatePlayerOptions()
            {
                Data = new Dictionary<string, PlayerDataObject>
            {
                { "PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, newPlayerName) }
            }
            });
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError(e);
        }
    }

    private Player GetPlayer()
    {
        return new Player
        {
            Data = new Dictionary<string, PlayerDataObject>{
                    {"PlayerName",new PlayerDataObject( PlayerDataObject.VisibilityOptions.Member, playerName )}
                }
        };
    }

    private void PrintPlayers(Lobby lobby)
    {
        foreach(Player player in lobby.Players)
        {
            Debug.Log(player.Data["PlayerName"].Value);
        }
    }

    private void PrintPlayers()
    {
        PrintPlayers(joinedLobby);
    }

    private async void LeaveLobby()
    {
        try
        {
            await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);
        }
        catch(LobbyServiceException e)
        {
            Debug.Log(e);
        }  
    }

    private async void KickPlayer()
    {
        try
        {
          //  await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, joinedLobby.Players[].Id);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

}
