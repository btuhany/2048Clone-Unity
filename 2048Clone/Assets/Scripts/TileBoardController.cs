using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class TileBoardController : MonoBehaviour
{
    [SerializeField] TileController _tilePrefab;
    GridController _grid;
    List<TileController> _tilesList;
    List<TileController> _mergedTilesList = new List<TileController>();   //to prevent mulitple merging in one move
    [SerializeField] TileState[] _tileStates;
    public bool CanGetInput;
    public bool IsThereAnyMovedTile;
    private void Awake()
    {
        _grid = GetComponentInChildren<GridController>();
        _tilesList = new List<TileController>();
    }
    private void OnEnable()
    {
        GameManager.Instance.OnGameOver += HandleOnGameOver;
        GameManager.Instance.OnGameOver += HandleOnGameCompleted;
        GameManager.Instance.OnRestart += HandleOnRestart;
    }
    private void Start()
    {   
        SpawnNewTile();
        SpawnNewTile();
        CanGetInput = true;
    }
    private void HandleOnGameOver()
    {
        CanGetInput = false;
    }
    private void HandleOnGameCompleted()
    {
        CanGetInput = false;
    }
    private void HandleOnRestart()
    {
        CanGetInput = true;
        IsThereAnyMovedTile = false;
        _mergedTilesList.Clear();
        for (int i = 0; i < _tilesList.Count; i++)
        {
            if (_tilesList[i])
            {
                _tilesList[i].ClearCell();
                Destroy(_tilesList[i].gameObject);
            }
        }
        _tilesList.Clear();
        SpawnNewTile();
        SpawnNewTile();
    }
    void SpawnNewTile()
    {
        TileController newTile = Instantiate(_tilePrefab, _grid.transform);
        if(Random.Range(1,101)>96)
        {
            newTile.SetState(_tileStates[1]);
        }
        else
        {
            newTile.SetState(_tileStates[0]);
        }
        newTile.SetCell(_grid.GetRandomEmptyCell());
        _tilesList.Add(newTile);
    }
    private void Update()
    {
        if (!CanGetInput) return;
        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            CheckAndMoveOccupiedTiles(Vector2Int.up, 0, 1, 1, 1);

        }
        else if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            CheckAndMoveOccupiedTiles(Vector2Int.left, 1, 1, 0, 1);

        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            CheckAndMoveOccupiedTiles(Vector2Int.down, 0, 1, _grid.GridHeight - 2, -1);

        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            CheckAndMoveOccupiedTiles(Vector2Int.right, _grid.GridWidth - 2, -1, 0, 1);
            
        }
        if(IsThereAnyMovedTile)
        {
            StartCoroutine(InputCooldown());
        }
    }
    void CheckAndMoveOccupiedTiles(Vector2Int direction, int minX, int incrementX, int minY, int incrementY)
    {
        for (int i = minX; i >= 0 && i < _grid.GridWidth; i += incrementX)
        {
            for (int j = minY; j >= 0 && j < _grid.GridHeight; j += incrementY)
            {
                CellDataHandler tileCell = _grid.GetCell(i, j);
                if (!tileCell.IsEmpty)
                {
                    MoveTile(tileCell.CurrentTile, direction);
   
                }
            }
        }


        //DOESN'T WORK!! TILES SHOULD BE MOVED IN ORDER
        //for (int i = 0; i < _tilesList.Count; i++)
        //{
        //    MoveTile(_tilesList[i], direction);
        //}
    }
    void MoveTile(TileController tile, Vector2Int direction)
    {
        bool isMerged = false;
        CellDataHandler newCell = null;
        CellDataHandler adjacentCell = _grid.GetAdjacentCell(tile.Cell, direction);
        while (adjacentCell) //till adjacentCell is null (till there is no adjacentCell)
        {
            if(!adjacentCell.IsEmpty)
            {
                TileController mergeTile = adjacentCell.CurrentTile;
                if (CanMerge(tile,mergeTile))
                {
                    Merge(tile, mergeTile);
                    IsThereAnyMovedTile = true;
                    isMerged= true;
                }
                
                break;
                
            }
            newCell = adjacentCell;
            adjacentCell = _grid.GetAdjacentCell(adjacentCell, direction);
        }
        if(newCell && !isMerged)
        {
            IsThereAnyMovedTile = true;
            tile.MoveToCell(newCell);
        }
    }
    public bool CanMerge(TileController tileA, TileController tileB)
    {
        if (tileA.CurrentNumber == tileB.CurrentNumber && (!tileA.IsMerged) && (!tileB.IsMerged))
        {
            return true;
        }
        else
            return false;
    }
    void Merge(TileController tile, TileController mergedTile)
    {
        tile.Cell.CurrentTile = null;
        tile.Merge(mergedTile.Cell);
        int nextStateIndex = FindNextStateIndex(tile.State);
        mergedTile.SetState(_tileStates[nextStateIndex]);
        mergedTile.IsMerged= true;
        _mergedTilesList.Add(mergedTile);
       
    }
    int FindNextStateIndex(TileState currentState)
    {
        for (int i = 0; i < _tileStates.Length; i++)
        {
            if (_tileStates[i] == currentState)
            {
                if (i + 1 == _tileStates.Length - 1)
                    GameManager.Instance.GameCompleted();  //GameCompleted
                    
                return Mathf.Clamp(i+1,0,_tileStates.Length-1);
            }
        }
        return 0;
    }
    IEnumerator InputCooldown()  
    {
        CanGetInput = false;
        IsThereAnyMovedTile= false;
        yield return new WaitForSeconds(0.15f);
        if(_mergedTilesList.Count > 0)
        {
            foreach (TileController tile in _mergedTilesList)
            {
                tile.IsMerged = false;
            }
            _mergedTilesList.Clear();
        }
        CanGetInput = true;

        if (!_grid.IsGridFull())
        {
            SpawnNewTile();
            if(_grid.IsGridFull() && IsGameOver())
            {
                GameManager.Instance.GameOver();
            }
        }

        
        yield return null;
    }
    public bool IsGameOver()
    {
        for (int i = 0; i < 4; i++)
        {
            switch (i)
            {
                case 0:
                    if (IsThereAnyMergableTiles(Vector2Int.up, 0, 1, 1, 1))
                        return false;
                    break;
                case 1:
                    if(IsThereAnyMergableTiles(Vector2Int.left, 1, 1, 0, 1))
                        return false;
                    break;
                case 2:
                    if (IsThereAnyMergableTiles(Vector2Int.down, 0, 1, _grid.GridHeight - 2, -1))
                        return false;
                    break;
                case 3:
                    if (IsThereAnyMergableTiles(Vector2Int.right, _grid.GridWidth - 2, -1, 0, 1))
                        return false;
                    break;

            }
        }
        return true;
        
    }
    bool IsThereAnyMergableTiles(Vector2Int direction, int minX, int incrementX, int minY, int incrementY)
    {
        for (int i = minX; i >= 0 && i < _grid.GridWidth; i += incrementX)
        {
            for (int j = minY; j >= 0 && j < _grid.GridHeight; j += incrementY)
            {
                CellDataHandler tileCell = _grid.GetCell(i, j);
                if (!tileCell.IsEmpty)
                {
                    if(IsTileMergable(tileCell.CurrentTile, direction))
                    {
                        return true;
                    }

                }
            }
        }
        return false;
    }
    bool IsTileMergable(TileController tile, Vector2Int direction)
    {
        CellDataHandler adjacentCell = _grid.GetAdjacentCell(tile.Cell, direction);
        while (adjacentCell) //till adjacentCell is null (till there is no adjacentCell)
        {
            if (!adjacentCell.IsEmpty)
            {
                TileController mergeTile = adjacentCell.CurrentTile;
                if (CanMerge(tile, mergeTile))
                {
                    return true;
                }

                break;

            }
            
        }
        return false;
    }

}
