using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class circleDrawer : MonoBehaviour
{
    /*
        the circle is drawn by segmenting it into line renderers between two points. the more points added, the more acurate the circle will be.

        NOTE: the math to draw the points in a circle was a little wonky, so to compensate, the camera is looking in the y- direction instead of the z- direction
        like a normal unity camera.

    */
    //line renderer prefab to be put between points
    public LineRenderer linePrefab;
    //amount of points used to draw the circle
    public int points;
    //current radius of circle
    public float radius;
    //multiplier for speed of expansion of circle
    public float radiusExpansionMultiplier;
    //width of individual lines on circle
    public float lineWidth;
    //starting color for lines
    public Color startColor;
    //current color for lines
    private Color currentColor;
    //final color for lines
    public Color endColor;
    //center of ripple represented as vector3
    public Vector3 center;
    //these Vector2s represent the outer constraints of the screen to be used for richocheting of walls.
    public Vector2 xConstraints;
    public Vector2 zConstraints;
    //total lifetime of ripple
    public float lifetime;
    //current time alive for ripple
    private float timeAlive;
    //basic material used by all line renderers
    public Material lineMaterial;
    //list of all points
    private List<Vector3> positions = new List<Vector3>();
    //list of all line renderers
    private List<LineRenderer> lines = new List<LineRenderer>();
    //boolean used as a flag for reversing lifetime
    private bool reverse;


    private void Start(){
        lifetime = placeRipple.instance.lifetime;
        reverse = placeRipple.instance.reverseRipple;
        radiusExpansionMultiplier = placeRipple.instance.radiusExpansionSpeed;
        center = transform.position;
        if(reverse){
            timeAlive = lifetime;
        }
        placePoints();
        newLines();
        updateLines();
        
    }

    private void FixedUpdate(){
        updateRadius();
        updateColor();
        placePoints();
        updateLines();
        if(!reverse){
            timeAlive += Time.deltaTime;
            if(timeAlive >= lifetime){
                endRipple();
            }
        }else{
            timeAlive -= Time.deltaTime;
            if(timeAlive <= 0){
                endRipple();
            }
        }
        
    }

    //place points for lines
    private void placePoints(){
        positions.Clear();
        for(int i = 0; i < points; i++){
            //note: the camera is rotated to be looking down in the -y direction instead of looking in the -z direction like a normal camera to accomodate this math.
            Vector3 newPoint = center + Quaternion.Euler(0,365*((float)i/points),0) * (Vector3.forward*radius);
            newPoint = constrainPoint(newPoint);
            positions.Add(newPoint);
        }
      
    }

    //helper recursive function for placing points to let them bounce off walls;
    private Vector3 constrainPoint(Vector3 input){
        if(input.x > xConstraints.x){
            float difference = input.x - xConstraints.x;
            input.x = xConstraints.x - difference;
            return constrainPoint(input);
        }else if(input.x < xConstraints.y){
            float difference = input.x - xConstraints.y;
            input.x = xConstraints.y - difference;
            return constrainPoint(input);
        }else if(input.z > zConstraints.x){
            float difference = input.z - zConstraints.x;
            input.z = zConstraints.x - difference;
            return constrainPoint(input);
        }else if(input.z < zConstraints.y){
            float difference = input.z - zConstraints.y;
            input.z = zConstraints.y - difference;
            return constrainPoint(input);
        }else{
            return input;
        }
    }

    //create new line renderer objects to be edited by updateLines()
    private void newLines(){
        for(int i = 0; i < points; i++){
            LineRenderer lineRenderer = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
            lineRenderer.startWidth = lineWidth;
            lineRenderer.endWidth = lineWidth;
            lineRenderer.positionCount = 2;
            lineRenderer.useWorldSpace = true;
            lineRenderer.materials[0] = lineMaterial;

            startColor = new Color(placeRipple.instance.circleR, placeRipple.instance.circleG, placeRipple.instance.circleB); 
            endColor = new Color(placeRipple.instance.backgroundR, placeRipple.instance.backgroundG, placeRipple.instance.backgroundB);

            lines.Add(lineRenderer);
        }
    }

    //Update teh color and posiiton of the lines
    private void updateLines(){
        //lineMaterial.color = currentColor;
        for(int i = 0; i < points; i++){
            LineRenderer lineRenderer = lines[i];
            lineRenderer.SetWidth(0.1f*(1-((float)timeAlive/lifetime)), 0.1f*(1-((float)timeAlive/lifetime)));

            //lineRenderer.materials[0].color = currentColor;
            //update color/*
            //lineRenderer.SetColors(currentColor, currentColor);
            /*
            lineRenderer.startColor = currentColor;
            lineRenderer.endColor = currentColor;
            
            Debug.Log(currentColor == lineRenderer.startColor);
            */

            Vector3 position1;
            Vector3 position2;

            if(i != points - 1){
                //normal case
                position1 = positions[i];
                position2 = positions[i + 1];
            }else{
                //case of last index
                position1 = positions[i];
                position2 = positions[0];
            }

            lineRenderer.SetPosition(0, position1); 
            lineRenderer.SetPosition(1, position2);
        }
    }

    //update radius with multiplier
    private void updateRadius(){
        radius = timeAlive * radiusExpansionMultiplier;
    }

    //update color based on lifetime
    private void updateColor(){
        //currentColor = new Color(color.r, color.g, color.b, 1-((float)timeAlive/lifetime));
        currentColor = Color.Lerp(startColor, endColor, (float)timeAlive/lifetime);
    }

    //delete at end of lifetime
    private void endRipple(){
        for(int i = 0; i < lines.Count; i++){
            Destroy(lines[i].gameObject);
        }
        Destroy(this.gameObject);
    }
}
