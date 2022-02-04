﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MAReturnToGame : MenuAction
{
    public override void Action()
    {
        InputManager.Instance?.PlayerControlsActive(true);
        base.Action();
    }
}

