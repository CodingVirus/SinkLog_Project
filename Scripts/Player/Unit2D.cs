using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit2D : Stat
{
    protected int hashMove;
    protected int hashisJumping;
    protected int hashAttack;
    protected int hashisAttacking;
    protected int hashxVelocity;
    protected int hashisAir;
    protected int hashJump;
    protected int hashOnDamage;

    protected void Initalize()
    {
        hashMove = Animator.StringToHash("isMoving");
        hashisJumping = Animator.StringToHash("isJumping");
        hashAttack = Animator.StringToHash("Attack");
        hashisAttacking = Animator.StringToHash("isAttacking");
        hashxVelocity = Animator.StringToHash("xVelocity");
        hashisAir = Animator.StringToHash("isAir");
        hashJump = Animator.StringToHash("Jump");
        hashOnDamage = Animator.StringToHash("OnDamage");
    }
}
