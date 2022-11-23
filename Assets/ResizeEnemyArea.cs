using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeEnemyArea : MonoBehaviour
{
    private float width;

    // Update is called once per frame
    void Update()
    {
        width = (Screen.width - ((Screen.width / 10) * 5)) / 2;
        GetComponent<BoxCollider>().size = new Vector3(width, 10, 240);
    }
}
