using UnityEngine;

public class DisableOnExit : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Disable animator object
        animator.gameObject.SetActive(false);
    }
}
