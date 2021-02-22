using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadMoving : MonoBehaviour
{
    private float time;
    private float deltaTime = 0.15f;
    private Vector3 step = new Vector3(1f, 0f, 0f);
    private bool isPlaying = true;
    private enum State { right = 0, left = 1, up = 2, down = 3 };
    private State state = State.right;
    private int addCount = 0;
    private Vector3 tailPosition = new Vector3(0f, 0f, 0f);
    private Quaternion rotation = Quaternion.Euler(0, 0, 0);

    public GameObject bodyPart;
    public GameObject chershnyaObj;
    private GameObject chereshnya;
    public GameObject tailObj;
    private GameObject tail;
    public GameObject angleObj;
    private List<GameObject> snake = new List<GameObject>();
    private List<State> directions = new List<State>();

    void Start()
    {
        snake.Add(Instantiate(bodyPart));
        snake[0].transform.position = new Vector3(-1f, 0f, 0f);
        tail = Instantiate(tailObj);
        tail.transform.position = new Vector3(-2f, 0f, 0f);
        chereshnya = Instantiate(chershnyaObj);
        chereshnya.transform.position = new Vector3(Random.Range(-8, 8), Random.Range(-7, 7), 0f);
        time = Time.time;
    }

    void Update()
    {
        if (isPlaying)
        {
            if (Time.time - time > deltaTime)
            {
                time = Time.time;
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
                step[0] = 0f;
                step[1] = 1f;
                state = State.up;
                rotation = Quaternion.Euler(0, 0, 90);
            }
            else if (Input.GetKey(KeyCode.DownArrow) && state != State.up)
            {
                step[0] = 0f;
                step[1] = -1f;
                state = State.down;
                rotation = Quaternion.Euler(0, 0, 270);
            }
            else if (Input.GetKey(KeyCode.RightArrow) && state != State.left)
            {
                step[0] = 1f;
                step[1] = 0f;
                state = State.right;
                rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (Input.GetKey(KeyCode.LeftArrow) && state != State.right)
            {
                step[0] = -1f;
                step[1] = 0f;
                state = State.left;
                rotation = Quaternion.Euler(0, 0, 180);
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
        tail.transform.position = snake[snake.Count - 1].transform.position;
        for (int i = snake.Count - 1; i > 1; i--)
        {
            //Debug.Log(snake[i].transform.position.x + " - " + snake[i - 1].transform.position.x + "   " + snake[i].transform.position.y + " - " + snake[i - 1].transform.position.y);
            if ((snake[i].transform.position.x < snake[i - 2].transform.position.x && snake[i].transform.position.y < snake[i - 2].transform.position.y) ||
                (snake[i].transform.position.x > snake[i - 2].transform.position.x && snake[i].transform.position.y > snake[i - 2].transform.position.y))
            {
                //вниз влево, вправо вверх
                Vector3 pos = snake[i - 1].transform.position;
                Destroy(snake[i]);
                snake[i] = Instantiate(angleObj, pos, Quaternion.Euler(0, 0, 0));
            }
            else if ((snake[i].transform.position.x < snake[i - 2].transform.position.x && snake[i].transform.position.y > snake[i - 2].transform.position.y) ||
                (snake[i].transform.position.x > snake[i - 2].transform.position.x && snake[i].transform.position.y < snake[i - 2].transform.position.y))
            {
                //вверх влево, вниз вправо
                Vector3 pos = snake[i - 1].transform.position;
                Destroy(snake[i]);
                snake[i] = Instantiate(angleObj, pos, Quaternion.Euler(0, 0, 90));
            }
            else if ((snake[i].transform.position.x > snake[i - 2].transform.position.x && snake[i].transform.position.y < snake[i - 2].transform.position.y) ||
                (snake[i].transform.position.x < snake[i - 2].transform.position.x && snake[i].transform.position.y > snake[i - 2].transform.position.y))
            {
                Vector3 pos = snake[i - 1].transform.position;
                Destroy(snake[i]);
                snake[i] = Instantiate(angleObj, pos, Quaternion.Euler(0, 0, 90));
            }
            else if ((snake[i].transform.position.x > snake[i - 2].transform.position.x && snake[i].transform.position.y > snake[i - 2].transform.position.y) ||
                (snake[i].transform.position.x < snake[i - 2].transform.position.x && snake[i].transform.position.y < snake[i - 2].transform.position.y))
            {
                Vector3 pos = snake[i - 1].transform.position;
                Destroy(snake[i]);
                snake[i] = Instantiate(angleObj, pos, Quaternion.Euler(0, 0, 0));
            }
            else
            {
                Vector3 pos = snake[i - 1].transform.position;
                Destroy(snake[i]);
                snake[i] = Instantiate(bodyPart, pos, Quaternion.Euler(0, 0, 0));
            }

            /*if (snake[i].transform.position.x < snake[i - 1].transform.position.x)
            {
                if (snake[i].transform.position.y < snake[i - 1].transform.position.y)
                {
                    Vector3 pos = snake[i - 1].transform.position;
                    Destroy(snake[i]);
                    snake[i] = Instantiate(angleObj, pos, Quaternion.Euler(0, 0, 0));
                }
                else if (snake[i].transform.position.y > snake[i - 1].transform.position.y)
                {
                    Vector3 pos = snake[i - 1].transform.position;
                    Destroy(snake[i]);
                    snake[i] = Instantiate(angleObj, pos, Quaternion.Euler(0, 0, 180));
                }
                else
                {
                    Vector3 pos = snake[i - 1].transform.position;
                    Destroy(snake[i]);
                    snake[i] = Instantiate(bodyPart, pos, Quaternion.Euler(0, 0, 0));
                }
            }
            else if (snake[i].transform.position.x < snake[i - 1].transform.position.x)
            {
                if (snake[i].transform.position.y < snake[i - 1].transform.position.y)
                {
                    Vector3 pos = snake[i - 1].transform.position;
                    Destroy(snake[i]);
                    snake[i] = Instantiate(angleObj, pos, Quaternion.Euler(0, 0, 90));
                }
                else if (snake[i].transform.position.y > snake[i - 1].transform.position.y)
                {
                    Vector3 pos = snake[i - 1].transform.position;
                    Destroy(snake[i]);
                    snake[i] = Instantiate(angleObj, pos, Quaternion.Euler(0, 0, 270));
                }
                else
                {
                    Vector3 pos = snake[i - 1].transform.position;
                    Destroy(snake[i]);
                    snake[i] = Instantiate(bodyPart, pos, Quaternion.Euler(0, 0, 0));
                }
            }
            else
            {
                Vector3 pos = snake[i - 1].transform.position;
                Destroy(snake[i]);
                snake[i] = Instantiate(bodyPart, pos, Quaternion.Euler(0, 0, 0));
            }*/
            //snake[i].transform.position = snake[i - 1].transform.position;
        }
        if (snake.Count > 1)
            snake[1].transform.position = snake[0].transform.position;
        snake[0].transform.position = transform.position;
        transform.position += step;
        transform.rotation = rotation;
        if (tail.transform.position.x > snake[snake.Count - 1].transform.position.x)
            tail.transform.rotation = Quaternion.Euler(0, 0, 180);
        else if (tail.transform.position.x < snake[snake.Count - 1].transform.position.x)
            tail.transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (tail.transform.position.y > snake[snake.Count - 1].transform.position.y)
            tail.transform.rotation = Quaternion.Euler(0, 0, 270);
        else if (tail.transform.position.y < snake[snake.Count - 1].transform.position.y)
            tail.transform.rotation = Quaternion.Euler(0, 0, 90);
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