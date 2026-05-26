using UnityEngine;

public class KillFeed : MonoBehaviour
{
    [SerializeField]
    GameObject killFeedItemPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(GameManager.instance != null){
            GameManager.instance.onPlayerKilledCallback += OnKill;
        }
        else{
        Debug.LogError("GameManager introuvable depuis KillFeed !");
        }
    }

    // Update is called once per frame
    public void OnKill(string player, string source)
    {
        Debug.Log("OnKill reçu : " + source + " a tué " + player);
    
        GameObject go = Instantiate(killFeedItemPrefab, transform);
        Debug.Log("KillFeedItem instancié : " + go.name);
    
        go.GetComponent<KillFeedItem>().Setup(player, source);
        Destroy(go, 3f);
    }
}
