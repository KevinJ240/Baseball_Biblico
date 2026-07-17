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

        // Debe ser la única llamada a InitWindow
        // dentro de todo el proyecto.
        Raylib.InitWindow(
            ScreenScaler.VirtualWidth,
            ScreenScaler.VirtualHeight,
            "Baseball Bíblico"
        );

        if (!Raylib.IsWindowReady())
        {
            throw new InvalidOperationException(
                "No se pudo inicializar la ventana del juego."
            );
        }

        Raylib.SetTargetFPS(60);

        renderTexture = Raylib.LoadRenderTexture(
            ScreenScaler.VirtualWidth,
            ScreenScaler.VirtualHeight
        );

        if (renderTexture.Id <= 0)
        {
            Raylib.CloseWindow();

            throw new InvalidOperationException(
                "No se pudo crear la textura de renderizado del juego."
            );
        }

        Raylib.SetTextureFilter(
            renderTexture.Texture,
            TextureFilter.Bilinear
        );

        /*
         * Estas clases deben crearse después de InitWindow,
         * porque cargan fuentes, imágenes y texturas.
         *
         * Se asignan directamente dentro del constructor
         * porque los campos están declarados como readonly.
         */
        menuScreen = new MenuScreen();
        gameScreen = new GameScreen();
        optionsScreen = new OptionsScreen();
        creditsScreen = new CreditsScreen();

        currentScreen = GameScreenType.Menu;
    }

    public void Run()
    {
        try
        {
            while (!Raylib.WindowShouldClose())
            {
                ScreenScaler.Update();

                Update();
                Draw();
            }
        }
        finally
        {
            Unload();
        }
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

        switch (menuScreen.NextScreen)
        {
            case GameScreenType.Game:
                gameScreen.IniciarNuevaPartida();
                menuScreen.Reset();

                currentScreen = GameScreenType.Game;
                break;

            case GameScreenType.Options:
                optionsScreen.Reset();
                menuScreen.Reset();

                currentScreen = GameScreenType.Options;
                break;

            case GameScreenType.Credits:
                creditsScreen.Reset();
                menuScreen.Reset();

                currentScreen = GameScreenType.Credits;
                break;
        }
    }

    private void UpdateGame()
    {
        gameScreen.Update();

        if (gameScreen.NextScreen != GameScreenType.Menu)
        {
            return;
        }

        menuScreen.Reset();
        currentScreen = GameScreenType.Menu;
    }

    private void UpdateOptions()
    {
        optionsScreen.Update();

        if (optionsScreen.NextScreen != GameScreenType.Menu)
        {
            return;
        }

        optionsScreen.Reset();
        menuScreen.Reset();

        currentScreen = GameScreenType.Menu;
    }

    private void UpdateCredits()
    {
        creditsScreen.Update();

        if (creditsScreen.NextScreen != GameScreenType.Menu)
        {
            return;
        }

        creditsScreen.Reset();
        menuScreen.Reset();

        currentScreen = GameScreenType.Menu;
    }

    private void Draw()
    {
        DrawVirtualScreen();
        DrawWindow();
    }

    private void DrawVirtualScreen()
    {
        Raylib.BeginTextureMode(
            renderTexture
        );

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
    }

    private void DrawWindow()
    {
        Raylib.BeginDrawing();

        Raylib.ClearBackground(
            Color.Black
        );

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
        {
            return;
        }

        recursosDescargados = true;

        /*
         * Los recursos deben descargarse antes
         * de cerrar la ventana de Raylib.
         */
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

        if (Raylib.IsWindowReady())
        {
            Raylib.CloseWindow();
        }
    }
}