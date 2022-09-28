using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitTimelineData 
{
    private float value;
    public float Value { get { return value; } }

    public UnitTimelineData(float speed, float actionValue)
    {
        value = (((256f - speed) / 256f) + actionValue) * 0.5f;
    }

    public void UpdateValue(float valueUpdate)
    {
        if(value - valueUpdate < -0.5f)
        {
            return;
        }

        value -= valueUpdate;
    }
}
