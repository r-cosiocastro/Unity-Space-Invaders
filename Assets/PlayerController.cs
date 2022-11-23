using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour
{

    public float speed = 5;
    public GameObject bullet;
    public GameObject giantBullet;
    public GameObject cheatBullet;
    public float fireRate;
    public float tilt;
    public Boundary boundary;

    public GameObject enemyFormation;
    public GameObject mainCamera;
    public GameObject newMainCamera;
    private GameObject lastEnemy;

    private float nextFire;
    private GameObject nMainCamera;
    private GameObject lastBullet;

    private bool canShoot = false;
    private bool canMove = false;
    private bool speededUp = false;
    private bool rapidFire = false;
    private bool shootEarth = false;

    IEnumerator AllowToShoot()
    {
        yield return new WaitForSeconds(5);
        canMove = true;
        GetComponent<Animator>().applyRootMotion = true;
        yield return new WaitForSeconds(1);
        canShoot = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        if(enemyFormation == null)
        {
            enemyFormation = GameObject.FindGameObjectWithTag("EnemiesFormation");
        }

        if(mainCamera == null)
        {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }

        StartCoroutine(AllowToShoot());
    }

    void Update()
    {
        if (enemyFormation == null)
        {
            enemyFormation = GameObject.FindGameObjectWithTag("EnemiesFormation");
        }
        if (canShoot)
        {

                int layerMask = LayerMask.GetMask("EnemyCenter","Shield");

                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, Mathf.Infinity, layerMask))
                {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                
                if (Input.GetButton("Fire1") && Time.time > nextFire)
                {
                    nextFire = Time.time + fireRate;
                    if (shootEarth)
                    {
                        lastBullet = Instantiate(cheatBullet, transform.position + new Vector3(0, 0, 3), Quaternion.Euler(0, 0, 0));
                    }
                    else
                    {
                        lastBullet = Instantiate(bullet, transform.position + new Vector3(0, 0, 3), Quaternion.Euler(0, 0, 0));
                        GetComponent<AudioSource>().Play();
                    }
                    //if (enemyFormation.transform.childCount == 1)
                    //{
                    if (hit.transform.tag == "Enemy")
                    {
                        Debug.Log("Enemy center");
                        canShoot = false;
                        GameObject[] enemyBullets = GameObject.FindGameObjectsWithTag("EnemyBullet");
                        GameObject[] playerBullets = GameObject.FindGameObjectsWithTag("PlayerBullet");
                        for (int x = 0; x < enemyBullets.Length; x++)
                        {
                            Destroy(enemyBullets[x]);
                        }

                        for(int x = 0; x < playerBullets.Length; x++)
                        {
                            Destroy(playerBullets[x]);
                        }

                        nextFire = Time.time + fireRate;
                        lastBullet = Instantiate(bullet, transform.position + new Vector3(0, 0, 3), Quaternion.Euler(0, 0, 0));
                        GetComponent<AudioSource>().Play();
                        
                        Time.timeScale = 0.3f;
                        enemyFormation.GetComponent<EnemyFormation>().StopMoving();
                        //nMainCamera.GetComponent<FollowTarget>().SetTarget(lastBullet.transform);
                        float dist = Vector3.Distance(transform.position, hit.transform.position);
                        float minZ = (dist / 10) * 2;
                        float randX = Random.Range(transform.position.x - 5, transform.position.x + 5);
                        Vector3 cameraPos = new Vector3(randX, Random.Range(-10, 10), Random.Range(minZ, dist - minZ));
                        nMainCamera = Instantiate(newMainCamera, cameraPos, Quaternion.LookRotation(lastBullet.transform.position));
                        //nMainCamera.transform.LookAt(lastBullet.transform);
                        nMainCamera.GetComponent<LookatTarget>().SetTarget(lastBullet.transform);
                        StartCoroutine(EnableMainCamera());
                    }
                    else
                    {
                        //Debug.Log("Not enemy center");
                        Debug.Log("Tag:" + hit.transform.tag);
                    }
                    //}
                }
            }
            else
            {
                if (Input.GetButton("Fire1") && Time.time > nextFire)
                {
                    nextFire = Time.time + fireRate;
                    if (shootEarth)
                    {
                        lastBullet = Instantiate(cheatBullet, transform.position + new Vector3(0, 0, 3), Quaternion.Euler(0, 0, 0));
                    }
                    else
                    {
                        lastBullet = Instantiate(bullet, transform.position + new Vector3(0, 0, 3), Quaternion.Euler(0, 0, 0));
                        GetComponent<AudioSource>().Play();
                    }
                }
                    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            }
            if (Input.GetButton("Fire2") && Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                lastBullet = Instantiate(giantBullet, transform.position + new Vector3(0, 0, 3), Quaternion.Euler(0, 0, 0));
            }
        }
    }

    IEnumerator EnableMainCamera()
    {
        while (lastBullet != null)
        {
            GameObject[] enemyBullets = GameObject.FindGameObjectsWithTag("EnemyBullet");
            for (int x = 0; x < enemyBullets.Length; x++)
            {
                Destroy(enemyBullets[x]);
            }
            yield return null;
        }
        nMainCamera.GetComponent<LookatTarget>().enabled = false;
        //mainCamera.SetActive(true);
        yield return new WaitForSecondsRealtime(1);
        Destroy(nMainCamera);
        canShoot = true;
        //nMainCamera.GetComponent<FollowTarget>().SetTarget(transform);
        Time.timeScale = 1f;
        enemyFormation.GetComponent<EnemyFormation>().ContinueMoving();
    }

    void FixedUpdate()
    {
        boundary.xMin = (-Screen.width) + (Screen.width * 0.85f);
        boundary.xMax = Screen.width - (Screen.width * 0.85f);
        if (canMove)
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
            GetComponent<Rigidbody>().velocity = movement * speed;

            GetComponent<Rigidbody>().position = new Vector3
            (
                Mathf.Clamp(GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax),
                0.0f,
                Mathf.Clamp(GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
            );

            GetComponent<Rigidbody>().rotation = Quaternion.Euler(0.0f, 0.0f, GetComponent<Rigidbody>().velocity.x * -tilt);
        }
    }

    public void StopMoving()
    {
        Vector3 movement = Vector3.zero;
        GetComponent<Rigidbody>().velocity = movement;
        canMove = false;
        canShoot = false;
    }

    public void SpeedUp()
    {
        if (!speededUp)
        {
            speed *= 4;
        }
        else
        {
            speed /= 4;
        }

        speededUp = !speededUp;
    }

    public void EnableRapidFire()
    {
        if (!rapidFire)
        {
            fireRate /= 4;
        }
        else
        {
            fireRate *= 4;
        }
    }

    public void ChangeBullet()
    {
        shootEarth = !shootEarth;
        if (shootEarth)
        {
            fireRate *= 4;
        }
        else
        {
            fireRate /= 4;
        }
    }
}
