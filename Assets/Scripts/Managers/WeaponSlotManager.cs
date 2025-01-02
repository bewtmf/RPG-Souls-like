using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DS
{
    public class WeaponSlotManager : MonoBehaviour
    {
        public WeaponItem attackingWeapon;
        WeaponHolderSlot leftHandSlot;
        WeaponHolderSlot rightHandSlot;
        WeaponHolderSlot backSlot;
        PlayerManager playerManager;
        PlayerInventory playerInventory;

        public DamageCollider leftHandDamgeCollider;
        public DamageCollider rightHandDamageCollider;

        Animator animator;

        QuickSlotsUI quickSlotsUI;

        PlayerStats playerStats;
        InputHandler inputHandler;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            quickSlotsUI = FindObjectOfType<QuickSlotsUI>();
            playerStats = GetComponentInParent<PlayerStats>();
            inputHandler = GetComponentInParent<InputHandler>();
            playerManager = GetComponentInParent<PlayerManager>();
            playerInventory = GetComponentInParent<PlayerInventory>();

            WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
            foreach(WeaponHolderSlot weaponSlot in weaponHolderSlots)
            {
                if (weaponSlot.isLeftHandSlot)
                    leftHandSlot = weaponSlot;
                else if(weaponSlot.isRightHandSlot)
                    rightHandSlot = weaponSlot;
                else if(weaponSlot.isBackSlot)
                    backSlot = weaponSlot;
            }
        }

        public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
        {
            if (isLeft)
            {
                leftHandSlot.currentWeapon = weaponItem;
                leftHandSlot.LoadWeaponModel(weaponItem);
                LoadLeftWeaponDamageCollider();
                quickSlotsUI.UpdateWeaponQuickSlotsUI(true, weaponItem);
                #region Handle Left Weapon Idle Animations
                if (weaponItem != null)
                {
                    animator.CrossFade(weaponItem.left_hand_idle, 0.2f);
                }
                else
                {
                    animator.CrossFade("Left Arm Empty", 0.2f);
                }
                #endregion
            }
            else
            {
                if (inputHandler.twoHandFlag)
                {
                    backSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
                    leftHandSlot.UnloadWeaponAndDestroy();
                    animator.CrossFade(weaponItem.th_idle, 0.2f);
                }
                else
                {
                    #region Handle Right Weapon Idle Animations

                    animator.CrossFade("Both Arms Empty", 0.2f);
                    
                    backSlot.UnloadWeaponAndDestroy();

                    if (weaponItem != null)
                    {
                        animator.CrossFade(weaponItem.right_hand_idle, 0.2f);
                    }
                    else
                    {
                        animator.CrossFade("Right Arm Empty", 0.2f);
                    }
                    #endregion
                }

                rightHandSlot.currentWeapon = weaponItem;
                rightHandSlot.LoadWeaponModel(weaponItem);
                LoadRightWeaponDamageCollider();
                quickSlotsUI.UpdateWeaponQuickSlotsUI(false, weaponItem);

            }
        }



        public void LoadLeftWeaponDamageCollider()
        {
            leftHandDamgeCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            leftHandDamgeCollider.currentWeaponDamage = playerInventory.leftWeapon.baseDamage;
        }

        public void LoadRightWeaponDamageCollider()
        {
            rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            rightHandDamageCollider.currentWeaponDamage = playerInventory.rightWeapon.baseDamage;
        }

        public void OpenDamageCollider()
        {
            if(playerManager.isUsingRightHand)
                rightHandDamageCollider.EnableDamageCollider();
            else if (playerManager.isUsingLeftHand)
                leftHandDamgeCollider.EnableDamageCollider();
        }

        public void CloseDamageCollider()
        {
            rightHandDamageCollider.DisableDamageCollider();
            leftHandDamgeCollider.DisableDamageCollider();
        }

        public void DrainStaminaLightAttack()
        {
            playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.lightAttackMultiplier));
        }

        public void DrainStaminaHeavyAttack()
        {
            playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.heavyAttackMultiplier));
        }


    }
}
