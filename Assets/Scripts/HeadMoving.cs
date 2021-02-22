using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadMoving : MonoBehaviour
{
    private int skip = 0;
    private Vector3 step = new Vector3(1f, 0f, 0f);
    private bool isPlaying = true;
    private enum State { right = 0, left = 1, up = 2, down = 3 };
    private State state = State.right;
    private int addCount = 0;
    private Vector3 tailPosition = new Vector3(0f, 0f, 0f);

    public GameObject bodyPart;
    public GameObject chershnyaObj;
    private GameObject chereshnya;
    private List<GameObject> snake = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        snake.Add(Instantiate(bodyPart));
        snake[0].transform.position = new Vector3(-1f, 0f, 0f);
        snake.Add(Instantiate(bodyPart));
        snake[1].transform.position = new Vector3(-2f, 0f, 0f);
        chereshnya = Instantiate(chershnyaObj);
        chereshnya.transform.position = new Vector3(Random.Range(-8, 8), Random.Range(-7, 7), 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying)
        {
            if (skip < 25)
                skip++;
            else
            {
                skip = 0;
                tailPosition[0] = snake[snake.Count - 1].transform.position.x;
                tailPosition[1] = snake[snake.Count - 1].transform.position.y;
                tailPosition[2] = snake[snake.Count - 1].transform.position.z;
                moveSnake();
                if (addCount > 0)
                {
                    snake.Add(Instantiate(bodyPart, tailPosition, Quaternion.Euler(0, 0, 0)));
                    addCount--;
                }
            }
            if (Input.GetKey(KeyCode.UpArrow) && state != State.down)
            {
                step = new Vector3(0f, 1f, 0f);
                state = State.up;
            }
            else if (Input.GetKey(KeyCode.DownArrow) && state != State.up)
            {
                step = new Vector3(0f, -1f, 0f);
                state = State.down;
            }
            else if (Input.GetKey(KeyCode.RightArrow) && state != State.left)
            {
                step = new Vector3(1f, 0f, 0f);
                state = State.right;
            }
            else if (Input.GetKey(KeyCode.LeftArrow) && state != State.right)
            {
                step = new Vector3(-1f, 0f, 0f);
                state = State.left;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<SpriteRenderer>().color == Color.red)
        {
            Destroy(other.gameObject);
            chereshnya = Instantiate(chershnyaObj);
            chereshnya.transform.position = newChereshnyaPosition();
            addCount++;
        }
        else
            isPlaying = false;
    }

    private void moveSnake()
    {
        for (int i = snake.Count - 1; i > 0; i--)
        {
            snake[i].transform.position = snake[i - 1].transform.position;
        }
        snake[0].transform.position = transform.position;
        transform.position += step;
    }

    private Vector3 newChereshnyaPosition()
    {
        //лучше тут сделать более детерменированный алгоритм
        bool flag = true;
        Vector3 result = new Vector3(0, 0, 0);
        while (flag)
        {
            flag = false;

            result[0] = Random.Range(-8, 8);
            result[1] = Random.Range(-7, 7);
            foreach (GameObject i in snake)
                if (i.transform.position == result)
                    flag = true;
        }
        return result;
    }
}