using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FinalSceneScript : MonoBehaviour {
    public Text tries;
    public Text time;
    
	// Use this for initialization
	void Start () {

        time.text = "Time : "+dataScript.time.ToString();
        tries.text = "Tries : "+dataScript.tries.ToString();


		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
