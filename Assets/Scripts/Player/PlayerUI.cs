using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    private Player player;

    private PlayerController controller;
    
    private WeaponManager weaponManager;

    [SerializeField]
    private RectTransform JetpackFuelBar; // Référence à la barre de carburant du jetpack


    [SerializeField]
    private RectTransform HealthBar; // Référence à la barre de santé du joueur

    [SerializeField]
    private Text ammoText; // Référence à l'élément UI pour afficher les munitions


    [SerializeField]
    private GameObject pauseMenu;

    [SerializeField]
    private GameObject scoreBoard; 
 
    private void Start()
    {
        PauseMenu.isOn = false;
    } 

    private void Update()
    {
        SetFuelAmount(controller.GetJetpackFuelAmount()); // Met à jour la barre de carburant en fonction du niveau actuel du jetpack
        SetHealthAmount(player.GetHealthPct()); // Met à jour la barre de santé en fonction du niveau actuel de santé du joueur
        SetAmmoAmount(weaponManager.currentAmmoSize); // Met à jour l'affichage des munitions en fonction du nombre actuel de munitions et du maximum

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Affiche ou masque le menu de pause
            // Vous pouvez implémenter la logique pour afficher un menu de pause ici
            TogglePauseMenu();
        }


        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // Affiche ou masque le tableau des scores
            scoreBoard.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            scoreBoard.SetActive(false);
        }
    }

    public void TogglePauseMenu()
    {
        // Implémentez la logique pour afficher ou masquer le menu de pause
        // Par exemple, vous pouvez activer/désactiver un GameObject qui représente le menu de pause
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        PauseMenu.isOn = pauseMenu.activeSelf;
    }

    public void SetPlayer(Player _player)
    {
        player = _player;
        controller = player.GetComponent<PlayerController>();
        weaponManager = player.GetComponent<WeaponManager>();
    }


    
    private void SetFuelAmount(float _amount)
    {
        JetpackFuelBar.localScale = new Vector3(1f, _amount, 1f); // Met à jour l'échelle de la barre pour refléter le niveau de carburant
    }

    private void SetHealthAmount(float _amount)
    {
        HealthBar.localScale = new Vector3(1f, _amount, 1f); // Met à jour l'échelle de la barre pour refléter le niveau de santé
    }

    // Implémentez la logique pour mettre à jour l'affichage des munitions
    // Par exemple, vous pouvez utiliser un Text ou une Image pour afficher le nombre de munitions restantes
    // Assurez-vous d'avoir une référence à l'élément UI approprié pour afficher les munitions
    private void SetAmmoAmount(int _amount)
    {
        ammoText.text = _amount.ToString();
    }

}
