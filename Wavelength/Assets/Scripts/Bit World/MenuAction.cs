using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BWLevels { Welcome, LevelSelect, Game, Pause, Victory };

public class MenuAction : MonoBehaviour
{
    protected List<string> levels = new List<string> { "Welcome", "", "BitWorldLevels", "PauseMenu", "Completed" };

    public virtual void Action() { }
}
