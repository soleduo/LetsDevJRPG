using System.Collections;
using UnityEngine;

public abstract class UnitController
{
    protected AbilityBase queuedCommand;
    protected UnitData owner;

    public abstract IEnumerator ActivateTurn();
}

public delegate void VoidEvent();

public class PlayerCombatController : UnitController
{
    public PlayerCombatController(UnitData owner)
    {
        this.owner = owner;
    }

    public override IEnumerator ActivateTurn()
    {
        GUIMessageHelper.PrintConsole("Character Turn: " + owner.name);

        //activate player ui
        GUIMessageHelper.PrintConsole("Select command");
        CombatMenuUI.Instance.Setup(owner.Commands, (_command)=> { SetQueuedCommand(_command); });

        // select action
        yield return new WaitUntil(() => queuedCommand != null);
        // select target
        Ability_Attack _command = (Ability_Attack)queuedCommand;
        _command.SetTarget(TurnBasedCombatController.Instance.unitsInCombat[0]);

        yield return new WaitUntil(() => queuedCommand.IsInitialized);

        queuedCommand.Execute();

        // do action
        queuedCommand = null;

        // end turn
        yield return true;
    }

    public void SetQueuedCommand(AbilityBase _command)
    {
        queuedCommand = _command;
    }
}

public class AICombatController : UnitController
{
    public AICombatController(UnitData owner)
    {
        this.owner = owner;
        queuedCommand = owner.Commands[0];
    }

    public override IEnumerator ActivateTurn()
    {
        GUIMessageHelper.PrintConsole("Character Turn: " + owner.name);
        Ability_Attack _command = (Ability_Attack)queuedCommand;

        _command.SetTarget(TurnBasedCombatController.Instance.unitsInCombat[1]);
        _command.Execute();

        // select new action
        queuedCommand = owner.Commands[0];

        // end turn
        yield return true;
    }
}
