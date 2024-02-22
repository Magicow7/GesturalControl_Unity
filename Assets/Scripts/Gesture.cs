//IMDM Course material
// Author: Myungin Lee
// Date: Fall 2023
// This code demonstrates the general applications of landmark information
// Pose + Left, Right hand landmarks data avaiable. Facial landmark need custom work
// Landmarks label reference:
// https://developers.google.com/mediapipe/solutions/vision/pose_landmarker
// https://developers.google.com/mediapipe/solutions/vision/hand_landmarker

using Mediapipe.Unity.Holistic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gesture : MonoBehaviour
{
    static int poseLandmark_number = 32;
    static int handLandmark_number = 20;
    // Declare landmark vectors
    public Vector3[] pose = new Vector3[poseLandmark_number];
    public Vector3[] righthandpos = new Vector3[handLandmark_number];
    public Vector3[] lefthandpos = new Vector3[handLandmark_number];
    public GameObject[] PoseLandmarks, LeftHandLandmarks, RightHandLandmarks;
    public static Gesture gen; // singleton
    public bool drawLandmarks = false;
    public bool trigger = false;
    private float distance;
    int totalNumberofLandmark;

    public GameObject obj; //I ADDED

    int[,] linePairsL = new int[,] { {4,3}, {3,2}, {2,1}, {1,0}, {8,7}, {7,6}, {6,5}, {5,0},
{12,11}, {11,10}, {10,9}, {16,15}, {15,14}, {14,13},
{20, 19}, {19,18}, {18,17}, {17,0}}; //I ADDED

    int[,] linePairsR = new int[,] { {4,3}, {3,2}, {2,1}, {1,0}, {8,7}, {7,6}, {6,5}, {5,0},
{12,11}, {11,10}, {10,9}, {16,15}, {15,14}, {14,13},
{20, 19}, {19,18}, {18,17}, {17,0}}; //I ADDED
    private GameObject[] capsuleContainerL; //I ADDED

    private GameObject[] capsuleContainerR; //I ADDED


    private void Awake()
    {
        if (Gesture.gen == null)
        {
            Gesture.gen = this;
        }
        totalNumberofLandmark = poseLandmark_number + handLandmark_number + handLandmark_number;
        PoseLandmarks = new GameObject[poseLandmark_number];
        LeftHandLandmarks = new GameObject[handLandmark_number];
        RightHandLandmarks = new GameObject[handLandmark_number];
    }
    // Start is called before the first frame update
    void Start()


    {
        // Initiate R+L hands landmarks as spheres
        for (int i = 0; i < handLandmark_number; i++)
        {
            LeftHandLandmarks[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            RightHandLandmarks[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            LeftHandLandmarks[i].GetComponent<MeshRenderer>().enabled = false;
            RightHandLandmarks[i].GetComponent<MeshRenderer>().enabled = false;
        }

        capsuleContainerL = new GameObject[linePairsL.GetLength(0)];
        for (int i = 0; i < capsuleContainerL.Length; i++)
        {
            capsuleContainerL[i] = Instantiate(obj);
            capsuleContainerL[i].SetActive(false);

        }

        capsuleContainerR = new GameObject[linePairsL.GetLength(0)];
        for (int i = 0; i < capsuleContainerR.Length; i++)
        {
            capsuleContainerR[i] = Instantiate(obj);
            capsuleContainerR[i].SetActive(false);

        }

        if (drawLandmarks)
        {
            // Initiate pose landmarks as spheres
            for (int i = 0; i < poseLandmark_number; i++)
            {
                PoseLandmarks[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                PoseLandmarks[i].GetComponent<MeshRenderer>().enabled = false;
            }

        }
    }
    // Update is called once per frame

    void placeCapsule(GameObject obj, Vector3 pos1, Vector3 pos2, float distance)
    {
        Vector3 v = pos2 - pos1;
        float fl = v.magnitude;
        obj.transform.position = pos1 + v / 2;
        obj.transform.up = v;
        obj.transform.localScale = new Vector3(distance, fl / 2, distance);
    }
    void Update()
    {
        // Case 0. Draw holistic shape
        // Assign Pose landmarks position
        int idx = 0;

        idx = 0;
        foreach (GameObject lhl in LeftHandLandmarks)
        {
            lhl.transform.transform.position = -lefthandpos[idx];
            Color customColor = new Color(idx * 4 / 255, idx * 15f / 255, idx * 30f / 255, 1); // Color of left hand landmarks
            lhl.GetComponent<Renderer>().material.SetColor("_Color", customColor);
            lhl.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            idx++;
        }
        // Assign Right hand landmarks position
        idx = 0;
        foreach (GameObject rhl in RightHandLandmarks)
        {
            rhl.transform.transform.position = -righthandpos[idx];
            Color customColor = new Color(idx * 4f / 255, idx * 15f / 255, idx * 30f / 255, 1); // Color of right hand landmarks
            rhl.GetComponent<Renderer>().material.SetColor("_Color", customColor);
            rhl.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            idx++;
        }

        for (int i = 0; i < linePairsL.GetLength(0); i++)
        {
            int first = linePairsL[i, 0];
            int second = linePairsL[i, 1];

            if (first < LeftHandLandmarks.Length && second < LeftHandLandmarks.Length)
            {
                GameObject capsule = capsuleContainerL[i];
                capsule.SetActive(true);
                Vector3 pos1 = LeftHandLandmarks[first].transform.position;
                pos1 = new Vector3(10 * ((pos1.x) + 0.5f), 10 * (pos1.y + 0.5f), 10);//5 * ((pos1.z) + .5f));
                Vector3 pos2 = LeftHandLandmarks[second].transform.position;
                pos2 = new Vector3(10 * ((pos2.x) + 0.5f), 10 * ((pos2.y) + 0.5f), 10);//5 * ((pos2.z) + .5f));

                placeCapsule(capsule, pos1,
                pos2, .05f);

            }
        }

        for (int i = 0; i < linePairsR.GetLength(0); i++)
        {
            int first = linePairsR[i, 0];
            int second = linePairsR[i, 1];

            if (first < RightHandLandmarks.Length && second < RightHandLandmarks.Length)
            {
                GameObject capsule = capsuleContainerR[i];
                capsule.SetActive(true);
                Vector3 posr1 = RightHandLandmarks[first].transform.position;
                posr1 = new Vector3(10 * ((posr1.x) + 0.5f), 10 * ((posr1.y) + 0.5f), 10);//5 * ((posr1.z) + .5f));
                Vector3 posr2 = RightHandLandmarks[second].transform.position;
                posr2 = new Vector3(10 * ((posr2.x) + 0.5f), 10 * ((posr2.y) + 0.5f), 10);//5 * ((posr2.z) + .5f));

                placeCapsule(capsule, posr1,
                posr2, .05f);


            }
        }
        idx = 0;
        if (drawLandmarks)
        {
            foreach (GameObject pl in PoseLandmarks)
            {
                pl.transform.transform.position = -pose[idx];
                Color customColor = new Color(idx * 10 / 255, idx * 7 / 255, idx * 3 / 255, 1); // Color of pose landmarks
                pl.GetComponent<Renderer>().material.SetColor("_Color", customColor);
                pl.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                idx++;
            }
            // Assign Left hand landmarks position


        }
    }
}
