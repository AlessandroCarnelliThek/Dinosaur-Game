using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TogglePauseButton : MonoBehaviour, IPointerDownHandler
{
    public UnityEvent onTogglePauseButtonDown;

    public void OnPointerDown(PointerEventData eventData)
    {
        onTogglePauseButtonDown.Invoke();
        Debug.Log("PAUSE");
    }
}
