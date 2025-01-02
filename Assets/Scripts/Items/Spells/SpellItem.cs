using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DS
{
    public class SpellItem : Item
    {
        public GameObject spellWarmupFX;
        public GameObject spellCastFX;
        public string spellAnimation;

        [Header("Spell Cost")]
        public int focusPointCost;

        [Header("Spell Type")]
        public bool isFaithSpell;
        public bool isMagicSpell;
        public bool isPyromancySpell;

        [Header("Spell Description")]
        [TextArea]
        public string spellDescription;
         
        public virtual void AttemptToCastSpell(AnimatorHandler animatorHandler, PlayerStats playerStats)
        {
            Debug.Log("Attempting to cast spell...");

        }

        public virtual void SuccessfullyCastSpell(AnimatorHandler animatorHandler, PlayerStats playerStats)
        {
            Debug.Log("Successfully cast spell!");
            playerStats.DeductFocusPoints(focusPointCost);
        }
    }
}
