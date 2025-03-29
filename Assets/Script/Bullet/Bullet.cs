using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    public float speed;

    private Rigidbody2D rb;
   



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    private void Start()
    {
        rb.velocity = transform.right * speed;

    }




    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other);
        if(other.gameObject.CompareTag("Enemy"))
        {
            other.GetComponent<Parameter>().TakeDamage(damage);
            Destroy(this.gameObject);

        }

        if (other.gameObject.CompareTag("Wall"))
        {
            Debug.Log("attack");
            Destroy(this.gameObject);
        }




    }
}















