using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : Weapon
{
    [SerializeField] private int damage;
    private Animator animator;
    public GameObject effect;
    private LineRenderer laser;
    [SerializeField] LayerMask layer;
    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();
        laser = firePoint.GetComponent<LineRenderer>();
        animator = GetComponent<Animator>();
    }

    public override void Fire()
    {
        Vector2 direction = camara.ScreenToWorldPoint(Input.mousePosition) - player.transform.position;
        RaycastHit2D hit2D = Physics2D.Raycast(firePoint.position, direction, 1000, layer);

        laser.SetPosition(0, firePoint.position);
        laser.SetPosition(1, hit2D.point);
        hit2D.collider?.gameObject.GetComponent<Parameter>()?.TakeDamage(damage);

        effect.transform.position = hit2D.point;
        effect.transform.forward = -direction;
    }

    override public void Update()
    {
        animator.SetBool("isFire", isfire);
        if(isfire){
            laser.enabled = true;
            effect.SetActive(true);
            Fire();
        }else{
            laser.enabled = false;
            effect.SetActive(false);
        }
        
    }
}
