using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { menu, game, pause };

public class GameManager : MonoBehaviour
{
    public GameState gameState;
}
