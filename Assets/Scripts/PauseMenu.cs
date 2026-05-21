using UnityEngine;
using Mirror;
using NUnit.Framework;

public class PauseMenu : NetworkBehaviour
{
    public static bool isOn = false;

    private NetworkManager networkManager;


    public void Start()
    {
        networkManager = NetworkManager.singleton;
    }


    public void DisconnectButton()
    {
        if (isClientOnly)
        {
            networkManager.StopClient();
        }
        else
        {
            networkManager.StopHost();
        }
    } 
}
