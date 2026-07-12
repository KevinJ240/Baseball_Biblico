using Raylib_cs;
using System.Numerics;
using BaseballBiblico.Entities;
using BaseballBiblico.Core;

namespace BaseballBiblico.UI;

public class QuestionPanel
{
    private readonly Font fuente;
    private readonly Rectangle panel;

    private readonly Texture2D botonNormal;
    private readonly Texture2D botonHover;

    public QuestionPanel(
        Rectangle panel,
        Font fuente,
        Texture2D botonNormal,
        Texture2D botonHover)
    {
        this.panel = panel;
        this.fuente = fuente;
        this.botonNormal = botonNormal;
        this.botonHover = botonHover;
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

        DrawCentered(
            "PREGUNTA",
            panel.Y + UILayout.QuestionTitleY,
            34,
            Color.Black
        );

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

        DrawQuestionText(
            preguntaTexto,
            areaPregunta,
            22,
            Color.Black
        );

        if (!string.IsNullOrWhiteSpace(dificultad))
        {
            DrawCentered(
                $"Dificultad: {dificultad}",
                panel.Y + UILayout.DifficultyY,
                22,
                Color.DarkBlue
            );
        }

        if (preguntaActual.Id > 0 &&
            preguntaActual.Answers != null &&
            preguntaActual.Answers.Length > 0)
        {
            DrawAnswers(
                preguntaActual,
                mostrarResultado,
                respuestaSeleccionada
            );
        }
    }

    private void DrawAnswers(
        Pregunta pregunta,
        bool mostrarResultado,
        int respuestaSeleccionada)
    {
        if (pregunta.Answers == null ||
            pregunta.Answers.Length == 0)
        {
            return;
        }

        Rectangle[] rects =
            GetAnswerRectangles(pregunta.Answers.Length);

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

        bool hover =
            Raylib.CheckCollisionPointRec(mouse, rect);

        bool presionado =
            hover &&
            Raylib.IsMouseButtonDown(MouseButton.Left);

        Texture2D texturaActual =
            hover ? botonHover : botonNormal;

        Color tinte = Color.White;

        if (mostrarResultado)
        {
            tinte = answerIndex == correctAnswer
                ? new Color(90, 220, 120, 255)
                : new Color(230, 90, 90, 255);
        }

        if (texturaActual.Id > 0)
        {
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
                tinte
            );
        }
        else
        {
            Color fondo = Color.White;

            if (mostrarResultado)
            {
                fondo = answerIndex == correctAnswer
                    ? new Color(90, 220, 120, 255)
                    : new Color(230, 90, 90, 255);
            }
            else if (hover)
            {
                fondo = Color.SkyBlue;
            }

            Raylib.DrawRectangleRec(rect, fondo);
            Raylib.DrawRectangleLinesEx(
                rect,
                2,
                Color.Black
            );
        }

        Rectangle areaTexto = rect;

        if (presionado)
        {
            areaTexto.Y += 4;
        }

        DrawTextInsideRect(
            text,
            areaTexto,
            20,
            Color.Black
        );
    }

    private void DrawCentered(
        string text,
        float y,
        int fontSize,
        Color color)
    {
        Vector2 size = Raylib.MeasureTextEx(
            fuente,
            text,
            fontSize,
            1
        );

        float x =
            panel.X +
            (panel.Width - size.X) / 2f;

        Raylib.DrawTextEx(
            fuente,
            text,
            new Vector2(x, y),
            fontSize,
            1,
            color
        );
    }

    private void DrawQuestionText(
        string text,
        Rectangle area,
        int fontSize,
        Color color)
    {
        List<string> lines =
            WrapTextByCharacters(text, 42);

        float lineHeight = fontSize + 8;
        float totalHeight = lines.Count * lineHeight;

        float startY =
            area.Y +
            (area.Height - totalHeight) / 2f;

        float x = area.X + 20;

        Raylib.BeginScissorMode(
            (int)area.X,
            (int)area.Y,
            (int)area.Width,
            (int)area.Height
        );

        for (int i = 0; i < lines.Count; i++)
        {
            float y =
                startY + i * lineHeight;

            Raylib.DrawTextEx(
                fuente,
                lines[i],
                new Vector2(x, y),
                fontSize,
                1,
                color
            );
        }

        Raylib.EndScissorMode();
    }

    private void DrawTextInsideRect(
    string text,
    Rectangle rect,
    int fontSize,
    Color color)
    {
        const float margenHorizontal = 14f;
        const float margenVertical = 6f;
        const int tamanoMinimo = 12;

        int currentFontSize = fontSize;

        float anchoDisponible =
            rect.Width - margenHorizontal * 2;

        float altoDisponible =
            rect.Height - margenVertical * 2;

        List<string> lines = WrapAnswerText(
            text,
            anchoDisponible,
            currentFontSize
        );

        while (currentFontSize > tamanoMinimo)
        {
            float lineHeight = currentFontSize + 3;
            float totalHeight = lines.Count * lineHeight;

            bool cabeVerticalmente =
                totalHeight <= altoDisponible;

            bool cabeHorizontalmente = lines.All(line =>
                Raylib.MeasureTextEx(
                    fuente,
                    line,
                    currentFontSize,
                    1
                ).X <= anchoDisponible
            );

            if (cabeVerticalmente && cabeHorizontalmente)
                break;

            currentFontSize--;

            lines = WrapAnswerText(
                text,
                anchoDisponible,
                currentFontSize
            );
        }

        float finalLineHeight = currentFontSize + 3;
        float finalTotalHeight = lines.Count * finalLineHeight;

        float startY =
            rect.Y +
            (rect.Height - finalTotalHeight) / 2f;

        Rectangle areaSegura = new(
            rect.X + margenHorizontal,
            rect.Y + margenVertical,
            anchoDisponible,
            altoDisponible
        );

        Raylib.BeginScissorMode(
            (int)areaSegura.X,
            (int)areaSegura.Y,
            (int)areaSegura.Width,
            (int)areaSegura.Height
        );

        for (int i = 0; i < lines.Count; i++)
        {
            Vector2 medida = Raylib.MeasureTextEx(
                fuente,
                lines[i],
                currentFontSize,
                1
            );

            float x =
                rect.X +
                (rect.Width - medida.X) / 2f;

            float y =
                startY + i * finalLineHeight;

            Raylib.DrawTextEx(
                fuente,
                lines[i],
                new Vector2(x, y),
                currentFontSize,
                1,
                color
            );
        }

        Raylib.EndScissorMode();
    }

    private List<string> WrapAnswerText(
        string text,
        float maxWidth,
        int fontSize)
    {
        List<string> lines = new();

        if (string.IsNullOrWhiteSpace(text))
            return lines;

        string[] words = text.Split(
            ' ',
            StringSplitOptions.RemoveEmptyEntries
        );

        string currentLine = "";

        foreach (string word in words)
        {
            string testLine =
                string.IsNullOrWhiteSpace(currentLine)
                    ? word
                    : $"{currentLine} {word}";

            float ancho = Raylib.MeasureTextEx(
                fuente,
                testLine,
                fontSize,
                1
            ).X;

            if (ancho <= maxWidth)
            {
                currentLine = testLine;
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(currentLine))
                    lines.Add(currentLine);

                currentLine = word;
            }
        }

        if (!string.IsNullOrWhiteSpace(currentLine))
            lines.Add(currentLine);

        return lines;
    }

    private List<string> WrapTextByCharacters(
        string text,
        int maxChars)
    {
        List<string> lines = new();

        if (string.IsNullOrWhiteSpace(text))
        {
            return lines;
        }

        string[] words = text.Split(
            ' ',
            StringSplitOptions.RemoveEmptyEntries
        );

        string current = "";

        foreach (string word in words)
        {
            string test =
                string.IsNullOrWhiteSpace(current)
                    ? word
                    : $"{current} {word}";

            if (test.Length <= maxChars)
            {
                current = test;
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(current))
                {
                    lines.Add(current);
                }

                current = word;
            }
        }

        if (!string.IsNullOrWhiteSpace(current))
        {
            lines.Add(current);
        }

        return lines;
    }

    public int GetClickedAnswer(Pregunta pregunta)
    {
        if (pregunta.Id <= 0 ||
            pregunta.Answers == null ||
            pregunta.Answers.Length == 0)
        {
            return -1;
        }

        Vector2 mouse =
            ScreenScaler.GetVirtualMouse();

        Rectangle[] rects =
            GetAnswerRectangles(pregunta.Answers.Length);

        for (int i = 0; i < rects.Length; i++)
        {
            bool mouseDentro =
                Raylib.CheckCollisionPointRec(
                    mouse,
                    rects[i]
                );

            if (mouseDentro &&
                Raylib.IsMouseButtonPressed(
                    MouseButton.Left
                ))
            {
                return i + 1;
            }
        }

        return -1;
    }

    private Rectangle[] GetAnswerRectangles(int count)
    {
        float botonW =
            UILayout.AnswerButtonWidth;

        float botonH =
            UILayout.AnswerButtonHeight;

        float espacioX =
            UILayout.AnswerButtonSpaceX;

        float espacioY =
            UILayout.AnswerButtonSpaceY;

        float startY =
            panel.Y +
            UILayout.AnswerButtonStartY;

        List<Rectangle> rects = new();

        if (count == 2)
        {
            float totalW =
                botonW * 2 + espacioX;

            float startX =
                panel.X +
                (panel.Width - totalW) / 2f;

            rects.Add(
                new Rectangle(
                    startX,
                    startY,
                    botonW,
                    botonH
                )
            );

            rects.Add(
                new Rectangle(
                    startX + botonW + espacioX,
                    startY,
                    botonW,
                    botonH
                )
            );
        }
        else if (count == 3)
        {
            float totalW =
                botonW * 2 + espacioX;

            float startX =
                panel.X +
                (panel.Width - totalW) / 2f;

            float centerX =
                panel.X +
                (panel.Width - botonW) / 2f;

            rects.Add(
                new Rectangle(
                    startX,
                    startY,
                    botonW,
                    botonH
                )
            );

            rects.Add(
                new Rectangle(
                    startX + botonW + espacioX,
                    startY,
                    botonW,
                    botonH
                )
            );

            rects.Add(
                new Rectangle(
                    centerX,
                    startY + botonH + espacioY,
                    botonW,
                    botonH
                )
            );
        }
        else
        {
            float totalW =
                botonW * 2 + espacioX;

            float startX =
                panel.X +
                (panel.Width - totalW) / 2f;

            rects.Add(
                new Rectangle(
                    startX,
                    startY,
                    botonW,
                    botonH
                )
            );

            rects.Add(
                new Rectangle(
                    startX + botonW + espacioX,
                    startY,
                    botonW,
                    botonH
                )
            );

            rects.Add(
                new Rectangle(
                    startX,
                    startY + botonH + espacioY,
                    botonW,
                    botonH
                )
            );

            rects.Add(
                new Rectangle(
                    startX + botonW + espacioX,
                    startY + botonH + espacioY,
                    botonW,
                    botonH
                )
            );
        }

        return rects.ToArray();
    }
}