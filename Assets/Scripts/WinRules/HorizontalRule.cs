public class HorizontalRule : IWinRule
{
    int MinLenSeq;

    public HorizontalRule(int length)
    {
        MinLenSeq = length;
    }

    public bool Check(ICell[,] cells, CellState state)
    {
        for (int row = 0; row < cells.GetLength(0); row++)
        {
            for (int col = 0; col < cells.GetLength(1); col++)
            {
                if (cells[row, col].GetState() == state)
                {
                    int lengthOfSeq = 0;
                    if (cells.GetLength(1) - col >= MinLenSeq) {
                        for (int i = col; i < cells.GetLength(1); i++)
                        {
                            if (cells[row, i].GetState() == state)
                            {
                                lengthOfSeq++;
                            }
                            else
                            {
                                break;
                            }
                        }
                        if (lengthOfSeq >= MinLenSeq)
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }
}
