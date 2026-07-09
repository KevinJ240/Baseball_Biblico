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

    public void Draw(
        Pregunta preguntaActual,
        string preguntaTexto,
        string dificultad,
        bool mostrarResultado = false,
        int respuestaSeleccionada = -1)
    {
        Raylib.DrawRectangleRec(panel, Color.RayWhite);
        Raylib.DrawRectangleLinesEx(panel, 3, Color.Black);

        DrawCentered("PREGUNTA", panel.Y + UILayout.QuestionTitleY, 34, Color.Black);

        Raylib.DrawLine(
            (int)(panel.X + 40),
            (int)(panel.Y + UILayout.QuestionLineY),
            (int)(panel.X + panel.Width - 40),
            (int)(panel.Y + UILayout.QuestionLineY),
            Color.LightGray
        );

        Rectangle areaPregunta = new(
            panel.X + UILayout.QuestionPaddingX,
            panel.Y + UILayout.QuestionTextY,
            panel.Width - (UILayout.QuestionPaddingX * 2) - 80,
            UILayout.QuestionAreaHeight
        );

        DrawWrappedCentered(preguntaTexto, areaPregunta, 20, Color.Black);

        if (!string.IsNullOrWhiteSpace(dificultad))
            DrawCentered($"Dificultad: {dificultad}", panel.Y + UILayout.DifficultyY, 22, Color.DarkBlue);

        if (preguntaActual.Id > 0 &&
            preguntaActual.Answers != null &&
            preguntaActual.Answers.Length > 0)
        {
            DrawAnswers(preguntaActual, mostrarResultado, respuestaSeleccionada);
        }
    }

    private void DrawAnswers(Pregunta pregunta, bool mostrarResultado, int respuestaSeleccionada)
    {
        Rectangle[] rects = GetAnswerRectangles(pregunta.Answers.Length);

        for (int i = 0; i < pregunta.Answers.Length; i++)
        {
            DrawAnswer(
                rects[i],
                pregunta.Answers[i],
                i + 1,
                pregunta.CorrectAnswer,
                respuestaSeleccionada,
                mostrarResultado
            );
        }
    }

    private void DrawAnswer(
        Rectangle rect,
        string text,
        int answerIndex,
        int correctAnswer,
        int selectedAnswer,
        bool mostrarResultado)
    {
        Vector2 mouse = ScreenScaler.GetVirtualMouse();
        bool hover = Raylib.CheckCollisionPointRec(mouse, rect);

        Color fondo = Color.White;

        if (mostrarResultado)
        {
            if (answerIndex == correctAnswer)
                fondo = new Color(90, 220, 120, 255); // verde
            else
                fondo = new Color(230, 90, 90, 255); // rojo
        }
        else if (hover)
        {
            fondo = Color.SkyBlue;
        }

        Raylib.DrawRectangleRec(rect, fondo);
        Raylib.DrawRectangleLinesEx(rect, 2, Color.Black);

        DrawTextInsideRect(text, rect, 22, Color.Black);
    }

    private void DrawCentered(string text, float y, int fontSize, Color color)
    {
        Vector2 size = Raylib.MeasureTextEx(fuente, text, fontSize, 1);
        float x = panel.X + (panel.Width - size.X) / 2f + UILayout.TitleOffsetX;
        y += UILayout.TitleOffsetY;

        Raylib.DrawTextEx(fuente, text, new Vector2(x, y), fontSize, 1, color);
    }

    private void DrawTextInsideRect(string text, Rectangle rect, int fontSize, Color color)
    {
        Vector2 size = Raylib.MeasureTextEx(fuente, text, fontSize, 1);

        while (size.X > rect.Width - 20 && fontSize > 14)
        {
            fontSize--;
            size = Raylib.MeasureTextEx(fuente, text, fontSize, 1);
        }

        float x = rect.X + (rect.Width - size.X) / 2f + UILayout.AnswerTextOffsetX;
        float y = rect.Y + (rect.Height - size.Y) / 2f + UILayout.AnswerTextOffsetY;

        Raylib.DrawTextEx(fuente, text, new Vector2(x, y), fontSize, 1, color);
    }

    private void DrawWrappedCentered(string text, Rectangle area, int fontSize, Color color)
    {
        float margenInterno = 50;
        float maxWidth = area.Width - margenInterno * 2;

        List<string> lines = WrapText(text, maxWidth, fontSize);

        float lineHeight = fontSize + 8;
        float totalHeight = lines.Count * lineHeight;
        float startY = area.Y + (area.Height - totalHeight) / 2f;

        for (int i = 0; i < lines.Count; i++)
        {
            Vector2 size = Raylib.MeasureTextEx(fuente, lines[i], fontSize, 1);

            float x = area.X + margenInterno + (maxWidth - size.X) / 2f;
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
            string test = string.IsNullOrWhiteSpace(current)
                ? word
                : current + " " + word;

            Vector2 size = Raylib.MeasureTextEx(fuente, test, fontSize, 1);

            if (size.X > maxWidth)
            {
                if (!string.IsNullOrWhiteSpace(current))
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

    public int GetClickedAnswer(Pregunta pregunta)
    {
        if (pregunta.Id <= 0 || pregunta.Answers == null || pregunta.Answers.Length == 0)
            return -1;

        Vector2 mouse = ScreenScaler.GetVirtualMouse();
        Rectangle[] rects = GetAnswerRectangles(pregunta.Answers.Length);

        for (int i = 0; i < rects.Length; i++)
        {
            if (Raylib.CheckCollisionPointRec(mouse, rects[i]) &&
                Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                return i + 1;
            }
        }

        return -1;
    }

    private Rectangle[] GetAnswerRectangles(int count)
    {
        float botonW = UILayout.AnswerButtonWidth;
        float botonH = UILayout.AnswerButtonHeight;
        float espacioX = UILayout.AnswerButtonSpaceX;
        float espacioY = UILayout.AnswerButtonSpaceY;
        float startY = panel.Y + UILayout.AnswerButtonStartY;

        List<Rectangle> rects = new();

        if (count == 2)
        {
            float totalW = botonW * 2 + espacioX;
            float startX = panel.X + (panel.Width - totalW) / 2f;

            rects.Add(new Rectangle(startX, startY, botonW, botonH));
            rects.Add(new Rectangle(startX + botonW + espacioX, startY, botonW, botonH));
        }
        else if (count == 3)
        {
            float totalW = botonW * 2 + espacioX;
            float startX = panel.X + (panel.Width - totalW) / 2f;
            float centerX = panel.X + (panel.Width - botonW) / 2f;

            rects.Add(new Rectangle(startX, startY, botonW, botonH));
            rects.Add(new Rectangle(startX + botonW + espacioX, startY, botonW, botonH));
            rects.Add(new Rectangle(centerX, startY + botonH + espacioY, botonW, botonH));
        }
        else
        {
            float totalW = botonW * 2 + espacioX;
            float startX = panel.X + (panel.Width - totalW) / 2f;

            rects.Add(new Rectangle(startX, startY, botonW, botonH));
            rects.Add(new Rectangle(startX + botonW + espacioX, startY, botonW, botonH));
            rects.Add(new Rectangle(startX, startY + botonH + espacioY, botonW, botonH));
            rects.Add(new Rectangle(startX + botonW + espacioX, startY + botonH + espacioY, botonW, botonH));
        }

        return rects.ToArray();
    }
}