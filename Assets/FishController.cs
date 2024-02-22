using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishController : MonoBehaviour
{
    public Vector3 targetPosition;
    public Animator anim;
    public float animSpeed;
    private float minSwimSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        animSpeed = Mathf.Clamp(10*Vector3.Distance(transform.position, targetPosition),minSwimSpeed,25);
        anim.speed = animSpeed;
        if(Vector3.Distance(transform.position, targetPosition) < 0.01f){
            transform.position = targetPosition;
        }else{
            transform.position = Vector3.Lerp(transform.position, targetPosition, 0.1f);
        }
        
    }

    public void updateTargetPosition(){
        Vector3 temp = new Vector3(Random.Range(-8,8),0,Random.Range(-4,4));
        if(Vector3.Distance(temp,targetPosition) < 3){
            updateTargetPosition();
        }else{
            targetPosition = temp;
            transform.LookAt(targetPosition);
            transform.eulerAngles += new Vector3(0,90,0);
            minSwimSpeed = Random.Range(1,1.5f);
        }
    }
}
