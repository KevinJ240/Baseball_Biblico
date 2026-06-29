using Raylib_cs;
using BaseballBiblico.UI;
using BaseballBiblico.Core;

namespace BaseballBiblico.Screens;

public class MenuScreen
{
    private readonly GameButton btnJugar;
    private readonly GameButton btnOpciones;
    private readonly GameButton btnSalir;

    public GameScreenType NextScreen { get; private set; } = GameScreenType.Menu;

    public MenuScreen()
    {
        btnJugar = new GameButton(520, 300, 240, 60, "JUGAR");
        btnOpciones = new GameButton(520, 380, 240, 60, "OPCIONES");
        btnSalir = new GameButton(520, 460, 240, 60, "SALIR");
    }

    public void Update()
    {
        if (btnJugar.IsClicked())
            NextScreen = GameScreenType.Game;

        if (btnOpciones.IsClicked())
            NextScreen = GameScreenType.Options;

        if (btnSalir.IsClicked())
            Environment.Exit(0);
    }

    public void Draw()
    {
        Raylib.DrawText("BASEBALL BIBLICO", 330, 100, 55, Color.DarkBlue);
        Raylib.DrawText("Videojuego de preguntas biblicas", 360, 180, 28, Color.DarkGreen);

        btnJugar.Draw();
        btnOpciones.Draw();
        btnSalir.Draw();

        Raylib.DrawText("Version 0.1", 560, 650, 20, Color.Gray);
    }

    public void Reset()
    {
        NextScreen = GameScreenType.Menu;
    }
}