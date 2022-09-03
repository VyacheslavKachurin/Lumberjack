using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static float Height;
    public event Action PlayerMoved;
    public event Action PlayerDied;
    public event Action PlayerDisabled;

    private Transform _transform;
    private float _xStep;
    private Animator _anim;

    void Start()
    {
        _transform = GetComponent<Transform>();
        _anim = GetComponent<Animator>();

        _xStep = 2 * _transform.lossyScale.x;
        Height = _transform.lossyScale.y;
        PlayerDied += () => _anim.Play("Base Layer.Die");

    }

    public void Initialize(InputManager inputManager)
    {
        inputManager.LeftButtonPressed += (position) => ChangePosition(position);
        inputManager.RightButtonPressed += (position) => ChangePosition(position);

    }

    public void ChangePosition(EPosition position)
    {
        if (!Values.Instance.IsInputAllowed)
            return;
        PlayerMoved?.Invoke();

        switch (position)
        {
            case EPosition.Left:

                _transform.position = new Vector2(-_xStep, transform.position.y);
                _transform.localScale = new Vector2(-1, 1);
                break;

            case EPosition.Right:
                _transform.position = new Vector2(_xStep, transform.position.y);
                _transform.localScale = new Vector2(1, 1);
                break;

        };

        if (Values.Instance.IsGameOn)
            _anim.Play("Base Layer.Chop");

    }

    private void DieAnimationFinished() => PlayerDied?.Invoke();

    public void ResetState() => _anim.Play("Base Layer.Idle");


    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerDisabled?.Invoke();
        _anim.Play("Base Layer.Die");
    }



}
