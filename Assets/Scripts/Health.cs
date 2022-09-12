using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Cinemachine;
public class Health : MonoBehaviour
{
    public int health = 10;
    public TextMeshProUGUI healthDisplay;

    private PhotonView view;

    public GameObject gameOver;

    private void Start()
    {
        view = GetComponent<PhotonView>();
    }
    public void TakeDamage()
    {
        view.RPC("TakeDamageRPC", RpcTarget.All);
    }

    [PunRPC]
    private void TakeDamageRPC()
    {
        
        health--;
        if(health <= 0)
        {
            gameOver.SetActive(true);
        }

        healthDisplay.text = "Health: " + health.ToString();
    }


}
