using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ReactableAnimator : MonoBehaviour, IReactable
{
    [SerializeField] Animator animator;
    [SerializeField] string defaultAnim;
    [SerializeField] string[] animationNames;

    void Awake()
    {
        if (animator == null) animator = GetComponent<Animator>();
    }

    public void Act() => animator.SetTrigger(defaultAnim);
    public void Act(string name) => animator.SetTrigger(name);
    public void Act(int index)
    {
        if (index < 0 || index >= animationNames.Length) return;
        animator.SetTrigger(animationNames[index]);
    }
}
