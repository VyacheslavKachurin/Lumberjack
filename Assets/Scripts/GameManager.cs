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
    [SerializeField] private Shaft _shaft;
    [SerializeField] private GameObject _branch;

    private Coroutine _scoreCoroutine;
    private int _score;
    private int _levelPoints;
    private int _levelNumber;

    private float _secondsWaitedToDecreaseBar = 0.04f;
    private const float _stepForDecreaseBar = 0.001f;

    private float Counter
    {
        get { return _counter; }
        set { if (value >= 0 && value <= 20) _counter = value; }
    }

    private float _counter;

    private void Start()
    {
        if (Values.Instance == null)
            _ = new Values();

        _gameUI = Instantiate(_gameUI);

        _gameUI.PauseButtonToggled += TogglePause;
        _gameUI.PlayButtonPressed += StartGame;
        _gameUI.RestartButtonPressed += Restart;
        _player = Instantiate(_player, new Vector2(0, -4.1f), Quaternion.identity);
        _player.PlayerDied += FinishGame;
        _player.PlayerDisabled += () => ChangeInput(false);
        _player.Initialize(_gameUI.InputManager);

        _shaft = Instantiate(_shaft, new Vector2(0, 0), Quaternion.identity);
        _shaft.Initialize(_player);
        _branchSpawner = new(_player, _shaft, _branch);


        _cleanerTrigger = Instantiate(_cleanerTrigger, new Vector2(0, -5), Quaternion.identity);

        _floor = Instantiate(_floor, new Vector2(0, -5.67f), Quaternion.identity);

        _gameUI.UpdateHighScoreText(Values.Instance.HighScore);
        Values.Instance.IsInputAllowed = true;

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
        while (Values.Instance.IsGameOn)
        {
            Counter -= 0.1f;
            _gameUI.UpdateScoreBar(Counter);
            if (Counter < 1)
                FinishGame();
            yield return new WaitForSeconds(_secondsWaitedToDecreaseBar);
        }
    }

    public void FinishGame()
    {
        Values.Instance.FinishGame();
        _player.PlayerMoved -= AddScore;
        StopCoroutine(_scoreCoroutine);
        _gameUI.ShowRestartText();

        Values.Instance.HighScore = _score;
        _gameUI.UpdateHighScoreText(Values.Instance.HighScore);
        _gameUI.UpdateYourScore(_score);
    }

    public void ChangeInput(bool value) => Values.Instance.IsInputAllowed = value;

    private void AddScore()
    {
        _score++;
        _levelPoints++;
        TrackLevelNumber();

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
        _player.ResetState();
        Counter = 10;
        _score = 0;
        _levelPoints = 0;
        _levelNumber = 1;
        _gameUI.UpdateLevelNumber(_levelNumber);
        _gameUI.UpdateScoreBar(_counter);
        _gameUI.UpdateScoreText(_score);

        _branchSpawner.ClearBranches();
        _branchSpawner.SetDifficultyHarder(false);

        _player.PlayerMoved += AddScore;
        _gameUI.HideMenuPanel();
        Values.Instance.StartGame();
        Values.Instance.IsInputAllowed = true;
        _scoreCoroutine = StartCoroutine(DescreaseCounter());

    }

    private void Restart()
    {
        CreateInitialConditions();
        _gameUI.HideRestartText();
    }

    private void TrackLevelNumber()
    {
        if (_levelPoints % 20 == 0)
        {
            _levelNumber++;
            _secondsWaitedToDecreaseBar -= _stepForDecreaseBar;
            _levelPoints = 0;
            _gameUI.UpdateLevelNumber(_levelNumber);

            if (_levelNumber == 5)
                _branchSpawner.SetDifficultyHarder(true);
        }
    }

}
