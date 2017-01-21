using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPortal : MonoBehaviour {

    bool contact = false;
    bool decrement = false;
    float timer = 5f;

    [SerializeField]
    public string SceneName;

	// Use this for initialization
	void Start () {
        decrement = false;
		
	}
	
	// Update is called once per frame
	void Update () {

        if (contact == true && Input.GetKeyDown(KeyCode.Space))
        {
            decrement = true;
            Debug.Log("End of Level, Timer start");
        }

        if(timer > 0 && decrement == true)
        {
            timer -= Time.deltaTime;
            AnimateStatic();
        }

        if(timer <= 0)
        {
            SceneManager.LoadScene(SceneName, LoadSceneMode.Single);
            Debug.Log("Time Up");
            timer = 5;
        }
    }

    private void AnimateStatic()
    {

    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        contact = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        contact = false;
    }
}
