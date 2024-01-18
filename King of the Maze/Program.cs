
using King_of_the_Maze;
using System;

using var game = new King_of_the_Maze.Game1();
game.Run();
public enum GameState
{
    GameMenu,
    GameSettings,
    GameInstructions,
    GamePlaying,
    GamePaused,
    GameOver
}
