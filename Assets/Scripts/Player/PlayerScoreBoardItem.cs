using UnityEngine;
using UnityEngine.UI;

public class PlayerScoreBoardItem : MonoBehaviour
{
    [SerializeField]
    private Text playerNameText;
    [SerializeField]
    private Text playerKillsText;
    [SerializeField]
    private Text playerDeathsText;

    public void setup(Player player)
    {
        playerNameText.text = player.username;
        playerKillsText.text = player.kills.ToString();
        playerDeathsText.text = player.deaths.ToString();
    }
}
