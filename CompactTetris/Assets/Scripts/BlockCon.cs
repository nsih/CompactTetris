using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BlockCon : MonoBehaviour
{    
    GameObject grid;
    GameObject blockHolder;

    public BlockData blockData;


    public GameObject nextBlock = null;
    public GameObject curBlock = null;

    public Vector3 spawnPosition;
    public Quaternion spawnRotation;

    private float fallTime = 1.0f;
    private float previousTime;

    void Start()
    {
        grid = GameObject.Find("Grid");
        blockHolder = GameObject.Find("BlockHolder");
        previousTime = Time.time;

        nextBlock = null;
        curBlock = null;

        spawnPosition = new Vector3(4, 17, 0);
        spawnRotation = Quaternion.identity;


        SpawnBlock();
    }

    private void Update()
    {
        if (Time.time - previousTime > (Input.GetKey(KeyCode.DownArrow) ? fallTime / 10 : fallTime))
        {
            MoveDown();
            previousTime = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Move(Vector3.left);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move(Vector3.right);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Rotate();
        }
    }

    void SpawnBlock()
    {
        if (nextBlock == null)
        {
            CreateNextBlock();
        }

        curBlock = nextBlock;
        curBlock.transform.position = spawnPosition;
        curBlock.transform.rotation = spawnRotation;
        curBlock.name = "CurBlock";

        CreateNextBlock();
    }

    void CreateNextBlock()
    {
        if (blockData != null)
        {
            blockData.InitRandomBlockData();

            nextBlock = Instantiate(blockData.Shape, spawnPosition, spawnRotation);

            //UI
            GameManager.Instance.PlayerModel.ChangeNextBlockImg(blockData.BlockSprite);

            
            int cnt = nextBlock.transform.childCount;
            for (int i = cnt - 1; i >= 0; i--)
            {
                Transform child = nextBlock.transform.GetChild(i);
                child.GetComponent<SpriteRenderer>().color = blockData.RandomColor;
            }

            nextBlock.transform.SetParent(blockHolder.transform);
            nextBlock.name = "NextBlock";
        }
        else
        {
            Debug.LogError("BlockData is null.");
        }
    }


    void Move(Vector3 direction)
    {
        curBlock.transform.position += direction;

        if (!IsValidMove())
        {
            curBlock.transform.position -= direction;
        }
    }

    void MoveDown()
    {
        curBlock.transform.position += Vector3.down;

        if (!IsValidMove())
        {
            curBlock.transform.position -= Vector3.down;
            FixToGrid();
            //enabled = false;
            grid.GetComponent<GridManager>().DeleteFullRows();
            SpawnBlock();
        }
    }

    void Rotate()
    {
        curBlock.transform.Rotate(0, 0, 90);

        if (!IsValidMove())
        {
            curBlock.transform.Rotate(0, 0, -90);
        }
    }

    bool IsValidMove()
    {
        foreach (Transform block in curBlock.transform)
        {
            Vector2 position = grid.GetComponent<GridManager>().Round(block.position);

            if (!grid.GetComponent<GridManager>().InsideGrid(position))
            {
                return false;
            }

            if ( GridManager.grid[(int)position.x, (int)position.y] != null)
            {
                return false;
            }
        }
        return true;
    }

    void FixToGrid()
    {
        int cnt = curBlock.transform.childCount;
        
        for (int i = cnt - 1; i >= 0; i--)
        {
            //if(curBlock.transform.GetChild(i).name = "")
            Transform child = curBlock.transform.GetChild(i);
            grid.GetComponent<GridManager>().AddToGrid(child);
            child.SetParent(blockHolder.transform);
        }
    }



}
