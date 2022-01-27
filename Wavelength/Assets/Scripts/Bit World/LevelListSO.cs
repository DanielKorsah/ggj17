using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelList", menuName = "ScriptableObjects/LevelList")]
public class LevelListSO : ScriptableObject
{
    public List<LevelDetailsSO> Levels = new List<LevelDetailsSO>();

    public int GetLevelIndex(LevelDetailsSO deets)
    {
        for(int i = 0; i < Levels.Count; ++i)
        {
            if(Levels[i] == deets)
            {
                return i;
            }
        }
        return -1;
    }

}
