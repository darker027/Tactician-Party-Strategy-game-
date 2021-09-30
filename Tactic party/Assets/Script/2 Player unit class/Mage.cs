using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : BaseUnitFunction
{
    // - Set range of the skill -
    private int[] SkillRange = { 8, 6, 4, 2 };

    // - Re write skill function -
    public override void Unit_Skill()
    {
        base.Unit_Skill();

        // Skill damage type
        if (Random.value <= Mod_Critical / 100)
        {
            critHit = true;
            DamageDealType = "Critcal";
            Debug.Log(gameObject.name + " : do critical attack!");
        }
        if (Random.value <= Mod_Penetration / 100)
        {
            peneHit = true;
            DamageDealType = "Penetrate";
            Debug.Log(gameObject.name + " : do penetrate attack!");
        }
        if (!critHit && !peneHit)
        {
            DamageDealType = "Normal";
            Debug.Log(gameObject.name + " : do normal attack!");
        }

        // Deal damage to all targets in the range
        target_Unit.GetComponent<BaseUnitFunction>().DamageReceive = DealingDamage_Calculation(Mod_Attack);
        target_Unit.GetComponent<BaseUnitFunction>().DamageReceiveType = DamageDealType;
        target_Unit.GetComponent<BaseUnitFunction>().getAttack = true;

        for (int index = 0; index < SkillRange.Length; index++)
        {
            if (SkillAttackRange[SkillRange[index]] != null && SkillAttackRange[SkillRange[index]].GetComponent<TileScript>().unit_On == true)
            {
                SkillAttackRange[SkillRange[index]].GetComponent<TileScript>().unit.GetComponent<BaseUnitFunction>().DamageReceive = DealingDamage_Calculation(Mod_Attack) * 0.75f;
                SkillAttackRange[SkillRange[index]].GetComponent<TileScript>().unit.GetComponent<BaseUnitFunction>().DamageReceiveType = DamageDealType;
                SkillAttackRange[SkillRange[index]].GetComponent<TileScript>().unit.GetComponent<BaseUnitFunction>().getAttack = true;
            }
        }

        unitStatus = state.Attacking;
        critHit = false;
        peneHit = false;
    }
}
