using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rogue : BaseUnitFunction
{
    // - Set range of the skill -
    private int[] SkillRange = { };

    // - Re write skill function -
    public override void Unit_Skill()
    {
        base.Unit_Skill();

        // Finding a new target from the back of the row
        for (int index = TargetRow.Count - 1; index > -1; index--)
        {
            if (TargetRow[index].GetComponent<TileScript>().unit_On == true)
            {
                target_Tile = TargetRow[index];
                target_TilePosition = target_Tile.GetComponent<TileScript>().tilePosition;
                target_Unit = target_Tile.GetComponent<TileScript>().unit;
                break;
            }
        }

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
        target_Unit.GetComponent<BaseUnitFunction>().DamageReceive = DealingDamage_Calculation(Mod_Attack) * 2.5f;
        target_Unit.GetComponent<BaseUnitFunction>().DamageReceiveType = DamageDealType;
        target_Unit.GetComponent<BaseUnitFunction>().getAttack = true;

        unitStatus = state.Attacking;
        target_Unit = null;
        critHit = false;
        peneHit = false;
    }
}
