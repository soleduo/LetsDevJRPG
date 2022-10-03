using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DebugClickEnemy : MonoBehaviour, IPointerClickHandler
{
    public static DebugClickEnemy Instance { get; private set; }

    public event VoidEvent onClick;

    private void Awake()
    {
        Instance = this;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onClick?.Invoke();
        onClick = null;
    }
}
