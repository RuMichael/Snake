using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public struct SnakeBlock
    {
        public Block block;
        public int PositionX;
        public int PositionY; 
    }

    List<SnakeBlock> snake = new List<SnakeBlock>();
    List<GameObject> snakeT= new List<GameObject>();

    float speed = 0.8f;
    float speedTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        snake.Add(new SnakeBlock{ block = Block.BlockSnake, PositionX = 10, PositionY = 4});
        snakeT.Add((GameObject)Instantiate(Resources.Load("Prefabs/snake", typeof(GameObject), new Vector2(10f,4f,0), Quaternion.identity)));
        snake.Add(new SnakeBlock{ block = Block.BlockSnake, PositionX = 10, PositionY = 3});
        snakeT.Add((GameObject)Instantiate(Resources.Load("Prefabs/snake", typeof(GameObject), new Vector2(10f,3f,0), Quaternion.identity)));
        snake.Add(new SnakeBlock{ block = Block.BlockSnake, PositionX = 10, PositionY = 2});
        snakeT.Add((GameObject)Instantiate(Resources.Load("Prefabs/snake", typeof(GameObject), new Vector2(10f,2f,0), Quaternion.identity)));
        snake.Add(new SnakeBlock{ block = Block.BlockSnake, PositionX = 10, PositionY = 1});
        snakeT.Add((GameObject)Instantiate(Resources.Load("Prefabs/snake", typeof(GameObject), new Vector2(10f,1f,0), Quaternion.identity)));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) direction = DirectionOfMotion.Left;
        else if (Input.GetKeyDown(KeyCode.RightArrow)) direction = DirectionOfMotion.Right;
        else if (Input.GetKeyDown(KeyCode.UpArrow)) direction = DirectionOfMotion.Up;
        else if (Input.GetKeyDown(KeyCode.DownArrow)) direction = DirectionOfMotion.Down;

        if (Time.time - speedTime >= speed)
        {            
            
            MoveSnake();
            //UpdateGrid();
            speedTime = Time.time;
        }
    }

    void MoveSnake()
    {
        SnakeBlock posNew = snake[0];          
        switch (direction)
        {
            case DirectionOfMotion.Up:
                posNew.PositionY += 1;
            break;
            case DirectionOfMotion.Down:
                posNew.PositionY -= 1;
            break;
            case DirectionOfMotion.Left:
                posNew.PositionX -= 1;
            break;
            case DirectionOfMotion.Right:
                posNew.PositionX += 1;
            break;
        }        


        if (CheckBlockGrid(posNew.PositionX, posNew.PositionY))
            
            {
                SnakeBlock tmp = snake[snake.Count - 1];
                RefreshSnake(posNew);
                snake.Add(tmp);
                CreatePointBlock();
            }
            
        else
        {
            //GameOver
        }          
    }

    void RefreshSnake(SnakeBlock posNew)
    {

    }

    public bool CheckBlockGrid(int x,int y)    //есть ли возможность передвигаться в указанную точку
    {        
        if (x >= 0 && x < 20 && y >= 0 && y < 15)        
        {            
            if (snake.Find(item => item.PositionX == x, item.PositionY ==y) == null)
                return true;             
        }          
        return false;
    }
}
