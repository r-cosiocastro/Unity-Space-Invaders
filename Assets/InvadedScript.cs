using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvadedScript : MonoBehaviour
{
    public GameObject player;
    private Done_GameController gameController;
    private EnemyFormation enemyFormation;

    // Start is called before the first frame update
    void Start()
    {
        GameObject gameControllerObject = GameObject.FindGameObjectWithTag("GameController");
        GameObject enemyFormationObject = GameObject.FindGameObjectWithTag("EnemiesFormation");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<Done_GameController>();
        }
        if (enemyFormationObject != null)
        {
            enemyFormation = enemyFormationObject.GetComponent<EnemyFormation>();
        }
        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }
        if (enemyFormation == null)
        {
            Debug.Log("Cannot find 'EnemyFormation' script");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        if (enemyFormation == null)
        {
            GameObject enemyFormationObject = GameObject.FindGameObjectWithTag("EnemiesFormation");
            if (enemyFormationObject != null)
            {
                enemyFormation = enemyFormationObject.GetComponent<EnemyFormation>();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            player.GetComponent<PlayerController>().StopMoving();
            gameController.GameOver();
            gameController.SwitchEnemyRateFire();
            enemyFormation.ChangeLookStatus();
        }
    }
}
