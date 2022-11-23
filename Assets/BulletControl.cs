using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    public float bulletSpeed = 30f;
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
        //transform.Translate(new Vector3(0,0,1) * bulletSpeed * Time.deltaTime, Space.Self);
        GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * bulletSpeed * 2);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }

        if(other.tag == "Shield")
        {
            Destroy(gameObject);
        }
    }
}
