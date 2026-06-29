using Raylib_cs;
using System.Numerics;

namespace BaseballBiblico.Core;

public static class ScreenScaler
{
    public const int VirtualWidth = 1280;
    public const int VirtualHeight = 720;

    public static Rectangle DestinationRect { get; private set; }

    public static void Update()
    {
        float scale = Math.Min(
            Raylib.GetScreenWidth() / (float)VirtualWidth,
            Raylib.GetScreenHeight() / (float)VirtualHeight
        );

        float width = VirtualWidth * scale;
        float height = VirtualHeight * scale;

        float x = (Raylib.GetScreenWidth() - width) / 2f;
        float y = (Raylib.GetScreenHeight() - height) / 2f;

        DestinationRect = new Rectangle(x, y, width, height);
    }

    public static Vector2 GetVirtualMouse()
    {
        Vector2 mouse = Raylib.GetMousePosition();

        float scaleX = DestinationRect.Width / VirtualWidth;
        float scaleY = DestinationRect.Height / VirtualHeight;

        return new Vector2(
            (mouse.X - DestinationRect.X) / scaleX,
            (mouse.Y - DestinationRect.Y) / scaleY
        );
    }
}