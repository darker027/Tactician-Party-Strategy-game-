using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New class", menuName = "Unit class")]
public class ClassBaseStat : ScriptableObject
{
    public string Unit_ClassName;

    // - - - Base stat of a class - - -
    [Header("Offensive stats")]
    [SerializeField] private float Base_Attack;
    [HideInInspector] public float attack => Base_Attack;
    [SerializeField] private float Base_AttackSpeed;
    [HideInInspector] public float attackSpeed => Base_AttackSpeed;

    [Header("Defendsive stats")]
    [SerializeField] private float Base_Health;
    [HideInInspector] public float health => Base_Health;
    [SerializeField] private float Base_Defense;
    [HideInInspector] public float defense => Base_Defense;

    [Header("Skill stats")]
    [SerializeField] private float Base_Mana;
    [HideInInspector] public float mana => Base_Mana;
    [SerializeField] private float Base_EnergyRechange;
    [HideInInspector] public float energyRechange => Base_EnergyRechange;

    [Header("Attacking chance stats")]
    [SerializeField] private float Base_Critical;
    [HideInInspector] public float critical => Base_Critical;
    [SerializeField] private float Base_Penetration;
    [HideInInspector] public float penetration => Base_Penetration;

    [Header("Defending chance stats")]
    [SerializeField] private float Base_Dodge;
    [HideInInspector] public float dodge => Base_Dodge;
    [SerializeField] private float Base_Block;
    [HideInInspector] public float block => Base_Block;

}
