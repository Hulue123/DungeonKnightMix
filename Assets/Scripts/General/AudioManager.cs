using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    
    public static AudioSource FX;
    public static AudioClip junbiao6;
    public static AudioClip junbiaoisHurt;
    public static AudioClip junbiaoisDead;




    private void Awake()
    {
        
        FX= GetComponent<AudioSource>();
        junbiao6 = Resources.Load<AudioClip>("Junbiao/6");
        junbiaoisHurt = Resources.Load<AudioClip>("Junbiao/woshichongzhi");
        junbiaoisDead = Resources.Load<AudioClip>("Junbiao/woshichongzhifangguowoba");
    }




    public static void PlayJunBiao6()
    {
        FX.PlayOneShot(junbiao6);
    }


    public static void PlayJunBiaoIsHurt()
    {
        FX.PlayOneShot(junbiaoisHurt);
    }

    public static void PlayJunBiaoIsDead()
    {
        FX.PlayOneShot(junbiaoisDead);
    }






}
