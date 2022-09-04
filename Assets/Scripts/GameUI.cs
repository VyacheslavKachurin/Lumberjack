using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public event Action PauseButtonToggled;
    public event Action SFXButtonToggled;
    public event Action MusicButtonToggled;
    public event Action PlayButtonPressed;
    public event Action RestartButtonPressed;
    public InputManager InputManager { get { return _inputManager; } }

    [SerializeField] private InputManager _inputManager;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _gameOverText;
    [SerializeField] private TextMeshProUGUI _tryAgainText;
    [SerializeField] private TextMeshProUGUI _highScoreNumber;
    [SerializeField] private TextMeshProUGUI _yourScoreNumber;
    [SerializeField] private TextMeshProUGUI _levelNumberText;

    [SerializeField] private Slider _scoreSlider;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _menuPanel;

    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _sfxButton;
    [SerializeField] private Button _musicButton;
    [SerializeField] private Button _pauseButton;
    [SerializeField] private Button _playButton;

    void Start()
    {
        _pauseButton.onClick.AddListener(() => PauseButtonToggled?.Invoke());
        _continueButton.onClick.AddListener(() => PauseButtonToggled?.Invoke());
        _sfxButton.onClick.AddListener(() => SFXButtonToggled?.Invoke());
        _musicButton.onClick.AddListener(() => MusicButtonToggled?.Invoke());
        _playButton.onClick.AddListener(() => PlayButtonPressed?.Invoke());

        PauseButtonToggled += TogglePausePanel;
        PlayButtonPressed += HideMenuPanel;

        _tryAgainText.enabled = false;
        _gameOverText.gameObject.SetActive(false);
    }

    public void UpdateLevelNumber(int value) => _levelNumberText.text = value.ToString();
    public void UpdateScoreText(int value) => _scoreText.text = value.ToString();
    public void UpdateScoreBar(float value) => _scoreSlider.value = value;
    private void TogglePausePanel() => _pausePanel.SetActive(!_pausePanel.activeInHierarchy);
    public void HideMenuPanel() => _menuPanel.SetActive(false);

    public void HideStartText() => _titleText.gameObject.SetActive(false);
    public void HideRestartText()
    {
        _tryAgainText.enabled = false;
        _gameOverText.gameObject.SetActive(false);
    }

    public void UpdateHighScoreText(int text) => _highScoreNumber.text = text.ToString();

    public void ShowRestartText()
    {
        _tryAgainText.enabled = true;
        _gameOverText.gameObject.SetActive(true);
        _playButton.GetComponent<Image>().color = Color.green;
        _playButton.onClick.RemoveAllListeners();
        _playButton.onClick.AddListener(() => RestartButtonPressed?.Invoke());
        _menuPanel.SetActive(true);
    }

    public void UpdateYourScore(int value) => _yourScoreNumber.text = value.ToString();
}
