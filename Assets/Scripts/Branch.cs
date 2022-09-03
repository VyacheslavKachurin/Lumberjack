using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Branch : MonoBehaviour
{
    public event Action BranchDestroyed;
    [SerializeField] private ParticleSystem _branchParticle;

    private float _step;
    private Player _player;
    private Rigidbody2D _rb;
    private bool _movingAllowed = false;
    private Vector2 _particlePosition = new(1.43f, -3.81f);

    public void Initialize(Player player)
    {
        _player = player;
        _player.PlayerMoved += MoveDown;

        _step = Player.Height * 1.5f;

        if (transform.position.x < 0)
            _particlePosition.x *= -1;

        _rb = GetComponent<Rigidbody2D>();
    }

    private void MoveDown()
    {
        if (Values.Instance.IsGameOn)
            _movingAllowed = true;
        else
            _movingAllowed = false;
    }

    private void FixedUpdate()
    {
        if (_movingAllowed)
        {
            _rb.MovePosition(_rb.position + Vector2.down * _step);
            _movingAllowed = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) => BranchDestroyed?.Invoke();

    public void DestroyBranch()
    {
        _player.PlayerMoved -= MoveDown;

        Instantiate(_branchParticle, _particlePosition, Quaternion.identity);
        Destroy(gameObject);
    }



}
