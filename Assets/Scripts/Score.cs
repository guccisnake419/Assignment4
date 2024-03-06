// add score manager
using UnityEngine;
using UnityEngine.UI;

// access the Text Mesh Pro namespace
using TMPro;

public class Score : MonoBehaviour
{
    public GameObject sun;
    public TMP_Text scoreText;
    public int maxScore = 10;
    public int minScore = 0;

    public TMP_Text message;
    public GameObject spawnpoint;
    public GameObject objectToSpawn;
    int score;
    Transform t;
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        scoreText.text = "Score: " + score;
        

    }

    //we will call this method from our target script
    // whenever the player collides or shoots a target a point will be added
    public void AddPoint()
    {
        score++;

        if (score != maxScore){
            scoreText.text = "Score: " + score;
            sun.transform.eulerAngles = new Vector3(
            sun.transform.eulerAngles.x+15,
            sun.transform.eulerAngles.y,
            sun.transform.eulerAngles.z);
                
        } else{
            scoreText.text = "Hurray! You have collected all collectibles";
            Instantiate(objectToSpawn);
        }
        
    }
    public void DecementPoint()
    {
        Debug.Log("In Decrement!!");
        // if (score > 0){

            score= score-1;
            scoreText.text = "Score: " + score;
            Debug.Log("In Decrement");
        //}
           
    }

    public void AddMessage(string str)
    {
        message.text = "+ "+ str ;
    }
}