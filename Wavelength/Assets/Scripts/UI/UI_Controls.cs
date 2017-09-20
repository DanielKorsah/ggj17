using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Controls : MonoBehaviour
{
    Image i;
    GameObject confirm;
    void awake()
    {
        //confirm = gameObject.transform.GetChild(0).gameObject;
        //confirm = GameObject.Find("ConfirmDialogue");
        //confirm = GameObject.FindGameObjectWithTag("Dialogue");
        //MenuDeny();
    }

    public void MenuButtonClick()
    {
        confirm.SetActive(true);
    }

    public void MenuConfirm()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void MenuDeny()
    {
        confirm.SetActive(false);
    }
}
