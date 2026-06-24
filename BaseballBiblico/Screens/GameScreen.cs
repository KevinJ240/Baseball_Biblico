using Raylib_cs;
using System.Numerics;
using BaseballBiblico.UI;

namespace BaseballBiblico.Screens;

public class GameScreen
{
    private Texture2D campo;

    private string pregunta = "Selecciona una base para recibir una pregunta.";
    private string dificultadSeleccionada = "";

    private readonly Color colorPasto = new(73, 138, 44, 255);

    private readonly Rectangle campoDestino = new(90, 185, 540, 540);
    private readonly Rectangle panelPregunta = new(720, 190, 520, 535);

    private readonly Rectangle respuesta1 = new(760, 425, 440, 55);
    private readonly Rectangle respuesta2 = new(760, 495, 440, 55);
    private readonly Rectangle respuesta3 = new(760, 565, 440, 55);
    private readonly Rectangle respuesta4 = new(760, 635, 440, 55);

    private BaseButton btnHit;
    private BaseButton btnDoble;
    private BaseButton btnTriple;
    private BaseButton btnHomeRun;

    private ScoreBoardPanel scoreBoard;

    public GameScreen()
    {
        campo = Raylib.LoadTexture("Assets/Images/Campo_Baseball.png");

        scoreBoard = new ScoreBoardPanel
        {
            EquipoA = 0,
            EquipoB = 0,
            Inning = 1,
            Strikes = 0,
            Outs = 0,
            Turno = "A"
        };

        btnDoble = new BaseButton(
            new Vector2(campoDestino.X + campoDestino.Width * 0.50f, campoDestino.Y + campoDestino.Height * 0.14f),
            35,
            "Doble",
            "Media"
        );

        btnTriple = new BaseButton(
            new Vector2(campoDestino.X + campoDestino.Width * 0.20f, campoDestino.Y + campoDestino.Height * 0.45f),
            35,
            "Triple",
            "Dificil"
        );

        btnHit = new BaseButton(
            new Vector2(campoDestino.X + campoDestino.Width * 0.80f, campoDestino.Y + campoDestino.Height * 0.45f),
            35,
            "Hit",
            "Facil"
        );

        btnHomeRun = new BaseButton(
            new Vector2(campoDestino.X + campoDestino.Width * 0.50f, campoDestino.Y + campoDestino.Height * 0.82f),
            35,
            "Home Run",
            "Muy dificil"
        );
    }

    public void Update()
    {
        if (btnHit.IsClicked())
            SeleccionarDificultad("Hit");

        if (btnDoble.IsClicked())
            SeleccionarDificultad("Doble");

        if (btnTriple.IsClicked())
            SeleccionarDificultad("Triple");

        if (btnHomeRun.IsClicked())
            SeleccionarDificultad("Home Run");

        if (Raylib.IsKeyPressed(KeyboardKey.S))
            scoreBoard.Strikes = Math.Min(scoreBoard.Strikes + 1, 3);

        if (Raylib.IsKeyPressed(KeyboardKey.O))
            scoreBoard.Outs = Math.Min(scoreBoard.Outs + 1, 3);
    }

    public void Draw()
    {
        Raylib.ClearBackground(colorPasto);

        scoreBoard.Draw();
        DibujarCampo();
        DibujarPanelPregunta();
    }

    private void DibujarCampo()
    {
        Raylib.DrawTexturePro(
            campo,
            new Rectangle(0, 0, campo.Width, campo.Height),
            campoDestino,
            Vector2.Zero,
            0,
            Color.White
        );

        btnHit.Draw();
        btnDoble.Draw();
        btnTriple.Draw();
        btnHomeRun.Draw();
    }

    private void DibujarPanelPregunta()
    {
        Raylib.DrawRectangleRec(panelPregunta, Color.RayWhite);
        Raylib.DrawRectangleLinesEx(panelPregunta, 3, Color.Black);

        Raylib.DrawText("PREGUNTA", 900, 250, 34, Color.Black);

        Raylib.DrawText(pregunta, 775, 320, 20, Color.Black);

        if (!string.IsNullOrWhiteSpace(dificultadSeleccionada))
        {
            Raylib.DrawText($"Dificultad: {dificultadSeleccionada}", 850, 370, 24, Color.DarkBlue);
        }

        DibujarRespuesta(respuesta1, "Respuesta 1");
        DibujarRespuesta(respuesta2, "Respuesta 2");
        DibujarRespuesta(respuesta3, "Respuesta 3");
        DibujarRespuesta(respuesta4, "Respuesta 4");
    }

    private void DibujarRespuesta(Rectangle rect, string texto)
    {
        Vector2 mouse = Raylib.GetMousePosition();
        bool hover = Raylib.CheckCollisionPointRec(mouse, rect);

        Raylib.DrawRectangleRec(rect, hover ? Color.SkyBlue : Color.White);
        Raylib.DrawRectangleLinesEx(rect, 2, Color.Black);

        int anchoTexto = Raylib.MeasureText(texto, 24);
        int x = (int)(rect.X + rect.Width / 2 - anchoTexto / 2);
        int y = (int)(rect.Y + rect.Height / 2 - 12);

        Raylib.DrawText(texto, x, y, 24, Color.Black);
    }

    private void SeleccionarDificultad(string jugada)
    {
        dificultadSeleccionada = jugada;

        pregunta = jugada switch
        {
            "Hit" => "Facil: ¿Cuantos discipulos tuvo Jesus?",
            "Doble" => "Media: ¿Quien construyo el arca?",
            "Triple" => "Dificil: ¿Quien interpreto sueños en Egipto?",
            "Home Run" => "Muy dificil: ¿Cuantos libros tiene la Biblia?",
            _ => "Selecciona una base para recibir una pregunta."
        };
    }
}