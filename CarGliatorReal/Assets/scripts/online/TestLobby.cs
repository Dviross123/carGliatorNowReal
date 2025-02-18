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
    private static Lobby joinedLobby;
    private float heartBeatTimer;
    private float heartBeatTimerMax = 15f;
    private float lobbyUpdateTimer = 1.1f; 
    private string playerName;

    [SerializeField] TextMeshProUGUI debugger;
    [SerializeField] private Transform playersCardsGrid;
    [SerializeField] private GameObject playerCardPrefab;
    [SerializeField] private GameObject startGameButton;

    int playersInLobby;
    public static string lobbyId;

    [SerializeField] private TextMeshProUGUI codeText;
    //continue update code text;

    private async void Start()
    {
        if(codeText != null)
        {
            codeText.text ="Code: " + UserData.codeEntered;
        }

        playersInLobby = 0;
        await UnityServices.InitializeAsync();

        if (!AuthenticationService.Instance.IsSignedIn)
        {
            AuthenticationService.Instance.SignedIn += () =>
            {
                Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
            };

            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

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
            CreateLobbyOptions options = new CreateLobbyOptions
            {
                IsPrivate = true,
                Player = GetPlayer(),
                Data = new Dictionary<string, DataObject> 
                {
                  { "GameStarted", new DataObject(DataObject.VisibilityOptions.Member, "false") }
                }
            };

            hostLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);
            joinedLobby = hostLobby;
            lobbyId = hostLobby.Id;
            UserData.codeEntered = hostLobby.LobbyCode;

            Debug.Log("Lobby Created: " + hostLobby.Name + " Code: " + hostLobby.LobbyCode);
            SceneManager.LoadScene("Lobby");
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
            JoinLobbyByCodeOptions options = new JoinLobbyByCodeOptions
            {
                Player = GetPlayer()
            };

            joinedLobby = await Lobbies.Instance.JoinLobbyByCodeAsync(code, options);
            lobbyId = joinedLobby.Id;

            Debug.Log("Joined lobby with code: " + code);
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
    
              try
              {
                  Lobby lobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);
                  joinedLobby = lobby;
                  UpdateLobbyUI();
    
                  if (joinedLobby.Data.ContainsKey("GameStarted") && joinedLobby.Data["GameStarted"].Value == "true")
                  {
                      SceneManager.LoadScene("race");
                  }
              }
              catch (LobbyServiceException e)
              {
                  Debug.LogWarning("Failed to update lobby: " + e);
              }
          }
      }
   }

    private void UpdateLobbyUI()
    {
        Debug.Log("Updating Lobby UI");

        if (joinedLobby.Players.Count > playersInLobby)
        {
            for (int i = playersInLobby; i < joinedLobby.Players.Count; i++)
            {
                GameObject newCard = Instantiate(playerCardPrefab, playersCardsGrid);
                newCard.GetComponentInChildren<TextMeshProUGUI>().text = joinedLobby.Players[i].Data["PlayerName"].Value;
            }

            // Show Start Game button only for the host
            startGameButton.SetActive(AuthenticationService.Instance.PlayerId == joinedLobby.HostId);
            playersInLobby = joinedLobby.Players.Count;
        }
    }

    private async void UpdatePlayerName(string newPlayerName)
    {
        try
        {
            playerName = newPlayerName;
            await LobbyService.Instance.UpdatePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId, new UpdatePlayerOptions
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
            Data = new Dictionary<string, PlayerDataObject>
            {
                { "PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerName) }
            }
        };
    }

    public async void StartGame()
    {
        if (AuthenticationService.Instance.PlayerId == joinedLobby.HostId)
        {
            try
            {
                await Lobbies.Instance.UpdateLobbyAsync(joinedLobby.Id, new UpdateLobbyOptions
                {
                    Data = new Dictionary<string, DataObject>
                    {
                        { "GameStarted", new DataObject(DataObject.VisibilityOptions.Member, "true") }
                    }
                });

                Debug.Log("Game started. Players should transition to the race scene.");
            }
            catch (LobbyServiceException e)
            {
                Debug.LogError(e);
            }
        }
        else
        {
            Debug.Log("Only the host can start the game.");
        }
    }

    public async void LeaveLobby()
    {
        try
        {
            await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);
            SceneManager.LoadScene("menu");
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
}
