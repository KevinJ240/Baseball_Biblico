using Raylib_cs;
using System.Numerics;
using BaseballBiblico.UI;
using BaseballBiblico.Core;

namespace BaseballBiblico.Screens;

public class MenuScreen
{
    private readonly GameButton btnJugar;
    private readonly GameButton btnOpciones;
    private readonly GameButton btnSalir;

    private readonly Texture2D fondoMenu;
    private readonly Font fuente;

    public GameScreenType NextScreen { get; private set; }
        = GameScreenType.Menu;

    public MenuScreen()
    {
        fondoMenu = Raylib.LoadTexture(
            "Assets/Images/FondoMenu.png"
        );

        fuente = FontManager.CargarFuenteEspanol(
            "Assets/Fonts/PatuaOne-Regular.ttf",
            40
        );

        btnJugar = new GameButton(
            520,
            300,
            240,
            60,
            "JUGAR"
        );

        btnOpciones = new GameButton(
            520,
            380,
            240,
            60,
            "OPCIONES"
        );

        btnSalir = new GameButton(
            520,
            460,
            240,
            60,
            "SALIR"
        );
    }

    public void Update()
    {
        if (btnJugar.IsClicked())
        {
            NextScreen = GameScreenType.Game;
        }

        if (btnOpciones.IsClicked())
        {
            NextScreen = GameScreenType.Options;
        }

        if (btnSalir.IsClicked())
        {
            Environment.Exit(0);
        }
    }

    public void Draw()
    {
        Raylib.ClearBackground(Color.Black);

        DibujarFondo();

        DibujarTextoCentrado(
            "BASEBALL BÍBLICO",
            100,
            55,
            Color.DarkBlue
        );

        DibujarTextoCentrado(
            "Videojuego de preguntas bíblicas",
            180,
            28,
            Color.DarkGreen
        );

        btnJugar.Draw();
        btnOpciones.Draw();
        btnSalir.Draw();

        DibujarTextoCentrado(
            "Versión 0.1",
            650,
            20,
            Color.Gray
        );
    }

    private void DibujarFondo()
    {
        Rectangle origen = new(
            0,
            0,
            fondoMenu.Width,
            fondoMenu.Height
        );

        Rectangle destino = new(
            0,
            0,
            ScreenScaler.VirtualWidth,
            ScreenScaler.VirtualHeight
        );

        Raylib.DrawTexturePro(
            fondoMenu,
            origen,
            destino,
            Vector2.Zero,
            0,
            Color.White
        );
    }

    private void DibujarTextoCentrado(
        string texto,
        float y,
        float tamano,
        Color color)
    {
        Vector2 medida = Raylib.MeasureTextEx(
            fuente,
            texto,
            tamano,
            1
        );

        float x =
            (ScreenScaler.VirtualWidth - medida.X) / 2f;

        Raylib.DrawTextEx(
            fuente,
            texto,
            new Vector2(x, y),
            tamano,
            1,
            color
        );
    }

    public void Reset()
    {
        NextScreen = GameScreenType.Menu;
    }

    public void Unload()
    {
        if (fondoMenu.Id > 0)
        {
            Raylib.UnloadTexture(fondoMenu);
        }

        if (fuente.Texture.Id > 0)
        {
            Raylib.UnloadFont(fuente);
        }
    }
}