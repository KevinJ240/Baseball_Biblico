using Raylib_cs;
using System.Numerics;
using BaseballBiblico.Core;

namespace BaseballBiblico.UI;

public class BaseButton
{
    private readonly Vector2 center;
    private readonly float radius;

    private readonly string title;
    private readonly string difficulty;

    private readonly Font fuente;

    private readonly Color colorHover =
        new(190, 145, 55, 210);

    private readonly Color colorBorder =
        new(255, 215, 0, 255);

    private readonly Color colorText =
        new(255, 255, 255, 255);

    public BaseButton(
        Vector2 center,
        float radius,
        string title,
        string difficulty,
        Font fuente)
    {
        this.center = center;
        this.radius = radius;
        this.title = title;
        this.difficulty = difficulty;
        this.fuente = fuente;
    }

    public bool IsClicked()
    {
        Vector2 mouse =
            ScreenScaler.GetVirtualMouse();

        bool hover =
            Vector2.Distance(mouse, center) <= radius;

        return hover &&
               Raylib.IsMouseButtonPressed(
                   MouseButton.Left
               );
    }

    public void Draw()
    {
        Vector2 mouse =
            ScreenScaler.GetVirtualMouse();

        bool hover =
            Vector2.Distance(mouse, center) <= radius;

        // Cuando el mouse no está encima,
        // no se dibuja absolutamente nada.
        if (!hover)
            return;

        Raylib.DrawCircleV(
            center,
            radius,
            colorHover
        );

        Raylib.DrawCircleLines(
            (int)center.X,
            (int)center.Y,
            radius,
            colorBorder
        );

        Raylib.DrawCircleLines(
            (int)center.X,
            (int)center.Y,
            radius - 2,
            Color.Black
        );

        DrawCenteredText(
            title,
            center.Y - 13,
            17,
            colorText
        );

        DrawCenteredText(
            difficulty,
            center.Y + 7,
            12,
            colorText
        );
    }

    private void DrawCenteredText(
        string text,
        float y,
        float fontSize,
        Color color)
    {
        Vector2 measurement =
            Raylib.MeasureTextEx(
                fuente,
                text,
                fontSize,
                1
            );

        float x =
            center.X -
            measurement.X / 2f;

        Raylib.DrawTextEx(
            fuente,
            text,
            new Vector2(x, y),
            fontSize,
            1,
            color
        );
    }
}