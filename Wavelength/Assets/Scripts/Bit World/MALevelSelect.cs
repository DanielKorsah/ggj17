using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MALevelSelect : MenuAction
{
    public override void Action()
    {
        SceneManager.LoadScene(levels[(int)BWLevels.LevelSelect]);
    }
}
