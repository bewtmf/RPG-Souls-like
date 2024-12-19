using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace DS
{
    public class EnemyManager : CharacterManager
    {
        EnemyLocomotionManager enemyLocomotionManager;
        EnemyAnimatorManager enemyAnimatorManager;
        EnemyStats enemyStats;


        public State currentState;
        public CharacterStats currentTarget;
        public Rigidbody enemyRigidbody;
        public NavMeshAgent navmeshAgent;


        public bool isPerformingAction;
        public bool isBusy;
        public float distanceFromTarget;
        public float rotationSpeed = 15;
        public float maximumAttackRange = 1.5f;

        [Header("AI Settings")]
        public float detectionRadius = 20;
        public float maximumDetectionAngle = 50;
        public float minimumDetectionAngle = -50;
        public float viewableAngle;
        public float currentRecoveryTime = 0;

        private void Awake()
        {   
            enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
            enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
            enemyStats = GetComponent<EnemyStats>();
            navmeshAgent = GetComponentInChildren<NavMeshAgent>();
            enemyRigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            enemyRigidbody.isKinematic = false;
            navmeshAgent.enabled = false;
        }

        private void Update()
        {
            HandleRecoveryTimer();
            
            isBusy = enemyAnimatorManager.anim.GetBool("isBusy");
        }

        private void FixedUpdate()
        {
            HandleStateMachine();

        }

        private void HandleStateMachine()
        {
            if (currentState != null)
            {
                State nextState = currentState.Tick(this, enemyStats, enemyAnimatorManager);

                if (nextState != null)
                {
                    SwitchToNextState(nextState);
                }
            }
        }

        private void SwitchToNextState(State state)
        {
            currentState = state;
        }

        private void HandleRecoveryTimer()
        {
            if (currentRecoveryTime > 0)
            {
                currentRecoveryTime -= Time.deltaTime;
            }

            if (isPerformingAction)
            {
                if (currentRecoveryTime < 0)
                {
                    isPerformingAction = false;
                }
            }
        }
    }
}
