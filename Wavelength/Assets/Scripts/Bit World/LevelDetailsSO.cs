using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "LevelDetails", menuName = "ScriptableObjects/LevelDetails")]
public class LevelDetailsSO : ScriptableObject
{
    public string LevelName = "A Puzzle";
    public string LevelNumber = "99.99";
    private string pngName = "";
    public Texture2D LevelLayout;
    public string AdditionalDialogue = "";

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!LevelLayout.name.Equals(pngName))
        {
            pngName = LevelLayout.name;
            string[] numParts = LevelLayout.name.Split('.');
            if (numParts[0].Equals("1"))
            {
                numParts[1] = (int.Parse(numParts[1]) + 4).ToString();
            }
            if (numParts[2].Length == 1)
            {
                numParts[2] = "0" + numParts[2];
            }
            LevelNumber = numParts[1] + "." + numParts[2];

            string assetPath = AssetDatabase.GetAssetPath(GetInstanceID());
            AssetDatabase.RenameAsset(assetPath, LevelNumber);
            AssetDatabase.SaveAssets();
        }
    }
#endif
}