using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Combat character data for speed and attack value
/// </summary>
[CreateAssetMenu(
        fileName = "CharacterData.asset",
        menuName = "ScriptableObjects/Game Data/Character Data",
        order = 0)]
public class CombatCharacterData : ScriptableObject
{
    public int speed;
    public int attackValue;
}
