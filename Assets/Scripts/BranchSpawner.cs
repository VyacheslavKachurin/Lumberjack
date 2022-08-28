using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchSpawner : MonoBehaviour
{
    [SerializeField] Transform _branchSpawnPoint;
    [SerializeField] Branch _branch;

    private int _counter = 0;
    private Player _player;
    private Queue<Branch> _branches = new();


    public void Initialize(Player player)
    {
        _player = player;
        Shaft shaft = GetComponent<Shaft>();
        shaft.Initialize(player);

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
        Branch branchInstance = Instantiate(_branch, _branchSpawnPoint.position, Quaternion.identity);
        branchInstance.Initialize(_player);
        branchInstance.BranchDestroyed += DestroyBranch;
        _branches.Enqueue(branchInstance);

        var offset = branchInstance.transform.lossyScale.x / 2 + Shaft.Width / 2; //TODO: avoid repeating calculation

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
