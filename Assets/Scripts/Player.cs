using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static float Height;
    public event Action PlayerMoved;
    public event Action<EPosition> PlayerChangedPosition;

    public event Action PlayerDied;
    public event Action PlayerDisabled;

    private float _xStep;
    private Animator _anim;

    void Start()
    {

        Height = transform.lossyScale.y;
        _anim = GetComponent<Animator>();

        _xStep = 2 * transform.lossyScale.x;
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
        PlayerChangedPosition?.Invoke(position);

        switch (position)
        {
            case EPosition.Left:

                transform.position = new Vector2(-_xStep, transform.position.y);
                transform.localScale = new Vector2(-1, 1);
                break;

            case EPosition.Right:
                transform.position = new Vector2(_xStep, transform.position.y);
                transform.localScale = new Vector2(1, 1);
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
