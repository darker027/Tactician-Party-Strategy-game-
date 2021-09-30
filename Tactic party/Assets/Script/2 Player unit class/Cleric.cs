using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cleric : BaseUnitFunction
{
    // - Set range of the skill -
    private int[] SkillRange = { 8, 6, 4, 2 };

    public override void Unit_Skill()
    {
        base.Unit_Skill();

        // Healing health to all targets in the range
        gameObject.GetComponent<BaseUnitFunction>().HealingReceive = AmountOfHeal_Calculation();
        gameObject.GetComponent<BaseUnitFunction>().getHeal = true;

        for (int index = 0; index < SkillRange.Length; index++)
        {
            if (SkillDefendRange[SkillRange[index]] != null && SkillDefendRange[SkillRange[index]].GetComponent<TileScript>().unit_On == true)
            {
                if(SkillDefendRange[SkillRange[index]].GetComponent<TileScript>().unit.tag == "PlayerUnit")
                {
                    SkillDefendRange[SkillRange[index]].GetComponent<TileScript>().unit.GetComponent<BaseUnitFunction>().HealingReceive = AmountOfHeal_Calculation();
                    SkillDefendRange[SkillRange[index]].GetComponent<TileScript>().unit.GetComponent<BaseUnitFunction>().getHeal = true;
                }
            }
        }

        unitStatus = state.Attacking;
    }

    public float AmountOfHeal_Calculation()
    {
        float HealingAmount = Mod_Attack * 2;

        return Mathf.Round(HealingAmount);
    }
}
