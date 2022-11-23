using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public GameObject[] enemyType;
    public int enemiesQuantity;
    private Vector3 initialPos;
    private Vector3 actualPos;
    public float spaceBetweenX = 15;
    public float spaceBetweenZ = 7;

    // Start is called before the first frame update
    void Start()
    {
        initialPos = transform.position;
        actualPos = initialPos;
        for(int x = 0; x < enemyType.Length; x++)
        {
            for(int y = 0; y < enemiesQuantity; y++)
            {
                GameObject enemy = Instantiate(enemyType[x], actualPos, Quaternion.Euler(0, 180, 0));
                enemy.transform.parent = GameObject.FindGameObjectWithTag("EnemiesFormation").transform;
                actualPos += new Vector3(spaceBetweenX, 0, 0);
            }
            spaceBetweenX *= -1;
            actualPos += new Vector3(spaceBetweenX, 0, -spaceBetweenZ);
        }
        Destroy(gameObject);
    }
}
