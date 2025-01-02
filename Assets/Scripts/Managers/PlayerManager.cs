using DS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DS
{
    public class PlayerManager : CharacterManager
    {
        InputHandler inputHandler;
        Animator anim;
        CameraHandler cameraHandler;
        PlayerLocomotion playerLocomotion;
        PlayerStats playerStats;
        AnimatorHandler animatorHandler;

        InteractableUI interactableUI;
        public GameObject interactableUIGameObject;
        public GameObject itemInteractableGameObject;

        public bool isBusy;


        [Header("Player Flags")]
        public bool isSprinting;
        public bool isInAir;
        public bool isGrounded;
        public bool canDoCombo;
        public bool isUsingRightHand;
        public bool isUsingLeftHand;
        public bool isInvulnerable;

        private void Awake()
        {
            cameraHandler = FindObjectOfType<CameraHandler>();
            backStabCollider = GetComponentInChildren<BackStabCollider>();
            inputHandler = GetComponent<InputHandler>();
            anim = GetComponentInChildren<Animator>();
            playerLocomotion = GetComponent<PlayerLocomotion>();
            interactableUI = FindObjectOfType<InteractableUI>();
            playerStats = GetComponent<PlayerStats>();
            animatorHandler = GetComponentInChildren<AnimatorHandler>();

            WorldSaveGameManager.instance.player = this;
            
        }

        void Update()
        {
            float delta = Time.deltaTime;
            isBusy = anim.GetBool("isBusy");
            canDoCombo = anim.GetBool("canDoCombo");
            anim.SetBool("isInAir", isInAir);
            isUsingRightHand = anim.GetBool("isUsingRightHand");
            isUsingLeftHand = anim.GetBool("isUsingLeftHand");
            isInvulnerable = anim.GetBool("isInvulnerable");
            anim.SetBool("isDead", playerStats.isDead);
            

            inputHandler.TickInput(delta);
            animatorHandler.canRotate  = anim.GetBool("canRotate");
            playerLocomotion.HandleRollingAndSprinting(delta);
            //playerLocomotion.HandleJumping();
            playerStats.RegenerateStamina();


            CheckForInteractableObject();
        }

        private void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;
            playerLocomotion.HandleMovement(delta);
            playerLocomotion.HandleFalling(delta, playerLocomotion.moveDirection);
            playerLocomotion.HandleRotation(delta);

        }

        private void LateUpdate()
        {
            inputHandler.rollFlag = false;
            inputHandler.rb_Input = false;
            inputHandler.rt_Input = false;
            inputHandler.d_Pad_Up = false;
            inputHandler.d_Pad_Down = false;
            inputHandler.d_Pad_Left = false;
            inputHandler.d_Pad_Right = false;
            inputHandler.a_Input = false;
            inputHandler.jump_Input = false;
            inputHandler.inventory_Input = false;

            float delta = Time.fixedDeltaTime;

            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
            }

            if (isInAir)
            {
                playerLocomotion.inAirTimer = playerLocomotion.inAirTimer + Time.deltaTime;
            }
        }

        #region Player Interactions

        public void CheckForInteractableObject()
        {
            RaycastHit hit;

            if (Physics.SphereCast(transform.position, 0.3f, transform.forward, out hit, 1f))
            {
                if (hit.collider.tag == "Interactable")
                {
                    Interactable interactableObject = hit.collider.GetComponent<Interactable>();

                    if (interactableObject != null)
                    {
                        string interactableText = interactableObject.interactbleText;
                        //SET THE UI TEXT TO THE INTERACTABLE OBJECT'S TEXT
                        //SET THE TEXT POP UP TO TRUE
                        interactableUI.interactableText.text = interactableText;
                        interactableUIGameObject.SetActive(true);

                        if (inputHandler.a_Input)
                        {
                            hit.collider.GetComponent<Interactable>().Interact(this);
                        }
                    }
                }
            }
            else
            {
                if(interactableUIGameObject != null)
                {
                    interactableUIGameObject.SetActive(false);
                }

                if(itemInteractableGameObject != null && inputHandler.a_Input)
                {
                    itemInteractableGameObject.SetActive(false);
                }
            }

        }

        public void OpenChestInteraction(Transform playerStandHereWhenOpeningChest)
        {
            playerLocomotion.rigidbody.velocity = Vector3.zero;
            transform.position = playerStandHereWhenOpeningChest.transform.position;
            animatorHandler.PlayTargetAnimation("OpenChest", true);
        }

        #endregion
        public void SaveCharacterDataToCurrentSaveData(ref CharacterSaveData currentCharacterSaveData)
        {
            currentCharacterSaveData.xPosition = transform.position.x;
            currentCharacterSaveData.yPosition = transform.position.y;
            currentCharacterSaveData.zPosition = transform.position.z;
        }

        public void LoadCharacterDataFromCurrentCharacterSaveData(ref CharacterSaveData currentCharacterSaveData)
        {
            transform.position = new Vector3(currentCharacterSaveData.xPosition, currentCharacterSaveData.yPosition, currentCharacterSaveData.zPosition);
        }
    }
}
