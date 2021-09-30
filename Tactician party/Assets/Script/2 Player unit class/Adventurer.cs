using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adventurer : BaseUnitFunction
{
    public override void Unit_Skill()
    {
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

        target_Unit.GetComponent<BaseUnitFunction>().DamageReceive = DealingDamage_Calculation(Mod_Attack) + (DealingDamage_Calculation(Mod_Attack) / 2);
        target_Unit.GetComponent<BaseUnitFunction>().DamageReceiveType = DamageDealType;
        target_Unit.GetComponent<BaseUnitFunction>().getAttack = true;
        unitStatus = state.Attacking;
        critHit = false;
        peneHit = false;
    }
}
