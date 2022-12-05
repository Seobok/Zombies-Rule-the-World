using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeIn : MonoBehaviour
{
    private AudioSource audioSource;
    //public bool fadeIn;
    // fade in 시간 설정 1s
    public double fadeInSeconds = 1.0;
    bool isFadeIn = true;
    double fadeDeltaTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isFadeIn)
        {
            fadeDeltaTime += Time.deltaTime;
            if(fadeDeltaTime >= fadeInSeconds)
            {
                fadeDeltaTime = fadeInSeconds;
                isFadeIn = false;
            }
            audioSource.volume = (float)(fadeDeltaTime / fadeInSeconds);
        }
        
    }
}
