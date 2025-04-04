using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ShadowSprite : MonoBehaviour
{
    private bool isInitialaized;
    public ObjectPool<GameObject> shadowPool;
    //private Transform player;

    private SpriteRenderer thisSprite;
    private SpriteRenderer playerSprite;

    private Color color;//当前颜色

    [Header("时间控制参数")]
    [SerializeField] private float activeTime;//活跃持续时间
    private float activeStart;//活跃开始时间
    [Header("不透明度控制")]
    private float alpha;//当前不透明度
    [SerializeField] private float alphaSet;//初始不透明度
    [SerializeField] private float alphaMultiplier;//不透明度降低系数

    public void Initialization(Transform player)
    {
        thisSprite = GetComponent<SpriteRenderer>();
        playerSprite = player.GetComponent<SpriteRenderer>();

        alpha = alphaSet;

        thisSprite.sprite = playerSprite.sprite;

        transform.position = player.position;
        transform.localScale = player.localScale;
        transform.rotation = player.rotation;

        activeStart = Time.time;

        isInitialaized = true;
    }
    void FixedUpdate()
    {
        if(isInitialaized){
            //不透明度逐渐降低
            alpha *= alphaMultiplier;
            color = new Color(0.5f, 0.5f, 1, alpha);
            thisSprite.color = color;
            if(Time.time >= activeStart + activeTime){
                //返回对象池
                
            }
        }
    }
}
