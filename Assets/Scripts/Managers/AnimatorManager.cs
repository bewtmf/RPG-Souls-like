using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DS
{
    public class AnimatorManager : MonoBehaviour
    {
        public Animator anim;
        public bool canRotate;

        public void PlayTargetAnimation(string targetAnim, bool isBusy)
        {
            anim.applyRootMotion = isBusy;
            anim.SetBool("canRotate", false);
            anim.SetBool("isBusy", isBusy);
            anim.CrossFade(targetAnim, 0.2f);
        }

        public virtual void TakeCriticalDamageAnimationEvent()
        {
            
        }
    }
}