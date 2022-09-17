using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnBasedCombatController : MonoBehaviour
{
    public List<Image> turnOrderIcons;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TurnUpdate());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator TurnUpdate() // might be able to change this to a better async method
    {
        Image activeTurn; // something to store our next character to move
        float _nearest = GetNextTurn(out activeTurn); // get the amount of "ticks" to next move

        foreach (Image turnOrder in turnOrderIcons) // move all character in "ticks" amount
        {
            turnOrder.rectTransform.localPosition -= Vector3.right * _nearest;
        }

        yield return new WaitForSeconds(3f); // replace this with wait for input + action execute

        if(activeTurn != null) 
            activeTurn.rectTransform.anchoredPosition = new Vector2(1228f, 0f); // move the character taking action to the end of the timeline

        StartCoroutine(TurnUpdate()); // repeat
    }

    /// <summary>
    /// Calculates the character to move next. In the final MVP it will be affected by character speed stat.
    /// </summary>
    /// <param name="activeTurn">Character to move next</param>
    /// <returns>Amount of "ticks" for the next character to move</returns>
    public float GetNextTurn(out Image activeTurn)
    {
        activeTurn = null;
        float _nearest = 9999f; ;

        // get nearest character by distance to action timeline 0 point
        foreach(Image turnOrder in turnOrderIcons)
        {
            if (turnOrder.rectTransform.anchoredPosition.x < _nearest)
            {
                _nearest = turnOrder.rectTransform.anchoredPosition.x;
                activeTurn = turnOrder;
            }
        }

        Debug.Log("Nearest set " + _nearest);

        return _nearest;
    }
}
