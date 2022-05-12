using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{

    public GameObject cam;
    private Vector2 length;
    private Vector2 startPos;
    public float parallaxEffect;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        length = GetComponent<SpriteRenderer>().bounds.size;
    }

    // Update is called once per frame
    void Update()
    {
        float temp = (cam.transform.position.x *  (1- parallaxEffect));
        Vector2 dist = (cam.transform.position * parallaxEffect);

        transform.position = startPos + dist;

        if(temp > startPos[0] + length[0]) startPos[0]+= length[0];
        else if(temp< startPos[0] - length[0]) startPos[0] -= length[0];
    }
}
