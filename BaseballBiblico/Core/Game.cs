using Raylib_cs;
using BaseballBiblico.Screens;
using System.Numerics;

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
        Raylib.SetConfigFlags(
            ConfigFlags.ResizableWindow
        );

        Raylib.InitWindow(
            1280,
            720,
            "Baseball Bíblico"
        );

        Raylib.SetTargetFPS(60);

        renderTexture =
            Raylib.LoadRenderTexture(
                ScreenScaler.VirtualWidth,
                ScreenScaler.VirtualHeight
            );

        menuScreen =
            new MenuScreen();

        gameScreen =
            new GameScreen();

        optionsScreen =
            new OptionsScreen();

        currentScreen =
            GameScreenType.Menu;
    }

    public void Run()
    {
        while (!Raylib.WindowShouldClose())
        {
            ScreenScaler.Update();

            Update();
            Draw();
        }

        DescargarRecursos();

        Raylib.UnloadRenderTexture(
            renderTexture
        );

        Raylib.CloseWindow();
    }

    private void Update()
    {
        switch (currentScreen)
        {
            case GameScreenType.Menu:
                ActualizarMenu();
                break;

            case GameScreenType.Game:
                ActualizarPartida();
                break;

            case GameScreenType.Options:
                ActualizarOpciones();
                break;
        }
    }

    private void ActualizarMenu()
    {
        menuScreen.Update();

        if (menuScreen.NextScreen ==
            GameScreenType.Game)
        {
            /*
             * Aquí es donde comienza realmente
             * una partida completamente nueva.
             */
            gameScreen.IniciarNuevaPartida();

            menuScreen.Reset();

            currentScreen =
                GameScreenType.Game;

            return;
        }

        if (menuScreen.NextScreen ==
            GameScreenType.Options)
        {
            optionsScreen.Reset();
            menuScreen.Reset();

            currentScreen =
                GameScreenType.Options;
        }
    }

    private void ActualizarPartida()
    {
        gameScreen.Update();

        if (gameScreen.NextScreen ==
            GameScreenType.Menu)
        {
            /*
             * VOLVER únicamente cambia de pantalla.
             * No es necesario reiniciar aquí.
             */
            menuScreen.Reset();

            currentScreen =
                GameScreenType.Menu;
        }
    }

    private void ActualizarOpciones()
    {
        optionsScreen.Update();

        if (optionsScreen.NextScreen ==
            GameScreenType.Menu)
        {
            menuScreen.Reset();
            optionsScreen.Reset();

            currentScreen =
                GameScreenType.Menu;
        }
    }

    private void Draw()
    {
        Raylib.BeginTextureMode(
            renderTexture
        );

        Raylib.ClearBackground(
            new Color(
                73,
                138,
                44,
                255
            )
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
        }

        Raylib.EndTextureMode();

        Raylib.BeginDrawing();

        Raylib.ClearBackground(
            Color.Black
        );

        Raylib.DrawTexturePro(
            renderTexture.Texture,

            new Rectangle(
                0,
                0,
                ScreenScaler.VirtualWidth,
                -ScreenScaler.VirtualHeight
            ),

            ScreenScaler.DestinationRect,

            Vector2.Zero,

            0,

            Color.White
        );

        Raylib.EndDrawing();
    }

    private void DescargarRecursos()
    {
        menuScreen.Unload();
        gameScreen.Unload();
        optionsScreen.Unload();
    }
}