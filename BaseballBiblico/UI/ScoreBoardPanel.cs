using Raylib_cs;
using System.Numerics;
using BaseballBiblico.Core;

namespace BaseballBiblico.UI;

public class ScoreBoardPanel
{
    private readonly Font fuente;

    public int EquipoA { get; set; }
    public int EquipoB { get; set; }
    public int Inning { get; set; } = 1;
    public int Strikes { get; set; }
    public int Outs { get; set; }
    public string Turno { get; set; } = "A";

    private readonly Rectangle board =
        new(25, 20, 1230, 150);

    private readonly Color fondo =
        new(25, 25, 25, 255);

    private readonly Color borde =
        new(85, 85, 85, 255);

    private readonly Color rojoLed =
        new(255, 25, 18, 255);

    private readonly Color texto =
        new(235, 235, 235, 255);

    private readonly Color amarillo =
        new(255, 215, 0, 255);

    public ScoreBoardPanel(Font fuente)
    {
        this.fuente = fuente;
    }

    public void Draw()
    {
        DrawBoardBase();

        Rectangle leftPanel = new(
            board.X + 10,
            board.Y + 10,
            380,
            board.Height - 20
        );

        Rectangle centerPanel = new(
            board.X + 400,
            board.Y + 10,
            430,
            board.Height - 20
        );

        Rectangle rightPanel = new(
            board.X + 840,
            board.Y + 10,
            380,
            board.Height - 20
        );

        DrawTeamPanel(
            leftPanel,
            GameSettings.NombreEquipoA,
            EquipoA
        );

        DrawCenterPanel(centerPanel);

        DrawTeamPanel(
            rightPanel,
            GameSettings.NombreEquipoB,
            EquipoB
        );

        DrawStrikePanel(leftPanel);
        DrawOutPanel(rightPanel);
    }

    private void DrawBoardBase()
    {
        Raylib.DrawRectangleRounded(
            board,
            0.03f,
            12,
            fondo
        );

        Raylib.DrawRectangleRoundedLinesEx(
            board,
            0.03f,
            12,
            4,
            borde
        );

        Raylib.DrawLineEx(
            new Vector2(
                board.X + 390,
                board.Y + 12
            ),
            new Vector2(
                board.X + 390,
                board.Y + board.Height - 12
            ),
            2,
            borde
        );

        Raylib.DrawLineEx(
            new Vector2(
                board.X + 840,
                board.Y + 12
            ),
            new Vector2(
                board.X + 840,
                board.Y + board.Height - 12
            ),
            2,
            borde
        );
    }

    private void DrawTeamPanel(
        Rectangle rect,
        string title,
        int score)
    {
        DrawCenteredText(
            title,
            rect,
            15,
            27,
            texto
        );

        DrawCenteredText(
            score.ToString(),
            rect,
            50,
            52,
            rojoLed
        );
    }

    private void DrawCenterPanel(Rectangle rect)
    {
        DrawCenteredText(
            "INNING",
            rect,
            15,
            27,
            texto
        );

        DrawCenteredText(
            Inning.ToString(),
            rect,
            50,
            52,
            rojoLed
        );

        string nombreTurno =
            Turno == "A"
                ? GameSettings.NombreEquipoA
                : GameSettings.NombreEquipoB;

        string turnoText =
            $"TURNO: {nombreTurno}";

        DibujarTextoCentrado(
            turnoText,
            rect,
            rect.Y + 103,
            21,
            amarillo
        );
    }

    private void DrawStrikePanel(Rectangle leftPanel)
    {
        const float fontSize = 23;

        string label = "STRIKE";

        Vector2 medida = Raylib.MeasureTextEx(
            fuente,
            label,
            fontSize,
            1
        );

        float y = leftPanel.Y + 99;

        float labelX =
            leftPanel.X +
            leftPanel.Width / 2f -
            110;

        Raylib.DrawTextEx(
            fuente,
            label,
            new Vector2(labelX, y),
            fontSize,
            1,
            texto
        );

        int lightsX =
            (int)(labelX + medida.X + 25);

        DrawLightGroup(
            lightsX,
            (int)y + 13,
            Strikes
        );
    }

    private void DrawOutPanel(Rectangle rightPanel)
    {
        const float fontSize = 23;

        string label = "OUT";

        Vector2 medida = Raylib.MeasureTextEx(
            fuente,
            label,
            fontSize,
            1
        );

        float y = rightPanel.Y + 99;

        float labelX =
            rightPanel.X +
            rightPanel.Width / 2f -
            80;

        Raylib.DrawTextEx(
            fuente,
            label,
            new Vector2(labelX, y),
            fontSize,
            1,
            texto
        );

        int lightsX =
            (int)(labelX + medida.X + 25);

        DrawLightGroup(
            lightsX,
            (int)y + 13,
            Outs
        );
    }

    private void DrawLightGroup(
        int startX,
        int centerY,
        int activeCount)
    {
        for (int i = 0; i < 3; i++)
        {
            int x = startX + i * 35;

            Color lightColor =
                i < activeCount
                    ? new Color(255, 0, 0, 255)
                    : new Color(18, 18, 18, 255);

            Raylib.DrawCircle(
                x,
                centerY,
                11,
                new Color(5, 5, 5, 255)
            );

            Raylib.DrawCircle(
                x,
                centerY,
                8,
                lightColor
            );

            Raylib.DrawCircleLines(
                x,
                centerY,
                11,
                Color.Black
            );
        }
    }

    private void DrawCenteredText(
        string value,
        Rectangle rect,
        float yOffset,
        float fontSize,
        Color color)
    {
        Vector2 medida = Raylib.MeasureTextEx(
            fuente,
            value,
            fontSize,
            1
        );

        float x =
            rect.X +
            (rect.Width - medida.X) / 2f;

        float y =
            rect.Y + yOffset;

        Raylib.DrawTextEx(
            fuente,
            value,
            new Vector2(x, y),
            fontSize,
            1,
            color
        );
    }

    private void DibujarTextoCentrado(
        string value,
        Rectangle rect,
        float y,
        float fontSize,
        Color color)
    {
        Vector2 medida = Raylib.MeasureTextEx(
            fuente,
            value,
            fontSize,
            1
        );

        while (medida.X > rect.Width - 30 &&
               fontSize > 13)
        {
            fontSize--;

            medida = Raylib.MeasureTextEx(
                fuente,
                value,
                fontSize,
                1
            );
        }

        float x =
            rect.X +
            (rect.Width - medida.X) / 2f;

        Raylib.DrawTextEx(
            fuente,
            value,
            new Vector2(x, y),
            fontSize,
            1,
            color
        );
    }
}