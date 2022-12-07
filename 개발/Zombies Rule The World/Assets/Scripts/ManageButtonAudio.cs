using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageButtonAudio : MonoBehaviour
{
    public void PlayButtonAudio()
    {
        if (GameManager.Instance.isEffect)
        {
            transform.parent.GetComponent<AudioSource>().Play();
        }
    }
}
