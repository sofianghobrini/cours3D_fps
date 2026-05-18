using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    private PlayerController playerController;

    [SerializeField]
    private RectTransform JetpackFuelBar; // Référence à la barre de carburant du jetpack

    private void SetFuelAmount(float amount)
    {
        JetpackFuelBar.localScale = new Vector3(1f, amount, 1f); // Met à jour l'échelle de la barre pour refléter le niveau de carburant
    }

    private void Update()
    {
        SetFuelAmount(playerController.GetJetpackFuelAmount()); // Met à jour la barre de carburant en fonction du niveau actuel du jetpack
    }

    public void SetController(PlayerController controller)
    {
        playerController = controller;
    }
}
