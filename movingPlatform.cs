using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingPlatform : MonoBehaviour
{
    [SerializeField] private GameObject [] points;

    public float speedPlatform;
    
    private int i = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if( Vector2.Distance(points[i].transform.position, transform.position) < 0.1f)
        {
            i++;
            if(i >= points.Length)
                i=0;
        }
        transform.position = Vector2.MoveTowards(transform.position, points[i].transform.position, Time.deltaTime * speedPlatform);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
            other.transform.SetParent(transform);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        other.transform.SetParent(null);
    }

}
