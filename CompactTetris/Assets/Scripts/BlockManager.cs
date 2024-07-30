using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BlockManager : MonoBehaviour
{
    GameObject grid;
    GameObject blockHolder;

    public List<GameObject> blockLst;   
    public GameObject curBlock = null;
    public Vector3 spawnPosition;
    public Quaternion spawnRotation;

    private float fallTime = 1.0f; // 블록이 떨어지는 시간 간격
    private float previousTime; // 이전 시간

    void Start()
    {
        grid = GameObject.Find("Grid");
        blockHolder = GameObject.Find("BlockHolder");
        previousTime = Time.time;

        curBlock = null;
        spawnPosition = new Vector3(4, 17, 0);
        spawnRotation = Quaternion.identity;
        SpawnBlock();
    }

    private void Update()
    {
        // 아래 방향키를 누를 때 더 빠르게 떨어지도록 설정
        if (Time.time - previousTime > (Input.GetKey(KeyCode.DownArrow) ? fallTime / 10 : fallTime))
        {
            MoveDown();
            previousTime = Time.time;
        }

        // 왼쪽 방향키
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Move(Vector3.left);
        }

        // 오른쪽 방향키
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move(Vector3.right);
        }

        // 위 방향키 (회전)
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Rotate();
        }
    }

    void SpawnBlock()
    {
        if(curBlock)
        {
            Destroy(curBlock);
        }

        int idx = Random.Range(0, blockLst.Count);
        curBlock = Instantiate(blockLst[idx], spawnPosition, spawnRotation);
        curBlock.transform.SetParent(blockHolder.transform);

        curBlock.name = "CurBlock";
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
            grid.GetComponent<GridManager>().DeleteFullRows(); // 가득 찬 행 삭제
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
