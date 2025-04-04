using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashCoolDown : MonoBehaviour
{
    
    [SerializeField] private Transform coolDown;
    public Dash dash;

    private void SetCoolDownBar(){
        float temp = dash.GetCoolDownState();
        if(temp < 0){
            gameObject.SetActive(false);
        }else{
            coolDown.localScale = new(temp, 1);
            coolDown.position = new(transform.position.x - (1 - temp)/2,transform.position.y + 0);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
        SetCoolDownBar();
    }
}
