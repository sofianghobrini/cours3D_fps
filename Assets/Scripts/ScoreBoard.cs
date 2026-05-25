using UnityEngine;

public class ScoreBoard : MonoBehaviour
{

    [SerializeField]
    GameObject playerScoreBoardItems; 

    [SerializeField]
    Transform playerScoreBoardList; // Préfabriqué pour une entrée du tableau des scores




    private void OnEnable()
    {
        //Récupere tous les joueurs et met à jour le tableau des scores

        Player[] players = GameManager.GetAllPlayers();
        foreach(Player player in players)
        {
            GameObject itemGo = Instantiate(playerScoreBoardItems, playerScoreBoardList.transform);
            PlayerScoreBoardItem item = itemGo.GetComponent<PlayerScoreBoardItem>();
            if(item != null)
            {
                item.setup(player);
            }
        }
    }

    private void OnDisable()
    {
        // vider notre list
        foreach(Transform child in playerScoreBoardList)
        {
            Destroy(child.gameObject);
        }
    }

}
