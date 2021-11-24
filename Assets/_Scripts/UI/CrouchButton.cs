using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CrouchButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public UnityEvent onCrouchButtonDown;
    public UnityEvent onCrouchButtonUp;

    public void OnPointerDown(PointerEventData eventData)
    {
        onCrouchButtonDown.Invoke();
        Debug.Log("Crouch BTN Down");
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        onCrouchButtonUp.Invoke();
        Debug.Log("Crouch BTN Up");
    }
}
