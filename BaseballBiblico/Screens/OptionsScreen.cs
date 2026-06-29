using Raylib_cs;
using BaseballBiblico.UI;
using BaseballBiblico.Core;

namespace BaseballBiblico.Screens;

public class OptionsScreen
{
    private readonly GameButton btnFullscreen;
    private readonly GameButton btnVolver;

    public GameScreenType NextScreen { get; private set; } = GameScreenType.Options;

    public OptionsScreen()
    {
        btnFullscreen = new GameButton(470, 300, 340, 60, "PANTALLA COMPLETA");
        btnVolver = new GameButton(520, 420, 240, 60, "VOLVER");
    }

    public void Update()
    {
        if (btnFullscreen.IsClicked())
        {
            Raylib.ToggleFullscreen();
        }

        if (btnVolver.IsClicked())
        {
            NextScreen = GameScreenType.Menu;
        }
    }

    public void Draw()
    {
        Raylib.ClearBackground(new Color(73, 138, 44, 255));

        Raylib.DrawText("OPCIONES", 470, 120, 50, Color.White);

        btnFullscreen.Draw();
        btnVolver.Draw();

        Raylib.DrawText("Activa o desactiva pantalla completa", 405, 250, 24, Color.White);
    }

    public void Reset()
    {
        NextScreen = GameScreenType.Options;
    }
}