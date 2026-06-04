using UnityEngine;
using UnityEngine.UI;


public class PlayerNamePlate : MonoBehaviour
{
    [SerializeField] 
    private Text playerNameText;

    [SerializeField]
    private RectTransform healthBarFill;

    [SerializeField]
    private Player player;

    void Update()
    {
        playerNameText.text = player.username;
        healthBarFill.localScale = new Vector3(player.GetHealthPct(), 1, 1);
    }
}
