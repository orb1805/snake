using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadMoving : MonoBehaviour
{
    private float time;
    private float deltaTime = 0.32f;
    private Vector3 step = new Vector3(1f, 0f, 0f);
    private bool isPlaying = true;
    private enum State { right = 0, left = 1, up = 2, down = 3, downLeft = 4, upLeft = 5, rightUp = 6, rightDown = 7, leftUp = 8, downRight = 9, upRight = 10, leftDown = 11 };
    private State state = State.right;
    private State tailState = State.right;
    private int addCount = 0;
    private Vector3 tailPosition = new Vector3(0f, 0f, 0f);
    private Quaternion rotation = Quaternion.Euler(0, 0, 0);
    private bool addFlag = false;

    public GameObject bodyPart;
    public GameObject chershnyaObj;
    private GameObject chereshnya;
    //public GameObject tailObj;
    public GameObject tail;
    public GameObject angleObj;
    private List<GameObject> snake = new List<GameObject>();
    private List<State> directions = new List<State>();

    private Animator anim;

    void Start()
    {
        snake.Add(Instantiate(bodyPart));
        snake[0].transform.position = new Vector3(-1f, 0f, 0f);
        directions.Add(State.right);
        //tail = Instantiate(tailObj);
        tail.transform.position = new Vector3(-2f, 0f, 0f);
        chereshnya = Instantiate(chershnyaObj);
        chereshnya.transform.position = new Vector3(Random.Range(-8, 8), Random.Range(-7, 7), 0f);
        time = Time.time;

        anim = GetComponent<Animator>();
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
                    directions.Add(tailState);
                    addFlag = true;
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
        if (addFlag)
        {
            snake.Add(Instantiate(bodyPart, snake[snake.Count - 1].transform.position, Quaternion.Euler(0, 0, 0)));
            addFlag = false;
        }
        else
            tail.transform.position = snake[snake.Count - 1].transform.position;
        for (int i = snake.Count - 1; i > 0; i--)
        {
            if (directions[i] != directions[i - 1])
            {
                Destroy(snake[i]);
                snake[i] = bodyPartGeneration(directions[i - 1], snake[i - 1].transform.position);
                directions[i] = directions[i - 1];
            }
            else
                snake[i].transform.position = snake[i - 1].transform.position;
        }
        State dir = generateDirection();
        if (dir != directions[0])
        {
            directions[0] = dir;
            Destroy(snake[0]);
            snake[0] = bodyPartGeneration(directions[0], transform.position);
        }
        else
            snake[0].transform.position = transform.position;

        transform.position += step;
        transform.rotation = rotation;

        anim.Play("layer.HeadMoveAnimation", 0, 0f);
        //anim.PlayInFixedTime("layer.HeadMoveAnimation", 0, 0.25f);
        //Debug.Log("play anim");

        if (tail.transform.position.x > snake[snake.Count - 1].transform.position.x)
        {
            tail.transform.rotation = Quaternion.Euler(0, 0, 180);
            tailState = State.left;
        }
        else if (tail.transform.position.x < snake[snake.Count - 1].transform.position.x)
        {
            tail.transform.rotation = Quaternion.Euler(0, 0, 0);
            tailState = State.right;
        }
        else if (tail.transform.position.y > snake[snake.Count - 1].transform.position.y)
        {
            tail.transform.rotation = Quaternion.Euler(0, 0, 270);
            tailState = State.down;
        }
        else if (tail.transform.position.y < snake[snake.Count - 1].transform.position.y)
        {
            tail.transform.rotation = Quaternion.Euler(0, 0, 90);
            tailState = State.up;
        }
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

    private GameObject bodyPartGeneration(State state, Vector3 position)
    {
        switch (state)
        {
            case State.rightUp:
                return Instantiate(angleObj, position, Quaternion.Euler(0, 0, 0));
            case State.downLeft:
                return Instantiate(angleObj, position, Quaternion.Euler(0, 0, 0));
            case State.upLeft:
                return Instantiate(angleObj, position, Quaternion.Euler(0, 0, 90));
            case State.rightDown:
                return Instantiate(angleObj, position, Quaternion.Euler(0, 0, 90));
            case State.leftDown:
                return Instantiate(angleObj, position, Quaternion.Euler(0, 0, 180));
            case State.upRight:
                return Instantiate(angleObj, position, Quaternion.Euler(0, 0, 180));
            case State.leftUp:
                return Instantiate(angleObj, position, Quaternion.Euler(0, 0, 270));
            case State.downRight:
                return Instantiate(angleObj, position, Quaternion.Euler(0, 0, 270));
        }
        return Instantiate(bodyPart, position, Quaternion.Euler(0, 0, 0));
    }

    private State generateDirection()
    {
        State dir = State.right;
        switch (directions[0])
        {
            case State.down:
                switch (state)
                {
                    case State.left:
                        dir = State.downLeft;
                        break;
                    case State.right:
                        dir = State.downRight;
                        break;
                    case State.down:
                        dir = State.down;
                        break;
                }
                break;
            case State.rightDown:
                switch (state)
                {
                    case State.left:
                        dir = State.downLeft;
                        break;
                    case State.right:
                        dir = State.downRight;
                        break;
                    case State.down:
                        dir = State.down;
                        break;
                }
                break;
            case State.leftDown:
                switch (state)
                {
                    case State.left:
                        dir = State.downLeft;
                        break;
                    case State.right:
                        dir = State.downRight;
                        break;
                    case State.down:
                        dir = State.down;
                        break;
                }
                break;

            case State.up:
                switch (state)
                {
                    case State.left:
                        dir = State.upLeft;
                        break;
                    case State.right:
                        dir = State.upRight;
                        break;
                    case State.up:
                        dir = State.up;
                        break;
                }
                break;
            case State.leftUp:
                switch (state)
                {
                    case State.left:
                        dir = State.upLeft;
                        break;
                    case State.right:
                        dir = State.upRight;
                        break;
                    case State.up:
                        dir = State.up;
                        break;
                }
                break;
            case State.rightUp:
                switch (state)
                {
                    case State.left:
                        dir = State.upLeft;
                        break;
                    case State.right:
                        dir = State.upRight;
                        break;
                    case State.up:
                        dir = State.up;
                        break;
                }
                break;

            case State.right:
                switch (state)
                {
                    case State.up:
                        dir = State.rightUp;
                        break;
                    case State.down:
                        dir = State.rightDown;
                        break;
                    case State.right:
                        dir = State.right;
                        break;
                }
                break;
            case State.downRight:
                switch (state)
                {
                    case State.up:
                        dir = State.rightUp;
                        break;
                    case State.down:
                        dir = State.rightDown;
                        break;
                    case State.right:
                        dir = State.right;
                        break;
                }
                break;
            case State.upRight:
                switch (state)
                {
                    case State.up:
                        dir = State.rightUp;
                        break;
                    case State.down:
                        dir = State.rightDown;
                        break;
                    case State.right:
                        dir = State.right;
                        break;
                }
                break;

            case State.left:
                switch (state)
                {
                    case State.up:
                        dir = State.leftUp;
                        break;
                    case State.down:
                        dir = State.leftDown;
                        break;
                    case State.left:
                        dir = State.left;
                        break;
                }
                break;
            case State.upLeft:
                switch (state)
                {
                    case State.up:
                        dir = State.leftUp;
                        break;
                    case State.down:
                        dir = State.leftDown;
                        break;
                    case State.left:
                        dir = State.left;
                        break;
                }
                break;
            case State.downLeft:
                switch (state)
                {
                    case State.up:
                        dir = State.leftUp;
                        break;
                    case State.down:
                        dir = State.leftDown;
                        break;
                    case State.left:
                        dir = State.left;
                        break;
                }
                break;
        }
        return dir;
    }
}