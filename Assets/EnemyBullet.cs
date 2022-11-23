using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float bulletSpeed = 50f;
    public float duration = 1f;

    // Start is called before the first frame update

    IEnumerator Start()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(new Vector3(0, 0, 1) * bulletSpeed * Time.deltaTime, Space.Self);
        //Vector3 movement = new Vector3(0.0f, 0.0f, -1f);
        //GetComponent<Rigidbody>().velocity = movement * bulletSpeed;
        GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * bulletSpeed * 2);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(other.gameObject);
            CameraShake.Shake(2f, 2f);
        }
    }
}
