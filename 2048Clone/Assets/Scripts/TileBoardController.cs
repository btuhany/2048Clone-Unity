using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBoardController : MonoBehaviour
{
    [SerializeField] TileController _tilePrefab;
    GridController _grid;
    List<TileController> _tiles;
    [SerializeField] TileState[] _tileStates;
    private void Awake()
    {
        _grid = GetComponentInChildren<GridController>();
        _tiles = new List<TileController>();
    }
    private void Start()
    {
        SpawnNewTile();
        SpawnNewTile();
    }
    void SpawnNewTile()
    {
        TileController newTile = Instantiate(_tilePrefab, _grid.transform);
        if(Random.Range(1,101)>90)
        {
            newTile.SetState(_tileStates[1]);
        }
        else
        {
            newTile.SetState(_tileStates[0]);
        }
        newTile.SetCell(_grid.GetRandomEmptyCell());
        _tiles.Add(newTile);
    }

}
