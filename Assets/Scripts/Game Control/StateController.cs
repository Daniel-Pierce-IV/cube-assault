using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour
{
	private GameState _gameState = GameState.Active;

    public GameState GetGameState()
	{
		return _gameState;
	}

	public void SetGameState(GameState gameState)
	{
		_gameState = gameState;
	}

	public enum GameState
	{
		Active,
		Stopped
	}
}
