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
    public int maxHitPoint;
    public int currentHitPoint;
    public int attack;
    public int speed;

    [SerializeField]private List<CommandData> commands;

    public List<CombatCommandBase> Commands { get; private set; }
    public void CreateCommands()
    {
        Commands = new List<CombatCommandBase>();
        for(int i =0; i < commands.Count; i++)
        {
            CombatCommandBase _temp = commands[i].Create(this);

            if (Commands.Count <= i)
            {
                Commands.Add(_temp);
                continue;
            }

            Commands[i] = _temp;
        }
    }
}
