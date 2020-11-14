﻿using UnityEngine;

public class DisableOnExit : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // base.OnStateExit(animator, stateInfo, layerIndex);
        animator.gameObject.SetActive(false);
    }
}
