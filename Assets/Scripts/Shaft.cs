using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaft : MonoBehaviour
{

    public static float Width;
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
        if (Settings.Instance.IsGameOn)
        {
            float Y = Mathf.Repeat(_y += 0.1f, 1);
            _spriteRenderer.material.mainTextureOffset = new Vector2(0, Y);
        }
    }

}

