using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class SaveSystem
{
    private static SaveSystem instance = null;

    private SaveSystem () { }

    public static SaveSystem Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SaveSystem ();
            }
            return instance;
        }
    }

    void shit ()
    {
        Data d = new Data ();
        var x = JsonConvert.SerializeObject (d);

    }
}