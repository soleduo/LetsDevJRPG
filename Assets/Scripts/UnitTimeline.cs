using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitTimeline 
{
    private float value;
    public float Value { get { return value; } }
    public event FloatEvent onValueChanged;

    int baseValue;
    int startValue;

    public UnitTimeline(int speed)
    {
        CalculateStartValue(speed);

        value = startValue;
        Debug.Log("Value " + value);
    }

    public void CalculateStartValue(int currentSpeed)
    {
        baseValue = 28 - Mathf.RoundToInt(4 * Mathf.Log(currentSpeed) + 1);
        baseValue = Mathf.Clamp(baseValue, 3, 28);

        startValue = 68 + Random.Range(baseValue, (Mathf.FloorToInt(baseValue / 3) + 1) * 3);
    }

    public void Update()
    {
        if (CombatUtility.TimelineSpeed < 0)
            return;

        if(value <= 0)
        {
            value = startValue;
            onValueChanged?.Invoke(value);

            return;
        }

        value -= CombatUtility.TimelineSpeed * Time.deltaTime * 10;
        Mathf.Clamp(value, 0, 100);
        onValueChanged?.Invoke(value);
    }

}

public delegate void FloatEvent(float value);

