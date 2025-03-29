using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("输入")]
    private InputSystem inputControl;//获取输入系统
    public Vector2 inputDirection;//获取坐标输入

    [Header("属性")]
    private Parameter parameter;//获取基本参数 

    [Header("动画")]
    private Animator animator;//获取动画播放器

    [Header("物理")]
    private Rigidbody2D rb;//获取刚体

    public GameObject weapon;//所获得的武器

    public Weapon weaponScript;
    
    




    private void Awake()
    {
        //此处用于初始量的获得
        inputControl = new InputSystem();
        animator = GetComponent<Animator>();
        parameter = GetComponent<Parameter>();
        rb = GetComponent<Rigidbody2D>();
        weaponScript = weapon.GetComponent<Weapon>();

        inputControl.GamePlay.Fire.started += FireStart;
        inputControl.GamePlay.Fire.canceled += FireCancle;






    }

    private void FireCancle(InputAction.CallbackContext context)
    {
        weaponScript.isfire = false;
    }

    private void FireStart(InputAction.CallbackContext context)
    {
        weaponScript.isfire = true;
    }









    //开火函数


    private void OnEnable()
    {
        inputControl.Enable();//开始时启动输入
    }

    private void OnDisable()
    {
        inputControl.Disable();//结束时关闭输入
    }


    private void Update()
    {

        //读取输入 
        inputDirection = inputControl.GamePlay.Move.ReadValue<Vector2>();










    }

    public void FixedUpdate()
    {
        //移动
        Move();
        Face();





    }






    //移动 
    public void Move()
    {
        //计算速度
        rb.velocity = new Vector2(inputDirection.x * parameter.speed * Time.deltaTime, inputDirection.y * parameter.speed * Time.deltaTime);

        //改变paramater中的状态
        if (inputDirection.x != 0 || inputDirection.y != 0)
            parameter.isMove = true;
        else
            parameter.isMove = false;




    }

    
    public void Face()
    {
        //角色朝向 
        if (inputDirection.x > 0)
            transform.localScale = new Vector3(1,1,1);
        if (inputDirection.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);



    }































































}
