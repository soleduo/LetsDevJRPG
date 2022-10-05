using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class CombatCommandBase<T> : CombatCommandBase
{
    protected new T owner;
    
}

public abstract class CombatCommandBase
{
    protected string owner;
    protected string name;
    protected int value;

    public abstract bool IsInitialized { get; }
    public abstract bool IsExecuted { get; }

    public abstract void Execute();
    public virtual int GetOwner()
    {
        return owner.GetHashCode();
    }
}

public abstract class TargettedCommand<T1, T2> : CombatCommandBase<T1>
{
    protected T2 target;
}

public class AttackCommand : TargettedCommand<UnitData, Unit>
{
    private bool isInitialized = false;
    private bool isExecuted = false;

    private event VoidEvent onExecuted;

    public override bool IsInitialized => isInitialized;
    public override bool IsExecuted => isExecuted;

    public AttackCommand(UnitData owner, string name, int value, System.Action additionalEffects = null)
    {
        this.owner = owner;
        this.name = name;
        this.value = value;


        if (additionalEffects != null)
            onExecuted += additionalEffects.Invoke;
    }

    public AttackCommand(CommandData baseCommand, System.Action additionalEffects = null)
    {
        owner = baseCommand.owner;
        name = baseCommand.name;
        value = baseCommand.value;

        if (additionalEffects != null)
            onExecuted += additionalEffects.Invoke;
    }

    public void SetTarget(Unit target)
    {
        this.target = target;

        isInitialized = true;
    }

    public override void Execute()
    {
        int _damage = Mathf.FloorToInt(value * owner.attack * 0.01f);
        GUIMessageHelper.PrintConsole(string.Format("Attack Command: {2} use {0} to {3} for {1} damage", name, _damage, owner.name, target.name));

        target.DoDamage(_damage);

        isExecuted = true;

        onExecuted?.Invoke();

        isInitialized = false;
        isExecuted = false;
    }
}

public class SupportCommand : TargettedCommand<UnitData,string>
{
    private event VoidEvent onExecuted;

    public override bool IsInitialized => throw new System.NotImplementedException();

    public override bool IsExecuted => throw new System.NotImplementedException();

    public SupportCommand(UnitData owner, string name, System.Action onExecution = null)
    {
        this.owner = owner;
        this.name = name;
    }

    public SupportCommand(CommandData baseCommand, System.Action additionalEffects = null)
    {
        owner = baseCommand.owner;
        name = baseCommand.name;
        value = baseCommand.value;

        if (additionalEffects != null)
            onExecuted += additionalEffects.Invoke;
    }

    public override void Execute()
    {
        throw new System.NotImplementedException();
    }
}

public class MoveCommand : TargettedCommand<NavMeshAgent,Vector3>
{
    private bool isInitialized = false;

    public override bool IsInitialized => isInitialized;
    public override bool IsExecuted => owner.remainingDistance < owner.stoppingDistance && owner.isStopped;

    public MoveCommand(NavMeshAgent owner, Vector3 target)
    {
        this.owner = owner;
        this.target = target;

        //owner validation
        //find ownerId in CombatManager
        bool ownerValidated = true;


        //target validation
        NavMeshPath path = new NavMeshPath();
        bool targetValidated = NavMesh.CalculatePath(owner.transform.position, target, NavMesh.AllAreas, path);


        //on target validation passed
        isInitialized = ownerValidated && targetValidated;
    }

    public override void Execute()
    {
        if (!isInitialized)
            return;

        owner.SetDestination(target);
    }
}

[System.Serializable]
public class CommandData
{
    public UnitData owner { get; private set; }
    public string name;
    public int value;
    public ECommandType type;



    public CombatCommandBase Create(UnitData data)
    {
        owner = data;

        CombatCommandBase c = null;

        switch (type)
        {
            case ECommandType.ATTACK:
                c = new AttackCommand(this);
                break;
            case ECommandType.SUPPORT:
                c = new SupportCommand(this);
                break;
            default:
                break;
        }

        return c;
    }
}

public enum ECommandType
{
    ATTACK,
    SUPPORT
}