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
public class UnitData : ScriptableObject
{
    public int maxHitPoint;
    public int currentHitPoint;
    public int attack;
    public int speed;

    [SerializeField]private List<AbilityData> commands;

    public List<AbilityBase> Commands { get; private set; }
    public void CreateCommands()
    {
        Commands = new List<AbilityBase>();
        for(int i =0; i < commands.Count; i++)
        {
            AbilityBase _temp = commands[i].Create(this);

            if (Commands.Count <= i)
            {
                Commands.Add(_temp);
                continue;
            }

            Commands[i] = _temp;
        }
    }
}
