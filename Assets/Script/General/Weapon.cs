using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    
    public Transform firePoint;//开火点
    public Camera camara;//相机
    public Transform player;//获取玩家
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





    //转向
    public void Rotate()
    {
        Vector2 diffenrence = camara.ScreenToWorldPoint(Input.mousePosition) - player.transform.position;//鼠标方向
        float rotZ = Mathf.Atan2(diffenrence.y, diffenrence.x) * Mathf.Rad2Deg;//将弧度转化为角度
        transform.rotation = Quaternion.Euler(0,0, rotZ);// 旋转

        //左右转向
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
        Vector2 diffenrence = camara.ScreenToWorldPoint(Input.mousePosition) - player.transform.position;//鼠标方向
        float rotZ = Mathf.Atan2(diffenrence.y, diffenrence.x) * Mathf.Rad2Deg;//将弧度转化为角度
        Instantiate(bullet, firePoint.transform.position, Quaternion.Euler(0, 0, rotZ));
    }


}
