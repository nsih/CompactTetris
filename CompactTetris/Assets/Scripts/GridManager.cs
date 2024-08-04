using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class GridManager : MonoBehaviour
{
    public static int width = 10;
    public static int height = 20;

    public static Transform[,] grid = new Transform[width, height];


    void Start()
    {
    }



    public void InitializeGrid()
    {
        grid = new Transform[width, height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                grid[x, y] = null;
            }
        }
    }

    //Fix Block to Grid
    public void AddToGrid(Transform block)
    {
        Vector2 position = Round(block.position);
        if (position.y < height)
        {
            grid[(int)position.x, (int)position.y] = block;
        }
    }

    //Delete Block from Grid
    public void RemoveFromGrid(Vector2 position)
    {
        if (position.y < height)
        {
            grid[(int)position.x, (int)position.y] = null;
        }
    }

    public Vector2 Round(Vector2 position)
    {
        return new Vector2(Mathf.Round(position.x), Mathf.Round(position.y));
    }

    //Pos Check
    public bool InsideGrid(Vector2 position)
    {
        return (int)position.x >= 0 && (int)position.x < width && (int)position.y >= 0;
    }

    // Tetris?
    public bool IsRowFull(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x, y] == null)
            {
                return false;
            }
        }
        return true;
    }

    // Tetris!
    public void DeleteRow(int y)
    {
        for (int x = 0; x < width; x++)
        {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }

    public void DecreaseRow(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x, y] != null)
            {
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;
                grid[x, y - 1].position += new Vector3(0, -1, 0);
            }
        }
    }

    public void DecreaseRowsAbove(int y)
    {
        for (int i = y; i < height; i++)
        {
            DecreaseRow(i);
        }
    }

    public void DeleteFullRows()
    {
        int baseScore = 100;
        int exp = 1; 

        int currentScore = 0;

        for (int y = 0; y < height; y++)
        {
            if (IsRowFull(y))
            {
                currentScore = baseScore * exp;
                
                DeleteRow(y);
                DecreaseRowsAbove(y + 1);
                y--;

                GameManager.Instance.PlayerModel.AddScore(currentScore);

                exp *= 2;
            }
        }
        
    }
}
