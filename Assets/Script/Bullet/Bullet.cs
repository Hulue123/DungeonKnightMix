using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int power;



    public void FixedUpdate()
    {
        Move();
    }








    private void Move()
    {
        transform.position = Vector3.forward;

    }
}
