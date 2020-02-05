using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MAReturnToGame : MenuAction
{
    public override void Action()
    {
        SceneManager.UnloadSceneAsync(levels[(int)BWLevels.Pause]);
        InputManagerSuper.Instance?.FreezeInputs();
        InputManager.Instance?.PlayerControlsActive(true);
    }
}

