using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace DS
{
    public class EnemyLocomotionManager : CharacterManager
    {
        EnemyManager enemyManager;
        EnemyAnimatorManager enemyAnimatorManager;

        public CapsuleCollider characcterCollider;
        public CapsuleCollider characterCollisionBlockerCollider;

        public LayerMask detectionLayer;

        private void Awake()
        {
            enemyManager = GetComponent<EnemyManager>();
            enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
        }

        private void Start()
        {
            Physics.IgnoreCollision(characcterCollider, characterCollisionBlockerCollider, true);
        }
    }
}
