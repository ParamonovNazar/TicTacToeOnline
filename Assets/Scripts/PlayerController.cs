using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{
    [SyncVar (hook = "OnWinCountChange")]
    private int WinCount;

    [SyncVar]
    private bool IsWantNewGame;

    #region singleton
    private static PlayerController instance;

    public static PlayerController Instance
    {
        get
        {
            if (instance == null)
            {
                foreach (var v in FindObjectsOfType<PlayerController>())
                {
                    if (v.isLocalPlayer)
                    {
                        instance = v;
                        break;
                    }
                }
            }
            return instance;
        }
    }
    #endregion

    public override void OnStartLocalPlayer()
    {
        instance = this;
    }

    public void TryChangeCell(int row, int col)
    {
        if (!isLocalPlayer)
        {
            return;
        }
        if (isServer)
            CmdTryChangeCell(row, col, CellState.X);
        else
        {
            CmdTryChangeCell(row, col, CellState.O);
        }
    }
 
    [Command]
    void CmdTryChangeCell(int row, int col, CellState state )
    {
        if (GameProgress.Instance.IsPlaying)
        {
            if (GameProgress.Instance.CurTurnPlayer == this.netId.Value)
            {
                if (GameViewer.Instance.CheckEmptyCell(row, col))
                {
                    GameViewer.Instance.ChangeCell(row, col, state);
                    RpcChangeCell(row, col, state);
                    if (GameViewer.Instance.CheckWinner(state))
                    {
                        WinCount++;
                        GameProgress.Instance.IsPlaying = false;
                        RpcGameEnd(false);
                        return;
                    }
                    if (GameViewer.Instance.CheckEnd())
                    {
                        GameProgress.Instance.IsPlaying = false;
                        RpcGameEnd(true);
                        return;
                    }
                    ChangeTurn();
                }
            }
        }
    }

    [ClientRpc]
    public void RpcGameEnd(bool draw)
    {
        if (draw)
        {
            GameProgress.Instance.ShowEndGame(GameEnd.Draw);
        }
        else
        {
            if (isLocalPlayer)
            {
                GameProgress.Instance.ShowEndGame(GameEnd.You);
            }
            else
            {
                GameProgress.Instance.ShowEndGame(GameEnd.Opponent);
            }
        }
    }

    [ClientRpc]
    public void RpcChangeCell(int row, int col, CellState state)
    {
        GameViewer.Instance.ChangeCell(row, col, state);
    }

    [Server]
    public void StartGame()
    {
        var players = CustomNetworkManager.Instance.players;
        int randPlayer = Random.Range(0, players.Length);
        GameProgress.Instance.IsPlaying = true;
        foreach (var player in CustomNetworkManager.Instance.players)
        {
            player.IsWantNewGame = false;
        }

        RpcChangeTurn(players[randPlayer].netId.Value);
        RpcStartGame();
    }

    [ClientRpc]
    private void RpcStartGame()
    {
        GameViewer.Instance.ShowGame();
    }

    [Server]
    public void ChangeTurn()
    {
        foreach (var v in CustomNetworkManager.Instance.players)
        {
            if (v.netId.Value != GameProgress.Instance.CurTurnPlayer)
            {
                RpcChangeTurn(v.netId.Value);
                break;
            }
        }
    }

    [ClientRpc]
    private void RpcChangeTurn(uint curTurnPlayer)
    {
        GameProgress.Instance.CurTurnPlayer = curTurnPlayer;
    }

    private void OnWinCountChange(int winCount)
    {
        if (isLocalPlayer)
        {
            GameProgress.Instance.UpdateYouWins(winCount);
        }
        else
        {
            GameProgress.Instance.UpdateOpponentWins(winCount);
        }
    }

    public void WantPlayNewGame()
    {
        CmdTryStartNewGame();
    }

    [Command]
    private void CmdTryStartNewGame()
    {
        IsWantNewGame = true;

        if (!GameProgress.Instance.IsPlaying)
        {
            foreach (var player in CustomNetworkManager.Instance.players)
            {
                if (!player.IsWantNewGame)
                {
                    return;
                }
            }

            StartGame();
        }
    }
}