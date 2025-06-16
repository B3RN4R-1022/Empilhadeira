using UnityEngine;

public class ForkliftController : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private float motorTorque = 100;
    [SerializeField] private float brakeForce = 30;
    [SerializeField] private float maxSteerAngle = 45;
    
    [Header("lift")]
    [SerializeField] public Transform lift;
    [SerializeField] private float speedLift;
    [SerializeField] private float maxDownLift = 2.4f;
    [SerializeField] private float maxUpLift = 9.5f;
    [Header("Collider")]
    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;

    [Header("Transform")]
    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;


    private float horizontalInput;
    private float verticalInput;
    private bool isBrake;
    private float brakeTorque;
    private float steerAngle;

    private bool isLiftDown;
    private bool isLiftUp;

    private Vector3 posicaoAnterior;
    private bool cargaAltaDetectada;

    private void FixedUpdate()
    {
        GetInput();
        HandleTorque();
        HandleSteering();
        UpdateWheelPosition();
        HandleLift();
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        isBrake = Input.GetKey(KeyCode.Space);

        if (Input.GetKey(KeyCode.I))
        {
            isLiftUp = true;
            isLiftDown = false;
        }
        else if (Input.GetKey(KeyCode.K))
        {
            isLiftUp = false;
            isLiftDown = true;
        }
        else
        {
            isLiftUp = false;
            isLiftDown = false;
        }
    }


    private void HandleTorque()
    {
        frontLeftWheelCollider.motorTorque = verticalInput * motorTorque;
        frontRightWheelCollider.motorTorque = verticalInput * motorTorque;
        rearLeftWheelCollider.motorTorque = verticalInput * motorTorque;
        rearRightWheelCollider.motorTorque = verticalInput * motorTorque;

        brakeTorque = (isBrake) ? brakeForce : 0;
        frontLeftWheelCollider.brakeTorque = brakeTorque;
        frontRightWheelCollider.brakeTorque = brakeTorque;
        rearLeftWheelCollider.brakeTorque = brakeTorque;
        rearRightWheelCollider.brakeTorque = brakeTorque;
    }

    private void HandleSteering()
    {
        steerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = steerAngle;
        frontRightWheelCollider.steerAngle = steerAngle;
    }

    private void UpdateWheelPosition()
    {
        ChangeWheelPosition(frontLeftWheelCollider, frontLeftWheelTransform);
        ChangeWheelPosition(frontRightWheelCollider, frontRightWheelTransform);
        ChangeWheelPosition(rearLeftWheelCollider, rearLeftWheelTransform);
        ChangeWheelPosition(rearRightWheelCollider, rearRightWheelTransform);

    }

    private void ChangeWheelPosition(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.position = pos;
        wheelTransform.rotation = rot;
    }

    private void HandleLift()
    {
        float y = lift.localPosition.y;
        if (isLiftUp)
        {
            y += speedLift * Time.deltaTime;
            y = Mathf.Clamp(y, maxDownLift, maxUpLift);

            lift.localPosition = new Vector3(lift.localPosition.x, y, lift.localPosition.z);
        }
        else if (isLiftDown)
        {
            y -= speedLift * Time.deltaTime;
            y = Mathf.Clamp(y, maxDownLift, maxUpLift);

            lift.localPosition = new Vector3(lift.localPosition.x, y, lift.localPosition.z);
        }
    }
    
    public Vector3 GetVelocity()
    {
        return frontLeftWheelCollider.attachedRigidbody.linearVelocity;
    }
}
