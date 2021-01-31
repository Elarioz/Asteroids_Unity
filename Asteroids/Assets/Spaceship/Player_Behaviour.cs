using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player_Behaviour : MonoBehaviour
{
    public Rigidbody2D rb;

    //Bullet properties
    public GameObject bullet;
    public float bulletForce;

    //Player movements
    [SerializeField]
    public float thrust;
    public float rotationThrust;
    private float thrustInput;
    private float turnInput;
    public float deathForce;

    //UI VARIABLES
    private int score;
    private int lives;
    

    public Text scoreText;
    public Text livesText;


    public Color inColor;
    public Color normalColor;

    public GameObject gameOverPannel;

    //Screen wrapping
    public float screenTop;
    public float screenBottom;
    public float screenLeft;
    public float screenRight;



    // Start is called before the first frame update
    void Start()
    {
        lives = 3;
        score = 0;

        scoreText.text = "Score : " + score;
        livesText.text = "Lives : " + lives;
    }

    // Update is called once per frame
    void Update()
    {
        //Check keyboard inputs
        thrustInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");

        //Check inputs from the fire key
        if (Input.GetButtonDown("Fire1"))
        {
            GameObject newBullet = Instantiate(bullet, transform.position, transform.rotation);
            newBullet.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * bulletForce);
            Destroy(newBullet, 3.0f);
        }

        //Rotate the spaceship

        transform.Rotate(Vector3.forward * turnInput * Time.deltaTime * -rotationThrust);

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

    private void FixedUpdate()
    {
        // On crée la force de mouvement sur le joueur via son rigidbody
        rb.AddRelativeForce(Vector2.up * thrustInput);
        //rb.AddTorque(-turnInput);
    }


    void ScorePoints(int pointsToAdd)
    {
        score += pointsToAdd;
        scoreText.text = "Score : " + score;
        
    }

    void Respawn()
    {
        rb.velocity = Vector2.zero;
        transform.position = Vector2.zero;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.enabled = true;
        sr.color = inColor;
        Invoke("invincible", 3f);
    }

    void invincible()
    {
        GetComponent<Collider2D>().enabled = true;
        GetComponent<SpriteRenderer> ().color = normalColor;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log(col.relativeVelocity.magnitude);
        if (col.relativeVelocity.magnitude > deathForce)
        {
            lives--;
            livesText.text = "Lives : " + lives;

            //Respawn system
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
            Invoke("Respawn", 3f);

            if (lives == 0)
            {
                //GameOver
                GameOver();
            }
        }
    }


    void GameOver()
    {
        CancelInvoke();
        gameOverPannel.SetActive(true);
        
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("InGameScene");
    }

    public void QuitGame()
    {
        if (Application.isEditor)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
        else
        {
            Application.Quit();
        }
        
    }

}
