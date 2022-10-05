using System.Collections;
using UnityEngine;

public abstract class CombatCharacterController
{
    protected CombatCommandBase queuedCommand;
    protected CombatCharacterData owner;

    public abstract IEnumerator ActivateTurn();
}

public delegate void VoidEvent();

public class PlayerCombatController : CombatCharacterController
{
    public PlayerCombatController(CombatCharacterData owner)
    {
        this.owner = owner;
    }

    public override IEnumerator ActivateTurn()
    {
        GUIMessageHelper.PrintConsole("Character Turn: " + owner.name);

        //activate player ui
        GUIMessageHelper.PrintConsole("Click the enemy to Attack");
        DebugClickEnemy.Instance.onClick += () => queuedCommand = owner.Commands[0];

        // select action
        yield return new WaitUntil(() => queuedCommand != null);
        // select target
        AttackCommand _command = (AttackCommand)queuedCommand;
        _command.SetTarget(TurnBasedCombatController.Instance.combatCharacterData[0]);

        yield return new WaitUntil(() => queuedCommand.IsInitialized);

        queuedCommand.Execute();

        // do action
        yield return new WaitForSeconds(3f);
        queuedCommand = null;

        // end turn
        yield return true;
    }
}

public class AICombatController : CombatCharacterController
{
    public AICombatController(CombatCharacterData owner)
    {
        this.owner = owner;
        queuedCommand = owner.Commands[0];
    }

    public override IEnumerator ActivateTurn()
    {
        GUIMessageHelper.PrintConsole("Character Turn: " + owner.name);
        AttackCommand _command = (AttackCommand)queuedCommand;

        _command.SetTarget(TurnBasedCombatController.Instance.combatCharacterData[1]);
        _command.Execute();

        // do action
        yield return new WaitForSeconds(3f);

        // select new action
        queuedCommand = owner.Commands[0];

        // end turn
        yield return true;
    }
}
