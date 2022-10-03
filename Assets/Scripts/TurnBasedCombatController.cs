using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TurnBasedCombatController : MonoBehaviour
{
    public List<UnitTimelineData> unitTimelineDatas;

    private int unitInCombat = 4;
    [SerializeField]private CombatUIController combatUIController;

    // Start is called before the first frame update
    void Start()
    {
        unitTimelineDatas = new List<UnitTimelineData>();

        for(int i = 0; i < unitInCombat; i++)
        {
            int randomSpeed = Random.Range(4, 65);
            unitTimelineDatas.Add(new UnitTimelineData(randomSpeed , 0));
        }

        List<int> values = unitTimelineDatas.Select((u) => u.Value).ToList();
        Debug.Log(values.Count);

        combatUIController.InitializeUI(values);

        StartCoroutine(TurnUpdate());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator TurnUpdate() // might be able to change this to a better async method
    {
        int activeTurn = -1; // something to store our next character to move
        int _nearest = GetNextTurn(out activeTurn); // get the amount of "ticks" to next move

        for(int i = 0; i < unitTimelineDatas.Count; i++) //Move all UnitTimelineData for x amount
        {
            unitTimelineDatas[i].UpdateValue(_nearest);

            yield return null;
        }

        //Move all timeline icons for x amounts in t seconds
        yield return combatUIController.MoveUI(_nearest * 0.01f, activeTurn);

        yield return new WaitForSeconds(3f); // wait for actions to be executed;

        //reset the active character position in timeline
        if( activeTurn >= 0)
        { 
            int randomSpeed = Random.Range(4, 25);
            unitTimelineDatas[activeTurn] = new UnitTimelineData(randomSpeed); //let 1 is default action value
            combatUIController.ResetUI(activeTurn, unitTimelineDatas[activeTurn].Value);
        }

        StartCoroutine(TurnUpdate()); // repeat
    }

    /// <summary>
    /// Calculates the character to move next. In the final MVP it will be affected by character speed stat.
    /// </summary>
    /// <param name="activeTurn">Character to move next</param>
    /// <returns>Amount of "ticks" for the next character to move</returns>
    public int GetNextTurn(out int activeTurn)
    {
        activeTurn = -1;
        int _nearest = 9999;

        // get nearest character by distance to action timeline 0 point
        for(int i = 0; i < unitTimelineDatas.Count; i++)
        {
            if(unitTimelineDatas[i].Value < _nearest)
            {
                _nearest = unitTimelineDatas[i].Value;
                activeTurn = i;
            }
        }

        Debug.Log("Nearest set " + _nearest);

        return _nearest;
    }

    
}

[System.Serializable]
public class CombatUIController
{
    public List<Image> turnOrderIcons;
    private float maxTimelinePosition = 1228f;

    private const int TimelineUpdateDuration = 3;

    public void InitializeUI(List<int> values)
    {
        for (int i = 0; i < values.Count; i++)
        {
            turnOrderIcons[i].rectTransform.anchoredPosition = Vector2.right * values[i] * 0.01f * maxTimelinePosition;
        }
    }

    public void ResetUI(int activeTurn, int value)
    {
        turnOrderIcons[activeTurn].rectTransform.anchoredPosition = Vector2.right * value * 0.01f * maxTimelinePosition;
    }

    public IEnumerator MoveUI(float totalReduction, int activeTurn)
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
