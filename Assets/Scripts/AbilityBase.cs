using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class AbilityBase<T> : AbilityBase
{
    protected new T owner;
    
}

public abstract class AbilityBase
{
    protected string owner;
    protected string name;

    public abstract bool IsInitialized { get; }
    public abstract bool IsExecuted { get; }
    public string Name { get { return name; } }

    public abstract void Execute();
    public virtual int GetOwner()
    {
        return owner.GetHashCode();
    }
}

public abstract class TargettedAbility<T1, T2> : AbilityBase<T1>
{
    protected T2 target;
    protected int value;
}

public class Ability_Attack : TargettedAbility<UnitData, Unit>
{
    private bool isInitialized = false;
    private bool isExecuted = false;

    private event VoidEvent onExecuted;

    public override bool IsInitialized => isInitialized;
    public override bool IsExecuted => isExecuted;

    public Ability_Attack(UnitData owner, string name, int value, System.Action additionalEffects = null)
    {
        this.owner = owner;
        this.name = name;
        this.value = value;


        if (additionalEffects != null)
            onExecuted += additionalEffects.Invoke;
    }

    public Ability_Attack(AbilityData baseCommand, System.Action additionalEffects = null)
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

public class Ability_Support : TargettedAbility<UnitData, Unit>
{
    private event VoidEvent onExecuted;

    public override bool IsInitialized => throw new System.NotImplementedException();

    public override bool IsExecuted => throw new System.NotImplementedException();

    public Ability_Support(UnitData owner, string name, System.Action onExecution = null)
    {
        this.owner = owner;
        this.name = name;
    }

    public Ability_Support(AbilityData baseCommand, System.Action additionalEffects = null)
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

public class Ability_Passive : AbilityBase<UnitData>
{
    public override bool IsInitialized => throw new System.NotImplementedException();

    public override bool IsExecuted => throw new System.NotImplementedException();

    public Ability_Passive(UnitData owner, string name, int value)
    {
        this.owner = owner;
        this.name = name;
    }

    public Ability_Passive(AbilityData data)
    {
        owner = data.owner;
        name = data.name;
    }

    public override void Execute()
    {
        throw new System.NotImplementedException();
    }
}

public class MoveCommand : TargettedAbility<NavMeshAgent,Vector3>
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

public enum EAbilityType
{
    ATTACK,
    SUPPORT,
    PASSIVE
}