using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{

    private Animator anim;
    private Parameter parameter;





    private void Awake()
    {
        anim = GetComponent<Animator>();
        parameter = GetComponent<Parameter>();
    }



    private void Update()
    {
        anim.SetBool("isMove", parameter.isMove);



    }



























}














