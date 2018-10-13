using UnityEngine;

public class GameViewer : MonoBehaviour
{
    [SerializeField]
    private GameObject CellPrefab;
    [SerializeField]
    private Transform CellsCenter;
    [SerializeField]
    private Vector2 CellOffset;

    [SerializeField]
    private GameType Game;
    private GameSettings Settings;

    private int CountOfColumns;
    private int CountOfRows;

    private Cell[,] Cells;

#region singleton
    private static GameViewer instance;

    public static GameViewer Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameViewer>();
            }
            return instance;
        }
    }

    #endregion

    private void Awake()
    {
        Settings = new GameSettings(Game);
    }

    public void ShowGame()
    {
        CountOfColumns = Settings.CountOfColumns;
        CountOfRows = Settings.CountOfRows;

        if (Cells == null || Cells.Length != CountOfRows || 
            Cells.GetLength(0) != CountOfRows || Cells.GetLength(1) != CountOfColumns)
        {
            GenerateCells();
        }
    }
    [ContextMenu ("GenerateCells")]
    private void GenerateCells()
    {
        if (Cells != null)
        {
            foreach (var v in Cells)
            {
                if (v != null)
                {
                    DestroyImmediate(v.gameObject);
                }
            }
        }

        Cells = new Cell[CountOfRows,CountOfColumns];

        for (int row = 0; row < CountOfRows; row++)
        {
            for (int col = 0; col < CountOfColumns; col++)
            {
                GameObject cellGO = Instantiate(CellPrefab, CellsCenter);
                cellGO.transform.localPosition = new Vector3(col * CellOffset.x - CellOffset.x/2* (CountOfColumns-1),
                    row * CellOffset.y - CellOffset.y /2 * (CountOfRows-1));
                Cell cell = cellGO.GetComponent<Cell>();
                cell.Row = row;
                cell.Column = col;
                cell.State = CellState.None;

                Cells[row, col] = cell;
            }
        }
    }

    public bool CheckEnd()
    {
        foreach (var cell in Cells)
        {
            if (cell.GetState() == CellState.None)
            {
                return false;
            }
        }

        return true;
    }

    public int MaxPlayers()
    {
        return Settings.MAX_PLAYERS;
    }

    public void ChangeCell(int row, int col, CellState state )
    {
        Cells[row, col].State = state;
    }

    public bool CheckEmptyCell(int row, int col)
    {
        return Cells[row, col].GetState() == CellState.None;
    }

    public bool CheckWinner(CellState state)
    {
        foreach (var v in Settings.winRules)
        {
            if (v.Check(Cells, state))
            {
                return true;
            }

        }
        return false;
    }

}
