using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class TileController : MonoBehaviour
{
    public TileState State { get; private set; }
    public CellDataHandler Cell { get; private set; }
    public int CurrentNumber { get; private set; }

    public bool IsMerged;
    Image _backGround;
    TextMeshProUGUI _text;
    private void Awake()
    {
        _backGround= GetComponent<Image>();
        _text= GetComponentInChildren<TextMeshProUGUI>();
    }
    public void ClearCell()
    {
        if (this.Cell)
            this.Cell.CurrentTile = null;
        Cell = null;
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
    
    }
    public void MoveToCell(CellDataHandler cell)
    {
        if (this.Cell)
            this.Cell.CurrentTile = null;
        Cell = cell;
        cell.CurrentTile = this;

        StartCoroutine(Animate(cell.transform.position));
    }
    public void Merge(CellDataHandler cell)
    {
        if (this.Cell)
            this.Cell.CurrentTile = null;
        Cell = null;
        IsMerged = true;
        StartCoroutine(Animate(cell.transform.position));
        Destroy(this.gameObject, 0.11f);
    }

    public void AnimateTransform(Vector3 lastPos)
    {
        StartCoroutine(Animate(lastPos));
    }
    IEnumerator Animate(Vector3 lastPos)
    {
        float elapsed = 0f;
        float duration = 0.1f;
        Vector3 startPos = transform.position;
        while(elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPos, lastPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = lastPos;
        

    }
}
