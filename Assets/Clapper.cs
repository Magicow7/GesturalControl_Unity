using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clapper : MonoBehaviour
{
    public Vector3 averageRHPos, averageLHPos;
    public float clapDistThreshold, unclapDistThreshold;
    public placeRipple rippler;
    public MakeSound soundPlayer;
    public List<FishController> fish;

    private bool clapAvailable = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAverageHandPos();
        //LogPositions();
        CheckClap();
    }

    void UpdateAverageHandPos(){
        /*positions are generally between 1(left), 0(right), 0(up), 1(down)
        */
        Vector3 temp = new Vector3(0,0,0);
        foreach(Vector3 pos in Gesture.gen.lefthandpos){
            temp += pos;
        }
        temp /= Gesture.gen.lefthandpos.Length;
        averageLHPos = temp;
        temp = new Vector3(0,0,0);
        foreach(Vector3 pos in Gesture.gen.righthandpos){
            temp += pos;
        }
        temp /= Gesture.gen.righthandpos.Length;
        averageRHPos = temp;
    }

    void CheckClap(){
        if(clapAvailable){
            if(Vector3.Distance(averageLHPos,averageRHPos) <= clapDistThreshold){
                OnClap();
                clapAvailable = false;
            }
        }else{
            if(Vector3.Distance(averageLHPos,averageRHPos) > unclapDistThreshold){
                clapAvailable = true;
            }
        }
        
    }
    void LogPositions(){
        //Debug.Log("RH Average = " + averageRHPos + "\nLH Average = " + averageLHPos);
        //Debug.Log("Distance = " + Vector3.Distance(averageLHPos,averageRHPos));
    }

    void OnDrawGizmos()
    {
        Vector3 baba = new Vector3(2*((-averageRHPos.x) + 0.5f),2*((-averageRHPos.y) + 0.5f), 0);
        Vector3 temp = new Vector3(5 * baba.x,0,5 * baba.y);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(temp,0.05f);
        Vector3 booey = new Vector3(2*((-averageLHPos.x) + 0.5f),2*((-averageLHPos.y) + 0.5f), 0);
        temp = new Vector3(5 * booey.x,0,5 * booey.y);
        Gizmos.DrawSphere(temp,0.05f);
        Gizmos.color = Color.blue;
        Vector3 clapPos = -((averageLHPos + averageRHPos)/2);
        Vector3 positionRatio = new Vector3(2*((clapPos.x) + 0.5f),2*((clapPos.y) + 0.5f), 0);
        temp = new Vector3(5 * positionRatio.x,0,5 * positionRatio.y);
        temp += new Vector3 (0.5f,0,0);//correction to guess correct value
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(temp,0.05f);
    }

    public void OnClap(){
        Debug.Log("CLAP");
        //calculate position ratio
        Vector3 clapPos = -((averageLHPos + averageRHPos)/2);
        Vector3 positionRatio = new Vector3(2*((clapPos.x) + 0.5f),2*((clapPos.y) + 0.5f), 0);
        Debug.Log(positionRatio);
        rippler.forceSpawnRipple(positionRatio, new Vector2(5,5));
        soundPlayer.whenClap(positionRatio);
        foreach(FishController f in fish){
            f.updateTargetPosition();
        }
        /*
        Debug.Log(clapPos);
        rippler.forceSpawnRipple(clapPos, new Vector2(1,1));
        */
        
    }
}
