using System;
using UnityEngine;


public class FollowTarget : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(20f, 5f, -15f);
    public Vector3 rotationOffset = new Vector3(0, 0, 0);


    private void LateUpdate()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
        transform.position = target.position + offset;
        transform.rotation.SetLookRotation(rotationOffset);
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}
