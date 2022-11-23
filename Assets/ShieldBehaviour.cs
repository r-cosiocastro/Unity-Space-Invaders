using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBehaviour : MonoBehaviour
{
    public GameObject explosion;
    public GameObject biggerExplosion;
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "EnemyBullet" || other.tag == "PlayerBullet")
        {
            if(other.tag == "PlayerBullet")
            {
                Instantiate(explosion, other.transform.position, other.transform.rotation);
                CameraShake.Shake(1f, 1f);
            }
            if(transform.localScale.z > 0.5f)
            {
                transform.localScale += new Vector3(0,0,-0.5f);
            }
            else
            {
                Instantiate(biggerExplosion, other.transform.position, other.transform.rotation);
                CameraShake.Shake(1.5f, 1.5f);
                Destroy(gameObject);
            }
        }
    }
}
