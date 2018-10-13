using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class GameSettings  
{
    public int CountOfColumns;
    public int CountOfRows;

    public List<IWinRule> winRules;

    public  int MAX_PLAYERS = 2;

    public GameSettings(GameType gameType)
    {
        switch (gameType)
        {
            case GameType.Default3:
                CountOfColumns = 3;
                CountOfRows = 3;
                winRules = new List<IWinRule>() {  new HorizontalRule(3) , new VerticalRule(3) , new DiagonalRule(3) };
                break;
            case GameType.Default4:
                CountOfColumns = 4;
                CountOfRows = 4;
                winRules = new List<IWinRule>() { new VerticalRule(2), new HorizontalRule(4), new DiagonalRule(4)};
                break;
        }
    }
}

public enum GameType
{
    Default3,
    Default4
}