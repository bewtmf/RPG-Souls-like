using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DS
{
    public class UIManager : MonoBehaviour
    {
        public PlayerInventory playerInventory;
        public EquipmentWindowUI equipmentWindowUI;

        [Header("UI Windows")]
        public GameObject hudWindow;
        public GameObject selectWindow;
        public GameObject equipmentScreenWindow;
        public GameObject weaponInventoryWindow;

        [Header("Equipment Window Slot Selected")]
        public bool rightHandSlot01Selected;
        public bool rightHandSlot02Selected;
        public bool leftHandSlot01Selected;
        public bool leftHandSlot02Selected;

        [Header("Weapon Inventory")]
        public GameObject weaponInventorySlotPrefab;
        public Transform weaponInventorySlotsParent;
        WeaponInventorySlot[] weaponInventorySlots;

        private void Awake()
        {
            
        }

        private void Start()
        {
            weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
            equipmentWindowUI.LoadWeaponsOnEquipmentScreen(playerInventory);
        }

        //public void UpdateUI()
        //{
        //    #region Weapon Inventory Slots

        //    for (int i = 0; i < playerInventory.weaponsInventory.Count; i++)
        //    {
        //        // Nếu số lượng slot chưa đủ, thêm mới
        //        if (weaponInventorySlots.Length < playerInventory.weaponsInventory.Count)
        //        {
        //            Instantiate(weaponInventorySlotPrefab, weaponInventorySlotsParent);
        //            weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
        //        }

        //        // Thêm item vào slot
        //        weaponInventorySlots[i].AddItem(playerInventory.weaponsInventory[i]);
        //    }

        //    // Dọn dẹp các slot thừa
        //    for (int i = playerInventory.weaponsInventory.Count; i < weaponInventorySlots.Length; i++)
        //    {
        //        weaponInventorySlots[i].ClearInventorySlot();
        //    }


        //    //for (int i = 0; i < weaponInventorySlots.Length; i++)
        //    //{
        //    //    if (i < playerInventory.weaponsInventory.Count)
        //    //    {
        //    //        if (weaponInventorySlots.Length < playerInventory.weaponsInventory.Count)
        //    //        {
        //    //            Instantiate(weaponInventorySlotPrefab, weaponInventorySlotsParent);
        //    //            weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
        //    //        }
        //    //        weaponInventorySlots[i].AddItem(playerInventory.weaponsInventory[i]);
        //    //    }
        //    //    else
        //    //    {
        //    //        weaponInventorySlots[i].ClearInventorySlot();
        //    //    }
        //    //}
        //    #endregion
        //}

        public void UpdateUI()
        {
            // Kiểm tra playerInventory
            if (playerInventory == null)
            {
                Debug.LogError("playerInventory is null! Please assign it in the inspector or initialize it.");
                return;
            }

            // Kiểm tra weaponInventorySlotsParent
            if (weaponInventorySlotsParent == null)
            {
                Debug.LogError("weaponInventorySlotsParent is null! Please assign it in the inspector.");
                return;
            }

            // Kiểm tra weaponInventorySlotPrefab
            if (weaponInventorySlotPrefab == null)
            {
                Debug.LogError("weaponInventorySlotPrefab is null! Please assign it in the inspector.");
                return;
            }

            // Weapon Inventory Slots
            for (int i = 0; i < playerInventory.weaponsInventory.Count; i++)
            {
                // Nếu số lượng slot chưa đủ, thêm mới
                if (weaponInventorySlots.Length < playerInventory.weaponsInventory.Count)
                {
                    Instantiate(weaponInventorySlotPrefab, weaponInventorySlotsParent);
                    weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
                }

                // Kiểm tra weaponInventorySlots[i] trước khi gọi AddItem
                if (weaponInventorySlots[i] != null)
                {
                    weaponInventorySlots[i].AddItem(playerInventory.weaponsInventory[i]);
                }
            }

            // Dọn dẹp các slot thừa
            for (int i = playerInventory.weaponsInventory.Count; i < weaponInventorySlots.Length; i++)
            {
                if (weaponInventorySlots[i] != null)
                {
                    weaponInventorySlots[i].ClearInventorySlot();
                }
            }
        }


        public void OpenSelectWindow()
        {
            selectWindow.SetActive(true);
        }

        public void CloseSelectWindow()
        {
            selectWindow.SetActive(false);
        }

        public void CloseAllInventoryWindows()
        {
            ResetAllSelectedSlots();
            weaponInventoryWindow.SetActive(false);
            equipmentScreenWindow.SetActive(false);
        }

        public void ResetAllSelectedSlots()
        {
            rightHandSlot01Selected = false;
            rightHandSlot02Selected = false;
            leftHandSlot01Selected = false;
            leftHandSlot02Selected = false;
        }

    }
}