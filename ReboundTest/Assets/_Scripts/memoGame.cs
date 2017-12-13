using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class memoGame : MonoBehaviour {

    public GameObject[] preFabsCards;
    public Camera camera; 
    int[] indiceOfInstance = new int[] { 0,0,1, 1, 2, 2, 3, 3, 4, 4, 5, 5,};//indices of pairs
    GameObject revealedCard=null;//this variable stocks the first revealed card 
    bool interact = true;//bool to stop the interaction when a second card is revealed
    int foundedPairs;//number of pair found
    int nbrColonnes = 3;//number of colomn
    int nbrsLignes=4;// number of line 
    float elapsedTime;// elapsed time 
    int tries;//number of tries


    private float offSet=4; //space between the cards

   

    
    // Use this for initialization
    void Start () {
        elapsedTime = 0;
        reshuffle(indiceOfInstance);//shuffle the indices of pairs to generate a random disposition at each execution
        placeCards();      //the function that instance the cards and place it on the scene
        foundedPairs = 0;
    }
	
	// Update is called once per frame
	void Update () {
        elapsedTime += Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && interact)
        {
            chooseCard();
        }
		
	}
    IEnumerator NoMatch(GameObject firstCard, GameObject secondCard)
    {

        //Debug.Log("pause");
        interact = false;//on bloque l'interaction pour une 1/2 seconde le temps que les deux carte se retourne 

        yield return new WaitForSeconds(0.5f);
        firstCard.transform.Rotate(0, 180, 0);
        secondCard.transform.Rotate(0, 180, 0);

        interact = true;// on reautorise l'interaction


    }


    


    public void placeCards()
    {// cette fonction permet de mettre en place les carte au debut du jeu
        int cpt = 0;//indice pour parcourir le vecteur des indice de pairs ( 6 pairs )
        for (int i = 0; i < nbrsLignes; i++)
        {
            for (int c = 0; c < nbrColonnes; c++)
            {
                // instancier une carte à la position (offset est pris en compte)  l'axe Z est passer en dur (not the best way)
                Instantiate(preFabsCards[indiceOfInstance[cpt]], new Vector3(c * offSet, i  * offSet, 8), Quaternion.identity);
                cpt++;// 
            }

        }
    }

    static void reshuffle(int[] vals)
    {
        //shuffle a vector 
        for (int t = 0; t < vals.Length; t++)
        {
            int tmp = vals[t];
            int r = Random.Range(t, vals.Length);
            vals[t] = vals[r];
            vals[r] = tmp;
        }

     
    }

public void chooseCard()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {

            if (hit.transform.tag == "whiteCard" || hit.transform.tag == "blackCard"|| hit.transform.tag == "greenCard" || 
                hit.transform.tag == "redCard"|| hit.transform.tag == "blueCard"|| hit.transform.tag == "yellowCard")
            {
                Transform objectHit = hit.transform;
                 if (revealedCard == null)
                { 
                    //if no card revealed reveal this one                    
                    objectHit.Rotate(0, 180, 0);
                    revealedCard = objectHit.gameObject;
                  
                }
                else
                {
                     //test if the same card was choosen (compare with the position)
                    if (revealedCard.transform.position == objectHit.transform.position)
                    {
                        objectHit.Rotate(0, 180, 0);

                        revealedCard = null;
                        //faceDown(objectHit.gameObject);

                    }
                    else
                    {
                        //this case means that the user choose two card we test if they it's a pair then we destroy it after 1/2 second
                        if(revealedCard.gameObject.tag== objectHit.gameObject.tag)
                        {
                            objectHit.Rotate(0, 180, 0); //rotate the card
                            Destroy(revealedCard,0.5f);// destroy the cards
                            Destroy(objectHit.gameObject,0.5f);
                            foundedPairs += 1;//inremente number of pairs found
                            revealedCard = null;//clear revealed card
                            tries++;//incremetne the number of tries 
                            GameOver();//test if it's the end of game
                            
                        }
                        else
                        { //here the two card doesn't match
                            //reveal the second card for a few moment then turn face down booth
                            objectHit.Rotate(0, 180, 0);
                            StartCoroutine( NoMatch(objectHit.gameObject, revealedCard));//couroutine to let the card returne for moment 
                            revealedCard = null;// if no match we supress the revealed card to get new one in the next interaction
                            tries++;
                        }
                    }
                }


            }
        }

    }
 public void GameOver()
    {
        //teste if ther's card object in scene 
        
        if (foundedPairs==6)
        { 
            // i used i script to save the time and the number of tries to display them in the End scene
            dataScript.time =(float)System.Math.Round( elapsedTime,2);
            dataScript.tries = tries;
            SceneManager.LoadScene(2);//load the End scene 
        }

    }
}
