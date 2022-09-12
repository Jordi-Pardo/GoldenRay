using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    private int score = 0;
    public TMPro.TextMeshProUGUI scoreDisplay;
    private PhotonView view;

    private void Start()
    {
        view = GetComponent<PhotonView>();
    }

    public void IncreaseScore()
    {
        view.RPC("IncreaseScoreRPC", RpcTarget.All);
    }

    [PunRPC]
    private void IncreaseScoreRPC()
    {
        score++;
        scoreDisplay.text = "Score: " + score.ToString();
    }
}
