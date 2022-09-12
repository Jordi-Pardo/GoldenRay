using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class StartAudio : MonoBehaviour
{
    bool start = false;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount != 2)
        {
            return;
        }
        else if(!start)
        {
            audioSource.Play();
            start = true;
        }

        
    }
}
