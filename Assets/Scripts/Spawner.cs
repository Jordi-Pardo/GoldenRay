using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Spawner : MonoBehaviour
{
    public TMPro.TextMeshProUGUI enemyCountDisplay;
    private int enemyCount;
    public Transform[] spawnPoints;
    public GameObject enemy;
    public float startTimeBtwSpawns;
    private float timeBtwSpawns;
    bool startGame = false;

    PhotonView view;
    private void Start()
    {
        view = GetComponent<PhotonView>();

    }

    private void Update()
    {

        if (PhotonNetwork.IsMasterClient == false || PhotonNetwork.CurrentRoom.PlayerCount != 2)
        {
            return;
        }

        if (timeBtwSpawns <= 0)
        {
            Vector3 spawnPosition = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
            PhotonNetwork.Instantiate(enemy.name, spawnPosition, Quaternion.identity);
            UpdateEnemyCount();
            timeBtwSpawns = startTimeBtwSpawns;
        }
        else
        {
            timeBtwSpawns -= Time.deltaTime;
        }
    }

    private void UpdateEnemyCount()
    {
        view.RPC("UpdateEnemyCountRPC", RpcTarget.All);
    }
    [PunRPC]
    private void UpdateEnemyCountRPC()
    {
        enemyCount++;
        enemyCountDisplay.text = "Enemies: " + enemyCount.ToString();
    }

    //IEnumerator SpawnEnemiesCO()
    //{
    //    if (PhotonNetwork.IsMasterClient == false)
    //    {
    //        yield return null;
    //    }
    //    yield return new WaitForSecondsRealtime(3f);
    //    Vector3 spawnPosition = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
    //    PhotonNetwork.Instantiate(enemy.name, spawnPosition, Quaternion.identity);

    //}
}
