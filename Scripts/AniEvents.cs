using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AniEvents : MonoBehaviour
{
    public UnityEvent OnAnimationEventTriggered;
    public UnityEvent AttackAction;
    public UnityEvent OnDamageAction;

    public void TriggerEvent()
    {
        OnAnimationEventTriggered?.Invoke();
    }

    public void OnAttack()
    {
        AttackAction?.Invoke();
    }

    public void OnDamage()
    {
        OnDamageAction?.Invoke();
    }
}
