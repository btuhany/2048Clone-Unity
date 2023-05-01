using UnityEngine;

public class GridController : MonoBehaviour
{
    public RowDataHandler[] Rows { get; private set; }
    public CellDataHandler[] Cells { get; private set; }

    public int GridSize => Cells.Length;
    public int GridHeight => Rows.Length;
    public int GridWidth => GridSize / GridHeight;

    private void Awake()
    {
        Rows = GetComponentsInChildren<RowDataHandler>();
        Cells = GetComponentsInChildren<CellDataHandler>();
    }
    private void Start()
    {
        for (int i = 0; i < Rows.Length; i++)
        {
            for (int j = 0; j < Rows[i].Cells.Length; j++)
            {
                Rows[i].Cells[j].Coordinates = new Vector2Int(j, i);
            }
        }
    }
    public CellDataHandler GetRandomEmptyCell()
    {
        CellDataHandler randomCell = Cells[Random.Range(0, Cells.Length)];
        if (randomCell.IsEmpty)
            return randomCell;
        else if (!IsGridFull())
        {
            return GetRandomEmptyCell();
        }           
        else
        {
            //GameOver
            return null;
        }
    }

    public bool IsGridFull()
    {
        for (int i = 0; i < Cells.Length; i++)
        {
            if (Cells[i].IsEmpty)
                return false;
        }
        return true;
    }
    public CellDataHandler GetCell(int x, int y)
    {
        return Rows[y].Cells[x];
    }
    public CellDataHandler GetCell(Vector2Int cellCoordinates)
    {
        if(cellCoordinates.x<0 || cellCoordinates.y<0 || cellCoordinates.x>=GridWidth || cellCoordinates.y>=GridHeight)
            return null;
        else
            return Rows[cellCoordinates.y].Cells[cellCoordinates.x];
    }
    public CellDataHandler GetAdjacentCell(CellDataHandler cell, Vector2Int direction)
    {
        Vector2Int cellCoordinates = cell.Coordinates;
        cellCoordinates.x += direction.x;
        cellCoordinates.y -= direction.y; //Vector2.up = 0,1  ,, but in the coordinates increasing y means going to last row means down direction.
        return GetCell(cellCoordinates);
    }
}
