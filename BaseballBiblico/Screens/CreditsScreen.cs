using Raylib_cs;
using System.Numerics;
using BaseballBiblico.UI;
using BaseballBiblico.Core;

namespace BaseballBiblico.Screens;

public class CreditsScreen
{
    private readonly Texture2D fondoMenu;
    private readonly Texture2D botonNormal;
    private readonly Texture2D botonHover;
    private readonly Font fuente;

    private readonly ImageButton btnVolver;

    private readonly Rectangle panelCreditos =
        new(315, 105, 650, 515);

    public GameScreenType NextScreen { get; private set; }
        = GameScreenType.Credits;

    public CreditsScreen()
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

        btnVolver = new ImageButton(
            new Rectangle(500, 635, 280, 58),
            botonNormal,
            botonHover,
            fuente,
            "VOLVER",
            25
        );
    }

    public void Update()
    {
        if (btnVolver.IsClicked())
        {
            NextScreen = GameScreenType.Menu;
        }
    }

    public void Draw()
    {
        Raylib.ClearBackground(Color.Black);

        DibujarFondo();
        DibujarPanel();

        btnVolver.Draw();
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

    private void DibujarPanel()
    {
        // Fondo semitransparente para facilitar la lectura.
        Raylib.DrawRectangleRounded(
            panelCreditos,
            0.05f,
            12,
            new Color(245, 245, 245, 240)
        );

        Raylib.DrawRectangleRoundedLinesEx(
            panelCreditos,
            0.05f,
            12,
            4,
            Color.Black
        );

        DibujarTextoCentradoEnPanel(
            "CRÉDITOS",
            panelCreditos.Y + 25,
            42,
            Color.DarkBlue
        );

        Raylib.DrawLine(
            (int)(panelCreditos.X + 45),
            (int)(panelCreditos.Y + 85),
            (int)(panelCreditos.X + panelCreditos.Width - 45),
            (int)(panelCreditos.Y + 85),
            Color.Gray
        );

        DibujarTituloSeccion(
            "DESARROLLO Y PROGRAMACIÓN",
            panelCreditos.Y + 110
        );

        DibujarNombre(
            "Kevin Aguilera",
            panelCreditos.Y + 143
        );

        DibujarTituloSeccion(
            "DISEÑO DEL JUEGO",
            panelCreditos.Y + 190
        );

        DibujarNombre(
            "Kevin Aguilera",
            panelCreditos.Y + 223
        );

        DibujarTituloSeccion(
            "DISEÑO GRÁFICO",
            panelCreditos.Y + 270
        );

        DibujarNombre(
            "Kevin Aguilera",
            panelCreditos.Y + 303
        );

        DibujarTituloSeccion(
            "BANCO DE PREGUNTAS BÍBLICAS",
            panelCreditos.Y + 350
        );

        DibujarNombre(
            "Wilmer Mejía",
            panelCreditos.Y + 383
        );

        DibujarTextoCentradoEnPanel(
            "Desarrollado con C#, .NET 8 y Raylib-cs",
            panelCreditos.Y + 435,
            18,
            Color.DarkGray
        );

        DibujarTextoCentradoEnPanel(
            "Baseball Bíblico - Versión 0.1.0",
            panelCreditos.Y + 470,
            18,
            Color.DarkGray
        );
    }

    private void DibujarTituloSeccion(
        string texto,
        float y)
    {
        DibujarTextoCentradoEnPanel(
            texto,
            y,
            20,
            Color.DarkGreen
        );
    }

    private void DibujarNombre(
        string texto,
        float y)
    {
        DibujarTextoCentradoEnPanel(
            texto,
            y,
            25,
            Color.Black
        );
    }

    private void DibujarTextoCentradoEnPanel(
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
            panelCreditos.X +
            (panelCreditos.Width - medida.X) / 2f;

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
        NextScreen = GameScreenType.Credits;
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