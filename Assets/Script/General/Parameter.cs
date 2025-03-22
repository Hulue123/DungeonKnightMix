using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parameter : MonoBehaviour
{

    [Header("基本属性")]
    public int health;
    public int shield;
    public float speed;

    [Header("基本状态")]
    public bool isMove;
    public bool ishurt;


    private void Awake()
    {
        isMove = false;
        ishurt = false;
    }






















}
