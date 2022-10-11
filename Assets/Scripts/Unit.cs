using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public UnitController Controller { get; private set; }
    public UnitTimeline Timeline { get; private set; }

    [SerializeField] private UnitData data;

    [SerializeField] private float hitPoint;

    [SerializeField] private EUnitControlType controlType;

    public EUnitControlType UnitType { get { return controlType; } }


    public event UnitControllerEvent onTurnStart;
    public event UnitTypeEvent onUnitFallen;

    private void Start()
    {
        data.CreateCommands();
        hitPoint = data.maxHitPoint;

        Controller = controlType switch
        {
            EUnitControlType.PLAYER => new PlayerCombatController(data),
            EUnitControlType.AI => new AICombatController(data),
            _ => new AICombatController(data),
        };

        Timeline = new UnitTimeline(data.speed);

        gameObject.name = data.name;
    }

    public void DoDamage(int damage)
    {
        hitPoint -= damage;

        if (hitPoint <= 0)
            Die();
    }

    public void Die()
    {
        onUnitFallen?.Invoke(UnitType);
        gameObject.SetActive(false);
        enabled = false;
    }
}

public enum EUnitControlType
{
    PLAYER,
    AI
}

public delegate void UnitControllerEvent(UnitController controller);
public delegate void UnitTypeEvent(EUnitControlType type);
