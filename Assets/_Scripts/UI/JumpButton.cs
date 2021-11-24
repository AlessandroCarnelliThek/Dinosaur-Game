using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JumpButton : MonoBehaviour, IPointerDownHandler
{
    public UnityEvent onJumpButtonDown;

    public void OnPointerDown(PointerEventData eventData)
    {
        onJumpButtonDown.Invoke();
        Debug.Log("Jump Down");
    }

}
