using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Transform shadowPrefab;
    [Header("����")]
    private InputSystem inputControl;//��ȡ����ϵͳ
    public Vector2 inputDirection;//��ȡ��������

    [Header("����")]
    private Parameter parameter;//��ȡ�������� 

    [Header("����")]
    private Animator animator;//��ȡ����������

    [Header("����")]
    private Rigidbody2D rb;//��ȡ����

    [Header("����ϵͳ")]
    public GameObject weaponGameObjectIsGot;//����õ�����
    public GameObject weaponGameObjectCanBeGot;
    public Weapon weaponIsGot;
    


    
    




    private void Awake()
    {
        //�˴����ڳ�ʼ���Ļ��
        inputControl = new InputSystem();
        animator = GetComponent<Animator>();
        parameter = GetComponent<Parameter>();
        rb = GetComponent<Rigidbody2D>();
        

        inputControl.GamePlay.Fire.started += FireStart;
        inputControl.GamePlay.Fire.canceled += FireCancle;
        inputControl.GamePlay.GetWeapon.started += GetWeapon;





    }

    private void GetWeapon(InputAction.CallbackContext context)
    {
        Debug.Log("Get!");
        if (weaponGameObjectCanBeGot != null)
        {
            if (weaponGameObjectIsGot != null)
            {
                weaponIsGot = weaponGameObjectIsGot.GetComponent<Weapon>();
                weaponIsGot.enabled = false;
                
            }
            weaponGameObjectIsGot = weaponGameObjectCanBeGot;
            weaponIsGot=weaponGameObjectIsGot.GetComponent<Weapon>();
            weaponIsGot.enabled = true;
        }
    }

    private void FireCancle(InputAction.CallbackContext context)
    {
        if (weaponIsGot != null)
        {
            weaponIsGot.isfire = false;
        }
    }

    private void FireStart(InputAction.CallbackContext context)
    { 
        if (weaponIsGot != null)
        {
            weaponIsGot.isfire = true;
        }
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
        // var shadow = Instantiate(shadowPrefab);
        // shadow.GetComponent<ShadowSprite>().Initialization(GetComponent<Transform>());









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






    private void OnTriggerEnter2D(Collider2D other )
    {
        Debug.Log("Trigger!");
        if (other.gameObject.CompareTag("Weapon"))
        {
            weaponGameObjectCanBeGot = other.gameObject;
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Weapon"))
        {
            weaponGameObjectCanBeGot = null;
        }
    }





















































}
