using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Tooltip("Sahnede oluşturduğumuz Car objesini buraya sürükleyin")]
    public CarController carController;
    
    [Tooltip("Bu buton hızlanma butonu ise işaretleyin. Değilse (fren ise) işareti kaldırın.")]
    public bool isAccelerateButton;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (carController == null) return;

        if (isAccelerateButton)
        {
            carController.AccelerateDown();
        }
        else
        {
            carController.BrakeDown();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (carController == null) return;

        if (isAccelerateButton)
        {
            carController.AccelerateUp();
        }
        else
        {
            carController.BrakeUp();
        }
    }
}
