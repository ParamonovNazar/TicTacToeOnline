public class DiagonalRule : IWinRule
{
    int MinLenSeq;

    public DiagonalRule(int length)
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

                    if (cells.GetLength(1) - col >= MinLenSeq&& cells.GetLength(0) - row >= MinLenSeq)
                    {
                        for (int i = 0; i < MinLenSeq; i++)
                        {
                            if (cells[row + i, col + i].GetState() == state)
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

                    lengthOfSeq = 0;
                    if (col + 1 - MinLenSeq >= 0 && cells.GetLength(0) - row >= MinLenSeq)
                    {
                        for (int i = 0; i < MinLenSeq; i++)
                        {
                            if (cells[row + i, col - i].GetState() != state)
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
