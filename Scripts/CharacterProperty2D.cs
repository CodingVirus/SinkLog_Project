using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterProperty2D : MonoBehaviour
{
    Animator _anim;
    protected Animator myAnim
    {
        get
        {
            if (_anim == null)
            {
                _anim = GetComponent<Animator>();
                if(_anim == null)
                {
                    _anim = GetComponentInChildren<Animator>();
                }
            }
            return _anim;
        }
    }

    Rigidbody2D _rigid;
    protected Rigidbody2D myRigid
    {
        get
        {
            if(_rigid == null)
            {
                _rigid = GetComponent<Rigidbody2D>();
                if (_rigid == null)
                {
                    _rigid = GetComponentInChildren<Rigidbody2D>();
                }
            }
            return _rigid;
        }
    }

    SpriteRenderer _render;
    protected SpriteRenderer myRender
    {
        get
        {
            if(_render == null)
            {
                _render = GetComponent<SpriteRenderer>();
                if(_render == null)
                {
                    _render = GetComponentInChildren<SpriteRenderer>();
                }
            }
            return _render;
        }
    }

    Collider2D _collider;
    protected Collider2D myCollider
    {
        get
        {
            if(_collider == null)
            {
                _collider = GetComponent<Collider2D>();
                if(_collider == null)
                {
                    _collider = GetComponentInChildren<Collider2D>();
                }
            }
            return _collider;
        }
    }
    Collision2D _collision;
    protected Collision2D myCollision
    {
        get
        {
            if(_collision == null)
            {
                _collision = GetComponent<Collision2D>();
                if (_collision == null)
                {
                    _collision = GetComponentInChildren<Collision2D>();
                }
            }
            return _collision;
        }
    }

    public Stats stat;
    public Slider myHpBar = null;
    float _hp = 0.0f;

    protected float curHP
    {
        get => _hp;
        set
        {
            _hp = Mathf.Clamp(value, 0.0f, stat.MaxHp);
            if (myHpBar != null) myHpBar.value = _hp / stat.MaxHp;
        }
    }
}
