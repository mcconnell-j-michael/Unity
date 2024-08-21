using UnityEngine;
using System.Collections;

public class CombatOptions : A_EnumList<CombatOption, CombatOptions>
{
    public CombatOption ATTACK;
    public CombatOption SKILLS;
    public CombatOption DEFEND;
    public CombatOption ITEMS;
    public CombatOption MOVE;
    public CombatOption FORCE;
    public CombatOption ESCAPE;
}
