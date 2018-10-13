using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager
{
    public PlayerController[] players;

    #region singleton
    private static CustomNetworkManager instance;

    public static CustomNetworkManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CustomNetworkManager>();
            }
            return instance;
        }
    }

    #endregion

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        var currentPlayerCount = NetworkServer.connections.Count;

        if (currentPlayerCount <= GameViewer.Instance.MaxPlayers())
        {
            GameObject player = Instantiate(playerPrefab);
            NetworkServer.AddPlayerForConnection(conn,player,playerControllerId);
            
            if (currentPlayerCount == GameViewer.Instance.MaxPlayers())
            {
                players = FindObjectsOfType<PlayerController>();
                PlayerController.Instance.StartGame();
            }
        }
        else
        {
            if (currentPlayerCount > 2)
            {
                conn.Disconnect();
            }
        }
    }
   
}
