using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyGeneration : MonoBehaviour
{
    public GameObject bodyPart;
    public GameObject headObj;
    public GameObject chershnyaObj;
    private GameObject head;
    private GameObject chereshnya;
    private List<GameObject> snake = new List<GameObject>();
    private Vector3 movement;

    // Start is called before the first frame update
    void Start()
    {
        /*head = Instantiate(headObj);
        head.transform.position = new Vector3(0f, 0f, 0f);
        movement = head.transform.position;
        snake.Add(Instantiate(bodyPart));
        snake[0].transform.position = new Vector3(-1f, 0f, 0f);
        snake.Add(Instantiate(bodyPart));
        snake[1].transform.position = new Vector3(-2f, 0f, 0f);
        chereshnya = Instantiate(chershnyaObj);
        chereshnya.transform.position = new Vector3(Random.Range(-8, 8), Random.Range(-7, 7), 0f);*/
    }

    // Update is called once per frame
    void Update()
    {

        /*if(head.transform.position != movement)
        {
            for (int i = snake.Count - 1; i > 0; i--)
            {
                snake[i].transform.position = snake[i - 1].transform.position;
            }
            snake[0].transform.position = movement;
            movement = head.transform.position;
        }*/
    }
}
