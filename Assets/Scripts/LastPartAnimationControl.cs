using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastPartAnimationControl : MonoBehaviour
{
    private Animator anim;
    private Vector3 postion = new Vector3();

    void Start()
    {
        anim = GetComponent<Animator>();
        postion = transform.position;
    }

    void Update()
    {
        if (postion != transform.position)
        {
            postion = transform.position;
            anim.Play("layer.LastPartAnimation", 0, 0.25f);
        }
    }
}
