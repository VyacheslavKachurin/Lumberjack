using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaft : MonoBehaviour
{
    [SerializeField] private Transform _branchSpawnPoint;
    public Transform BranchSpawnPoint => _branchSpawnPoint;

    public float Width { get; private set; }
    private float _y;
    private SpriteRenderer _spriteRenderer;


    public void Initialize(Player player)
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        player.PlayerMoved += ScrollTexture;
        Width = this.transform.lossyScale.x;
    }


    private void ScrollTexture()
    {
        if (Values.Instance.IsGameOn)
        {
            float Y = Mathf.Repeat(_y += 0.1f, 1);
            _spriteRenderer.material.mainTextureOffset = new Vector2(0, Y);
        }
    }

}

