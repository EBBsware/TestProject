using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Tekerlekler")]
    public WheelJoint2D backWheel;
    public WheelJoint2D frontWheel;

    [Header("Araç Özellikleri")]
    public float motorTorque = 2500f; 
    public float maxMotorSpeed = -5000f; 
    public float brakeForce = 5000f;

    [Header("Fizik & Denge (Stabilizer)")]
    [Tooltip("Arabanın geriye şahlanabileceği maksimum açı (Derece)")]
    public float maxBackwardTilt = 5f;
    [Tooltip("Arabanın öne kapaklanabileceği maksimum açı (Derece) - Daha az esner")]
    public float maxForwardTilt = 2f;

    private bool isAccelerating = false;
    private bool isBraking = false;

    private Rigidbody2D rb;
    private JointMotor2D motor;
    
    private Vector2 startPosition;
    private Quaternion startRotation;
    private Vector2 backWheelStartPos;
    private Vector2 frontWheelStartPos;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        startPosition = transform.position;
        startRotation = transform.rotation;

        if (backWheel != null && backWheel.connectedBody != null)
            backWheelStartPos = backWheel.connectedBody.transform.position;
        if (frontWheel != null && frontWheel.connectedBody != null)
            frontWheelStartPos = frontWheel.connectedBody.transform.position;
        

        // Ağırlık merkezini iki WheelJoint'in tam ortasına al
        if (backWheel != null && frontWheel != null)
        {
            Vector2 backAnchor  = backWheel.anchor;   // Local space anchor
            Vector2 frontAnchor = frontWheel.anchor;  // Local space anchor
            Vector2 midPoint    = (backAnchor + frontAnchor) / 2f;
            rb.centerOfMass     = midPoint;
        }
        else
        {
            rb.centerOfMass = Vector2.zero;
        }

        // 0.3 saniye bekleyip süspansiyon yerleştikten sonra dinlenme açısını kaydet
        StartCoroutine(CalibrateRestAngle());

        if (backWheel != null)
        {
            backWheel.useMotor = true;
            motor = backWheel.motor;
            motor.motorSpeed = 0f;
            motor.maxMotorTorque = 0f;
            backWheel.motor = motor;
        }
    }

    void FixedUpdate()
    {
        if (backWheel != null)
        {
            if (isAccelerating)
            {
                motor.motorSpeed = maxMotorSpeed;
                motor.maxMotorTorque = motorTorque;
                backWheel.motor = motor;
            }
            else if (isBraking)
            {
                motor.motorSpeed = 0f;
                motor.maxMotorTorque = brakeForce;
                backWheel.motor = motor;
            }
            else
            {
                motor.motorSpeed = 0f;
                motor.maxMotorTorque = 0f;
                backWheel.motor = motor;
            }
        }
    }

    void LateUpdate()
    {
        float currentAngle = rb.rotation % 360f;
        if (currentAngle > 180f) currentAngle -= 360f;
        else if (currentAngle < -180f) currentAngle += 360f;

        // Gaz veya fren: limit uygula
        if (currentAngle > maxBackwardTilt)
        {
            rb.rotation = maxBackwardTilt;
            rb.angularVelocity = 0f;
        }
        else if (currentAngle < -maxForwardTilt)
        {
            rb.rotation = -maxForwardTilt;
            rb.angularVelocity = 0f;
        }

        // Boşta: yavaşça 0 dereceye dön (doğal öne yatışı düzeltiyor)
        if (!isAccelerating && !isBraking)
        {
            float corrected = Mathf.LerpAngle(currentAngle, 0f, Time.deltaTime * 2f);
            rb.rotation = corrected;
            rb.angularVelocity *= 0.85f;
        }
    }

    public void ResetCar()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        if (backWheel != null && backWheel.connectedBody != null)
        {
            backWheel.connectedBody.transform.position = backWheelStartPos;
            backWheel.connectedBody.linearVelocity = Vector2.zero;
            backWheel.connectedBody.angularVelocity = 0f;
        }
        if (frontWheel != null && frontWheel.connectedBody != null)
        {
            frontWheel.connectedBody.transform.position = frontWheelStartPos;
            frontWheel.connectedBody.linearVelocity = Vector2.zero;
            frontWheel.connectedBody.angularVelocity = 0f;
        }
    }

    public void AccelerateDown() { isAccelerating = true; }
    public void AccelerateUp() { isAccelerating = false; }
    public void BrakeDown() { isBraking = true; }
    public void BrakeUp() { isBraking = false; }
}
