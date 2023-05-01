
using UnityEngine;

public class RowDataHandler : MonoBehaviour
{
    public CellDataHandler[] Cells { get; private set; }

    private void Awake()
    {
        Cells = GetComponentsInChildren<CellDataHandler>();
    }
}
