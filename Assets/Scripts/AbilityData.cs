using UnityEngine;

[CreateAssetMenu(
        fileName = "AbilityData.asset",
        menuName = "ScriptableObjects/Game Data/Ability Data",
        order = 0)]
public class AbilityData : ScriptableObject
{
    public UnitData owner { get; private set; }
    public new string name;
    public int value;
    [TextArea(1,10)] public string description;
    public EAbilityType type;

    public AbilityBase Create(UnitData data)
    {
        owner = data;

        AbilityBase c = null;

        switch (type)
        {
            case EAbilityType.ATTACK:
                c = new Ability_Attack(this);
                break;
            case EAbilityType.SUPPORT:
                c = new Ability_Support(this);
                break;
            case EAbilityType.PASSIVE:
                c = new Ability_Passive(this);
                break;
            default:
                break;
        }

        return c;
    }
}
