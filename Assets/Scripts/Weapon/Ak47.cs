using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ak47 : Weapon
{
    private float fireTimer;
    public float fireTime;
    public override void Fire()
    {
        
        Vector2 diffenrence = camara.ScreenToWorldPoint(Input.mousePosition) - player.transform.position;//鼠标方向
        float rotZ = Mathf.Atan2(diffenrence.y, diffenrence.x) * Mathf.Rad2Deg;//将弧度转化为角度
        Instantiate(bullet, firePoint.transform.position, Quaternion.Euler(0, 0, rotZ));
    }

    override public void Update()
    {
        fireTimer -= Time.deltaTime;


        if (fireTimer < 0 && isfire)
        {
            Fire();
            fireTimer = fireTime;
        }
    }

    public override void Awake()
    {
        base.Awake();
        fireTimer = fireTime;
    }

}
