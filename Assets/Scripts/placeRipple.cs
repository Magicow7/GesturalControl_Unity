using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class placeRipple : MonoBehaviour
{
    //prefab spawned to create ripples
    [SerializeField] private GameObject circlePrefab;

    //slider for ripple rate, gets hidden when random ripples are off
    [SerializeField] private GameObject rippleRateSlider;

    //canvas object holding UI
    [SerializeField] private GameObject canvas;

    //audio for opening and closing menus
    [SerializeField] private AudioSource UIOpen;
    [SerializeField] private AudioSource UIClose;

    public static placeRipple instance;

    public void Start(){
        instance = this;
    }

    //booleans for different modes, edited by toggle boxes on UI
    public bool reverseRipple = false;
    public bool randomRipple = false;
    public bool clapMode;
    public Vector3 cameraOffset;

    //values edited by bottom three sliders
    public float radiusExpansionSpeed = 3;
    public float randomRate = 1;
    public float lifetime = 3;
    //RGB values for circles
    public float circleR = 1;
    public float circleG = 1;
    public float circleB = 1;
    //RGB values for background
    public float backgroundR = 0;
    public float backgroundG = 0;
    public float backgroundB = 0;

    //used for time between randomly placed ripples
    private float timeRemain = 1;

    //used for turning the UI on and off
    private bool UIActive = true;

    void start(){
        instance = this;
    }


    // Update is called once per frame
    void Update()
    {
        //enable and disable UI elements with spacebar
        if(Input.GetButtonDown("Jump") && !clapMode){
            UIActive = !UIActive;
            canvas.SetActive(UIActive);
            if(UIActive){
                UIOpen.Play();
            }else{
                UIClose.Play();
            }
        }
        if(!randomRipple && !clapMode){
            //spawns a circle on left mouse button click
            if (Input.GetButtonDown("Fire1"))
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Instantiate(circlePrefab, new Vector3(mousePos.x, 0, mousePos.z), Quaternion.identity);
            }
        }else if(!clapMode){
            //random ripple
            if(timeRemain > 0){
                timeRemain -= Time.deltaTime;
            }else{
                timeRemain = (float)1/randomRate;
                //spawn in random position
                Instantiate(circlePrefab, new Vector3(Random.Range(-9,9), 0, Random.Range(-5,5)), Quaternion.identity);
            }

        }
        


        //update values

        //update background camera color
        //Camera.main.backgroundColor = new Color(backgroundR, backgroundG, backgroundB);

    }

    public void forceSpawnRipple(Vector3 positionRatio, Vector2 screenRatio){
        Vector3 temp = new Vector3(screenRatio.x * positionRatio.x,0,screenRatio.y * positionRatio.y);
        temp += new Vector3(0.5f,0,0);//this is to try to offset a weird inconsisency
        Instantiate(circlePrefab, temp + cameraOffset, Quaternion.identity);
    }

    //below is all the helper functions for the UI
    public void updateBackgroundR(float newValue){
        backgroundR = newValue;
    }

    public void updateBackgroundG(float newValue){
        backgroundG = newValue;
    }

    public void updateBackgroundB(float newValue){
        backgroundB = newValue;
    }

    public void updateCircleR(float newValue){
        circleR = newValue;
    }

    public void updateCircleG(float newValue){
        circleG = newValue;
    }

    public void updateCircleB(float newValue){
        circleB = newValue;
    }

    public void updateLifetime(float newValue){
        lifetime = newValue;
    }

    public void updateRandomRate(float newValue){
        randomRate = newValue;
    }

    public void updateExpansionSpeed(float newValue){
        radiusExpansionSpeed = newValue;
    }

    public void flipReverseRipple(bool newValue){
        reverseRipple = newValue;
    }

    public void flipRandomRipple(bool newValue){
        rippleRateSlider.SetActive(newValue);
        randomRipple = newValue;
    }
}
