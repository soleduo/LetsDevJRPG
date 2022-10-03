using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TurnBasedCombatController : MonoBehaviour
{
    public List<CombatCharacterData> combatCharacterData;
    public List<UnitTimelineData> unitTimelineDatas;

    public List<CombatCharacterController> characterController;

    [SerializeField] private TimelineUIController combatUIController;

    // Start is called before the first frame update
    void Start()
    {
        unitTimelineDatas = new List<UnitTimelineData>();
        characterController = new List<CombatCharacterController>();

        for(int i = 0; i < combatCharacterData.Count; i++)
        {
            unitTimelineDatas.Add(new UnitTimelineData(combatCharacterData[i].speed));
            if (combatCharacterData[i].name == "Enemy")
                characterController.Add(new AICombatController(combatCharacterData[i].name));
            else
                characterController.Add(new PlayerCombatController(combatCharacterData[i].name));
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


        yield return characterController[activeTurn].ActivateTurn();

        //reset the active character position in timeline
        if( activeTurn >= 0)
        { 
            unitTimelineDatas[activeTurn] = new UnitTimelineData(combatCharacterData[activeTurn].speed); //let 1 is default action value
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
