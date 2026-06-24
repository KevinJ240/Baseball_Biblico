using Raylib_cs;
using System.Numerics;

namespace BaseballBiblico.UI;

public class ScoreBoardPanel
{
    private Texture2D marcador;

    public int EquipoA { get; set; }
    public int EquipoB { get; set; }
    public int Inning { get; set; } = 1;
    public int Strikes { get; set; }
    public int Outs { get; set; }
    public string Turno { get; set; } = "A";

    private readonly Rectangle destino = new(25, 20, 1230, 150);

    public ScoreBoardPanel()
    {
        marcador = Raylib.LoadTexture("Assets/Images/Marcador.png");
    }

    public void Draw()
    {
        if (marcador.Id > 0)
        {
            Raylib.DrawTexturePro(
                marcador,
                new Rectangle(0, 0, marcador.Width, marcador.Height),
                destino,
                Vector2.Zero,
                0,
                Color.White
            );
        }
        else
        {
            Raylib.DrawRectangleRec(destino, new Color(25, 25, 25, 255));
            Raylib.DrawRectangleLinesEx(destino, 3, Color.Gray);
        }

        DrawLabels();
        DrawNumbers();
        DrawLights();
    }

    private void DrawLabels()
    {
        Raylib.DrawText("EQUIPO A", 95, 38, 28, Color.White);
        Raylib.DrawText("INNING", 570, 38, 28, Color.White);
        Raylib.DrawText("EQUIPO B", 965, 38, 28, Color.White);

        Raylib.DrawText("STRIKE", 350, 130, 24, Color.White);
        Raylib.DrawText("OUT", 820, 130, 24, Color.White);

        Raylib.DrawText($"TURNO: EQUIPO {Turno}", 535, 130, 22, Color.Gold);
    }

    private void DrawNumbers()
    {
        Color rojoLed = new(255, 30, 20, 255);

        Raylib.DrawText(EquipoA.ToString(), 180, 70, 64, rojoLed);
        Raylib.DrawText(Inning.ToString(), 605, 70, 64, rojoLed);
        Raylib.DrawText(EquipoB.ToString(), 1045, 70, 64, rojoLed);
    }

    private void DrawLights()
    {
        DrawLightGroup(450, 142, Strikes);
        DrawLightGroup(875, 142, Outs);
    }

    private void DrawLightGroup(int startX, int y, int activeCount)
    {
        for (int i = 0; i < 3; i++)
        {
            Color color = i < activeCount
                ? new Color(255, 0, 0, 255)
                : new Color(35, 35, 35, 255);

            Raylib.DrawCircle(startX + i * 35, y, 10, color);
            Raylib.DrawCircleLines(startX + i * 35, y, 10, Color.Black);
        }
    }
} 