using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnakeMove : MonoBehaviour
{
    public enum Block
    {
        BlockPoint,
        BlockSnake,
        BlockGrid,
        BlockError
    }    
    public struct SnakeBlock
    {
        public bool Head;
        public Block block;
        public int PositionX;
        public int PositionY; 
    }
    public enum DirectionOfMotion:byte
    {
        Left,
        Right,
        Up,
        Down
    }

    DirectionOfMotion direction = DirectionOfMotion.Up;
    List<SnakeBlock> snake = new List<SnakeBlock>();
    float speed = 0.8f;
    float speedTime = 0;

    Block[,] grid = new Block[20,15];


    
    
    void Start()
    {
        snake.Add(new SnakeBlock{ Head = true, block = Block.BlockSnake, PositionX = 10, PositionY = 4});
        snake.Add(new SnakeBlock{ Head = false, block = Block.BlockSnake, PositionX = 10, PositionY = 3});
        snake.Add(new SnakeBlock{ Head = false, block = Block.BlockSnake, PositionX = 10, PositionY = 2});
        snake.Add(new SnakeBlock{ Head = false, block = Block.BlockSnake, PositionX = 10, PositionY = 1});
        
        UpdateGrid();
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) direction = DirectionOfMotion.Left;
        else if (Input.GetKeyDown(KeyCode.RightArrow)) direction = DirectionOfMotion.Right;
        else if (Input.GetKeyDown(KeyCode.UpArrow)) direction = DirectionOfMotion.Up;
        else if (Input.GetKeyDown(KeyCode.DownArrow)) direction = DirectionOfMotion.Down;

        if (Time.time - speedTime >= speed)
        {            
            SnakeView();
            //MoveSnake();
            //UpdateGrid();
            speedTime = Time.time;
        }
    }

    void MoveSnake()
    {
        SnakeBlock posNew = snake.Find(item => item.Head==true);  
        posNew.Head = false;

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

        Block checkBlock;

        if (CheckBlockGrid(out checkBlock, posNew.PositionX, posNew.PositionY))
            if (checkBlock == Block.BlockPoint)
            {
                SnakeBlock tmp = snake[snake.Count -1];
                RefreshSnake(posNew);
                snake.Add(tmp);
                CreatePointBlock();
            }
            else
                RefreshSnake(posNew);
        else
        {
            //GameOver
        }
            
        
    }

    void RefreshSnake(SnakeBlock posNew)
    {
        /* foreach (SnakeBlock item in snake)
        {                
            int x = item.PositionX , y = item.PositionY;
            item.PositionX = posNew.PositionX;
            item.PositionY = posNew.PositionY;
            posNew.PositionX = x;
            posNew.PositionY = y;
                
        }*/
        
        SnakeBlock[] items = snake.ToArray();
        snake = null;
        for (int i= 0; i<items.Length; i++)
        {
            int x = items[i].PositionX , y = items[i].PositionY;
            items[i].PositionX = posNew.PositionX;
            items[i].PositionY = posNew.PositionY;
            posNew.PositionX = x;
            posNew.PositionY = y;
            snake.Add(items[i]);
        }
    }

    public bool CheckBlockGrid(out Block result,int x,int y)    //есть ли возможность передвигаться в указанную точку, в result null когда шаг за пределы грид
    {
        result = Block.BlockError;
        if (x>=0 && x<20 && y>=0 && y<15)
        {
            result = grid[x,y];
            if (grid[x,y] != Block.BlockSnake)
                return true;             
        }          
        return false;
    }

    void UpdateGrid()
    {
        for (int x = 0; x<20; x++)
            for (int y = 0; y<15 ; y++)
                if (grid[x,y] != Block.BlockPoint)
                    grid[x,y] = Block.BlockGrid;
        foreach (SnakeBlock item in snake)    
                grid[item.PositionX,item.PositionY] = Block.BlockSnake;
        
    }

    void CreatePointBlock()
    {
        int randomPoindPosX;
        int randomPoindPosY;
        do{
            randomPoindPosX = Random.Range(0,19);
            randomPoindPosY = Random.Range(0,14);
        }while (grid[randomPoindPosX, randomPoindPosY] == Block.BlockSnake);
        grid[randomPoindPosX, randomPoindPosY] = Block.BlockPoint;
    }


    public void SnakeView()
    {
        //foreach (Transform item in transform)
        //{
        //    if ((int)item.position.y %2 == 0 )
         //       item.position += new Vector3(0,1,0);
        //}
        //transform.position += new Vector3(1,0,0);
    }

}
