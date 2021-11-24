using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StartButton : MonoBehaviour, IPointerDownHandler
{
    public UnityEvent onStartButtonDown;

    public void OnPointerDown(PointerEventData eventData)
    {
        onStartButtonDown.Invoke();
        Debug.Log("START GAME");
    }

}
