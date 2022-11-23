using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayAudio : MonoBehaviour
{
    public float waitTime = 2f;

    public void Play()
    {
        GetComponent<AudioSource>().PlayDelayed(waitTime);
    }
}
