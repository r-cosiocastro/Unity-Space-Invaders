using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantBulletControl : MonoBehaviour
{
    public float bulletSpeed = 20f;
    public float duration = 5f;

    // Start is called before the first frame update

    IEnumerator Start()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, 0, 1) * bulletSpeed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Destroy(other.gameObject);
        }
    }
}
