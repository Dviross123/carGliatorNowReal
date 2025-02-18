using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] TestLobby lobbyScript;
    [SerializeField] GameObject PlayerCard;
    [SerializeField] Transform PlayersCardsHolder;

    public async void CreatePlayerCard()
    {
        try
        {
            GameObject temp = Instantiate(PlayerCard);
            temp.transform.parent = PlayersCardsHolder;
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
}
