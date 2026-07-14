using Raylib_cs;
using System.Numerics;
using BaseballBiblico.Screens;

namespace BaseballBiblico.Core;

public class Game
{
    private GameScreenType currentScreen;

    private readonly MenuScreen menuScreen;
    private readonly GameScreen gameScreen;
    private readonly OptionsScreen optionsScreen;
    private readonly CreditsScreen creditsScreen;

    private RenderTexture2D renderTexture;

    private bool recursosDescargados;

    public Game()
    {
        Raylib.SetConfigFlags(
            ConfigFlags.ResizableWindow
        );

        Raylib.SetTraceLogLevel(
            TraceLogLevel.Error
        );

        // Esta debe ser la única llamada a InitWindow
        // en todo el proyecto.
        Raylib.InitWindow(
            ScreenScaler.VirtualWidth,
            ScreenScaler.VirtualHeight,
            "Baseball Bíblico"
        );

        Raylib.SetTargetFPS(60);

        renderTexture = Raylib.LoadRenderTexture(
            ScreenScaler.VirtualWidth,
            ScreenScaler.VirtualHeight
        );

        menuScreen = new MenuScreen();
        gameScreen = new GameScreen();
        optionsScreen = new OptionsScreen();
        creditsScreen = new CreditsScreen();

        currentScreen = GameScreenType.Menu;
    }

    public void Run()
    {
        while (!Raylib.WindowShouldClose())
        {
            ScreenScaler.Update();

            Update();
            Draw();
        }

        Unload();
    }

    private void Update()
    {
        switch (currentScreen)
        {
            case GameScreenType.Menu:
                UpdateMenu();
                break;

            case GameScreenType.Game:
                UpdateGame();
                break;

            case GameScreenType.Options:
                UpdateOptions();
                break;

            case GameScreenType.Credits:
                UpdateCredits();
                break;
        }
    }

    private void UpdateMenu()
    {
        menuScreen.Update();

        if (menuScreen.NextScreen == GameScreenType.Game)
        {
            gameScreen.IniciarNuevaPartida();

            menuScreen.Reset();
            currentScreen = GameScreenType.Game;
            return;
        }

        if (menuScreen.NextScreen == GameScreenType.Options)
        {
            optionsScreen.Reset();

            menuScreen.Reset();
            currentScreen = GameScreenType.Options;
            return;
        }

        if (menuScreen.NextScreen == GameScreenType.Credits)
        {
            creditsScreen.Reset();

            menuScreen.Reset();
            currentScreen = GameScreenType.Credits;
        }
    }

    private void UpdateGame()
    {
        gameScreen.Update();

        if (gameScreen.NextScreen == GameScreenType.Menu)
        {
            menuScreen.Reset();
            currentScreen = GameScreenType.Menu;
        }
    }

    private void UpdateOptions()
    {
        optionsScreen.Update();

        if (optionsScreen.NextScreen == GameScreenType.Menu)
        {
            optionsScreen.Reset();
            menuScreen.Reset();

            currentScreen = GameScreenType.Menu;
        }
    }

    private void UpdateCredits()
    {
        creditsScreen.Update();

        if (creditsScreen.NextScreen == GameScreenType.Menu)
        {
            creditsScreen.Reset();
            menuScreen.Reset();

            currentScreen = GameScreenType.Menu;
        }
    }

    private void Draw()
    {
        // Primero se dibuja en la resolución virtual.
        Raylib.BeginTextureMode(renderTexture);

        Raylib.ClearBackground(
            new Color(73, 138, 44, 255)
        );

        switch (currentScreen)
        {
            case GameScreenType.Menu:
                menuScreen.Draw();
                break;

            case GameScreenType.Game:
                gameScreen.Draw();
                break;

            case GameScreenType.Options:
                optionsScreen.Draw();
                break;

            case GameScreenType.Credits:
                creditsScreen.Draw();
                break;
        }

        Raylib.EndTextureMode();

        // Luego se dibuja la textura virtual en la ventana real.
        Raylib.BeginDrawing();

        Raylib.ClearBackground(Color.Black);

        Raylib.DrawTexturePro(
            renderTexture.Texture,
            new Rectangle(
                0,
                0,
                renderTexture.Texture.Width,
                -renderTexture.Texture.Height
            ),
            ScreenScaler.DestinationRect,
            Vector2.Zero,
            0,
            Color.White
        );

        Raylib.EndDrawing();
    }

    private void Unload()
    {
        if (recursosDescargados)
            return;

        recursosDescargados = true;

        menuScreen.Unload();
        gameScreen.Unload();
        optionsScreen.Unload();
        creditsScreen.Unload();

        if (renderTexture.Id > 0)
        {
            Raylib.UnloadRenderTexture(
                renderTexture
            );
        }

        Raylib.CloseWindow();
    }
}