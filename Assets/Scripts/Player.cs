using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static float Height;
    public event Action PlayerMoved;
    public event Action PlayerDied;

    private Transform _transform;
    private float _xStep;

    void Start()
    {
        _transform = GetComponent<Transform>();

        _xStep = 2 * _transform.lossyScale.x;
        Height = _transform.lossyScale.y;

    }

    public void Initialize(InputManager inputManager)
    {
        inputManager.LeftButtonPressed += (position) => ChangePosition(position);
        inputManager.RightButtonPressed += (position) => ChangePosition(position);
       
    }

    public void ChangePosition(EPosition position)
    {
        PlayerMoved?.Invoke();

        _ = position switch
        {
            EPosition.Left => _transform.position = new Vector2(-_xStep, transform.position.y),
            EPosition.Right => _transform.position = new Vector2(_xStep, transform.position.y),
            _ => throw new ArgumentOutOfRangeException(),
        };

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerDied?.Invoke();
    }

}
