using Raylib_cs;
using BaseballBiblico.UI;
using BaseballBiblico.Core;
using System.Numerics;

namespace BaseballBiblico.Screens;

public class OptionsScreen
{
    // Recursos gráficos
    private readonly Texture2D botonNormal;
    private readonly Texture2D botonHover;
    private readonly Font fuente;

    // Botones
    private readonly ImageButton btnFullscreen;
    private readonly ImageButton btnGuardar;
    private readonly ImageButton btnVolver;

    private readonly ImageButton btnRestarInning;
    private readonly ImageButton btnSumarInning;

    // Valores editables
    private string nombreEquipoA;
    private string nombreEquipoB;
    private int totalInnings;

    private bool editandoEquipoA;
    private bool editandoEquipoB;

    // Cuadros de texto
    private readonly Rectangle cajaEquipoA =
        new(420, 190, 440, 50);

    private readonly Rectangle cajaEquipoB =
        new(420, 280, 440, 50);

    private readonly Rectangle cajaInnings =
        new(570, 385, 140, 55);

    public GameScreenType NextScreen { get; private set; }
        = GameScreenType.Options;

    public OptionsScreen()
    {
        // Cargar imágenes de los botones
        botonNormal = Raylib.LoadTexture(
            "Assets/Images/Boton1.png"
        );

        botonHover = Raylib.LoadTexture(
            "Assets/Images/Boton2.png"
        );

        // Cargar fuente con caracteres en español
        fuente = FontManager.CargarFuenteEspanol(
            "Assets/Fonts/PatuaOne-Regular.ttf",
            40
        );

        // Cargar los valores actuales
        nombreEquipoA = GameSettings.NombreEquipoA;
        nombreEquipoB = GameSettings.NombreEquipoB;
        totalInnings = GameSettings.TotalInnings;

        // Botón para disminuir innings
        btnRestarInning = new ImageButton(
            new Rectangle(480, 385, 70, 55),
            botonNormal,
            botonHover,
            fuente,
            "-",
            30
        );

        // Botón para aumentar innings
        btnSumarInning = new ImageButton(
            new Rectangle(730, 385, 70, 55),
            botonNormal,
            botonHover,
            fuente,
            "+",
            30
        );

        btnFullscreen = new ImageButton(
            new Rectangle(470, 480, 340, 60),
            botonNormal,
            botonHover,
            fuente,
            "PANTALLA COMPLETA",
            22
        );

        btnGuardar = new ImageButton(
            new Rectangle(520, 560, 240, 60),
            botonNormal,
            botonHover,
            fuente,
            "GUARDAR",
            24
        );

        btnVolver = new ImageButton(
            new Rectangle(520, 640, 240, 60),
            botonNormal,
            botonHover,
            fuente,
            "VOLVER",
            24
        );
    }

    public void Update()
    {
        Vector2 mouse = ScreenScaler.GetVirtualMouse();

        if (Raylib.IsMouseButtonPressed(MouseButton.Left))
        {
            editandoEquipoA =
                Raylib.CheckCollisionPointRec(
                    mouse,
                    cajaEquipoA
                );

            editandoEquipoB =
                Raylib.CheckCollisionPointRec(
                    mouse,
                    cajaEquipoB
                );
        }

        LeerTexto();

        if (btnRestarInning.IsClicked())
        {
            totalInnings--;

            if (totalInnings < 1)
            {
                totalInnings = 1;
            }
        }

        if (btnSumarInning.IsClicked())
        {
            totalInnings++;

            if (totalInnings > 9)
            {
                totalInnings = 9;
            }
        }

        if (btnFullscreen.IsClicked())
        {
            Raylib.ToggleFullscreen();
        }

        if (btnGuardar.IsClicked())
        {
            GuardarConfiguracion();
        }

        if (btnVolver.IsClicked())
        {
            NextScreen = GameScreenType.Menu;
        }
    }

    public void Draw()
    {
        Raylib.ClearBackground(
            new Color(73, 138, 44, 255)
        );

        DibujarTextoCentrado(
            "OPCIONES",
            75,
            46,
            Color.White
        );

        Raylib.DrawTextEx(
            fuente,
            "Nombre Equipo A",
            new Vector2(420, 155),
            23,
            1,
            Color.White
        );

        DibujarCajaTexto(
            cajaEquipoA,
            nombreEquipoA,
            editandoEquipoA
        );

        Raylib.DrawTextEx(
            fuente,
            "Nombre Equipo B",
            new Vector2(420, 245),
            23,
            1,
            Color.White
        );

        DibujarCajaTexto(
            cajaEquipoB,
            nombreEquipoB,
            editandoEquipoB
        );

        DibujarTextoCentrado(
            "CANTIDAD DE INNINGS",
            345,
            24,
            Color.White
        );

        btnRestarInning.Draw();
        DibujarCajaInnings();
        btnSumarInning.Draw();

        DibujarTextoCentrado(
            "Mínimo 1 - Máximo 9",
            445,
            18,
            Color.White
        );

        btnFullscreen.Draw();
        btnGuardar.Draw();
        btnVolver.Draw();
    }

    public void Reset()
    {
        NextScreen = GameScreenType.Options;

        nombreEquipoA =
            GameSettings.NombreEquipoA;

        nombreEquipoB =
            GameSettings.NombreEquipoB;

        totalInnings =
            GameSettings.TotalInnings;

        editandoEquipoA = false;
        editandoEquipoB = false;
    }

    private void LeerTexto()
    {
        int codepoint = Raylib.GetCharPressed();

        while (codepoint > 0)
        {
            bool caracterValido =
                codepoint >= 32 &&
                codepoint != 127;

            if (caracterValido)
            {
                string caracter =
                    char.ConvertFromUtf32(codepoint);

                if (editandoEquipoA &&
                    nombreEquipoA.Length < 14)
                {
                    nombreEquipoA += caracter;
                }

                if (editandoEquipoB &&
                    nombreEquipoB.Length < 14)
                {
                    nombreEquipoB += caracter;
                }
            }

            codepoint = Raylib.GetCharPressed();
        }

        if (Raylib.IsKeyPressed(
                KeyboardKey.Backspace))
        {
            if (editandoEquipoA &&
                nombreEquipoA.Length > 0)
            {
                nombreEquipoA =
                    nombreEquipoA[..^1];
            }

            if (editandoEquipoB &&
                nombreEquipoB.Length > 0)
            {
                nombreEquipoB =
                    nombreEquipoB[..^1];
            }
        }

        if (Raylib.IsKeyPressed(
                KeyboardKey.Enter))
        {
            editandoEquipoA = false;
            editandoEquipoB = false;
        }
    }

    private void GuardarConfiguracion()
    {
        GameSettings.NombreEquipoA =
            string.IsNullOrWhiteSpace(nombreEquipoA)
                ? "EQUIPO A"
                : nombreEquipoA
                    .Trim()
                    .ToUpperInvariant();

        GameSettings.NombreEquipoB =
            string.IsNullOrWhiteSpace(nombreEquipoB)
                ? "EQUIPO B"
                : nombreEquipoB
                    .Trim()
                    .ToUpperInvariant();

        GameSettings.TotalInnings =
            Math.Clamp(totalInnings, 1, 9);

        nombreEquipoA =
            GameSettings.NombreEquipoA;

        nombreEquipoB =
            GameSettings.NombreEquipoB;

        totalInnings =
            GameSettings.TotalInnings;

        editandoEquipoA = false;
        editandoEquipoB = false;
    }

    private void DibujarCajaTexto(
        Rectangle rect,
        string texto,
        bool activo)
    {
        Color fondo = activo
            ? new Color(225, 225, 225, 255)
            : Color.White;

        Color borde = activo
            ? Color.Yellow
            : Color.Black;

        Raylib.DrawRectangleRec(
            rect,
            fondo
        );

        Raylib.DrawRectangleLinesEx(
            rect,
            3,
            borde
        );

        int tamaño = 22;

        Vector2 medida = Raylib.MeasureTextEx(
            fuente,
            texto,
            tamaño,
            1
        );

        float x = rect.X + 12;

        float y =
            rect.Y +
            (rect.Height - medida.Y) / 2f;

        Raylib.BeginScissorMode(
            (int)rect.X + 5,
            (int)rect.Y + 5,
            (int)rect.Width - 10,
            (int)rect.Height - 10
        );

        Raylib.DrawTextEx(
            fuente,
            texto,
            new Vector2(x, y),
            tamaño,
            1,
            Color.Black
        );

        if (activo &&
            ((int)(Raylib.GetTime() * 2) % 2 == 0))
        {
            float cursorX =
                x + medida.X + 3;

            Raylib.DrawLineEx(
                new Vector2(
                    cursorX,
                    rect.Y + 10
                ),
                new Vector2(
                    cursorX,
                    rect.Y + rect.Height - 10
                ),
                2,
                Color.Black
            );
        }

        Raylib.EndScissorMode();
    }

    private void DibujarCajaInnings()
    {
        Raylib.DrawRectangleRec(
            cajaInnings,
            Color.White
        );

        Raylib.DrawRectangleLinesEx(
            cajaInnings,
            3,
            Color.Black
        );

        string texto =
            totalInnings.ToString();

        float tamaño = 32;

        Vector2 medida = Raylib.MeasureTextEx(
            fuente,
            texto,
            tamaño,
            1
        );

        float x =
            cajaInnings.X +
            (cajaInnings.Width - medida.X) / 2f;

        float y =
            cajaInnings.Y +
            (cajaInnings.Height - medida.Y) / 2f;

        Raylib.DrawTextEx(
            fuente,
            texto,
            new Vector2(x, y),
            tamaño,
            1,
            Color.Black
        );
    }

    private void DibujarTextoCentrado(
        string texto,
        float y,
        float tamaño,
        Color color)
    {
        Vector2 medida = Raylib.MeasureTextEx(
            fuente,
            texto,
            tamaño,
            1
        );

        float x =
            (1280 - medida.X) / 2f;

        Raylib.DrawTextEx(
            fuente,
            texto,
            new Vector2(x, y),
            tamaño,
            1,
            color
        );
    }

    public void Unload()
    {
        if (botonNormal.Id > 0)
        {
            Raylib.UnloadTexture(
                botonNormal
            );
        }

        if (botonHover.Id > 0)
        {
            Raylib.UnloadTexture(
                botonHover
            );
        }

        if (fuente.Texture.Id > 0)
        {
            Raylib.UnloadFont(
                fuente
            );
        }
    }
}