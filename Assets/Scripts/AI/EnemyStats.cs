using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DS
{
    public class EnemyStats : CharacterStats
    {
        Animator animator;
        EnemyAnimatorManager enemyAnimatorManager;

        public int soulAwardedOnDeath = 30;

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
        }

        private void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
        }

        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        public void TakeDamageNoAnimation(int damage)
        {
            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;
            }
        }

        public void TakeDamage(int damage)
        {
            if (isDead)
                return;

            currentHealth -= damage;

            enemyAnimatorManager.PlayTargetAnimation("Damaged", true);

            if(currentHealth <= 0)
            {
                HandleDead();
            }
        }

        public void HandleDead()
        {
            currentHealth = 0;
            enemyAnimatorManager.PlayTargetAnimation("Dead", true);
            isDead = true;
            PlayerStats playerStats = FindObjectOfType<PlayerStats>();

            if (playerStats != null)
            {
                playerStats.AddSouls(soulAwardedOnDeath);
            }
        }
    }
}
