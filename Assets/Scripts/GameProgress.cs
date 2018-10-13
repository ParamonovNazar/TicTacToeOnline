using UnityEngine;
using UnityEngine.UI;

public class GameProgress : MonoBehaviour
{
    private  uint curTurnPlayer;
    private bool isPlaying=false;

    [SerializeField]
    private Text YouWinsCount;
    [SerializeField]
    private Text OpponentWinsCount;
    [SerializeField]
    private Text PlayerTurn;
    
    [SerializeField]
    private Text WinPanelText;
    [SerializeField]
    private Animator WinPanelAnim;

    #region singleton
    private static GameProgress instance;
    public static GameProgress Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameProgress>();
            }
            return instance;
        }
    }
    #endregion

    public uint CurTurnPlayer
    {
        get
        {
            return curTurnPlayer;
        }

        set
        {
            curTurnPlayer = value;
            uint localPlauer = PlayerController.Instance.netId.Value;
            PlayerTurn.text = curTurnPlayer == localPlauer ? "YOU" : "OPPONENT";
        }
    }

    public bool IsPlaying
    {
        get
        {
            return isPlaying;
        }

        set
        {
            isPlaying = value;
        }
    }

    private void Start()
    {
        PlayerTurn.text = "";
    }

    

    public void ShowEndGame(GameEnd gameEnd)
    {
        switch (gameEnd)
        {
            case GameEnd.Draw:
                WinPanelText.text = "DRAW";
                break;
            case GameEnd.You:
                WinPanelText.text = "YOU WIN";
                break;
            case GameEnd.Opponent:
                WinPanelText.text = "OPPONENT WINS";
                break;
        }
        WinPanelAnim.SetTrigger("EndGame");
    }

    public void UpdateYouWins(int winCount)
    {
        YouWinsCount.text = winCount.ToString();
    }

    public void UpdateOpponentWins(int winCount)
    {
        OpponentWinsCount.text = winCount.ToString();
    }

    public void WantPlayNewGame()
    {
        PlayerController.Instance.WantPlayNewGame();
    }
}
public enum GameEnd
{
    You,
    Opponent,
    Draw
}