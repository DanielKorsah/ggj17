using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    Text[] options;
    [SerializeField]
    int escOption = 0;
    int selIdx = 0;
    MenuAction[] menuActions;

    Color32 unselected = new Color32(212, 212, 212, 255);
    Color32 selected = new Color32(120, 232, 120, 255);

    bool vertIn = false;

    // Start is called before the first frame update
    void Start()
    {
        menuActions = new MenuAction[options.Length];
        for (int i = 0; i < options.Length; ++i)
        {
            menuActions[i] = options[i].GetComponent<MenuAction>();
        }
        HoverText();
        InputManager.Instance?.PlayerControlsActive(false);
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
            menuActions[selIdx].Action();
            //switch (selIdx)
            //{
            //    case 0:
            //        SceneManager.LoadScene("BitWorldLevels");
            //        break;
            //    case 1:
            //        // ~~~ LevelSelect
            //        break;
            //    case 2:
            //        Application.Quit();
            //        break;
            //    default:
            //        break;
            //}
        }

        if (Input.GetAxisRaw("Cancel") > 0)
        {
            menuActions[escOption].Action();
            // Application.Quit();
        }
    }
}
