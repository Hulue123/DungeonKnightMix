using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parameter : MonoBehaviour
{

    [Header("��������")]
    public int maxHealth;
    public int currentHelath;
    public int shield;
    public float speed;

    [Header("����״̬")]
    public bool isMove;
    public bool isHurt;
    public bool isDead;


    private void Awake()
    {
        isMove = false;
        isHurt = false;
        currentHelath = maxHealth;
    }









    public void TakeDamage(int damage)
    {
        if (currentHelath - damage >= 0)
        {
            currentHelath -= damage;
        }
        else
        {
            currentHelath = 0;
            isDead = true;
            Destroy(gameObject);
        }
    }












}
