using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    private Rigidbody2D rb;
    [Header("Dash参数")]
    [SerializeField] private float dashTime;
    private float dashTimeLeft;
    private float lastDash = -10f;
    [SerializeField] private float dashCoolDown;
    private Vector2 dashDirection;
    [SerializeField] private float dashSpeed;
    public bool isDashing;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public bool CanDash(){
        return (Time.time >= (lastDash + dashCoolDown));
    }
    public void ReadyToDash(Vector2 direction){
        isDashing = true;
        dashTimeLeft = dashTime;
        lastDash = Time.time;
        dashDirection = direction;
    }

    public void Dashing(){
        if(isDashing){
            if(dashTimeLeft > 0){
                Debug.Log(dashDirection);
                rb.velocity = dashDirection * dashSpeed;
                dashTimeLeft -= Time.deltaTime;
                var shadow = ShadowPool.instance.shadowPool.Get();
                shadow.GetComponent<ShadowSprite>().Initialization(transform);
            }else{
                isDashing = false;
            }     
        }
    }
    void FixedUpdate()
    {
        Dashing();
    }
}
