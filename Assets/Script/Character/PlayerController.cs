using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("����")]
    private InputSystem inputControl;//��ȡ����ϵͳ
    public Vector2 inputDirection;//��ȡ��������

    [Header("����")]
    private Parameter parameter;//��ȡ�������� 

    [Header("����")]
    private Animator animator;//��ȡ����������

    [Header("����")]
    private Rigidbody2D rb;//��ȡ����

    public GameObject weapon;//����õ�����

    public Weapon weaponScript;
    
    




    private void Awake()
    {
        //�˴����ڳ�ʼ���Ļ��
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









    //������


    private void OnEnable()
    {
        inputControl.Enable();//��ʼʱ��������
    }

    private void OnDisable()
    {
        inputControl.Disable();//����ʱ�ر�����
    }


    private void Update()
    {

        //��ȡ���� 
        inputDirection = inputControl.GamePlay.Move.ReadValue<Vector2>();










    }

    public void FixedUpdate()
    {
        //�ƶ�
        Move();
        Face();





    }






    //�ƶ� 
    public void Move()
    {
        //�����ٶ�
        rb.velocity = new Vector2(inputDirection.x * parameter.speed * Time.deltaTime, inputDirection.y * parameter.speed * Time.deltaTime);

        //�ı�paramater�е�״̬
        if (inputDirection.x != 0 || inputDirection.y != 0)
            parameter.isMove = true;
        else
            parameter.isMove = false;




    }

    
    public void Face()
    {
        //��ɫ���� 
        if (inputDirection.x > 0)
            transform.localScale = new Vector3(1,1,1);
        if (inputDirection.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);



    }































































}
