using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public GameObject bullet;
    public float minFireRate = 4;
    public float maxFireRate = 12;

    private float nextFire;
    private EnemyFormation enemyFormation;
    private Done_GameController gameController;

    private bool canShoot = false;
    private bool canLookAtYou = false;
    private bool rateFireIncreased = false;

    IEnumerator AllowToShoot()
    {
        yield return new WaitForSeconds(6);
        canShoot = true;
        nextFire = Time.time + Random.Range(1f, 2f);
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AllowToShoot());
        GameObject enemyFormationObject = GameObject.FindGameObjectWithTag("EnemiesFormation");
        GameObject gameControllerObject = GameObject.FindGameObjectWithTag("GameController");
        if (enemyFormationObject != null)
        {
            enemyFormation = enemyFormationObject.GetComponent<EnemyFormation>();
        }
        if(gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<Done_GameController>();
        }
        if (enemyFormation == null)
        {
            Debug.Log("Cannot find 'EnemyFormation' script");
        }
        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }
    }

    

    // Update is called once per frame
    void Update()
    {
        if (gameController.HasGameStarted())
        {
            if (canLookAtYou)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    transform.LookAt(player.transform);
                }
            }

            if (gameController.ShouldIncreaseRateFire() && !rateFireIncreased)
            {
                minFireRate /= 8;
                maxFireRate /= 8;
                rateFireIncreased = true;
            }

            if (!gameController.ShouldIncreaseRateFire() && rateFireIncreased)
            {
                minFireRate *= 8;
                maxFireRate *= 8;
            }

            if (canShoot)
            {
                if (Time.time > nextFire)
                {
                    //int layerMask = 1 << 2;
                    //layerMask = ~layerMask;
                    canLookAtYou = enemyFormation.CanLookAtYou();

                    if (canLookAtYou)
                    {
                        nextFire = Time.time + Random.Range(minFireRate, maxFireRate);
                        //GameObject myBullet = Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, 0));
                        //myBullet.transform.SetParent(transform);
                        GameObject myBullet = Instantiate(bullet, transform.position, transform.rotation);
                        myBullet.GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity;
                        GetComponent<AudioSource>().Play();
                    }
                    int layerMask = LayerMask.GetMask("Default", "Shield");

                    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, 150, layerMask))
                    {
                        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);

                        if (hit.transform.tag == "Player" || hit.transform.tag == "Shield")
                        {
                            nextFire = Time.time + Random.Range(minFireRate, maxFireRate);
                            GameObject myBullet = Instantiate(bullet, transform.position, transform.rotation);
                            myBullet.GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity;
                            GetComponent<AudioSource>().Play();
                        }
                    }
                    else
                    {
                        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
                    }
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "EnemiesArea")
        {
            enemyFormation.GetDown();
        }
    }
}
