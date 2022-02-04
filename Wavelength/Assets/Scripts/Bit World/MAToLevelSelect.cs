using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MAToLevelSelect : MenuAction
{
    private static MAToLevelSelect _instance;
    public static MAToLevelSelect Instance { get { return _instance; } }
    private void Awake()
    {
        _instance = this;
    }

    [SerializeField]
    MenuController MenuController;
    [SerializeField]
    RectTransform LevelSelect;

    public override void Action()
    {
        LevelSelectOpen(true);
    }

    public void LevelSelectOpen(bool state)
    {
        // toggle main menu input
        MenuController.gameObject.SetActive(!state);
        // toggle level select
        LevelSelect.gameObject.SetActive(state);
    }
}
