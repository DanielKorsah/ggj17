using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MARestartLevel : MenuAction
{
    public override void Action()
    {
        FindObjectOfType<World>().ResetWorld();
        base.Action();
    }
}

