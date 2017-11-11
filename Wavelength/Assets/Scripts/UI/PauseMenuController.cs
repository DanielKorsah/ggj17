using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{

    void Start()
    {
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void ToMenu()
    {
        Debug.Log("ToMenu");
        SceneManager.LoadScene("Main Menu");
    }

    public void Continue()
    {

    }
}
