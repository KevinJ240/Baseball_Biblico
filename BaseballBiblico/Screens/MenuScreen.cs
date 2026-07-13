using Raylib_cs;
using System.Numerics;
using BaseballBiblico.UI;
using BaseballBiblico.Core;

namespace BaseballBiblico.Screens;

public class MenuScreen
{
    private readonly Texture2D fondoMenu;
    private readonly Texture2D botonNormal;
    private readonly Texture2D botonHover;
    private readonly Font fuente;

    private readonly ImageButton btnJugar;
    private readonly ImageButton btnOpciones;
    private readonly ImageButton btnSalir;

    public GameScreenType NextScreen { get; private set; }
        = GameScreenType.Menu;

    public MenuScreen()
    {
        fondoMenu = Raylib.LoadTexture(
            "Assets/Images/FondoMenu.png"
        );

        botonNormal = Raylib.LoadTexture(
            "Assets/Images/Boton1.png"
        );

        botonHover = Raylib.LoadTexture(
            "Assets/Images/Boton2.png"
        );

        fuente = FontManager.CargarFuenteEspanol(
            "Assets/Fonts/PatuaOne-Regular.ttf",
            40
        );

        btnJugar = new ImageButton(
            new Rectangle(500, 300, 280, 65),
            botonNormal,
            botonHover,
            fuente,
            "JUGAR",
            28
        );

        btnOpciones = new ImageButton(
            new Rectangle(500, 385, 280, 65),
            botonNormal,
            botonHover,
            fuente,
            "OPCIONES",
            28
        );

        btnSalir = new ImageButton(
            new Rectangle(500, 470, 280, 65),
            botonNormal,
            botonHover,
            fuente,
            "SALIR",
            28
        );
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
        Raylib.ClearBackground(Color.Black);

        DibujarFondo();

        DibujarTextoCentrado(
            "BASEBALL BÍBLICO",
            90,
            55,
            Color.DarkBlue
        );

        DibujarTextoCentrado(
            "Videojuego de preguntas bíblicas",
            165,
            28,
            Color.DarkGreen
        );

        btnJugar.Draw();
        btnOpciones.Draw();
        btnSalir.Draw();

        DibujarTextoCentrado(
            "Versión 0.1.0",
            650,
            20,
            Color.White
        );
    }

    private void DibujarFondo()
    {
        Raylib.DrawTexturePro(
            fondoMenu,
            new Rectangle(
                0,
                0,
                fondoMenu.Width,
                fondoMenu.Height
            ),
            new Rectangle(
                0,
                0,
                1280,
                720
            ),
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

        float x = (1280 - medida.X) / 2f;

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
            Raylib.UnloadTexture(fondoMenu);

        if (botonNormal.Id > 0)
            Raylib.UnloadTexture(botonNormal);

        if (botonHover.Id > 0)
            Raylib.UnloadTexture(botonHover);

        if (fuente.Texture.Id > 0)
            Raylib.UnloadFont(fuente);
    }
}