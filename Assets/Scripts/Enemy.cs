using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;
public class Enemy : MonoBehaviour
{
    private PlayerController[] players;
    private PlayerController nearestPlayer;

    private Score scoreScript;

    public float speed;

    [Header("VFX")]
    public GameObject deathVFX;

    private PhotonView view;

    private CinemachineImpulseSource cinemachineImpulse;
    private void Start()
    {
        view = GetComponent<PhotonView>();

        players = FindObjectsOfType<PlayerController>();
        scoreScript = FindObjectOfType<Score>();
        cinemachineImpulse = GetComponent<CinemachineImpulseSource>();
    }

    private void Update()
    {

        if(players.Length != 2)
        {
            players = FindObjectsOfType<PlayerController>();
            return;
        }
        float distanceOne = Vector2.Distance(transform.position, players[0].transform.position);
        float distanceTwo = Vector2.Distance(transform.position, players[1].transform.position);

        if (distanceOne < distanceTwo)
        {
            nearestPlayer = players[0];
        }
        else
        {
            nearestPlayer = players[1];
        }

        if(nearestPlayer != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, nearestPlayer.transform.position, speed * Time.deltaTime);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (collision.CompareTag("GoldenRay") || collision.CompareTag("Bullet"))
            {
                scoreScript.IncreaseScore();
                view.RPC("SpawnParticleRPC", RpcTarget.All);
                PhotonNetwork.Destroy(gameObject);
            }

        }
    }

    [PunRPC]
    private void SpawnParticleRPC()
    {
        cinemachineImpulse.GenerateImpulse();
        Instantiate(deathVFX, transform.position, Quaternion.identity);
    }
}
