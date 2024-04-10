using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlockController : MonoBehaviour
{
    [SerializeField] private GameObject[] blocks,dot,blockLine,blockCross;
    [SerializeField] private int disStage,rows, columns, target, up = 0;
    [SerializeField] private float distance;
    [SerializeField] private Transform blockContainer, dotContainer;
    [SerializeField] private int[] randomRow;
    private GameObject[] gridDot;
    private GameObject[,] grid;
    public UI UIScript;
    private void Awake()
    {
        switch (PlayerPrefs.GetInt("stage", 1))
        {
            case 1:
                {
                    rows = 4;
                    columns = 4;
                    UIScript.setTarget(10);
                    UIScript.setTurn(10);
                    UIScript.setTargetColor(1);
                    UIScript.turnToText();
                    UIScript.targetToText();
                    break;
                }
            case 2:
                {
                    rows = 4;
                    columns = 4;
                    UIScript.setTarget(12);
                    UIScript.setTurn(12);
                    UIScript.setTargetColor(2);
                    UIScript.turnToText();
                    UIScript.targetToText();
                    break;
                }
            case 3:
                {
                    rows = 6;
                    columns = 4;
                    UIScript.setTarget(15);
                    UIScript.setTurn(15);
                    UIScript.setTargetColor(0);
                    UIScript.turnToText();
                    UIScript.targetToText();
                    break;
                }
            case 4:
                {
                    rows = 6;
                    columns = 4;
                    UIScript.setTarget(15);
                    UIScript.setTurn(12);
                    UIScript.setTargetColor(1);
                    UIScript.turnToText();
                    UIScript.targetToText();
                    break;
                }
            default:
                break;
        }
    }
    void Start()
    {
        distance = blockContainer.localScale.x / 8;
        disStage = rows - 6;
        InitializeGrid();
        SpawnBlocks();
        SpawnTarget();
        
    }
    void InitializeGrid()
    {
        randomRow= new int[columns];
        grid = new GameObject[rows, columns];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                grid[i, j] = null;
            }
        }
        gridDot = new GameObject[columns];
    }
    void SpawnBlocks()
    {
        for(int i =0; i<dot.Length;i++)
        {
            GameObject block = Instantiate(dot[i], new Vector3(i * 0.3f - 1f, 4.25f, 0), Quaternion.identity);
        }
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                int x = Random.Range(0, blocks.Length);
                GameObject block = Instantiate(blocks[x], new Vector3(j * distance - 0.885f, (disStage - i) * distance - 0.43f, 0), Quaternion.identity);
                grid[i, j] = block;
                block.transform.parent = blockContainer;
                grid[i, j].GetComponent<Color>().numRow = i;
                grid[i, j].GetComponent<Color>().numCol = j;
                block.GetComponent<Color>().AppearBlock(0.2f);
            }
        }
        RandomRow();
    }
    void SpawnTarget()
    {
        GameObject target = Instantiate(blocks[UIScript.getTargetColor()], new Vector3(-0.25f, 3.65f, 0), Quaternion.identity);
        target.transform.localScale = new Vector3(0.13f, 0.13f, 0.13f);
    }
    void Update()
    {
        if(UIScript.getTurn() <= 0 && ( UIScript.getTarget() > 0 || !CanDestroyBlocks()))
        {
             UIScript.Lose();
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (grid[i,j]!= null)
                         grid[i,j].GetComponent<Color>().DestroyBlock(0.1f);
                }
            } 
                    return;
        }
        else if(UIScript.getTarget() <= 0)
        {
            UIScript.Win();
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (grid[i, j] != null)
                        grid[i, j].GetComponent<Color>().DestroyBlock(0.1f);
                }
            }
            return;
        }
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
            {
                Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero);
                if (hit.collider != null)
                {
                    int numRow = hit.collider.gameObject.GetComponent<Color>().numRow;
                    int numCol = hit.collider.gameObject.GetComponent<Color>().numCol;
                    Debug.Log(numRow + " " + numCol);
                    OnBlockClicked(numRow, numCol);
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
            if (hit.collider != null)
            {
                int numRow = hit.collider.gameObject.GetComponent<Color>().numRow;
                int numCol = hit.collider.gameObject.GetComponent<Color>().numCol;
                Debug.Log(numRow + " " + numCol);
                if (hit.collider.CompareTag("Block"))
                    OnBlockClicked(numRow, numCol);
                else if (hit.collider.CompareTag("Line"))
                    DestroyBlocksLine(numRow, numCol);
                else if (hit.collider.CompareTag("Cross"))
                    DestroyBlocksCross(numRow, numCol);
            }
        }
    }

    void DestroyBlocksLine(int row, int col)
    {
        if (grid[row,col].GetComponent<Color>().color % 2 == 0)
        {
            for (int j = 0; j < columns; j++)
            {
                if (grid[row, j] != null)
                {
                    if (grid[row, j].GetComponent<Color>().color == UIScript.getTargetColor())
                    {
                        UIScript.UpdateTarget(1);
                        UIScript.targetToText();
                    }
                    grid[row, j].GetComponent<Color>().DestroyBlock(0.1f);
                    grid[row, j] = null;
                }
            }
        }
        else
        {
            for (int i = 0; i < rows; i++)
            {
                if (grid[i, col] != null)
                {
                    if (grid[i, col].GetComponent<Color>().color == UIScript.getTargetColor())
                    {
                        UIScript.UpdateTarget(1);
                        UIScript.targetToText();
                    }
                    grid[i, col].GetComponent<Color>().DestroyBlock(0.1f);
                    grid[i, col] = null;
                }
            }
        }
        MoveGrid();
        //Update grid[rows+1,columns]
        if ((rows - CountEmptyRows()) <= 10)
            StartCoroutine(WaitAndUpdateGrid());
    }
    void DestroyBlocksCross(int row, int col)
    {
        for (int j = 0; j < columns; j++)
        {
            if (grid[row, j] != null)
            {
                if (grid[row, j].GetComponent<Color>().color == UIScript.getTargetColor())
                {
                    UIScript.UpdateTarget(1);
                    UIScript.targetToText();
                }
                grid[row, j].GetComponent<Color>().DestroyBlock(0.1f);
                grid[row, j] = null;
            }
        }

        for (int i = 0; i < rows; i++)
        {
            if (grid[i, col] != null)
            {
                if (grid[i, col].GetComponent<Color>().color == UIScript.getTargetColor())
                {
                    UIScript.UpdateTarget(1);
                    UIScript.targetToText();
                }
                grid[i, col].GetComponent<Color>().DestroyBlock(0.1f);
                grid[i, col] = null;
            }
        }
        MoveGrid();
        //Update grid[rows+1,columns]
        if ((rows - CountEmptyRows()) <= 10)
            StartCoroutine(WaitAndUpdateGrid());
    }
    void CheckAdjacentBlocks(int row, int col, HashSet<GameObject> visitedBlocks, int targetColor, ref HashSet<GameObject> blocksToDestroy)
    {
        if (row < 0 || row >= rows || col < 0 || col >= columns)
            return;

        if (visitedBlocks.Contains(grid[row, col]))
            return;

        if (grid[row, col] == null || grid[row, col].GetComponent<Color>() == null)
            return;

        if (grid[row, col].GetComponent<Color>().color != targetColor)
            return;

        visitedBlocks.Add(grid[row, col]);
        blocksToDestroy.Add(grid[row, col]);

        CheckAdjacentBlocks(row + 1, col, visitedBlocks, targetColor, ref blocksToDestroy);
        CheckAdjacentBlocks(row - 1, col, visitedBlocks, targetColor, ref blocksToDestroy);
        CheckAdjacentBlocks(row, col + 1, visitedBlocks, targetColor, ref blocksToDestroy);
        CheckAdjacentBlocks(row, col - 1, visitedBlocks, targetColor, ref blocksToDestroy);
    }
    public void OnBlockClicked(int row, int col)
    {
        HashSet<GameObject> blocksToDestroy = new HashSet<GameObject>();
        CheckAdjacentBlocks(row, col, new HashSet<GameObject>(), grid[row, col].GetComponent<Color>().color, ref blocksToDestroy);
        if (blocksToDestroy.Count >= 2)
        {
            int check = blocksToDestroy.Count;
            int checkRow, checkCol, checkColor = grid[row, col].GetComponent<Color>().color;
            Vector3 pos = grid[row, col].transform.position;
            UIScript.UpdateTurn(1);
            UIScript.turnToText();
            if (grid[row, col].GetComponent<Color>().color == UIScript.getTargetColor())
            {
                UIScript.UpdateTarget(blocksToDestroy.Count);
                UIScript.targetToText();
            }
            
            foreach (GameObject block in blocksToDestroy)
            {
                checkRow = block.GetComponent<Color>().numRow;
                checkCol = block.GetComponent<Color>().numCol;
                block.GetComponent<Color>().DestroyBlock(0.1f);
                //Destroy(block);
                grid[block.GetComponent<Color>().numRow, block.GetComponent<Color>().numCol] = null;
            }
            if(check>=10)
            {
                GameObject block = Instantiate(blockCross[checkColor], pos, Quaternion.identity);
                grid[row, col] = block;
                block.transform.parent = blockContainer;
                block.GetComponent<Color>().numRow = row;
                block.GetComponent<Color>().numCol = col;
                block.GetComponent<Color>().AppearBlock(0.2f);
            }
            else if (check >= 5)
            {
                int x = Random.Range(checkColor*2, checkColor*2 + 1);
                GameObject block = Instantiate(blockLine[x], pos, Quaternion.identity);
                grid[row, col] = block;
                block.transform.parent = blockContainer;
                block.GetComponent<Color>().numRow = row;
                block.GetComponent<Color>().numCol = col;
                block.GetComponent<Color>().AppearBlock(0.2f);
            }
            MoveGrid();
            //Update grid[rows+1,columns]
            if ((rows - CountEmptyRows()) <= 10)
                StartCoroutine(WaitAndUpdateGrid());


        }
    }

    IEnumerator WaitAndUpdateGrid()
    {
        yield return new WaitForSeconds(0.5f);
        UpDateGrid();
        UpdateBlockContainerPosition();
    }
    private int CountEmptyRows()
    {
        int emptyRowCount = 0;

        for (int i = 0; i < rows; i++)
        {
            bool rowEmpty = true;
            for (int j = 0; j < columns; j++)
            {
                if (grid[i, j] != null)
                {
                    rowEmpty = false;
                    break;
                }
            }
            if (rowEmpty)
            {
                emptyRowCount++;
            }
        }

        return emptyRowCount;
    }
    void MoveGrid()
    {
        for (int j = 0; j < columns; j++)
        {
            for (int i = rows - 1; i >= 0; i--)
            {
                if (grid[i, j] == null)
                {
                    for (int k = i - 1; k >= 0; k--)
                    {
                        if (grid[k, j] != null)
                        {
                            grid[k, j].GetComponent<Color>().MoveDown(i - k, 0.3f);
                            grid[k, j].GetComponent<Color>().numRow = i;
                            grid[k, j].GetComponent<Color>().numCol = j;
                            grid[i, j] = grid[k, j];
                            grid[k, j] = null;
                            break;
                        }
                    }
                }
            }
        }
    }
    void UpDateGrid()
    {
        rows++;
        GameObject[,] newGrid = new GameObject[rows, columns];

        for (int i = 0; i < rows - 1; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                newGrid[i, j] = grid[i, j];
            }
        }

        for (int j = 0; j < columns; j++)
        {
            int x = Random.Range(0, blocks.Length);
            GameObject block = Instantiate(blocks[randomRow[j]], new Vector3(j * distance - 0.885f, (rows - 1 - up - disStage) * -distance - 0.45f, 0), Quaternion.identity);
            newGrid[rows - 1, j] = block;
            block.transform.parent = blockContainer;
            block.GetComponent<Color>().numRow = rows - 1;
            block.GetComponent<Color>().numCol = j;
            block.GetComponent<Color>().AppearBlock(0.2f);
        }
        RandomRow();
        grid = newGrid;
    }
    void UpdateBlockContainerPosition()
    {
        Vector3 containerPosition = blockContainer.position;
        containerPosition.y += distance; 
        blockContainer.position = containerPosition;
        up++;
    }
    void RandomRow()
    {
        for (int i=0;i<columns;i++)
        {
            int x = Random.Range(0, blocks.Length);
            float xPos = -0.3f*(columns-1)+i*0.6f;
            Vector3 position = new Vector3(xPos, -3.97f, 0f);
            if (gridDot[i] != null) Destroy(gridDot[i]);
            GameObject dots = Instantiate(dot[x], position, Quaternion.identity);
            dots.transform.parent = dotContainer;
            gridDot[i] = dots;
            randomRow[i] = x;
        }

    }
    bool CanDestroyBlocks()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                HashSet<GameObject> blocksToDestroy = new HashSet<GameObject>();
                if (grid[i, j] != null) 
                {
                    CheckAdjacentBlocks(i, j, new HashSet<GameObject>(), grid[i, j].GetComponent<Color>().color, ref blocksToDestroy);
                    if (blocksToDestroy.Count >= 2)
                    {
                        return true; 
                    }
                }
            }
        }
        return false; 
    }

}
