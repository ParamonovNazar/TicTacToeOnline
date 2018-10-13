public class VerticalRule : IWinRule
{
    int MinLenSeq;

    public VerticalRule(int length)
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
                    if (cells.GetLength(0) - row >= MinLenSeq)
                    {
                        for (int i = row; i < cells.GetLength(0); i++)
                        {
                            if (cells[i, col].GetState() == state)
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
