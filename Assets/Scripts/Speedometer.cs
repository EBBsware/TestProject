using UnityEngine;
using UnityEngine.UI;

public class Speedometer : MonoBehaviour
{
    [Tooltip("Hızı ölçülecek aracın gövdesi (CarBody)")]
    public Rigidbody2D carRigidbody;
    
    [Tooltip("Hızın yazdırılacağı arayüz metni (Text)")]
    public Text speedText;

    void Update()
    {
        if (carRigidbody != null && speedText != null)
        {
            // Unity'nin varsayılan birimlerini baz alıp hızı KM/H'ye çeviriyoruz (x 3.6)
            float speed = carRigidbody.linearVelocity.magnitude * 3.6f;
            
            // Hızı tam sayıya yuvarlayıp KM/H yazısını ekliyoruz
            speedText.text = Mathf.RoundToInt(speed).ToString() + " KM/H";
        }
    }
}
