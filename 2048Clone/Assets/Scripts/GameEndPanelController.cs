using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndPanelController : MonoBehaviour
{
    [SerializeField] GameObject _gameOverPanel;
    [SerializeField] GameObject _gameCompletedPanel;
    private void OnEnable()
    {
        GameManager.Instance.OnGameOver += HandleOnGameOver;
        GameManager.Instance.OnGameCompleted += HandleOnGameCompleted;
        GameManager.Instance.OnRestart += HandleOnGameRestart;
    }
    void HandleOnGameOver()
    {
        _gameOverPanel.SetActive(true);
    }
    void HandleOnGameCompleted()
    {
        _gameCompletedPanel.SetActive(true);
    }
    void HandleOnGameRestart()
    {
        _gameOverPanel.SetActive(false);
        _gameCompletedPanel.SetActive(false);
    }
}
