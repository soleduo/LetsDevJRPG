using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TurnBasedCombatController : MonoBehaviour
{
    public static TurnBasedCombatController Instance { get; private set; }

    public List<Unit> unitsInCombat;
    public TimelineUIController timelineController;

    List<UnitTimeline> timelines = new List<UnitTimeline>();


    private int playerCount= 0;
    private int enemyCount= 0;

    public void OnUnitFallen(EUnitControlType type)
    {
        if (type == EUnitControlType.PLAYER)
            playerCount--;

        if (type == EUnitControlType.AI)
            enemyCount--;

        if (playerCount <= 0)
            CombatEnd(false);

        if (enemyCount <= 0)
            CombatEnd(true);
    }

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        for(int i =0; i < unitsInCombat.Count; i++)
        {
            unitsInCombat[i].onTurnStart += (turn)=>StartCoroutine(WaitForCommandExecution(turn, timelines[i]));
            timelines.Add(unitsInCombat[i].Timeline);

            if (unitsInCombat[i].UnitType == EUnitControlType.PLAYER)
                playerCount++;
            if (unitsInCombat[i].UnitType == EUnitControlType.AI)
                enemyCount++;

            unitsInCombat[i].onUnitFallen += OnUnitFallen;
        }

        timelineController.Initialize(timelines);
        StartCoroutine(CombatUpdate());
    }

    private IEnumerator CombatUpdate()
    {
        int activeIndex = -1;

        while (activeIndex < 0) {
            for (int i = 0; i < timelines.Count; i++)
            {
                timelines[i].Update();
            }

            activeIndex = timelines.FindIndex((i) => i.Value <= 0);

            yield return null;
        }

        yield return WaitForCommandExecution(unitsInCombat[activeIndex].Controller, timelines[activeIndex]);

        yield return CombatUpdate();
    }

    IEnumerator WaitForCommandExecution(UnitController activeUnit, UnitTimeline timeline)
    {
        yield return activeUnit.ActivateTurn();

        timeline.Update();

        yield return new WaitForSeconds(2f);
    }

    void CombatEnd(bool isWinning)
    {
        StopAllCoroutines();

        GUIMessageHelper.PrintConsole(isWinning ? "Player Wins!" : "Game Over");

        for (int i = 0; i < unitsInCombat.Count; i++)
        {
            unitsInCombat[i].enabled = false;
        }
    }
/*
    public IEnumerator TurnUpdate() // might be able to change this to a better async method
    {
        int activeTurn = -1; // something to store our next character to move
        int _nearest = GetNextTurn(out activeTurn); // get the amount of "ticks" to next move

        for(int i = 0; i < unitTimelineDatas.Count; i++) //Move all UnitTimelineData for x amount
        {
            unitTimelineDatas[i].Update(_nearest);

            yield return null;
        }

        //Move all timeline icons for x amounts in t seconds
        yield return combatUIController.Move(_nearest * 0.01f, activeTurn);


        yield return characterController[activeTurn].ActivateTurn();

        //reset the active character position in timeline
        if( activeTurn >= 0)
        { 
            unitTimelineDatas[activeTurn] = new UnitTimelineData(combatCharacterData[activeTurn].speed); //let 1 is default action value
            combatUIController.Reset(activeTurn, unitTimelineDatas[activeTurn].Value);
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

*/
}
