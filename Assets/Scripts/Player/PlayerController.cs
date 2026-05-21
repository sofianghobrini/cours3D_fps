using UnityEngine;


//Les RequireComponent ne sont pas obligatoires, mais ils permettent de s'assurer que les composants nécessaires sont attachés à l'objet du joueur.
[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(Animator))]
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


   [SerializeField]
   private float jetpackFuelBurnSpeed = 1f;
   [SerializeField]
   private float jetpackFuelRegenSpeed = 0.5f;
   private float jetpackFuelAmount = 1f; // Représente le niveau de carburant du jetpack, allant de 0 à 1

    public float GetJetpackFuelAmount()
    {
        return jetpackFuelAmount;
    }

   [Header("Joint Settings")] //Permet de séparer les paramètres pour le rendre plus lisible dans l'inspecteur de Unity
   [SerializeField]
   private float jointSpring = 20f;
   [SerializeField]
   private float jointMaxForce = 40f;

   private PlayerMotor motor;

   private ConfigurableJoint joint;
   private Animator animator;

    private void Start()
    {
        motor = GetComponent<PlayerMotor>();
        animator = GetComponent<Animator>();
        joint = GetComponent<ConfigurableJoint>();
        SetJointSettings(jointSpring);
    }

    private void Update()
    {
        if(PauseMenu.isOn)
        {
            if(Cursor.lockState != CursorLockMode.None)
            {
                Cursor.lockState = CursorLockMode.None;
            }

            motor.Move(Vector3.zero);
            motor.Rotate(Vector3.zero);
            motor.RotateCamera(0f);
            motor.ApplyThruster(Vector3.zero);

            return;
        }

        if(Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        RaycastHit _hit;
        if(Physics.Raycast(transform.position, Vector3.down, out _hit, 100f))
        {
            joint.targetPosition = new Vector3(0f, -_hit.point.y, 0f);
        }
        else
        {
            joint.targetPosition = new Vector3(0f, 0f, 0f);
        }
        //cacule la véloce du movement du joueur

        float xMov = Input.GetAxisRaw("Horizontal");
        float zMov = Input.GetAxisRaw("Vertical");

        Vector3 moveHorizontal = transform.right * xMov;
        Vector3 moveVertical = transform.forward * zMov;

        Vector3 velocity = (moveHorizontal + moveVertical).normalized * speed;

        // Joue l'animation
        animator.SetFloat("ForwardVelocity", zMov);

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
        if(Input.GetButton("Jump") && jetpackFuelAmount > 0f)
        {

            jetpackFuelAmount -= jetpackFuelBurnSpeed * Time.deltaTime;

            if(jetpackFuelAmount >= 0.01f)
            {
                thrusterVelocity = Vector3.up * thrustForce;
                SetJointSettings(0f);
            }
        }
        else
        {
            jetpackFuelAmount += jetpackFuelRegenSpeed * Time.deltaTime;
            SetJointSettings(jointSpring);
        }

        jetpackFuelAmount = Mathf.Clamp(jetpackFuelAmount, 0f, 1f); // S'assure que le niveau de carburant reste entre 0 et 1

        motor.ApplyThruster(thrusterVelocity);
    }

    private void SetJointSettings(float _jointSpring)
    {
        joint.yDrive = new JointDrive { positionSpring = _jointSpring, maximumForce = jointMaxForce };
    }
}
