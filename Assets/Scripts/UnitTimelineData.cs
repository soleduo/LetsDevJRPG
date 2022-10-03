using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitTimelineData 
{
    private int value;
    public int Value { get { return value; } }

    public UnitTimelineData(int speed, int actionValue = -1)
    {
        int baseValue = 28 - Mathf.RoundToInt(4 * Mathf.Log(speed) + 1);
        baseValue = Mathf.Clamp(baseValue, 3, 28);

        int startValue = Random.Range(baseValue, (Mathf.FloorToInt(baseValue / 3) + 1) * 3);

        if (actionValue <= 0)
            actionValue = 72;

        value = startValue + actionValue;
        Debug.Log("Value " + value);
    }

    public void UpdateValue(int valueUpdate)
    {
        if(value - valueUpdate < -0.5f)
        {
            return;
        }

        value -= valueUpdate;
    }
}
