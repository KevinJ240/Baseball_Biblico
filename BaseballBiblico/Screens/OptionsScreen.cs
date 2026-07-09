using Raylib_cs;
using BaseballBiblico.UI;
using BaseballBiblico.Core;
using System.Numerics;

namespace BaseballBiblico.Screens;

public class OptionsScreen
{
    private readonly GameButton btnFullscreen;
    private readonly GameButton btnGuardar;
    private readonly GameButton btnVolver;

    private string nombreEquipoA = GameSettings.NombreEquipoA;
    private string nombreEquipoB = GameSettings.NombreEquipoB;

    private bool editandoEquipoA = false;
    private bool editandoEquipoB = false;

    private readonly Rectangle cajaEquipoA = new(420, 230, 440, 50);
    private readonly Rectangle cajaEquipoB = new(420, 330, 440, 50);

    public GameScreenType NextScreen { get; private set; } = GameScreenType.Options;

    public OptionsScreen()
    {
        btnFullscreen = new GameButton(470, 430, 340, 60, "PANTALLA COMPLETA");
        btnGuardar = new GameButton(520, 510, 240, 60, "GUARDAR");
        btnVolver = new GameButton(520, 590, 240, 60, "VOLVER");
    }

    public void Update()
    {
        Vector2 mouse = Raylib.GetMousePosition();

        if (Raylib.IsMouseButtonPressed(MouseButton.Left))
        {
            editandoEquipoA = Raylib.CheckCollisionPointRec(mouse, cajaEquipoA);
            editandoEquipoB = Raylib.CheckCollisionPointRec(mouse, cajaEquipoB);
        }

        LeerTexto();

        if (btnFullscreen.IsClicked())
            Raylib.ToggleFullscreen();

        if (btnGuardar.IsClicked())
            GuardarNombres();

        if (btnVolver.IsClicked())
            NextScreen = GameScreenType.Menu;
    }

    public void Draw()
    {
        Raylib.ClearBackground(new Color(73, 138, 44, 255));

        Raylib.DrawText("OPCIONES", 470, 80, 50, Color.White);

        Raylib.DrawText("Nombre Equipo A", 420, 200, 24, Color.White);
        DibujarCajaTexto(cajaEquipoA, nombreEquipoA, editandoEquipoA);

        Raylib.DrawText("Nombre Equipo B", 420, 300, 24, Color.White);
        DibujarCajaTexto(cajaEquipoB, nombreEquipoB, editandoEquipoB);

        Raylib.DrawText("Activa o desactiva pantalla completa", 405, 390, 24, Color.White);

        btnFullscreen.Draw();
        btnGuardar.Draw();
        btnVolver.Draw();
    }

    public void Reset()
    {
        NextScreen = GameScreenType.Options;

        nombreEquipoA = GameSettings.NombreEquipoA;
        nombreEquipoB = GameSettings.NombreEquipoB;

        editandoEquipoA = false;
        editandoEquipoB = false;
    }

    private void LeerTexto()
    {
        int key = Raylib.GetCharPressed();

        while (key > 0)
        {
            if (key >= 32 && key <= 126)
            {
                if (editandoEquipoA && nombreEquipoA.Length < 14)
                    nombreEquipoA += (char)key;

                if (editandoEquipoB && nombreEquipoB.Length < 14)
                    nombreEquipoB += (char)key;
            }

            key = Raylib.GetCharPressed();
        }

        if (Raylib.IsKeyPressed(KeyboardKey.Backspace))
        {
            if (editandoEquipoA && nombreEquipoA.Length > 0)
                nombreEquipoA = nombreEquipoA[..^1];

            if (editandoEquipoB && nombreEquipoB.Length > 0)
                nombreEquipoB = nombreEquipoB[..^1];
        }
    }

    private void GuardarNombres()
    {
        GameSettings.NombreEquipoA = string.IsNullOrWhiteSpace(nombreEquipoA)
            ? "EQUIPO A"
            : nombreEquipoA.Trim().ToUpper();

        GameSettings.NombreEquipoB = string.IsNullOrWhiteSpace(nombreEquipoB)
            ? "EQUIPO B"
            : nombreEquipoB.Trim().ToUpper();

        nombreEquipoA = GameSettings.NombreEquipoA;
        nombreEquipoB = GameSettings.NombreEquipoB;
    }

    private void DibujarCajaTexto(Rectangle rect, string texto, bool activo)
    {
        Raylib.DrawRectangleRec(rect, activo ? Color.LightGray : Color.White);
        Raylib.DrawRectangleLinesEx(rect, 2, activo ? Color.Yellow : Color.Black);

        Raylib.DrawText(
            texto,
            (int)rect.X + 12,
            (int)rect.Y + 13,
            24,
            Color.Black
        );
    }
}