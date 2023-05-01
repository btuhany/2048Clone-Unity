using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public event System.Action OnGameOver;
    public event System.Action OnRestart;
    public event System.Action OnGameCompleted;
    private void Awake()
    {
        Instance= this;
    }
    public void GameOver()
    {
        OnGameOver?.Invoke();
      
    }
    public void Restart()
    {
        OnRestart?.Invoke();
    }
    public void GameCompleted()
    {
        OnGameCompleted?.Invoke();
        
    }
}
