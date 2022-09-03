using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchSpawner
{
    private int _counter = 0;

    private const float _xOffset = 2.3f;
    private readonly GameObject _leftBranch;
    private readonly GameObject _rightBranch;
    private readonly Player _player;
    private readonly Queue<Branch> _branches = new();
    private readonly Transform _branchSpawnPoint;

    private EPosition[] _positions;

    public BranchSpawner(Player player, Shaft shaft, GameObject leftBranch, GameObject rightBranch)
    {
        _player = player;
        _branchSpawnPoint = shaft.BranchSpawnPoint;
        _leftBranch = leftBranch;
        _rightBranch = rightBranch;

        _player.PlayerMoved += () =>
        {
            _counter++;
            if (_counter == 2)
            {
                SpawnBranch(GetRandomValue());
                _counter = 0;
            }
        };

        SetDifficultyHarder(false);
    }

    private EPosition GetRandomValue()
    {
        var random = UnityEngine.Random.Range(0, _positions.Length);
        return (EPosition)_positions.GetValue(random);
    }

    public void SetDifficultyHarder(bool value)
    {
        if (value)
            _positions = new EPosition[2] { EPosition.Right, EPosition.Left };
        else
            _positions = (EPosition[])Enum.GetValues(typeof(EPosition));
    }

    private void SpawnBranch(EPosition position)
    {
        if (position == EPosition.Middle)
            return;

        Vector2 branchPosition = Vector2.zero;
        GameObject branchType = new();

        switch (position)
        {
            case
            EPosition.Left:
                branchPosition = new Vector2(-_xOffset, _branchSpawnPoint.position.y);
                branchType = _leftBranch;
                break;

            case EPosition.Right:
                branchPosition = new Vector2(_xOffset, _branchSpawnPoint.position.y);
                branchType = _rightBranch;
                break;
        }

        Branch branchInstance = UnityEngine.Object.Instantiate(branchType, branchPosition, Quaternion.identity).GetComponent<Branch>();
        branchInstance.Initialize(_player);
        branchInstance.BranchDestroyed += DestroyBranch;
        _branches.Enqueue(branchInstance);

    }

    private void DestroyBranch()
    {
        var branch = _branches.Dequeue();
        branch.DestroyBranch();
    }

    public void ClearBranches()
    {
        while (_branches.Count != 0)
            _branches.Dequeue().DestroyBranch();

    }
}
