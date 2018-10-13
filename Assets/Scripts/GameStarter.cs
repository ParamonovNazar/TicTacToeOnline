using UnityEngine;
using UnityEngine.Networking;

public class GameStarter : MonoBehaviour
{
    private void Awake()
    {
        var Network = GetComponent<NetworkManager>();
        var netClient = Network.StartHost();
        if (netClient==null)
        {
            Network.StartClient();
        }
    }
   
}
