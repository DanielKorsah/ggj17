using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WelcomeController : MonoBehaviour
{
    [SerializeField]
    Text[] options;
    int selIdx = 0;

    Color32 unselected = new Color32(212, 212, 212, 255);
    Color32 selected = new Color32(120, 232, 120, 255);

    bool vertIn = false;

    // Start is called before the first frame update
    void Start()
    {
        HoverText();
    }
    // Highlight currently selected index
    private void HoverText()
    {
        options[selIdx].color = selected;
    }
    // Dehighlight currently selected index
    private void UnhoverText()
    {
        options[selIdx].color = unselected;
    }
    // Selected index wraps around options length
    private int SelectedIndex
    {
        get
        {
            return selIdx;
        }
        set
        {
            if(value == selIdx)
            {
                return;
            }
            UnhoverText();
            if (value < 0)
            {
                selIdx = options.Length - 1;
            }
            else if (value >= options.Length)
            {
                selIdx = 0;
            }
            else
            {
                selIdx = value;
            }
            HoverText();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Vertical") != 0.0f)
        {
            if (!vertIn)
            {
                vertIn = true;
                if (Input.GetAxisRaw("Vertical") > 0.0f)
                {
                    SelectedIndex--;
                }
                else if (Input.GetAxisRaw("Vertical") < 0.0f)
                {
                    SelectedIndex++;
                }
            }
        }
        else
        {
            vertIn = false;
        }

        if(Input.GetAxisRaw("Output") > 0)
        {
            switch (selIdx)
            {
                case 0:
                    SceneManager.LoadScene("BitWorldLevels");
                    break;
                case 1:
                    Application.Quit();
                    break;
                default:
                    break;
            }
        }

        if (Input.GetAxisRaw("Cancel") > 0)
        {
            Application.Quit();
        }
    }
}
