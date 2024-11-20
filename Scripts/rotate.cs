using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour
{
    public Vector3 rotateAmount;
    void Start()
    {

    }

    void Update()
    {
        transform.Rotate(rotateAmount * Time.deltaTime);
    }
}
