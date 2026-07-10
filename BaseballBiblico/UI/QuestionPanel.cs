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
            panel.X + 25,
            panel.Y + UILayout.QuestionTextY,
            panel.Width - 50,
            UILayout.QuestionAreaHeight
        );

        DrawQuestionText(preguntaTexto, areaPregunta, 22, Color.Black);

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
            fondo = answerIndex == correctAnswer
                ? new Color(90, 220, 120, 255)
                : new Color(230, 90, 90, 255);
        else if (hover)
            fondo = Color.SkyBlue;

        Raylib.DrawRectangleRec(rect, fondo);
        Raylib.DrawRectangleLinesEx(rect, 2, Color.Black);

        DrawTextInsideRect(text, rect, 20, Color.Black);
    }

    private void DrawCentered(string text, float y, int fontSize, Color color)
    {
        float x;

        if (text == "PREGUNTA")
            x = panel.X + 200;
        else if (text.StartsWith("Dificultad"))
            x = panel.X + 200;
        else
            x = panel.X + 40;

        Raylib.DrawTextEx(
            fuente,
            text,
            new Vector2(x, y),
            fontSize,
            1,
            color
        );
    }

    private void DrawQuestionText(string text, Rectangle area, int fontSize, Color color)
    {
        List<string> lines = WrapTextByCharacters(text, 42);

        float lineHeight = fontSize + 8;
        float totalHeight = lines.Count * lineHeight;
        float startY = area.Y + (area.Height - totalHeight) / 2f;

        float x = panel.X + 45; // margen izquierdo fijo

        Raylib.BeginScissorMode(
            (int)(panel.X + 35),
            (int)area.Y,
            (int)(panel.Width - 70),
            (int)area.Height
        );

        for (int i = 0; i < lines.Count; i++)
        {
            float y = startY + i * lineHeight;
            Raylib.DrawTextEx(fuente, lines[i], new Vector2(x, y), fontSize, 1, color);
        }

        Raylib.EndScissorMode();
    }

    private List<string> WrapTextByCharacters(string text, int maxChars)
    {
        List<string> lines = new();

        if (string.IsNullOrWhiteSpace(text))
            return lines;

        string[] words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        string current = "";

        foreach (string word in words)
        {
            string test = string.IsNullOrWhiteSpace(current)
                ? word
                : $"{current} {word}";

            if (test.Length <= maxChars)
            {
                current = test;
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(current))
                    lines.Add(current);

                current = word;
            }
        }

        if (!string.IsNullOrWhiteSpace(current))
            lines.Add(current);

        return lines;
    }

    private List<string> WrapTextByWidth(string text, float maxWidth, int fontSize)
    {
        List<string> lines = new();

        if (string.IsNullOrWhiteSpace(text))
            return lines;

        string[] words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        string line = "";

        foreach (string word in words)
        {
            string test = string.IsNullOrWhiteSpace(line) ? word : $"{line} {word}";
            Vector2 testSize = Raylib.MeasureTextEx(fuente, test, fontSize, 1);

            if (testSize.X <= maxWidth)
            {
                line = test;
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(line))
                    lines.Add(line);

                line = word;
            }
        }

        if (!string.IsNullOrWhiteSpace(line))
            lines.Add(line);

        return lines;
    }

    private void DrawTextInsideRect(string text, Rectangle rect, int fontSize, Color color)
    {
        int currentFontSize = fontSize;
        List<string> lines = WrapTextByCharacters(text, 12);

        while (lines.Count * (currentFontSize + 4) > rect.Height - 8 && currentFontSize > 12)
        {
            currentFontSize--;
        }

        float lineHeight = currentFontSize + 4;
        float totalHeight = lines.Count * lineHeight;
        float startY = rect.Y + (rect.Height - totalHeight) / 2f;

        Raylib.BeginScissorMode(
            (int)rect.X,
            (int)rect.Y,
            (int)rect.Width,
            (int)rect.Height
        );

        for (int i = 0; i < lines.Count; i++)
        {
            Vector2 size = Raylib.MeasureTextEx(fuente, lines[i], currentFontSize, 1);

            float x = rect.X + (rect.Width - size.X) / 2f + UILayout.AnswerTextOffsetX;
            float y = startY + i * lineHeight;

            Raylib.DrawTextEx(fuente, lines[i], new Vector2(x, y), currentFontSize, 1, color);
        }

        Raylib.EndScissorMode();
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