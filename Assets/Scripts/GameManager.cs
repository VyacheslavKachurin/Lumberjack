using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Collider2D _cleanerTrigger;
    [SerializeField] private GameUI _gameUI;
    [SerializeField] private Player _player;
    [SerializeField] private BranchSpawner _branchSpawner;
    [SerializeField] private GameObject _floor;

    private Coroutine _scoreCoroutine;
    private int _score;

    private float Counter
    {
        get { return _counter; }
        set { if (value >= 0 && value <= 20) _counter = value; }
    }

    private float _counter;

    private void Start()
    {
        if (Settings.Instance == null)
            _ = new Settings();

        _gameUI = Instantiate(_gameUI);

        _gameUI.PauseButtonToggled += TogglePause;
        _gameUI.PlayButtonPressed += StartGame;
        _gameUI.RestartButtonPressed += Restart;
        _player = Instantiate(_player, new Vector2(0, -4.1f), Quaternion.identity);
        _player.PlayerDied += FinishGame;
        _player.Initialize(_gameUI.InputManager);

        _branchSpawner = Instantiate(_branchSpawner);
        _branchSpawner.Initialize(_player);

        _cleanerTrigger = Instantiate(_cleanerTrigger, new Vector2(0, -5), Quaternion.identity);

        _floor = Instantiate(_floor, new Vector2(0, -5.67f), Quaternion.identity);

        _gameUI.UpdateHighScoreText(Settings.Instance.HighScore);

    }

    private void TogglePause()
    {
        if (Time.timeScale == 1)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    private IEnumerator DescreaseCounter()
    {
        while (Settings.Instance.IsGameOn)
        {
            Counter -= 0.1f;
            _gameUI.UpdateScoreBar(Counter);
            if (Counter < 1)
                FinishGame();
            yield return new WaitForSeconds(0.04f);
        }
    }

    public void FinishGame()
    {
        Settings.Instance.FinishGame();
        _player.PlayerMoved -= AddScore;
        StopCoroutine(_scoreCoroutine);
        _gameUI.ShowRestartText();

        Settings.Instance.HighScore = _score;
        _gameUI.UpdateHighScoreText(Settings.Instance.HighScore);
    }

    private void AddScore()
    {
        _score++;
        _gameUI.UpdateScoreText(_score);
        Counter++;
        _gameUI.UpdateScoreBar(Counter);
    }

    private void StartGame()
    {
        CreateInitialConditions();
        _gameUI.HideStartText();
    }

    private void CreateInitialConditions()
    {
        _player.ChangePosition(EPosition.Left);

        Counter = 10;
        _score = 0;

        _gameUI.UpdateScoreBar(_counter);
        _gameUI.UpdateScoreText(_score);
        _branchSpawner.ClearBranches();

        _player.PlayerMoved += AddScore;
        _gameUI.HideMenuPanel();
        Settings.Instance.StartGame();
        _scoreCoroutine = StartCoroutine(DescreaseCounter());

    }

    private void Restart()
    {
        CreateInitialConditions();
        _gameUI.HideRestartText();
    }

}
