using Raylib_cs;
using System.Numerics;
using BaseballBiblico.Core;

namespace BaseballBiblico.UI;

public class ScoreBoardPanel
{
    public int EquipoA { get; set; }
    public int EquipoB { get; set; }
    public int Inning { get; set; } = 1;
    public int Strikes { get; set; }
    public int Outs { get; set; }
    public string Turno { get; set; } = "A";

    private readonly Rectangle board = new(25, 20, 1230, 150);

    private readonly Color fondo = new(25, 25, 25, 255);
    private readonly Color borde = new(85, 85, 85, 255);
    private readonly Color rojoLed = new(255, 25, 18, 255);
    private readonly Color texto = new(235, 235, 235, 255);
    private readonly Color amarillo = new(255, 215, 0, 255);

    public void Draw()
    {
        DrawBoardBase();

        Rectangle leftPanel = new(board.X + 10, board.Y + 10, 380, board.Height - 20);
        Rectangle centerPanel = new(board.X + 400, board.Y + 10, 430, board.Height - 20);
        Rectangle rightPanel = new(board.X + 840, board.Y + 10, 380, board.Height - 20);

        DrawTeamPanel(leftPanel, GameSettings.NombreEquipoA, EquipoA);
        DrawCenterPanel(centerPanel);
        DrawTeamPanel(rightPanel, GameSettings.NombreEquipoB, EquipoB);

        DrawStrikePanel(leftPanel);
        DrawOutPanel(rightPanel);
    }

    private void DrawBoardBase()
    {
        Raylib.DrawRectangleRounded(board, 0.03f, 12, fondo);
        Raylib.DrawRectangleRoundedLinesEx(board, 0.03f, 12, 4, borde);

        Raylib.DrawLineEx(
            new Vector2(board.X + 390, board.Y + 12),
            new Vector2(board.X + 390, board.Y + board.Height - 12),
            2,
            borde
        );

        Raylib.DrawLineEx(
            new Vector2(board.X + 840, board.Y + 12),
            new Vector2(board.X + 840, board.Y + board.Height - 12),
            2,
            borde
        );
    }

    private void DrawTeamPanel(Rectangle rect, string title, int score)
    {
        DrawCenteredText(title, rect, 16, 28, texto);
        DrawCenteredText(score.ToString(), rect, 52, 58, rojoLed);
    }

    private void DrawCenterPanel(Rectangle rect)
    {
        DrawCenteredText("INNING", rect, 16, 28, texto);
        DrawCenteredText(Inning.ToString(), rect, 52, 58, rojoLed);

        string nombreTurno = Turno == "A"
            ? GameSettings.NombreEquipoA
            : GameSettings.NombreEquipoB;

        string turnoText = $"TURNO: {nombreTurno}";

        DrawCenteredText(turnoText, rect, 105, 22, amarillo);
    }

    private void DrawStrikePanel(Rectangle leftPanel)
    {
        int labelSize = 24;
        string label = "STRIKE";
        int labelW = Raylib.MeasureText(label, labelSize);

        int y = (int)(leftPanel.Y + 100);
        int labelX = (int)(leftPanel.X + leftPanel.Width / 2 - 110);

        Raylib.DrawText(label, labelX, y, labelSize, texto);

        int lightsX = labelX + labelW + 25;
        DrawLightGroup(lightsX, y + 13, Strikes);
    }

    private void DrawOutPanel(Rectangle rightPanel)
    {
        int labelSize = 24;
        string label = "OUT";
        int labelW = Raylib.MeasureText(label, labelSize);

        int y = (int)(rightPanel.Y + 100);
        int labelX = (int)(rightPanel.X + rightPanel.Width / 2 - 80);

        Raylib.DrawText(label, labelX, y, labelSize, texto);

        int lightsX = labelX + labelW + 25;
        DrawLightGroup(lightsX, y + 13, Outs);
    }

    private void DrawLightGroup(int startX, int centerY, int activeCount)
    {
        for (int i = 0; i < 3; i++)
        {
            int x = startX + i * 35;

            Color lightColor = i < activeCount
                ? new Color(255, 0, 0, 255)
                : new Color(18, 18, 18, 255);

            Raylib.DrawCircle(x, centerY, 11, new Color(5, 5, 5, 255));
            Raylib.DrawCircle(x, centerY, 8, lightColor);
            Raylib.DrawCircleLines(x, centerY, 11, Color.Black);
        }
    }

    private void DrawCenteredText(string value, Rectangle rect, int yOffset, int fontSize, Color color)
    {
        int w = Raylib.MeasureText(value, fontSize);

        Raylib.DrawText(
            value,
            (int)(rect.X + rect.Width / 2 - w / 2),
            (int)(rect.Y + yOffset),
            fontSize,
            color
        );
    }
}