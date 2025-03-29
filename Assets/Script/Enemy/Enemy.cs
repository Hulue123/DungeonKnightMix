using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform targetTransform;
    public GameObject player;
    public Rigidbody2D rb;
    public int damage;
    public float speed;
    public Parameter parameter;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        parameter=GetComponent<Parameter>();


        speed = parameter.speed;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        targetTransform = player.transform;
    }

    private void FixedUpdate()
    {
        Chase();
    }

    private void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        targetTransform = player.transform;
    }









    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<Parameter>().TakeDamage(damage);
        }
    }


    public void Chase()
    {
        Vector2 distance= targetTransform.position - transform.position;
        rb.velocity = distance.normalized * speed;
    }
















}
