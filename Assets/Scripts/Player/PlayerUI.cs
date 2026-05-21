using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    private PlayerController playerController;

    [SerializeField]
    private RectTransform JetpackFuelBar; // Référence à la barre de carburant du jetpack


    [SerializeField]
    private GameObject pauseMenu; 


    private void SetFuelAmount(float amount)
    {
        JetpackFuelBar.localScale = new Vector3(1f, amount, 1f); // Met à jour l'échelle de la barre pour refléter le niveau de carburant
    }

    
    private void Start()
    {
        PauseMenu.isOn = false;
    } 

    private void Update()
    {
        SetFuelAmount(playerController.GetJetpackFuelAmount()); // Met à jour la barre de carburant en fonction du niveau actuel du jetpack

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Affiche ou masque le menu de pause
            // Vous pouvez implémenter la logique pour afficher un menu de pause ici
            TogglePauseMenu();
        }
    }

    public void TogglePauseMenu()
    {
        // Implémentez la logique pour afficher ou masquer le menu de pause
        // Par exemple, vous pouvez activer/désactiver un GameObject qui représente le menu de pause
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        PauseMenu.isOn = pauseMenu.activeSelf;
    }

    public void SetController(PlayerController controller)
    {
        playerController = controller;
    }

}
