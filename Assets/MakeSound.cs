using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeSound : MonoBehaviour
{
    int xPos;
    int yPos;
    public AudioSource sound;
    public AudioClip soundClip;
    Vector3 xyzPos = new Vector3 (1, 1, 0);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
            /*
        if (Input.GetButtonDown("Fire1"))
        {
            Vector3 mousePos = Input.mousePosition;
            {
                Vector3 xyzPos1 = new Vector3 (mousePos.x, mousePos.y, mousePos.z);
                whenClap(xyzPos1);
            }
        }
        */
    }

    //Lowest value is -1, highest is 1.
    public void whenClap(Vector3 pos)
    {
        sound.panStereo = pos.x;
        sound.pitch = (pos.y/2) + 1;
        sound.PlayOneShot(soundClip);
    }
}
