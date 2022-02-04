using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MenuAction : MonoBehaviour
{
    protected static SceneController sceC;
    protected static InputManagerSuper inSup;

    [SerializeField]
    protected bool actionFreezesInput = false;
    [SerializeField]
    protected BWLevels loadLevel = BWLevels.None;
    [SerializeField]
    protected BWLevels unloadLevel = BWLevels.None;


    private void Start()
    {
        if(sceC == null)
        {
            sceC = SceneController.Instance;
        }
        if (inSup == null)
        {
            inSup = InputManagerSuper.Instance;
        }
    }

    public virtual void Action() {
        if (actionFreezesInput)
        {
            inSup?.FreezeInputs();
        }
        bool execute = false;
        if (unloadLevel != BWLevels.None)
        {
            sceC.QueueUnload(unloadLevel);
            execute = true;
        }
        if (loadLevel != BWLevels.None)
        {
            sceC.QueueLoad(loadLevel);
            execute = true;
        }
        if (execute)
        {
            sceC.ExecuteQueues();
        }
    }
}
