using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManageSetting : MonoBehaviour
{
    public Sprite on;
    public Sprite off;

    public GameObject fastBtn;
    public GameObject effectBtn;
    public GameObject bgmBtn;

    public GameObject bgm;

    private bool isFast = false;
    private bool isEffect = true;
    private bool isBgm = true;

    public void Fast()
    {
        if (isFast)
        {
            fastBtn.GetComponent<Image>().sprite = off;
            GameManager.Instance.gameSpeed = 1f;

            isFast = false;
        }
        else
        {
            fastBtn.GetComponent<Image>().sprite = on;
            GameManager.Instance.gameSpeed = 2f;

            isFast = true;
        }
    }

    public void Effect()
    {
        if (isEffect)
        {
            effectBtn.GetComponent<Image>().sprite = off;

            GameManager.Instance.isEffect = false;
            isEffect = false;
        }
        else
        {
            effectBtn.GetComponent<Image>().sprite = on;

            GameManager.Instance.isEffect = true;
            isEffect = true;
        }
    }

    public void Bgm()
    {
        if (isBgm)
        {
            bgmBtn.GetComponent<Image>().sprite = off;

            bgm.GetComponent<AudioSource>().Pause();
            isBgm = false;
        }
        else
        {
            bgmBtn.GetComponent<Image>().sprite = on;

            bgm.GetComponent<AudioSource>().Play();
            isBgm = true;
        }
    }

    public void Quit()
    {
        GameManager.Instance.gameSpeed = 1f;
        Time.timeScale = 1f;
        
        #if UNITY_EDITOR        //에디터에서만 실행되는 코드
                UnityEditor.EditorApplication.isPlaying = false;
        #else                   //빌드된 게임에서만 실행되는 코드
                        Application.Quit();
        #endif
    }
}
