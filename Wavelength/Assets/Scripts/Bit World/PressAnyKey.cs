using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PressAnyKey : MonoBehaviour
{
    Text text;
    float waitTime = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        StartCoroutine(DelayFunction());
    }

    IEnumerator DelayFunction()
    {
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(FlashText());
        StartCoroutine(WaitForInput());
    }

    IEnumerator WaitForInput()
    {
        while (true)
        {
            if (Input.anyKeyDown)
            {
                SceneManager.LoadScene("Welcome");
            }
            yield return null;
        }
    }

    IEnumerator FlashText()
    {
        Color32 texCol = text.color;
        int val = 255;
        while (true)
        {
            float mult = (-Mathf.Cos((Time.timeSinceLevelLoad - waitTime) * Mathf.PI / 1.5f) + 1) / 2.0f;
            texCol.a = (byte)(val * mult);
            text.color = texCol;
            yield return null;
        }
    }
}
