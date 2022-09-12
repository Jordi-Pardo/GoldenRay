using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class Lobby : MonoBehaviourPunCallbacks
{
    public List<TMPro.TextMeshProUGUI> playerListText;

    public Button startGameButton;
    Player[] players;

    PhotonView view;
    // Start is called before the first frame update

    private void Awake()
    {
        view = GetComponent<PhotonView>();
        startGameButton.onClick.AddListener(StartGame);
    }

    void Start()
    {
        players = PhotonNetwork.PlayerList;

        for (int i = 0; i < players.Length; i++)
        {
            playerListText[i].text = players[i].NickName;
            Debug.Log(players[i].NickName);

        }

    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        players = PhotonNetwork.PlayerList;

        for (int i = 0; i < players.Length; i++)
        {
            playerListText[i].text = players[i].NickName;
            Debug.Log(players[i].NickName);

        }

        if (players.Length >= 2 && PhotonNetwork.IsMasterClient)
        {
            startGameButton.interactable = true;
        }
    }

    private void StartGame()
    {
        view.RPC("StartGameRPC", RpcTarget.All);
    }

    [PunRPC]
    private void StartGameRPC()
    {
        PhotonNetwork.LoadLevel("Game");
    }

}
