using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject target;
    //targeti transform olarak al.

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(target.transform.position.x + 5f, target.transform.position.y + 4.5f, target.transform.position.z - 10), 0.3f);
    }

    //END LINE.
}