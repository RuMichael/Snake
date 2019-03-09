using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Snake : MonoBehaviour
{
    int gridWeight = 19, gridHeight = 14;

    Vector2 left = new Vector2(-1f, 0f);
    Vector2 right = new Vector2(1f, 0f);
    Vector2 up = new Vector2(0f, 1f);
    Vector2 down = new Vector2(0f, -1f);
    Vector2 direction;
    Vector2 tmpDirection;

    LinkedList<GameObject> snakeFull = new LinkedList<GameObject>();

    GameObject point;

    int score = 0;
    float speedUp = 0;
    
    public Text scoreText;
    public Text snakeText;

    float speed = 0.4f;
    float speedTime = 0;

    void Start()
    {
        direction = up;
        tmpDirection = up;

        snakeFull.AddFirst((GameObject)Instantiate(Resources.Load("Prefabs/Head", typeof(GameObject)), new Vector3(10f, 4f, 0f), Quaternion.identity));        

        snakeFull.AddLast((GameObject)Instantiate(Resources.Load("Prefabs/Snake", typeof(GameObject)), new Vector3(10f, 3f, 0f), Quaternion.identity));

        snakeFull.AddLast((GameObject)Instantiate(Resources.Load("Prefabs/Snake", typeof(GameObject)), new Vector3(10f, 2f, 0f), Quaternion.identity));

        snakeFull.AddLast((GameObject)Instantiate(Resources.Load("Prefabs/Snake", typeof(GameObject)), new Vector3(10f, 1f, 0f), Quaternion.identity));

        CreateNewPoint();
    }


    void Update()
    {
        UpdateUI();
        if (Input.GetKeyDown(KeyCode.LeftArrow)) direction = left;
        else if (Input.GetKeyDown(KeyCode.RightArrow)) direction = right;
        else if (Input.GetKeyDown(KeyCode.UpArrow)) direction = up;
        else if (Input.GetKeyDown(KeyCode.DownArrow)) direction = down;

        if (Time.time - speedTime >= speed - speedUp)
        {
            speedUp = (float)score / 10000f;
            score++;
            speedTime = Time.time;
            if (MoveSnake())            
                tmpDirection = direction;            
            else           
                direction = tmpDirection;
        }
    }

    void UpdateUI()
    {
        scoreText.text = "Score: \n" + score.ToString();
        snakeText.text = "Snake: \n" + snakeFull.Count.ToString();
    }

    bool MoveSnake()
    {
        GameObject first = PopFirst();
        GameObject last = PopLast();

        Vector3 lastPosition = last.transform.position;

        last.transform.position = first.transform.position;
        snakeFull.AddFirst(last);
        first.transform.position += (Vector3)direction;

        if (CheckValidGrid(first.transform.position) && CheckValidSnake(first.transform.position))
        {
            snakeFull.AddFirst(first);
            if (CheckMoveOnPoint(first.transform.position))
            {
                snakeFull.AddLast((GameObject)Instantiate(Resources.Load("Prefabs/Snake", typeof(GameObject)), lastPosition, Quaternion.identity));
                score += 10;
            }
                 
            
        }
        else
        {
            if (CheckMoveBackSnake(first.transform.position))
            {
                first.transform.position -= (Vector3)direction;
                first.transform.position -= (Vector3)direction;
                snakeFull.AddFirst(first);
                if (CheckMoveOnPoint(first.transform.position))
                {
                    snakeFull.AddLast((GameObject)Instantiate(Resources.Load("Prefabs/Snake", typeof(GameObject)), lastPosition, Quaternion.identity));
                    score += 10;
                }
                
                if (CheckValidGrid(first.transform.position))
                    return false;
            }
            Destroy(first);
            GameOver();
        }
        return true;

    }

    bool CheckMoveBackSnake(Vector3 pos)
    {
        if (snakeFull.First.Next.Value.transform.position.x == pos.x && snakeFull.First.Next.Value.transform.position.y == pos.y)        
            return true;
        return false;
    }

    bool CheckValidSnake(Vector3 pos)
    {
        foreach (var item in snakeFull)
        {
            if (item.transform.position == pos)
                return false;
        }
        return true;
    }

    bool CheckValidGrid(Vector3 pos)
    {
        if (pos.x>=0 && pos.x<=gridWeight && pos.y>=0 && pos.y<=gridHeight)
            return true;
        return false;
    }

    bool CheckMoveOnPoint(Vector3 pos)
    {
        if (point.transform.position == pos)
        {
            Destroy(point);
            CreateNewPoint();            
            return true;
        }
        return false;
    }

    void CreateNewPoint()
    {
        Vector3 pos;
        do
        {
            pos = CreateRandomPosition();
        }
        while (!CheckValidSnake(pos));
        point = (GameObject)Instantiate(Resources.Load("Prefabs/Point", typeof(GameObject)), pos, Quaternion.identity);
    }

    Vector3 CreateRandomPosition()
    {
        return new Vector3(Random.Range(0, gridWeight), Random.Range(0, gridHeight), 0f);
    }

    GameObject PopFirst()
    {
        GameObject tmp = snakeFull.First.Value;
        snakeFull.RemoveFirst();
        return tmp;
    }

    GameObject PopLast()
    {
        GameObject tmp = snakeFull.Last.Value;
        snakeFull.RemoveLast();
        return tmp;
    }

    void GameOver()
    {
        foreach (var item in snakeFull)
            Destroy(item);
        
        Destroy(point);
        SceneManager.LoadScene("GameOver");
    }
}
