using UnityEngine;


//Les RequireComponent ne sont pas obligatoires, mais ils permettent de s'assurer que les composants nécessaires sont attachés à l'objet du joueur.
[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(ConfigurableJoint))]
public class PlayerController : MonoBehaviour
{
    
    
    [SerializeField]
   private float speed = 3f;

    [SerializeField]
   private float mouseSensitivityX = 3f;

   [SerializeField]
   private float mouseSensitivityY = 10f;

   [SerializeField]
   private float thrustForce = 1000f;
   [Header("Joint Settings")] //Permet de séparer les paramètres pour le rendre plus lisible dans l'inspecteur de Unity
   [SerializeField]
   private float jointSpring = 20f;
   [SerializeField]
   private float jointMaxForce = 40f;

   private PlayerMotor motor;

   private ConfigurableJoint joint;

    private void Start()
    {
        motor = GetComponent<PlayerMotor>();
        joint = GetComponent<ConfigurableJoint>();
        SetJointSettings(jointSpring);
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

        float cameraRotationX = xRot * mouseSensitivityY;

        motor.RotateCamera(cameraRotationX);

        //Calcul de la vélocité de la force de propulsion
        Vector3 thrusterVelocity = Vector3.zero;

        //Appliquer une force de propulsion lorsque le joueur appuie sur la barre d'espace
        if(Input.GetButton("Jump"))
        {
            thrusterVelocity = Vector3.up * thrustForce;
            SetJointSettings(0f);
        }
        else
        {
            SetJointSettings(jointSpring);
        }

        motor.ApplyThruster(thrusterVelocity);
    }

    private void SetJointSettings(float _jointSpring)
    {
        joint.yDrive = new JointDrive { positionSpring = _jointSpring, maximumForce = jointMaxForce };
    }
}
