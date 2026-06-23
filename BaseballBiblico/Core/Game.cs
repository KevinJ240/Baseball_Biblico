using Raylib_cs;
using BaseballBiblico.Screens;

namespace BaseballBiblico.Core;

public class Game
{
    private GameScreenType currentScreen;

    private readonly MenuScreen menuScreen;
    private readonly GameScreen gameScreen;

    public Game()
    {
        Raylib.InitWindow(1280, 720, "Baseball Bíblico");
        Raylib.SetTargetFPS(60);

        currentScreen = GameScreenType.Menu;

        menuScreen = new MenuScreen();
        gameScreen = new GameScreen();
    }

    public void Run()
    {
        while (!Raylib.WindowShouldClose())
        {
            Update();
            Draw();
        }

        Raylib.CloseWindow();
    }

    private void Update()
    {
        if (currentScreen == GameScreenType.Menu)
        {
            menuScreen.Update();

            if (menuScreen.NextScreen == GameScreenType.Game)
            {
                currentScreen = GameScreenType.Game;
            }
        }
        else if (currentScreen == GameScreenType.Game)
        {
            gameScreen.Update();
        }
    }

    private void Draw()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.RayWhite);

        if (currentScreen == GameScreenType.Menu)
        {
            menuScreen.Draw();
        }
        else if (currentScreen == GameScreenType.Game)
        {
            gameScreen.Draw();
        }

        Raylib.EndDrawing();
    }
}