using Raylib_cs;
using System.Numerics;
using BaseballBiblico.Entities;
using BaseballBiblico.Core;

namespace BaseballBiblico.UI;

public class QuestionPanel
{
    private readonly Font fuente;
    private readonly Rectangle panel;

    public QuestionPanel(Rectangle panel, Font fuente)
    {
        this.panel = panel;
        this.fuente = fuente;
    }

    public void Draw(Pregunta preguntaActual, string preguntaTexto, string dificultad)
    {
        Raylib.DrawRectangleRec(panel, Color.RayWhite);
        Raylib.DrawRectangleLinesEx(panel, 3, Color.Black);

        DrawCentered("PREGUNTA", panel.Y + 30, 34, Color.Black);

        Raylib.DrawLine(
            (int)(panel.X + 40),
            (int)(panel.Y + 95),
            (int)(panel.X + panel.Width - 40),
            (int)(panel.Y + 95),
            Color.LightGray
        );

        Rectangle areaPregunta = new(panel.X + 50, panel.Y + 130, panel.Width - 100, 90);
        DrawWrappedCentered(preguntaTexto, areaPregunta, 22, Color.Black);

        if (!string.IsNullOrWhiteSpace(dificultad))
            DrawCentered($"Dificultad: {dificultad}", panel.Y + 245, 22, Color.DarkBlue);

        DrawAnswers(preguntaActual);
    }

    private void DrawAnswers(Pregunta pregunta)
    {
        float botonW = 220;
        float botonH = 58;
        float espacioX = 35;
        float espacioY = 25;

        float totalW = botonW * 2 + espacioX;
        float startX = panel.X + (panel.Width - totalW) / 2f;

        float startY = panel.Y + 300;

        Rectangle r1 = new(startX, startY, botonW, botonH);
        Rectangle r2 = new(startX + botonW + espacioX, startY, botonW, botonH);
        Rectangle r3 = new(startX, startY + botonH + espacioY, botonW, botonH);
        Rectangle r4 = new(startX + botonW + espacioX, startY + botonH + espacioY, botonW, botonH);

        DrawAnswer(r1, GetAnswer(pregunta, 0));
        DrawAnswer(r2, GetAnswer(pregunta, 1));
        DrawAnswer(r3, GetAnswer(pregunta, 2));
        DrawAnswer(r4, GetAnswer(pregunta, 3));
    }

    private string GetAnswer(Pregunta pregunta, int index)
    {
        if (pregunta.Answers != null && pregunta.Answers.Length == 4)
            return pregunta.Answers[index];

        return "";
    }

    private void DrawAnswer(Rectangle rect, string text)
    {
        Vector2 mouse = ScreenScaler.GetVirtualMouse();
        bool hover = Raylib.CheckCollisionPointRec(mouse, rect);

        Raylib.DrawRectangleRec(rect, hover ? Color.SkyBlue : Color.White);
        Raylib.DrawRectangleLinesEx(rect, 2, Color.Black);

        DrawTextInsideRect(text, rect, 22, Color.Black);
    }

    private void DrawCentered(string text, float y, int fontSize, Color color)
    {
        Vector2 size = Raylib.MeasureTextEx(fuente, text, fontSize, 1);
        float x = panel.X + panel.Width / 3f - size.X / 3f;

        Raylib.DrawTextEx(fuente, text, new Vector2(x, y), fontSize, 1, color);
    }

    private void DrawTextInsideRect(string text, Rectangle rect, int fontSize, Color color)
    {
        Vector2 size = Raylib.MeasureTextEx(fuente, text, fontSize, 1);

        float x = rect.X + rect.Width / 3f - size.X / 2f;
        float y = rect.Y + rect.Height / 2f - size.Y / 2f;

        Raylib.DrawTextEx(fuente, text, new Vector2(x, y), fontSize, 1, color);
    }

    private void DrawWrappedCentered(string text, Rectangle area, int fontSize, Color color)
    {
        List<string> lines = WrapText(text, area.Width, fontSize);

        float lineHeight = fontSize + 8;
        float totalHeight = lines.Count * lineHeight;
        float startY = area.Y + area.Height / 2f - totalHeight / 2f;

        for (int i = 0; i < lines.Count; i++)
        {
            Vector2 size = Raylib.MeasureTextEx(fuente, lines[i], fontSize, 1);
            float x = area.X + area.Width / 30f - size.X / 10f;
            float y = startY + i * lineHeight;

            Raylib.DrawTextEx(fuente, lines[i], new Vector2(x, y), fontSize, 1, color);
        }
    }

    private List<string> WrapText(string text, float maxWidth, int fontSize)
    {
        List<string> lines = new();
        string[] words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        string current = "";

        foreach (string word in words)
        {
            string test = string.IsNullOrWhiteSpace(current) ? word : current + " " + word;
            Vector2 size = Raylib.MeasureTextEx(fuente, test, fontSize, 1);

            if (size.X > maxWidth && !string.IsNullOrWhiteSpace(current))
            {
                lines.Add(current);
                current = word;
            }
            else
            {
                current = test;
            }
        }

        if (!string.IsNullOrWhiteSpace(current))
            lines.Add(current);

        return lines;
    }
}