using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CanvasScript : MonoBehaviour {

	 

    public void newGame()
    {
        //load game scene
        SceneManager.LoadScene(1);      
    
    }

    public void MainMenu()
    {
        //go back to main menu
        SceneManager.LoadScene(0);
    }
}
