using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManageEnd : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene("Scenes/InGame");
    }
    
    public void QuitGame()          //게임종료 (버튼이 없음)
    {
        #if UNITY_EDITOR        //에디터에서만 실행되는 코드
                UnityEditor.EditorApplication.isPlaying = false;
        #else                   //빌드된 게임에서만 실행되는 코드
                Application.Quit();
        #endif
    }
}
