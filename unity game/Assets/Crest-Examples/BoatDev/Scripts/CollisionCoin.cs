using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollisionCoin : MonoBehaviour
{
    public Text scoreText;
    private int score;
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter (Collider collision)
    {
        print(collision.gameObject.name);
        if (collision.gameObject.name == "coin")
        {
            Destroy(collision.gameObject);
            score = score + 1;
            scoreText.text = "Score: " + score.ToString();
        }
    }
}
