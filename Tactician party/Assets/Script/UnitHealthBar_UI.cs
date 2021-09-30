using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitHealthBar_UI : MonoBehaviour
{
    private BaseUnitData unitScript;
    private GameObject unitHealthBar;
    private GameObject unitEnergyBar;
    private Slider healthBar;
    private Slider energyBar;
    private TMPro.TextMeshProUGUI classText;

    //  - - - Unit health data - - -
    private float unit_MaxHealth;
    private float unit_MinHealth;
    private float unit_CurrentHealth;

    //  - - - Unit energy data - - -
    private float unit_MaxEnergy;
    private float unit_MinEnergy;
    private float unit_CurrentEnergy;

    // - - - - - - - - - - - - - - - - - - - - - - - - -
    void Start()
    {
        unitScript = gameObject.GetComponentInParent<BaseUnitData>();
        unitHealthBar = gameObject.transform.GetChild(0).gameObject;
        unitEnergyBar = gameObject.transform.GetChild(1).gameObject;
        healthBar = unitHealthBar.GetComponent<Slider>();
        energyBar = unitEnergyBar.GetComponent<Slider>();
        classText = GetComponentInChildren<TMPro.TextMeshProUGUI>();
        classText.SetText(unitScript.unit_ClassName);
    }

    void Update()
    {
        HealthBar_UI();
        EnergyBar_UI();
    }

    // - - - Unit UI function - - -
    private void HealthBar_UI()
    {
        unit_MaxHealth = unitScript.Mod_Health;
        unit_MinHealth = 0;

        healthBar.maxValue = unit_MaxHealth;
        healthBar.minValue = unit_MinHealth;

        if (unit_CurrentHealth == unit_MaxHealth)
        {
            //unitHealthBar.SetActive(false);
        }
        else if (unit_CurrentHealth < unit_MaxHealth)
        {
            unitHealthBar.SetActive(true);
        }

        unit_CurrentHealth = unitScript.currentHealth;
        healthBar.value = unit_CurrentHealth;
    }

    private void EnergyBar_UI()
    {
        if(gameObject.tag == "PlayerUnit")
        {
            unit_MaxEnergy = unitScript.Mod_Mana;
            unit_MinEnergy = 0;

            energyBar.maxValue = unit_MaxEnergy;
            energyBar.minValue = unit_MinEnergy;

            if (unit_CurrentEnergy == unit_MaxEnergy)
            {
                //unitEnergyBar.SetActive(false);
            }
            else if (unit_CurrentEnergy > unit_MinEnergy)
            {
                unitEnergyBar.SetActive(true);
            }

            unit_CurrentEnergy = unitScript.currentEnergy;
            energyBar.value = unit_CurrentEnergy;
        }
    }
}
