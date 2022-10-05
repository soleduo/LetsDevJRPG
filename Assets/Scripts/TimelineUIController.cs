using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TimelineUIController
{
    public List<Image> turnOrderIcons;
    private float maxTimelinePosition = 1820f;

    private const float TimelineUpdateDuration = 1.2f;

    public void Initialize(List<int> values)
    {
        for (int i = 0; i < values.Count; i++)
        {
            turnOrderIcons[i].rectTransform.anchoredPosition = Vector2.right * values[i] * 0.01f * maxTimelinePosition;
        }
    }

    public void Reset(int activeTurn, int value)
    {
        turnOrderIcons[activeTurn].rectTransform.anchoredPosition = Vector2.right * value * 0.01f * maxTimelinePosition;
    }

    public IEnumerator Move(float totalReduction, int activeTurn)
    {
        totalReduction *= maxTimelinePosition;
        float moveSpeed = (totalReduction / TimelineUpdateDuration);
        while (turnOrderIcons[activeTurn].rectTransform.anchoredPosition.x > 0)
        {
            for (int i = 0; i < turnOrderIcons.Count; i++)
            {
                turnOrderIcons[i].rectTransform.anchoredPosition -= Vector2.right * moveSpeed * Time.deltaTime;
            }

            yield return null;
        }
    }
}
