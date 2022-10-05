using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitTimeline 
{
    private float value;
    public float Value { get { return value; } }
    public event FloatEvent onValueChanged;

    public UnitTimeline(int speed)
    {
        int baseValue = 28 - Mathf.RoundToInt(4 * Mathf.Log(speed) + 1);
        baseValue = Mathf.Clamp(baseValue, 3, 28);

        int startValue = Random.Range(baseValue, (Mathf.FloorToInt(baseValue / 3) + 1) * 3);

        value = startValue + 72;
        Debug.Log("Value " + value);
    }

    public void Update()
    {
        if (CombatUtility.TimelineSpeed < 0)
            return;

        if (value <= 0)
        {
            value = 100;
            return;
        }

        value -= CombatUtility.TimelineSpeed * Time.deltaTime * 10;
        Mathf.Clamp(value, 0, 100);
        onValueChanged?.Invoke(value);
    }

}

public delegate void FloatEvent(float value);

