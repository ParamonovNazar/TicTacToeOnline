using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Cell : MonoBehaviour, ICell
{ 
    public int Row;
    public int Column;

    private CellState state;
    [SerializeField]
    private Animator anim;

    public CellState State
    {
        set
        {
            state = value;
            UpdateView();
        }
    }

    public CellState GetState()
    {
        return state;
    }

    private void OnMouseDown()
    {
        PlayerController.Instance.TryChangeCell(Row, Column);
    }

    private void UpdateView()
    {
        anim.SetInteger("State",(int)state);
    }
}
public enum CellState
{
    None,
    O,
    X
}
