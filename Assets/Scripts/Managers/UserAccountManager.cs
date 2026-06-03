using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//using TMPro;

public class UserAccountManager : MonoBehaviour
{
    public static UserAccountManager instance;

    public static string LoggedInUsername;

    public string lobbySceneName = "Lobby";

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Il y a plus d'une instance de UserAccountManager dans la scène.");
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);

    }

    public void Login(Text username)
    {
        LoggedInUsername = username.text;
        Debug.Log("Utilisateur connecté : " + LoggedInUsername);
        SceneManager.LoadScene(lobbySceneName);
    }

}
