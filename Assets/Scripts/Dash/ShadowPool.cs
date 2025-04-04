using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class ShadowPool : MonoBehaviour
{
    public GameObject shadowPrefab;
    private ObjectPool<GameObject> shadowPool;

    void Awake()
    {
        shadowPool = new ObjectPool<GameObject>(createFunc, actionOnGet, actionOnRelease, actionOnDestroy, true, 10, 1000);
    }

    private void actionOnDestroy(GameObject obj)
    {
        Destroy(obj);
    }

    private void actionOnRelease(GameObject obj)
    {
        obj.SetActive(false);
    }
    private void actionOnGet(GameObject obj)
    {
        obj.SetActive(true);
        //obj.GetComponent<ShadowSprite>()?.Initialization();
    }

    private GameObject createFunc()
    {
        var obj = Instantiate(shadowPrefab);
        obj.GetComponent<ShadowSprite>().shadowPool = shadowPool;
        return obj;
    }
}
