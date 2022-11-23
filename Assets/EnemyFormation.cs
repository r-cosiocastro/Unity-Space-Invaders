using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFormation : MonoBehaviour
{
    public float timeToMove = 10;
    public bool isMovingToRight = true;
    public float movementRange = 20;
    public float stepDown = 10;
    private float nextMoveTime;
    public float smoothFactor = 10;
    private Vector3 direction;
    private Vector3 target;
    private bool movingDown = false;
    private float initialTimeToMove;
    private int initialEnemiesQty;
    private float oldTimeToMove;
    private Done_GameController gameController;
    private bool canMove = false;
    private bool canLookAtYou = false;
    private bool movingToInitialPos = true;

    IEnumerator AllowToShoot()
    {
        yield return new WaitForSeconds(6);
        direction = new Vector3(movementRange, 0, 0);
        initialTimeToMove = timeToMove;
        initialEnemiesQty = gameObject.transform.childCount - 1;
        oldTimeToMove = timeToMove;
        //GetComponent<Animator>().applyRootMotion = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AllowToShoot());
        

        GameObject gameControllerObject = GameObject.FindGameObjectWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<Done_GameController>();
        }
        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (movingToInitialPos)
        {
            if (transform.position.z > gameController.initialZEnemiesPosition)
            {
                transform.Translate(new Vector3(0, 0, -1) * Time.deltaTime * 14, Space.Self);
                Debug.Log("Moving to initial pos");
            }
            else
            {
                canMove = true;
                movingToInitialPos = false;
                Debug.Log("Reached");
            }
        }
        if (!gameController.gameOver)
        {
            if (gameController.HasGameStarted())
            {
                if (gameObject.transform.childCount == 0)
                {
                    gameController.Win();
                    Destroy(gameObject);
                }
                else if (gameObject.transform.childCount <= 3)
                {
                    canLookAtYou = true;
                }
                if (canMove)
                {
                    if (Time.time > nextMoveTime)
                    {
                        if (movingDown)
                        {
                            target = transform.position + new Vector3(0, 0, -stepDown);
                            movingDown = false;
                        }
                        else
                        {
                            target = transform.position + direction;
                        }
                        nextMoveTime = Time.time + timeToMove;
                    }
                    transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * smoothFactor);
                    timeToMove = (initialTimeToMove / initialEnemiesQty) * gameObject.transform.childCount;
                    if (timeToMove != oldTimeToMove)
                    {
                        smoothFactor += 0.3f;
                        oldTimeToMove = timeToMove;
                    }
                }
            }
        }
    }

    public void GetDown()
    {
        if (!movingDown)
        {
            movingDown = true;
            isMovingToRight = !isMovingToRight;
            if (isMovingToRight)
            {
                direction = new Vector3(movementRange, 0, 0);
            }
            else
            {
                direction = new Vector3(-movementRange, 0, 0);
            }
        }
    }

    public void StopMoving()
    {
        canMove = false;
    }

    public void ContinueMoving()
    {
        canMove = true;
    }

    public void ChangeLookStatus()
    {
        canLookAtYou = !canLookAtYou;
    }

    public bool CanLookAtYou()
    {
        return canLookAtYou;
    }
}
