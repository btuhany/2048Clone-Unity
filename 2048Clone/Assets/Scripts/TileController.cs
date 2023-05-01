using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TileController : MonoBehaviour
{
    public TileState State { get; private set; }
    public CellDataHandler Cell { get; private set; }
    public int CurrentNumber { get; private set; }

    Image _backGround;
    TextMeshProUGUI _text;
    private void Awake()
    {
        _backGround= GetComponent<Image>();
        _text= GetComponentInChildren<TextMeshProUGUI>();
    }
    public void SetState(TileState newState)
    {
        State = newState;
        CurrentNumber= newState.Number;

        _backGround.color = newState.BackgroundColor;
        _text.color = newState.TextColor;
        _text.SetText(CurrentNumber.ToString());
    }
    public void SetCell(CellDataHandler cell)
    {
        if(this.Cell)
            this.Cell.CurrentTile = null;
        Cell = cell;
        cell.CurrentTile= this;
        transform.position = cell.transform.position;
        transform.SetParent(cell.transform);
    }
}
