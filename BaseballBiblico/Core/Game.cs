using Raylib_cs;
using BaseballBiblico.Screens;

namespace BaseballBiblico.Core;

public class Game
{
    private GameScreenType currentScreen;

    private readonly MenuScreen menuScreen;
    private readonly GameScreen gameScreen;
    private readonly OptionsScreen optionsScreen;

    private RenderTexture2D renderTexture;

    public Game()
    {
        Raylib.SetConfigFlags(ConfigFlags.ResizableWindow);
        Raylib.InitWindow(1280, 720, "Baseball Bíblico");
        Raylib.SetTargetFPS(60);

        renderTexture = Raylib.LoadRenderTexture(ScreenScaler.VirtualWidth, ScreenScaler.VirtualHeight);

        currentScreen = GameScreenType.Menu;

        menuScreen = new MenuScreen();
        gameScreen = new GameScreen();
        optionsScreen = new OptionsScreen();
    }

    public void Run()
    {
        while (!Raylib.WindowShouldClose())
        {
            ScreenScaler.Update();

            Update();
            Draw();
        }

        Raylib.UnloadRenderTexture(renderTexture);
        Raylib.CloseWindow();
    }

    private void Update()
    {
        if (currentScreen == GameScreenType.Menu)
        {
            menuScreen.Update();

            if (menuScreen.NextScreen == GameScreenType.Game)
            {
                menuScreen.Reset();
                currentScreen = GameScreenType.Game;
            }
            else if (menuScreen.NextScreen == GameScreenType.Options)
            {
                menuScreen.Reset();
                currentScreen = GameScreenType.Options;
            }
        }
        else if (currentScreen == GameScreenType.Game)
        {
            gameScreen.Update();
        }
        else if (currentScreen == GameScreenType.Options)
        {
            optionsScreen.Update();

            if (optionsScreen.NextScreen == GameScreenType.Menu)
            {
                optionsScreen.Reset();
                currentScreen = GameScreenType.Menu;
            }
        }
    }

    private void Draw()
    {
        Raylib.BeginTextureMode(renderTexture);

        // Limpia la pantalla virtual antes de dibujar cada screen
        Raylib.ClearBackground(new Color(73, 138, 44, 255));

        if (currentScreen == GameScreenType.Menu)
            menuScreen.Draw();
        else if (currentScreen == GameScreenType.Game)
            gameScreen.Draw();
        else if (currentScreen == GameScreenType.Options)
            optionsScreen.Draw();

        Raylib.EndTextureMode();

        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.Black);

        Raylib.DrawTexturePro(
            renderTexture.Texture,
            new Rectangle(0, 0, ScreenScaler.VirtualWidth, -ScreenScaler.VirtualHeight),
            ScreenScaler.DestinationRect,
            new System.Numerics.Vector2(0, 0),
            0,
            Color.White
        );

        Raylib.EndDrawing();
    }
}