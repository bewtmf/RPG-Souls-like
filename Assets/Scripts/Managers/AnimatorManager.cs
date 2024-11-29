using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DS
{
    public class AnimatorManager : MonoBehaviour
    {
        public Animator anim;


        public void PlayTargetAnimation(string targetAnim, bool isBusy)
        {
            anim.applyRootMotion = isBusy;
            anim.SetBool("isBusy", isBusy);
            anim.CrossFade(targetAnim, 0.2f);
        }
    }
}