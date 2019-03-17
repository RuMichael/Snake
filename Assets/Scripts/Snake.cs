using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Snake : MonoBehaviour
{
    enum block { Head, Point, Snake }

    const int gridWeight = 19, gridHeight = 14;

    Vector3 direction;
    Vector3 tmpDirection;
    
    LinkedList<Transform> snakeF = new LinkedList<Transform>();

    Transform pointPos;

    int score = 0;
    float speedUp = 0;          //модификатор скорости передвижения змейки
    
    public Text scoreText;
    public Text snakeText;

    const float speed = 0.4f;         //скорость движения змейки
    float speedTime = 0;

    void Start()
    {        
        direction = Vector3.up;
        tmpDirection = Vector3.up;

        snakeF.AddFirst(addBlock(block.Head, new Vector3(10f, 4f, 0f)));
        snakeF.AddLast(addBlock(block.Snake, new Vector3(10f, 3f, 0f)));
        snakeF.AddLast(addBlock(block.Snake, new Vector3(10f, 2f, 0f)));
        snakeF.AddLast(addBlock(block.Snake, new Vector3(10f, 1f, 0f)));

        CreateNewPoint();
    }

    Transform addBlock(block b, Vector3 pos)
    {
        GameObject gameTmp = null;
        switch (b)
        {
            case (block.Head):
                gameTmp = (GameObject)Instantiate(Resources.Load("Prefabs/Head", typeof(GameObject)), pos, Quaternion.identity);
                break;
            case (block.Snake):
                gameTmp = (GameObject)Instantiate(Resources.Load("Prefabs/Snake", typeof(GameObject)), pos, Quaternion.identity);
                break;
            case (block.Point):
                gameTmp = (GameObject)Instantiate(Resources.Load("Prefabs/Point", typeof(GameObject)), pos, Quaternion.identity);
                break;
        }

        return gameTmp != null ? gameTmp.transform : null;
    }

    void Update()
    {
        UpdateUI();
        if (Input.GetKeyDown(KeyCode.LeftArrow)) direction = Vector3.left;
        else if (Input.GetKeyDown(KeyCode.RightArrow)) direction = Vector3.right;
        else if (Input.GetKeyDown(KeyCode.UpArrow)) direction = Vector3.up;
        else if (Input.GetKeyDown(KeyCode.DownArrow)) direction = Vector3.down;

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

    void UpdateUI()         //вывод на экран Счет и Кол-во ячеек змейки
    {
        scoreText.text = "Score: \n" + score.ToString();
        snakeText.text = "Snake: \n" + snakeF.Count.ToString();
    }

    bool MoveSnake()
    {
        
        Transform first = PopFirst();
        Transform last = PopLast();

        Vector3 lastPosition = last.localPosition;

        last.localPosition = first.localPosition;
        snakeF.AddFirst(last);
        first.localPosition += direction;

        if (CheckValidGrid(first.localPosition) && CheckValidSnake(first.localPosition))
        {
            snakeF.AddFirst(first);
            if (CheckMoveOnPoint(first.localPosition))
            {
                snakeF.AddLast(addBlock(block.Snake, lastPosition));
                score += 10;
            }
        }
        else
        {
            if (CheckMoveBackSnake(first.localPosition))
            {
                first.localPosition -= direction;
                first.localPosition -= direction;
                snakeF.AddFirst(first);
                if (CheckMoveOnPoint(first.localPosition))
                {
                    snakeF.AddLast(addBlock(block.Snake, lastPosition));
                    score += 10;
                }
                
                if (CheckValidGrid(first.localPosition))
                    return false;
            }
            Destroy(first.gameObject);
            GameOver();
        }
        return true;

    }

    bool CheckMoveBackSnake(Vector3 pos)            //проверяем не начали мы движение в противоположную сторону
    {
        return snakeF.First.Next.Value.localPosition == pos;
    }

    bool CheckValidSnake(Vector3 pos)           // проверяем является ли позиция частью змейки
    {
        foreach (var item in snakeF)
        {
            if (item.localPosition == pos)
                return false;
        }
        return true;
    }

    bool CheckValidGrid(Vector3 pos)            //проверяем находится ли позиция на поле
    {
        return pos.x >= 0 && pos.x <= gridWeight && pos.y >= 0 && pos.y <= gridHeight;
    }

    bool CheckMoveOnPoint(Vector3 pos)          //проверкяем позицию на соответствие Поинт по координатам
    {
        if (pointPos.localPosition == pos)
        {
            Destroy(pointPos.gameObject);
            CreateNewPoint();            
            return true;
        }
        return false;
    }

    void CreateNewPoint()           //Создаем новый Поинт
    {
        Vector3 pos;
        if (snakeF.Count == ((gridHeight + 1) * (gridWeight + 1) ))
            return;
        do
        {
            pos = CreateRandomPosition();
        }
        while (!CheckValidSnake(pos));
        pointPos = addBlock(block.Point, pos);
    }

    Vector3 CreateRandomPosition()
    {
        return new Vector3(Random.Range(0, gridWeight), Random.Range(0, gridHeight), 0f);
    }

    Transform PopFirst()           //забираем из змейки первый элемент
    {
        Transform tmp = snakeF.First.Value;
        snakeF.RemoveFirst();
        return tmp;
    }

    Transform PopLast()            //забираем из змейки последний элемент
    {
        Transform tmp = snakeF.Last.Value;
        snakeF.RemoveLast();
        return tmp;
    }

    void GameOver()
    {
        foreach (var item in snakeF)
            Destroy(item.gameObject);
        
        Destroy(pointPos.gameObject);
        SceneManager.LoadScene("GameOver");
    }
}
