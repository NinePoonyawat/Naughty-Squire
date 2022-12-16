using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySong : MonoBehaviour
{
    void Start()
    {
        FindObjectOfType<AudioManager>().Play("Lobby");
    }
}
