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
}
