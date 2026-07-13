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
    private readonly ImageButton btnCreditos;
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
            new Rectangle(500, 275, 280, 60),
            botonNormal,
            botonHover,
            fuente,
            "JUGAR",
            27
        );

        btnOpciones = new ImageButton(
            new Rectangle(500, 350, 280, 60),
            botonNormal,
            botonHover,
            fuente,
            "OPCIONES",
            27
        );

        btnCreditos = new ImageButton(
            new Rectangle(500, 425, 280, 60),
            botonNormal,
            botonHover,
            fuente,
            "CRÉDITOS",
            27
        );

        btnSalir = new ImageButton(
            new Rectangle(500, 500, 280, 60),
            botonNormal,
            botonHover,
            fuente,
            "SALIR",
            27
        );
    }

    public void Update()
    {
        if (btnJugar.IsClicked())
        {
            NextScreen = GameScreenType.Game;
            return;
        }

        if (btnOpciones.IsClicked())
        {
            NextScreen = GameScreenType.Options;
            return;
        }

        if (btnCreditos.IsClicked())
        {
            NextScreen = GameScreenType.Credits;
            return;
        }

        if (btnSalir.IsClicked())
        {
            Raylib.CloseWindow();
        }
    }

    public void Draw()
    {
        Raylib.ClearBackground(Color.Black);

        DibujarFondo();

        DibujarTextoCentrado(
            "BASEBALL BÍBLICO",
            80,
            55,
            Color.DarkBlue
        );

        DibujarTextoCentrado(
            "Videojuego de preguntas bíblicas",
            155,
            28,
            Color.DarkGreen
        );

        btnJugar.Draw();
        btnOpciones.Draw();
        btnCreditos.Draw();
        btnSalir.Draw();

        DibujarTextoCentrado(
            "Versión 0.1.0",
            665,
            20,
            Color.White
        );
    }

    private void DibujarFondo()
    {
        if (fondoMenu.Id <= 0)
        {
            return;
        }

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
                ScreenScaler.VirtualWidth,
                ScreenScaler.VirtualHeight
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

        if (botonNormal.Id > 0)
        {
            Raylib.UnloadTexture(botonNormal);
        }

        if (botonHover.Id > 0)
        {
            Raylib.UnloadTexture(botonHover);
        }

        if (fuente.Texture.Id > 0)
        {
            Raylib.UnloadFont(fuente);
        }
    }
}