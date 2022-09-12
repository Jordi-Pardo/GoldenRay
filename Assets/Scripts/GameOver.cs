using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public TMPro.TextMeshProUGUI scoreText;

    public GameObject waitingText;
    public GameObject restartButton;

    private PhotonView view;
    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();

        scoreText.text = "Score: " + FindObjectOfType<Score>().scoreDisplay.text;

        if (PhotonNetwork.IsMasterClient)
        {
            restartButton.SetActive(true);
            waitingText.SetActive(false);
        }
        else
        {

            restartButton.SetActive(false);
            waitingText.SetActive(true);
        }
    }
    [PunRPC]
    private void RestartRPC()
    {
        PhotonNetwork.LoadLevel("Game");
    }

    public void OnClickRestart() 
    {
        view.RPC("RestartRPC", RpcTarget.All);
    }
}
