using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    
    public Transform firePoint;//�����
    public Camera camara;//���
    public Transform player;//��ȡ���
    public Vector3 offSet;
    public Transform bullet;
    public bool isfire;
    public float fireTimer;
    public float fireTime;


    public void Awake()
    {
       
        camara = Camera.main;
        fireTimer = fireTime;

        
    }

    

    public void Start()
    {
        
    }

    
    

    public void Update()
    {
        fireTimer -= Time.deltaTime;


        if (fireTimer < 0 && isfire)
        {
            Fire();
            fireTimer = fireTime;
        }









    }



    public void FixedUpdate()
    {
        Rotate();
        Move();
    }





    //ת��
    public void Rotate()
    {
        Vector2 diffenrence = camara.ScreenToWorldPoint(Input.mousePosition) - player.transform.position;//��귽��
        float rotZ = Mathf.Atan2(diffenrence.y, diffenrence.x) * Mathf.Rad2Deg;//������ת��Ϊ�Ƕ�
        transform.rotation = Quaternion.Euler(0,0, rotZ);// ��ת

        //����ת��
        if (camara.ScreenToWorldPoint(Input.mousePosition).x-player.transform.position.x > 0)
            transform.localScale = new Vector3(1, 1, 1); 
        if (camara.ScreenToWorldPoint(Input.mousePosition).x - player.transform.position.x < 0)
            transform.localScale = new Vector3(1, -1, 1);
    }

    public void Move()
    {
        transform.position = player.transform.position+offSet;
    }

    public void Fire()
    {
        Vector2 diffenrence = camara.ScreenToWorldPoint(Input.mousePosition) - player.transform.position;//��귽��
        float rotZ = Mathf.Atan2(diffenrence.y, diffenrence.x) * Mathf.Rad2Deg;//������ת��Ϊ�Ƕ�
        Instantiate(bullet, firePoint.transform.position, Quaternion.Euler(0, 0, rotZ));
    }


}
