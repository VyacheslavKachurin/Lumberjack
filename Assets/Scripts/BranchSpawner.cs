using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchSpawner
{
    private int _counter = 0;

    private readonly GameObject _branch;
    private readonly Player _player;
    private readonly Queue<Branch> _branches = new();
    private readonly Transform _branchSpawnPoint;
    private readonly float _shaftWidth;


    public BranchSpawner(Player player, Shaft shaft, GameObject branch)
    {
        _player = player;
        _branchSpawnPoint = shaft.BranchSpawnPoint;
        _shaftWidth = shaft.Width;
        _branch = branch;

        _player.PlayerMoved += () =>
        {
            _counter++;
            if (_counter == 2)
            {
                SpawnBranch(GetRandomValue());
                _counter = 0;
            }
        };

    }

    private EPosition GetRandomValue()
    {
        var values = Enum.GetValues(typeof(EPosition));
        var length = values.Length;
        var random = UnityEngine.Random.Range(0, length);
        return (EPosition)values.GetValue(random);

    }

    private void SpawnBranch(EPosition position)
    {
        if (position == EPosition.Middle)
            return;
        Branch branchInstance = UnityEngine.Object.Instantiate(_branch, _branchSpawnPoint.position, Quaternion.identity).GetComponent<Branch>();

        branchInstance.Initialize(_player);
        branchInstance.BranchDestroyed += DestroyBranch;
        _branches.Enqueue(branchInstance);

        var offset = branchInstance.transform.lossyScale.x / 2 + _shaftWidth / 2; //TODO: avoid repeating calculation

        _ = position switch
        {
            EPosition.Left => branchInstance.transform.position = new Vector2(-offset, branchInstance.transform.position.y),
            EPosition.Right => branchInstance.transform.position = new Vector2(offset, branchInstance.transform.position.y),
            _ => throw new NotImplementedException()
        };
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
