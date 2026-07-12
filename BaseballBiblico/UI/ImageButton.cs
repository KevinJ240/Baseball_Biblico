using Raylib_cs;
using System.Numerics;
using BaseballBiblico.Core;

namespace BaseballBiblico.UI;

public class ImageButton
{
    private readonly Rectangle rect;
    private readonly Texture2D texturaNormal;
    private readonly Texture2D texturaHover;
    private readonly Font fuente;
    private readonly string texto;
    private readonly float fontSize;

    private bool hover;
    private bool presionado;

    public ImageButton(
        Rectangle rect,
        Texture2D texturaNormal,
        Texture2D texturaHover,
        Font fuente,
        string texto,
        float fontSize = 28)
    {
        this.rect = rect;
        this.texturaNormal = texturaNormal;
        this.texturaHover = texturaHover;
        this.fuente = fuente;
        this.texto = texto;
        this.fontSize = fontSize;
    }

    public bool IsClicked()
    {
        Vector2 mouse = ScreenScaler.GetVirtualMouse();

        hover = Raylib.CheckCollisionPointRec(mouse, rect);
        presionado = hover && Raylib.IsMouseButtonDown(MouseButton.Left);

        return hover &&
               Raylib.IsMouseButtonReleased(MouseButton.Left);
    }

    public void Draw()
    {
        Texture2D texturaActual =
            hover || presionado
                ? texturaHover
                : texturaNormal;

        Raylib.DrawTexturePro(
            texturaActual,
            new Rectangle(
                0,
                0,
                texturaActual.Width,
                texturaActual.Height
            ),
            rect,
            Vector2.Zero,
            0,
            Color.White
        );

        Vector2 medida = Raylib.MeasureTextEx(
            fuente,
            texto,
            fontSize,
            1
        );

        float offsetY = presionado ? 3f : 0f;

        float x = rect.X + (rect.Width - medida.X) / 2f;
        float y = rect.Y + (rect.Height - medida.Y) / 2f + offsetY;

        Raylib.DrawTextEx(
            fuente,
            texto,
            new Vector2(x, y),
            fontSize,
            1,
            Color.Black
        );
    }
}