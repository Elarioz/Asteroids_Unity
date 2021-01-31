using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBehaviour : MonoBehaviour
{

    public float maxthrust;
    public float maxtorque;
    public Rigidbody2D rb;

    //Screen wrapping
    public float screenTop;
    public float screenBottom;
    public float screenLeft;
    public float screenRight;

    public int asteroidSize;  // 3=Big 2=Medium 1=Small
    public GameObject asteroidMedium;
    public GameObject asteroidSmall;

    public int points;
    public GameObject playerSpaceship;

    public GameManager gm;


    // Start is called before the first frame update
    void Start()
    {
        //Add a random amount of thrust & torque
        Vector2 thrust = new Vector2(Random.Range(-maxthrust, maxthrust), Random.Range(-maxthrust, maxthrust));
        float torque = Random.Range(-maxtorque, maxtorque);

        rb.AddForce(thrust);
        rb.AddTorque(torque);

        //Find the player
        playerSpaceship = GameObject.FindWithTag("Player");

        //Find the game manager
        gm = GameObject.FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //Screen wrapping, le joueur apparait de l'autre côté de l'écran si il sort de celui-ci
        Vector2 newPos = transform.position;
        if (transform.position.y > screenTop)
        {
            newPos.y = screenBottom;
        }
        if (transform.position.y < screenBottom)
        {
            newPos.y = screenTop;
        }
        if (transform.position.x > screenRight)
        {
            newPos.x = screenLeft;
        }
        if (transform.position.x < screenLeft)
        {
            newPos.x = screenRight;
        }

        transform.position = newPos;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {

        //Check if it is a bullet
        if (other.CompareTag("bullet"))
        {
            //Destroy the bullet
            Destroy(other.gameObject);
            // Check the size of the asteroid and diminish it
            if(asteroidSize == 3)
            {
                Instantiate(asteroidMedium, transform.position, transform.rotation);
                Instantiate(asteroidMedium, transform.position, transform.rotation);

                gm.UpdateNumberOfAsteroids(1);
            }

            else if(asteroidSize == 2)
            {
                Instantiate(asteroidSmall, transform.position, transform.rotation);
                Instantiate(asteroidSmall, transform.position, transform.rotation);

                gm.UpdateNumberOfAsteroids(1);
            }

            else if(asteroidSize == 1)
            {
                //Remove the asteroids

                gm.UpdateNumberOfAsteroids(-1);
            }

            //Score the points
            playerSpaceship.SendMessage("ScorePoints", points);

            Destroy(gameObject);
        }

    }
}
