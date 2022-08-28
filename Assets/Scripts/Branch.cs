using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Branch : MonoBehaviour
{
    public event Action BranchDestroyed;
    public static float Width;
    private float _step;
    private Player _player;

    public void Initialize(Player player)
    {
        _player = player;
        _player.PlayerMoved += MoveDown;

        _step = Player.Height * 1.5f;
        Width = this.transform.lossyScale.x;
      
    }

    public void MoveDown()
    {
        if (Values.Instance.IsGameOn)

            this.transform.Translate(Vector2.down * _step, Space.World);
        //   _rb.MovePosition(_rb.position + Vector2.down * Time.fixedDeltaTime);

    }

    private void OnTriggerEnter2D(Collider2D collision) => BranchDestroyed?.Invoke();

    public void DestroyBranch()
    {
        _player.PlayerMoved -= MoveDown;
        Destroy(gameObject);
    }



}
