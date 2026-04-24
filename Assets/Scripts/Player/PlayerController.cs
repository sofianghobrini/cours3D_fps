using UnityEngine;


[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    
    
    [SerializeField]
   private float speed = 3f;

    [SerializeField]
   private float mouseSensitivityX = 3f;

   [SerializeField]
   private float mouseSensitivityY = 10f;


   private PlayerMotor motor;

    private void Start()
    {
        motor = GetComponent<PlayerMotor>();
    }

    private void Update()
    {
        //cacule la véloce du movement du joueur

        float xMov = Input.GetAxisRaw("Horizontal");
        float zMov = Input.GetAxisRaw("Vertical");

        Vector3 moveHorizontal = transform.right * xMov;
        Vector3 moveVertical = transform.forward * zMov;

        Vector3 velocity = (moveHorizontal + moveVertical).normalized * speed;

        motor.Move(velocity);


        // On calcul la rotation du joueur en vector 3
        float yRot = Input.GetAxisRaw("Mouse X");

        Vector3 rotation = new Vector3(0f, yRot, 0f) * mouseSensitivityX;

        motor.Rotate(rotation);

        // On calcul la rotation de la camera en vector 3
        float xRot = Input.GetAxisRaw("Mouse Y");

        Vector3 cameraRotation = new Vector3(xRot, 0f, 0f) * mouseSensitivityY;

        motor.RotateCamera(cameraRotation);
    }
}
