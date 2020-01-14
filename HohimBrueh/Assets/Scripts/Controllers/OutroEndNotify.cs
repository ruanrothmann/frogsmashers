using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutroEndNotify : MonoBehaviour {


    public void Finished()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScreen");
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
