using Raylib_cs;
using System.Numerics;
using BaseballBiblico.Core;

namespace BaseballBiblico.UI;

public class GameButton
{
    public Rectangle Rect;
    public string Text;

    public GameButton(float x, float y, float width, float height, string text)
    {
        Rect = new Rectangle(x, y, width, height);
        Text = text;
    }

    public bool IsClicked()
    {
        Vector2 mouse = ScreenScaler.GetVirtualMouse();

        return Raylib.CheckCollisionPointRec(mouse, Rect)
               && Raylib.IsMouseButtonPressed(MouseButton.Left);
    }

    public void Draw()
    {
        Vector2 mouse = ScreenScaler.GetVirtualMouse();
        bool hover = Raylib.CheckCollisionPointRec(mouse, Rect);

        Color buttonColor = hover ? Color.SkyBlue : Color.LightGray;

        Raylib.DrawRectangleRec(Rect, buttonColor);
        Raylib.DrawRectangleLinesEx(Rect, 3, Color.DarkBlue);

        int textWidth = Raylib.MeasureText(Text, 28);
        int xText = (int)(Rect.X + Rect.Width / 2 - textWidth / 2);
        int yText = (int)(Rect.Y + Rect.Height / 2 - 14);

        Raylib.DrawText(Text, xText, yText, 28, Color.Black);
    }
}