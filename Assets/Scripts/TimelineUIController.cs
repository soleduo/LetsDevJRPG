using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimelineUIController : MonoBehaviour
{
    public List<Image> turnOrderIcons;
    public RectTransform container;

    private float maxTimelinePosition;

    private const float maxDuration = 1.2f;

    private bool[] updated;

    public void Initialize(List<UnitTimeline> unitTimelines)
    {
        maxTimelinePosition = container.rect.width;
        CombatUtility.TimelineSpeed = maxTimelinePosition * 0.01f / maxDuration;
        Debug.Log("Timeline Speed " + CombatUtility.TimelineSpeed);

        updated = new bool[unitTimelines.Count];

        for (int i = 0; i < unitTimelines.Count; i++)
        {
            int index = i;
            turnOrderIcons[i].rectTransform.anchoredPosition = Vector2.right * unitTimelines[i].Value * 0.01f * maxTimelinePosition;
            unitTimelines[i].onValueChanged += (value) => RefreshPosition(index, value);
        }
    }

    private void LateUpdate()
    {
        updated = new bool[updated.Length];
    }

    public void Reset(int activeTurn, int value)
    {
        turnOrderIcons[activeTurn].rectTransform.anchoredPosition = Vector2.right * value * 0.01f * maxTimelinePosition;
    }

    public void RefreshPosition(int index, float value)
    {
        if (!updated[index])
        {
            value = Mathf.Clamp(value, 0, 100);
            turnOrderIcons[index].rectTransform.anchoredPosition = Vector2.right * value * 0.01f * maxTimelinePosition;
            updated[index] = true;
        }
    }
}

public static class CombatUtility
{
    public static float TimelineSpeed = -1f;
}
