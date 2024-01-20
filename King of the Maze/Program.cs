
using King_of_the_Maze;
using Microsoft.Xna.Framework.Input;
using System;

using var game = new King_of_the_Maze.Game1();
game.Run();
public enum GameState
{
    GameMenu,
    GameInstructions,
    GamePlaying,
    GamePaused,
    GameOver, 
    GameExit
}
