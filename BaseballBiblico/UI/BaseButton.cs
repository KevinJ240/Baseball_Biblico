using Raylib_cs;
using System.Numerics;

namespace BaseballBiblico.UI;

public class BaseButton
{
    public Vector2 Center;
    public float Radius;
    public string Text;
    public string Difficulty;

    public BaseButton(Vector2 center, float radius, string text, string difficulty)
    {
        Center = center;
        Radius = radius;
        Text = text;
        Difficulty = difficulty;
    }

    public bool IsHovered()
    {
        return Raylib.CheckCollisionPointCircle(Raylib.GetMousePosition(), Center, Radius);
    }

    public bool IsClicked()
    {
        return IsHovered() && Raylib.IsMouseButtonPressed(MouseButton.Left);
    }

    public void Draw()
    {
        if (IsHovered())
        {
            Raylib.DrawCircleV(Center, Radius + 10, new Color(255, 215, 0, 90));
            Raylib.DrawCircleLines((int)Center.X, (int)Center.Y, Radius + 10, Color.Gold);

            int textWidth = Raylib.MeasureText(Text, 24);
            Raylib.DrawText(Text, (int)(Center.X - textWidth / 2), (int)(Center.Y + 35), 24, Color.Black);
        }
    }
}