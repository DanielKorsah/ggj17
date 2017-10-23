﻿using System.Collections;
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
    }

    public void ToMenu()
    {
        Debug.Log("ToMenu");
        SceneManager.LoadScene("Main Menu");
    }
}
