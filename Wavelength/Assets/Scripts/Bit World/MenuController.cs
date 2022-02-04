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

    public static Color32 unselected = new Color32(212, 212, 212, 255);
    public static Color32 selected = new Color32(120, 232, 120, 255);

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
            if (value == selIdx)
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
        if (Input.GetButtonDown("Vertical"))
        {
            if (Input.GetAxisRaw("Vertical") > 0.0f)
            {
                SelectedIndex--;
            }
            else if (Input.GetAxisRaw("Vertical") < 0.0f)
            {
                SelectedIndex++;
            }
        }

        if (Input.GetButtonDown("Output"))
        {
            if (menuActions[selIdx].isActiveAndEnabled)
            {
                menuActions[selIdx].Action();
            }
        }

        if (Input.GetButtonUp("Cancel"))
        {
            menuActions[escOption].Action();
        }
    }
}
