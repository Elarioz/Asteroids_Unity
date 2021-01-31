using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public int nbAsteroids; //Current number of asteroids in the scene
    public int currentLevel = 1 ;
    public GameObject asteroid;

    //Updating the current level UI
    private int level;
    public Text levelText;



    private void Start()
    {
        level = 1;
        levelText.text = "Level : " + level;
    }


    public void UpdateNumberOfAsteroids(int change)
    {
        nbAsteroids += change;

        //Check if we have any asteroids left in the scene

        if(nbAsteroids <= 0)
        {
            //Start next level
            Invoke("StartNextLevel", 3f);
            //Update level number
            level++;
            levelText.text = "Level : " + level;
        }
    }

    void StartNextLevel()
    {
        currentLevel++;

        //Spawn new asteroids
        for (int i = 0; i < currentLevel*2; i++)
        {
            Vector2 spawnPosition = new Vector2(Random.Range(-29.5f,29.5f),18.5f);
            Instantiate(asteroid,spawnPosition, Quaternion.identity);

            nbAsteroids++;
        }
    }
}
