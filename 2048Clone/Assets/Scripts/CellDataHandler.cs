using UnityEngine;

public class CellDataHandler : MonoBehaviour   //Get cells at RowData script in Awake() function
{
    public Vector2Int Coordinates { get; set; }   //set in OnEnable() function at GridController via RowDataHandler
    public TileController CurrentTile { get; set; }  //set in TileController SetCell() function

    public bool IsEmpty => CurrentTile == null;
}
