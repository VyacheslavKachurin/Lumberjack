using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    public event Action<EPosition> LeftButtonPressed;
    public event Action<EPosition> RightButtonPressed;

    [SerializeField] private Button _leftButton;
    [SerializeField] private Button _rightButton;

    void Start()
    {
        _leftButton.onClick.AddListener(() => LeftButtonPressed?.Invoke(EPosition.Left));
        _rightButton.onClick.AddListener(() => RightButtonPressed?.Invoke(EPosition.Right));
    }
}

public enum EPosition{Left, Right, Middle};
