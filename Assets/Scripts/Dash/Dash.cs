using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
    [Header("冷却条")]
    private Transform coolDownBar;
    public Transform coolDownBarPrefab;
    void Awake()
    {
        coolDownBar = Instantiate(coolDownBarPrefab);
        coolDownBar.position = new Vector2(transform.position.x, transform.position.y + 0.85f);
        coolDownBar.GetComponent<DashCoolDown>().dash = this;
        coolDownBar.SetParent(transform);
        coolDownBar.gameObject.SetActive(false);
        rb = GetComponent<Rigidbody2D>();
    }
    public bool CanDash(){
        return Time.time >= (lastDash + dashCoolDown);
    }
    public void ReadyToDash(Vector2 direction){
        isDashing = true;
        dashTimeLeft = dashTime;
        lastDash = Time.time;
        dashDirection = direction;
        coolDownBar.gameObject.SetActive(true);
    }

    public void Dashing(){
        if(isDashing){
            if(dashTimeLeft > 0){
                
                rb.velocity = dashDirection * dashSpeed;
                dashTimeLeft -= Time.deltaTime;
                var shadow = ShadowPool.instance.shadowPool.Get();
                shadow.GetComponent<ShadowSprite>().Initialization(transform);
            }else{
                
                isDashing = false;
            }     
        }
    }
    public float GetCoolDownState(){
        return 1 - (Time.time - lastDash) / dashCoolDown;
    }
    void Update()
    {
        Dashing();
    }
}
