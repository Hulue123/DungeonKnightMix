using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parameter : MonoBehaviour
{

    [Header("��������")]
    public int health;
    public int shield;
    public float speed;

    [Header("����״̬")]
    public bool isMove;
    public bool ishurt;


    private void Awake()
    {
        isMove = false;
        ishurt = false;
    }






















}
