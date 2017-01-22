using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToMenu : MonoBehaviour {

    float timer = 1f;

    [SerializeField]
    public string SceneName;

	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
        if (Input.anyKey)
        {
            if (timer <= 0)
            {
                SceneManager.LoadScene(SceneName, LoadSceneMode.Single);
            }
        }
    }
}
