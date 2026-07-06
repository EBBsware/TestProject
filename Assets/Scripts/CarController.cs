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
    
    // Başlangıç pozisyonunu hafızada tutmak için:
    private Vector2 startPosition;
    private Quaternion startRotation;
    private Vector2 backWheelStartPos;
    private Vector2 frontWheelStartPos;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Oyun başladığı anki konumu kaydet
        startPosition = transform.position;
        startRotation = transform.rotation;

        // Tekerleklerin de başlangıç konumunu kaydet (Işınlanırken patlamaması için)
        if (backWheel != null && backWheel.connectedBody != null)
            backWheelStartPos = backWheel.connectedBody.transform.position;
        if (frontWheel != null && frontWheel.connectedBody != null)
            frontWheelStartPos = frontWheel.connectedBody.transform.position;
        
        // Senin fikrini uyguluyoruz: Ağırlık merkezindeki tüm hileleri siliyoruz.
        // Artık gerçek bir binek araç gibi orijinal ağırlık merkezinde kalacak.
        rb.centerOfMass = Vector2.zero; 
        
        // Açısal sürtünmeyi normale alıyoruz
        rb.angularDamping = 1f;

        if (backWheel != null)
        {
            backWheel.useMotor = true;
            motor = backWheel.motor;
            motor.motorSpeed = 0f;
            motor.maxMotorTorque = 0f;
            backWheel.motor = motor;
        }
    }

    void FixedUpdate() // Fizik işlemleri olduğu için Update yerine FixedUpdate kullanıyoruz
    {
        // 1. Motor Hareketi
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

        // Açı limiti kontrolü LateUpdate'e taşındı (Aşağıda)
    }

    void LateUpdate()
    {
        float currentAngle = rb.rotation % 360f;
        if (currentAngle > 180f) currentAngle -= 360f;
        else if (currentAngle < -180f) currentAngle += 360f;

        float targetAngle = currentAngle;

        if (currentAngle > maxBackwardTilt)
        {
            targetAngle = maxBackwardTilt;
            rb.angularVelocity = 0f;
        }
        else if (currentAngle < -maxForwardTilt)
        {
            targetAngle = -maxForwardTilt;
            rb.angularVelocity = 0f;
        }
        else if (!isAccelerating && !isBraking)
        {
            // Boşta: yavaşça düzle (doğal öne yatışı düzeltiyor)
            targetAngle = Mathf.LerpAngle(currentAngle, 0f, Time.deltaTime * 3f);
            rb.angularVelocity *= 0.9f;
        }

        rb.rotation = targetAngle;

        // Görsel rotasyonu güncelle ama X ve Y'yi koru — mirror bozulmasın!
        Vector3 e = transform.eulerAngles;
        transform.rotation = Quaternion.Euler(e.x, e.y, targetAngle);
    }

    // Geri Dönüş (Reset) Butonu İçin
    public void ResetCar()
    {
        // Önce arabanın kendi konumunu, açısını ve hızını sıfırla
        transform.position = startPosition;
        transform.rotation = startRotation;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        // SONRA tekerlekleri tam başladıkları yere ışınla ve hızlarını sıfırla. 
        // Aksi takdirde gövde ışınlanırken tekerlekler eski yerinde kalır ve yaylar gerilip patlar!
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

    // UI Butonları
    public void AccelerateDown() { isAccelerating = true; }
    public void AccelerateUp() { isAccelerating = false; }
    public void BrakeDown() { isBraking = true; }
    public void BrakeUp() { isBraking = false; }
}
