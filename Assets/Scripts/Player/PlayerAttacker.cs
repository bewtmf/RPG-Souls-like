using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DS
{
    public class PlayerAttacker : MonoBehaviour
    {
        AnimatorHandler animatorHandler;
        InputHandler inputHandler;
        PlayerManager playerManager;
        PlayerInventory playerInventory;
        PlayerStats playerStats;
        WeaponSlotManager weaponSlotManager;
        public string lastAttack;

        LayerMask backStabLayer = 1 << 10;


        private void Awake()
        {
            animatorHandler = GetComponent<AnimatorHandler>();
            playerManager = GetComponentInParent<PlayerManager>();
            playerInventory = GetComponentInParent<PlayerInventory>();
            weaponSlotManager = GetComponent<WeaponSlotManager>();
            playerStats = GetComponentInParent<PlayerStats>();
            inputHandler = GetComponentInParent<InputHandler>();
        }

        public void HandleWeaponCombo(WeaponItem weapon)
        {
            if (playerStats.currentStamina <= 0)
            {
                return;
            }
            if (inputHandler.comboFlag)
            {
                animatorHandler.anim.SetBool("canDoCombo", false);
                if (lastAttack == weapon.OH_Light_Attack_1)
                {
                    animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_2, true);
                }
                else if (lastAttack == weapon.TH_Light_Attack_01)
                {
                    animatorHandler.PlayTargetAnimation(weapon.TH_Light_Attack_02, true);
                }
                else if (lastAttack == weapon.TH_Light_Attack_02)
                {
                    animatorHandler.PlayTargetAnimation(weapon.TH_Light_Attack_01, true);
                }
            }
        }

        public void HandleLightAttack(WeaponItem weapon)
        {
            if (playerStats.currentStamina <= 0)
            {
                return;
            }
            weaponSlotManager.attackingWeapon = weapon;

            if (inputHandler.twoHandFlag)
            {
                animatorHandler.PlayTargetAnimation(weapon.TH_Light_Attack_01, true);
                lastAttack = weapon.TH_Light_Attack_01;
            }
            else
            {
                animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_1, true);
                lastAttack = weapon.OH_Light_Attack_1;
            }
            
        }

        public void HandleHeavyAttack(WeaponItem weapon)
        {
            if (playerStats.currentStamina <= 0)
            {
                return;
            }
            weaponSlotManager.attackingWeapon = weapon;

            if (inputHandler.twoHandFlag)
            {

            }
            else
            {
                animatorHandler.PlayTargetAnimation(weapon.OH_Heavy_Attack_1, true);
                lastAttack = weapon.OH_Heavy_Attack_1;
            }
        }

        #region Input Actions

        public void HandleRBAction()
        {
            if (playerInventory.rightWeapon.isMeleeWeapon)
            {
                PerformRBMeleeAction();
            }
            else if(playerInventory.rightWeapon.isSpellCaster || playerInventory.rightWeapon.isFaithCaster || playerInventory.rightWeapon.isPyromancerCaster)
            {
                PerformRBMagicAction(playerInventory.rightWeapon);
            }  
        }

        #endregion 

        #region Attack Actions
        private void PerformRBMeleeAction()
        {
            if (playerManager.canDoCombo)
            {
                inputHandler.comboFlag = true;
                HandleWeaponCombo(playerInventory.rightWeapon);
                inputHandler.comboFlag = false;
            }
            else
            {
                if (playerManager.isBusy)
                    return;

                if (playerManager.canDoCombo)
                    return;

                animatorHandler.anim.SetBool("isUsingRightHand", true);
                HandleLightAttack(playerInventory.rightWeapon);
            }
        }

        private void PerformRBMagicAction(WeaponItem weapon)
        {
            if(playerManager.isBusy)
                return;

            if (weapon.isFaithCaster)
            {
                //Faith Caster
                if (playerInventory.currentSpell != null && playerInventory.currentSpell.isFaithSpell)
                {
                    //Check for FP
                    if(playerStats.currentFocusPoints >= playerInventory.currentSpell.focusPointCost)
                    {
                        playerInventory.currentSpell.AttemptToCastSpell(animatorHandler, playerStats);
                    }
                    else
                    {
                        animatorHandler.PlayTargetAnimation("Potion_Empty", true);
                    }
                    //Attempt to cast spell
                }
            }
            else if (weapon.isPyromancerCaster)
            {
                //Pyromancer Caster
            }
            else if (weapon.isSpellCaster)
            {
                //Spell Caster
            }
        }

        private void SuccesFullyCastSpell()
        {
            playerInventory.currentSpell.SuccessfullyCastSpell(animatorHandler, playerStats);    
        }
        #endregion

        public void AttemptBackStabOrRiposte()
        {
            if (playerStats.currentStamina <= 0)
            {
                return;
            }
            RaycastHit hit;
            if (Physics.Raycast(inputHandler.criticalAttackRayCastStartPoint.position, transform.TransformDirection(Vector3.forward), out hit, 0.5f, backStabLayer))
            {
                CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
                DamageCollider rightWeapon = weaponSlotManager.rightHandDamageCollider;

                if (enemyCharacterManager != null)
                {
                    //Check team ID
                    playerManager.transform.position = enemyCharacterManager.backStabCollider.backStabberStandPoint.position;
                    Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
                    rotationDirection = hit.transform.position - playerManager.transform.position;
                    rotationDirection.y = 0;
                    rotationDirection.Normalize();
                    Quaternion tr = Quaternion.LookRotation(rotationDirection);
                    Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.root.rotation, tr, 500 * Time.deltaTime);
                    playerManager.transform.rotation = targetRotation;

                    int criticalDamage = playerInventory.rightWeapon.criticalDamageMultipler * rightWeapon.currentWeaponDamage;
                    enemyCharacterManager.pendingCriticalDamage = criticalDamage;

                    animatorHandler.PlayTargetAnimation("Back_Stab", true);
                    enemyCharacterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Back_Stabbed", true);
                }
            }
        }
    }
}
