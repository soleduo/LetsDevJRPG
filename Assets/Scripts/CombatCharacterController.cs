using System.Collections;
using UnityEngine;

public abstract class CombatCharacterController
{
    protected string queuedCommand = string.Empty;
    protected string owner;

    public abstract IEnumerator ActivateTurn();
}

public delegate void VoidEvent();

public class PlayerCombatController : CombatCharacterController
{
    public PlayerCombatController(string owner)
    {
        this.owner = owner;
    }

    public override IEnumerator ActivateTurn()
    {
        GUIMessageHelper.PrintConsole("Character Turn: " + owner);

        //activate player ui
        GUIMessageHelper.PrintConsole("Click the enemy to Attack");
        DebugClickEnemy.Instance.onClick += () => queuedCommand = "Attack";

        // select action
        yield return new WaitUntil(() => queuedCommand != string.Empty);
        // select target

        // do action
        GUIMessageHelper.PrintConsole(owner +" do Action: " + queuedCommand);
        yield return new WaitForSeconds(3f);
        queuedCommand = string.Empty;

        // end turn
        yield return true;
    }
}

public class AICombatController : CombatCharacterController
{
    public AICombatController(string owner)
    {
        this.owner = owner;
        queuedCommand = "Attack";
    }

    public override IEnumerator ActivateTurn()
    {
        GUIMessageHelper.PrintConsole("Character Turn: " + owner);

        // do action
        GUIMessageHelper.PrintConsole(owner + " do Action: " + queuedCommand);
        yield return new WaitForSeconds(3f);

        // select new action
        queuedCommand = "Attack";

        // end turn
        yield return true;
    }
}
