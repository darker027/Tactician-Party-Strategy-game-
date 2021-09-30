using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUnitFunction : BaseUnitData
{
    // - - - Finding Target Variable - - -
    [HideInInspector] public List<GameObject> TargetRow = new List<GameObject>();
    [HideInInspector] public List<GameObject> OtherRow = new List<GameObject>();

    // - - - Move To Target Variable - - -
    private bool canAttack;
    private bool goAttack;

    // - - - Using Skill Variable - - -
    private bool doSkill;

    // - - - Charging Energy Variable - - -
    private float EnergyCharge;

    // - - - Dealing Damage Variable - - -
    private bool doAttack;

    [HideInInspector] public string DamageDealType;
    [HideInInspector] public float DamageDeal;

    // - - - Receiving Damage Variable - - -
    [HideInInspector] public bool getAttack;

    [HideInInspector] public string DamageReceiveType;
    [HideInInspector] public float DamageReceive;

    private float DamageTaken;

    // - - - Receiving Damage Variable - - -
    [HideInInspector] public bool getHeal;

    [HideInInspector] public float HealingReceive;

    private float HealingTaken;

    // - - - Skill Variable - - -
    public List<GameObject> SkillAttackRange = new List<GameObject>();
    public List<GameObject> SkillDefendRange = new List<GameObject>();
    private int maxSkillRange = 10;
    private GameObject emptyObject = null;
    private GameObject UpperTile;
    private GameObject LowerTile;
    private GameObject ForwardTile;
    private GameObject BackwardTile;
    private GameObject UpperForwardTile;
    private GameObject UpperBackwardTile;
    private GameObject LowerForwardTile;
    private GameObject LowerBackwardTile;

    private float maxRaydistance = 2;

    private bool scanSkillRange;

    // - - - - - - - - - - - - - - - - - - - - - - - - -
    void Awake()
    {
        mainCamera = Camera.main;

        Main_Data = GameObject.FindGameObjectWithTag("GameController");
        PlayerTiles = Main_Data.GetComponent<MainSystem>().Data_PlayerTiles;
        MonsterTiles = Main_Data.GetComponent<MainSystem>().Data_MonsterTiles;

        SkillAttackRange = new List<GameObject>(maxSkillRange);

        for(int index = 0; index < maxSkillRange; index++)
        {
            SkillAttackRange.Add(emptyObject);
            SkillDefendRange.Add(emptyObject);
        }
    }

    void Start()
    {
        StartUnit_Stat();
        unit_ClassName = unit_ClassStat.Unit_ClassName;

        if (this.gameObject.tag == "PlayerUnit")
        {
            TileScript unit_TileScript = unit_Tile.transform.gameObject.GetComponent<TileScript>();
            unit_TileScript.unit = gameObject;

            Transform unit_TileTransform = unit_TileScript.tileTransform;
            unit_Location = this.gameObject.transform.position;
            gameObject.transform.position = unit_Location;

            unit_TilePosition = unit_TileScript.tilePosition;
        }
        if (this.gameObject.tag == "MonsterUnit")
        {
            unit_TilePosition = unit_Tile.GetComponent<TileScript>().tilePosition;
            unit_Location = this.gameObject.transform.position;
        }
    }

    void Update()
    {
        if (MainSystem.BattlePhase)
        {
            if(unit_Tile.CompareTag("PlayerTile") || unit_Tile.CompareTag("MonsterTile"))
            {
                if (target_Unit == null)
                {
                    Finding_Target();
                }
                else
                {
                    if (AttackTarget && !goAttack)
                    {
                        goAttack = true;
                        AttackTarget = false;
                    }

                    MovingTo_Target();

                    if (!scanSkillRange)
                    {
                        SkillAttackRange_Calculation();
                        SkillDefendRange_Calculation();
                        scanSkillRange = true;
                    }

                    if (target_Unit == null && doAttack)
                    {
                        doAttack = false;
                        return;
                    }
                    else
                    {
                        if (doAttack)
                        {
                            Dealing_Damage();
                        }
                    }

                    if (getAttack)
                    {
                        Receiving_Damage();
                    }

                    if (getHeal)
                    {
                        Receiving_Heal();
                    }

                    if (doSkill)
                    {
                        Unit_Skill();
                        currentEnergy = 0;
                        skillReady = false;
                        doSkill = false;
                    }

                    Dead_Unit();
                }
            }
        }
        else
        {
            if (!dragging)
            {
                if (gameObject.transform.position.x != unit_Location.x || gameObject.transform.position.z != unit_Location.z)
                {
                    transform.position = unit_Location;
                }
            }
        }
    }

    // - - - Function - - -
    private void StartUnit_Stat()
    {
        Mod_Attack = unit_ClassStat.attack;
        Mod_AttackSpeed = unit_ClassStat.attackSpeed;

        Mod_Health = unit_ClassStat.health;
        Mod_Defense = unit_ClassStat.defense;

        Mod_Mana = unit_ClassStat.mana;
        Mod_EnergyRechange = unit_ClassStat.energyRechange;

        Mod_Critical = unit_ClassStat.critical;
        Mod_Penetration = unit_ClassStat.penetration;

        Mod_Dodge = unit_ClassStat.dodge;
        Mod_Block = unit_ClassStat.block;

        currentHealth = Mod_Health;
        Mathf.Clamp(currentHealth, 0, Mod_Health);
    }

    private void Finding_Target()
    {
        if (unit_TilePosition != null)
        {
            if (this.gameObject.tag == "PlayerUnit")
            {
                // - Getting Attacking Row -
                for (int index = 0; index < MonsterTiles.Length; index++)
                {
                    if (MonsterTiles[index].GetComponent<TileScript>().tilePosition.Substring(0, 1) == unit_TilePosition.Substring(0, 1))
                    {
                        if (TargetRow.Contains(MonsterTiles[index]))
                        {
                            break;
                        }
                        else
                        {
                            TargetRow.Add(MonsterTiles[index]);
                        }
                    }
                    else
                    {
                        if (OtherRow.Contains(MonsterTiles[index]))
                        {
                            break;
                        }
                        else
                        {
                            OtherRow.Add(MonsterTiles[index]);
                        }
                    }
                }
                // - Findind Target to Attack in the same Row -
                for (int index = 0; index < TargetRow.Count; index++)
                {
                    if (TargetRow[index].GetComponent<TileScript>().unit_On == true)
                    {
                        target_Tile = TargetRow[index];
                        target_TilePosition = target_Tile.GetComponent<TileScript>().tilePosition;
                        target_Unit = target_Tile.GetComponent<TileScript>().unit;
                        scanSkillRange = false;
                        return;
                    }
                }

                // - Findind Target to Attack in other Row -
                for (int index = 0; index < OtherRow.Count; index++)
                {
                    if (OtherRow[index].GetComponent<TileScript>().unit_On == true)
                    {
                        target_Tile = OtherRow[index];
                        target_TilePosition = target_Tile.GetComponent<TileScript>().tilePosition;
                        target_Unit = target_Tile.GetComponent<TileScript>().unit;
                        scanSkillRange = false;
                        return;
                    }
                }
            }

            else if (this.gameObject.tag == "MonsterUnit")
            {
                // - Getting Attacking Row -
                for (int index = 0; index < PlayerTiles.Length; index++)
                {
                    if (PlayerTiles[index].GetComponent<TileScript>().tilePosition.Substring(0, 1) == unit_TilePosition.Substring(0, 1))
                    {
                        TargetRow.Add(PlayerTiles[index]);
                    }
                    else
                    {
                        OtherRow.Add(PlayerTiles[index]);
                    }
                }
                // - Findind Target to Attack in the same Row -
                for (int index = 0; index < TargetRow.Count; index++)
                {
                    if (TargetRow[index].GetComponent<TileScript>().unit_On == true)
                    {
                        target_Tile = TargetRow[index];
                        target_TilePosition = target_Tile.GetComponent<TileScript>().tilePosition;
                        target_Unit = target_Tile.GetComponent<TileScript>().unit;
                        scanSkillRange = false;
                        return;
                    }
                }

                // - Findind Target to Attack in other Row -
                for (int index = 0; index < OtherRow.Count; index++)
                {
                    if (OtherRow[index].GetComponent<TileScript>().unit_On == true)
                    {
                        target_Tile = OtherRow[index];
                        target_TilePosition = target_Tile.GetComponent<TileScript>().tilePosition;
                        target_Unit = target_Tile.GetComponent<TileScript>().unit;
                        scanSkillRange = false;
                        return;
                    }
                }
            }
        }
    }

    private void MovingTo_Target()
    {
        // - Location Setting -
        current_Location = gameObject.transform.position;
        if (target_Tile != null && this.gameObject.tag == "PlayerUnit")
        {
            target_Location.x = target_Tile.transform.position.x - 1;
            target_Location.y = 0.75f;
            target_Location.z = target_Tile.transform.position.z;
        }
        if (target_Tile != null && this.gameObject.tag == "MonsterUnit")
        {
            target_Location.x = target_Tile.transform.position.x + 1;
            target_Location.y = 0.75f;
            target_Location.z = target_Tile.transform.position.z;
        }

        // - Check current location of the unit -
        if (current_Location.x == unit_Location.x && current_Location.z == unit_Location.z)
        {
            unitStatus = state.Idle;
            canAttack = true;
        }
        if (current_Location.x == target_Location.x && current_Location.z == target_Location.z)
        {
            if (gameObject.tag == "PlayerUnit")
            {
                if (skillReady)
                {
                    doSkill = true;
                    goAttack = false;
                }
                else
                {
                    doAttack = true;
                    goAttack = false;
                }
            }
            if (gameObject.tag == "MonsterUnit")
            {

                doAttack = true;
                goAttack = false;
            }
        }
        if (!canAttack && !doAttack)
        {
            unitStatus = state.Moving;

        }

        // - Moving to the location -
        if (goAttack)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, target_Location, characterMovespeed * Time.deltaTime);
            canAttack = false;
        }
        if (!goAttack)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, unit_Location, characterMovespeed * Time.deltaTime);
        }
    }

    public void Dealing_Damage()
    {
        if (Random.value <= Mod_Critical / 100)
        {
            critHit = true;
            DamageDealType = "Critcal";
            Debug.Log(gameObject.name + " : critical attack!");
        }
        if (Random.value <= Mod_Penetration / 100)
        {
            peneHit = true;
            DamageDealType = "Penetrate";
            Debug.Log(gameObject.name + " : penetrate attack!");
        }
        if (!critHit && !peneHit)
        {
            DamageDealType = "Normal";
            Debug.Log(gameObject.name + " : normal attack!");
        }

        target_Unit.GetComponent<BaseUnitFunction>().DamageReceive = DealingDamage_Calculation(Mod_Attack);
        target_Unit.GetComponent<BaseUnitFunction>().DamageReceiveType = DamageDealType;
        target_Unit.GetComponent<BaseUnitFunction>().getAttack = true;
        Charging_Energy(Mod_Mana, Mod_EnergyRechange);
        unitStatus = state.Attacking;
        critHit = false;
        peneHit = false;
        doAttack = false;
    }

    public void Receiving_Damage()
    {
        if (DamageReceiveType != "Critcal" && Random.value <= Mod_Dodge / 100)
        {
            dodging = true;
        }
        if (DamageReceiveType != "Penetrate" && Random.value <= Mod_Block / 100)
        {
            blocking = true;
        }

        if (dodging)
        {
            GameObject DamageText = Instantiate(floatTextDMG, new Vector3(gameObject.transform.position.x, 1.5f, gameObject.transform.position.z), Quaternion.identity, transform);
            DamageText.GetComponentInChildren<TMPro.TextMeshPro>().SetText("Dodge");
        }
        else if (blocking)
        {
            GameObject DamageText = Instantiate(floatTextDMG, new Vector3(gameObject.transform.position.x, 1.5f, gameObject.transform.position.z), Quaternion.identity, transform);
            DamageText.GetComponentInChildren<TMPro.TextMeshPro>().SetText("Block -" + ReceivingDamage_Calculation(Mod_Defense).ToString());
        }
        else
        {
            GameObject DamageText = Instantiate(floatTextDMG, new Vector3(gameObject.transform.position.x, 1.5f, gameObject.transform.position.z), Quaternion.identity, transform);
            DamageText.GetComponentInChildren<TMPro.TextMeshPro>().SetText("-" + ReceivingDamage_Calculation(Mod_Defense).ToString());
        }
        // - - - - - - - - -
        currentHealth -= ReceivingDamage_Calculation(Mod_Defense);
        currentHealth = Mathf.Clamp(currentHealth, 0, Mod_Health);
        Charging_Energy(Mod_Mana, Mod_EnergyRechange);
        dodging = false;
        blocking = false;
        getAttack = false;
    }

    public void Receiving_Heal()
    {
        GameObject DamageText = Instantiate(floatTextDMG, new Vector3(gameObject.transform.position.x, 1.5f, gameObject.transform.position.z), Quaternion.identity, transform);
        DamageText.GetComponentInChildren<TMPro.TextMeshPro>().SetText("Heal +" + ReceivingHeal_Calculation(Mod_Health).ToString());
        DamageText.GetComponentInChildren<TMPro.TextMeshPro>().color = Color.green;
        // - - - - - - - - -
        currentHealth += ReceivingHeal_Calculation(Mod_Health);
        currentHealth = Mathf.Clamp(currentHealth, 0, Mod_Health);
        getHeal = false;
    }

    public void Charging_Energy(float MaxEnergy, float EnergyRecharge)
    {
        if (currentEnergy < MaxEnergy)
        {
            currentEnergy += ChargingEnergy_Calculation(EnergyRecharge);
        }
        if (currentEnergy >= MaxEnergy)
        {
            skillReady = true;
        }

    }

    private void Dead_Unit()
    {
        if (currentHealth <= 0)
        {
            if (this.gameObject.tag == "PlayerUnit")
            {
                Destroy(gameObject);
            }
            if (this.gameObject.tag == "MonsterUnit")
            {
                Main_Data.GetComponent<MainSystem>().playerCurrency += Random.Range(5, 15);
                MainSystem.MonsterIn_Wave -= 1;
                Destroy(gameObject);
            }
        }
    }

    public virtual void Unit_Skill()
    {
        SkillAttackRange_Calculation();
        SkillDefendRange_Calculation();
    }

    // - - - Calculation - - -
    public float DealingDamage_Calculation(float Attack)
    {
        if (critHit)
        {
            DamageDeal = (Attack * 150) / 100;
        }
        if (peneHit)
        {
            DamageDeal = Attack + ((target_Unit.GetComponent<BaseUnitData>().Mod_Defense * 50) / 100);
        }
        if (!critHit && !peneHit)
        {
            DamageDeal = Attack;
        }

        return Mathf.Round(DamageDeal);
    }

    public float ReceivingDamage_Calculation(float Defense)
    {
        if (dodging)
        {
            DamageTaken = 0;
        }
        if (blocking)
        {
            DamageTaken = (DamageReceive - Defense) / 2;
            //Mathf.Clamp(DamageTaken, 50, DamageTaken);
        }
        if (!dodging && !blocking)
        {
            DamageTaken = DamageReceive - Defense;
        }

        return Mathf.Round(DamageTaken);
    }

    public float ReceivingHeal_Calculation(float Health)
    {
        HealingTaken = HealingReceive + ((Health * 10) / 100);

        return Mathf.Round(HealingTaken);
    }

    public float ChargingEnergy_Calculation(float Recharge_Rate)
    {
        // Attacking
        if (doAttack)
        {
            EnergyCharge = Recharge_Rate;
        }
        // Defending
        if (getAttack)
        {
            EnergyCharge = Recharge_Rate / 2;
        }
        return Mathf.Round(EnergyCharge);
    }

    public void SkillAttackRange_Calculation()
    {
        if (Physics.Raycast(target_Tile.transform.position, (target_Tile.transform.forward), out RaycastHit raycastupHit, maxRaydistance))
        {
            if (raycastupHit.collider.tag == "MonsterTile")
            {
                UpperTile = raycastupHit.transform.gameObject;
                SkillAttackRange[8] = UpperTile;
            }
            else
            {
                UpperTile = null;
                SkillAttackRange[8] = UpperTile;
            }
        }
        else
        {
            UpperTile = null;
            SkillAttackRange[8] = UpperTile;
        }

        if (Physics.Raycast(target_Tile.transform.position, (-target_Tile.transform.forward), out RaycastHit raycastdownHit, maxRaydistance))
        {
            if (raycastdownHit.collider.tag == "MonsterTile")
            {
                LowerTile = raycastdownHit.transform.gameObject;
                SkillAttackRange[2] = LowerTile;
            }
            else
            {
                LowerTile = null;
                SkillAttackRange[2] = LowerTile;
            }
        }
        else
        {
            LowerTile = null;
            SkillAttackRange[2] = LowerTile;
        }

        if (Physics.Raycast(target_Tile.transform.position, (target_Tile.transform.right), out RaycastHit raycastforwardHit, maxRaydistance))
        {
            if (raycastforwardHit.collider.tag == "MonsterTile")
            {
                ForwardTile = raycastforwardHit.transform.gameObject;
                SkillAttackRange[6] = ForwardTile;
            }
            else
            {
                ForwardTile = null;
                SkillAttackRange[6] = ForwardTile;
            }
        }
        else
        {
            ForwardTile = null;
            SkillAttackRange[6] = ForwardTile;
        }

        if (Physics.Raycast(target_Tile.transform.position, (-target_Tile.transform.right), out RaycastHit raycastbackwardHit, maxRaydistance))
        {
            if (raycastbackwardHit.collider.tag == "MonsterTile")
            {
                BackwardTile = raycastbackwardHit.transform.gameObject;
                SkillAttackRange[4] = BackwardTile;
            }
            else
            {
                BackwardTile = null;
                SkillAttackRange[4] = BackwardTile;
            }
        }
        else
        {
            BackwardTile = null;
            SkillAttackRange[4] = BackwardTile;
        }

        if (Physics.Raycast(target_Tile.transform.position, (target_Tile.transform.right + target_Tile.transform.forward), out RaycastHit raycastUpperForwardHit, maxRaydistance))
        {
            if (raycastUpperForwardHit.collider.tag == "MonsterTile")
            {
                UpperForwardTile = raycastUpperForwardHit.transform.gameObject;
                SkillAttackRange[9] = UpperForwardTile;
            }
            else
            {
                UpperForwardTile = null;
                SkillAttackRange[9] = UpperForwardTile;
            }
        }
        else
        {
            UpperForwardTile = null;
            SkillAttackRange[9] = UpperForwardTile;
        }

        if (Physics.Raycast(target_Tile.transform.position, (-target_Tile.transform.right + target_Tile.transform.forward), out RaycastHit raycastUpperBackwardHit, maxRaydistance))
        {
            if (raycastUpperBackwardHit.collider.tag == "MonsterTile")
            {
                UpperBackwardTile = raycastUpperBackwardHit.transform.gameObject;
                SkillAttackRange[7] = UpperBackwardTile;
            }
            else
            {
                UpperBackwardTile = null;
                SkillAttackRange[7] = UpperBackwardTile;
            }
        }
        else
        {
            UpperBackwardTile = null;
            SkillAttackRange[7] = UpperBackwardTile;
        }

        if (Physics.Raycast(target_Tile.transform.position, (target_Tile.transform.right + -target_Tile.transform.forward), out RaycastHit raycastLowerForwardHit, maxRaydistance))
        {
            if (raycastLowerForwardHit.transform.gameObject.tag == "MonsterTile")
            {
                LowerForwardTile = raycastLowerForwardHit.transform.gameObject;
                SkillAttackRange[3] = LowerForwardTile;
            }
            else
            {
                UpperBackwardTile = null;
                SkillAttackRange[3] = UpperBackwardTile;

            }
        }
        else
        {
            UpperBackwardTile = null;
            SkillAttackRange[3] = UpperBackwardTile;

        }

        if (Physics.Raycast(target_Tile.transform.position, (-target_Tile.transform.right + -target_Tile.transform.forward), out RaycastHit raycastLowerBackwardHit, maxRaydistance))
        {
            if (raycastLowerBackwardHit.transform.gameObject.tag == "MonsterTile")
            {
                LowerBackwardTile = raycastLowerBackwardHit.transform.gameObject;
                SkillAttackRange[1] = LowerBackwardTile;
            }
            else
            {
                LowerBackwardTile = null;
                SkillAttackRange[1] = LowerBackwardTile;
            }
        }
        else
        {
            LowerBackwardTile = null;
            SkillAttackRange[1] = LowerBackwardTile;
        }
    }

    public void SkillDefendRange_Calculation()
    {
        if (Physics.Raycast(unit_Tile.transform.position, (unit_Tile.transform.forward), out RaycastHit raycastupHit, maxRaydistance))
        {
            if (raycastupHit.transform.gameObject.tag == "PlayerTile")
            {
                UpperTile = raycastupHit.transform.gameObject;
                SkillDefendRange[8] = UpperTile;
            }
            else
            {
                UpperTile = null;
                SkillDefendRange[8] = UpperTile;
            }
        }
        else
        {
            UpperTile = null;
            SkillDefendRange[8] = UpperTile;
        }

        if (Physics.Raycast(unit_Tile.transform.position, (-unit_Tile.transform.forward), out RaycastHit raycastdownHit, maxRaydistance))
        {
            if (raycastdownHit.transform.gameObject.tag == "PlayerTile")
            {
                LowerTile = raycastdownHit.transform.gameObject;
                SkillDefendRange[2] = LowerTile;
            }
            else
            {
                LowerTile = null;
                SkillDefendRange[2] = LowerTile;
            }
        }
        else
        {
            LowerTile = null;
            SkillDefendRange[2] = LowerTile;
        }

        if (Physics.Raycast(unit_Tile.transform.position, (unit_Tile.transform.right), out RaycastHit raycastforwardHit, maxRaydistance))
        {
            if (raycastforwardHit.transform.gameObject.tag == "PlayerTile")
            {
                ForwardTile = raycastforwardHit.transform.gameObject;
                SkillDefendRange[6] = ForwardTile;
            }
            else
            {
                ForwardTile = null;
                SkillDefendRange[6] = ForwardTile;
            }
        }
        else
        {
            ForwardTile = null;
            SkillDefendRange[6] = ForwardTile;
        }

        if (Physics.Raycast(unit_Tile.transform.position, (-unit_Tile.transform.right), out RaycastHit raycastbackwardHit, maxRaydistance))
        {
            if (raycastbackwardHit.transform.gameObject.tag == "PlayerTile")
            {
                BackwardTile = raycastbackwardHit.transform.gameObject;
                SkillDefendRange[4] = BackwardTile;
            }
            else
            {
                BackwardTile = null;
                SkillDefendRange[4] = BackwardTile;
            }
        }
        else
        {
            BackwardTile = null;
            SkillDefendRange[4] = BackwardTile;
        }

        if (Physics.Raycast(unit_Tile.transform.position, (unit_Tile.transform.right + unit_Tile.transform.forward), out RaycastHit raycastUpperForwardHit, maxRaydistance))
        {
            if (raycastUpperForwardHit.transform.gameObject.tag == "PlayerTile")
            {
                UpperForwardTile = raycastUpperForwardHit.transform.gameObject;
                SkillDefendRange[9] = UpperForwardTile;
            }
            else
            {
                UpperForwardTile = null;
                SkillDefendRange[9] = UpperForwardTile;
            }
        }
        else
        {
            UpperForwardTile = null;
            SkillDefendRange[9] = UpperForwardTile;
        }

        if (Physics.Raycast(unit_Tile.transform.position, (-unit_Tile.transform.right + unit_Tile.transform.forward), out RaycastHit raycastUpperBackwardHit, maxRaydistance))
        {
            if (raycastUpperBackwardHit.transform.gameObject.tag == "PlayerTile")
            {
                UpperBackwardTile = raycastUpperBackwardHit.transform.gameObject;
                SkillDefendRange[7] = UpperBackwardTile;
            }
            else
            {
                UpperBackwardTile = null;
                SkillDefendRange[7] = UpperBackwardTile;
            }
        }
        else
        {
            UpperBackwardTile = null;
            SkillDefendRange[7] = UpperBackwardTile;
        }

        if (Physics.Raycast(unit_Tile.transform.position, (unit_Tile.transform.right + -unit_Tile.transform.forward), out RaycastHit raycastLowerForwardHit, maxRaydistance))
        {
            if (raycastLowerForwardHit.transform.gameObject.tag == "PlayerTile")
            {
                LowerForwardTile = raycastLowerForwardHit.transform.gameObject;
                SkillDefendRange[3] = LowerForwardTile;
            }
            else
            {
                LowerForwardTile = null;
                SkillDefendRange[3] = LowerForwardTile;
            }

        }
        else
        {
            LowerForwardTile = null;
            SkillDefendRange[3] = LowerForwardTile;
        }

        if (Physics.Raycast(unit_Tile.transform.position, (-unit_Tile.transform.right + -unit_Tile.transform.forward), out RaycastHit raycastLowerBackwardHit, maxRaydistance))
        {
            if (raycastLowerBackwardHit.transform.gameObject.tag == "PlayerTile")
            {
                LowerBackwardTile = raycastLowerBackwardHit.transform.gameObject;
                SkillDefendRange[1] = LowerBackwardTile;
            }
            else
            {
                LowerBackwardTile = null;
                SkillDefendRange[1] = LowerBackwardTile;
            }
        }
        else
        {
            LowerBackwardTile = null;
            SkillDefendRange[1] = LowerBackwardTile;
        }
    }

    // - - - Gameplay system - - -
    private void OnMouseDrag()
    {
        if(this.gameObject.tag == "PlayerUnit" && !MainSystem.BattlePhase)
        {
            dragging = true;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, layerMask))
            {
                transform.position = new Vector3(raycastHit.point.x, 0.75f, raycastHit.point.z);
                gameObject.GetComponent<CapsuleCollider>().enabled = false;
            }

            if (unit_Tile != null)
            {
                unit_Tile.GetComponent<TileScript>().unit = null;
                unit_Tile = null;
            }
        }
    }

    private void OnMouseUp()
    {
        if (this.gameObject.tag == "PlayerUnit" && !MainSystem.BattlePhase)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, layerMask))
            {
                if (raycastHit.transform.gameObject.tag == "PlayerTile" || raycastHit.transform.gameObject.tag == "InventoryTile")
                {
                    if(raycastHit.transform.gameObject.GetComponent<TileScript>().unit_On == false)
                    {
                        unit_Tile = raycastHit.transform.gameObject;
                        TileScript unit_TileScript = raycastHit.transform.gameObject.GetComponent<TileScript>();
                        unit_TileScript.unit = gameObject;

                        Transform unit_TileTransform = unit_TileScript.tileTransform;
                        unit_Location = new Vector3(unit_TileTransform.position.x, 0.75f, unit_TileTransform.position.z);
                        gameObject.transform.position = unit_Location;

                        unit_TilePosition = unit_TileScript.tilePosition;
                    }
                    else
                    {
                        // - unit not get a new tile, return to last tile -
                        gameObject.transform.position = unit_Location;
                    }
                }
                else
                {
                    // - unit not get a new tile, return to last tile -
                    gameObject.transform.position = unit_Location;
                }
            }

            gameObject.GetComponent<CapsuleCollider>().enabled = true;
            dragging = false;
        }
    }
}
