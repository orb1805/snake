using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartMoving : MonoBehaviour
{
    public GameObject nextPart;
    private Vector3 movement;

    // Start is called before the first frame update
    void Start()
    {
        movement = nextPart.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (nextPart.transform.position != movement)
        {
            transform.position = movement;
            movement = nextPart.transform.position;
        }
    }
}
