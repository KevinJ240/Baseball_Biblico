using Raylib_cs;
using System.Numerics;
using BaseballBiblico.UI;
using BaseballBiblico.Managers;
using BaseballBiblico.Entities;
using BaseballBiblico.UI;

namespace BaseballBiblico.Screens;

public class GameScreen
{
    private Texture2D campo;
    private Font fuente;

    private readonly QuestionManager questionManager = new();
    private Pregunta currentQuestion = new();

    private string pregunta = "Selecciona una base para recibir una pregunta.";
    private string dificultadSeleccionada = "";

    private readonly Color colorPasto = new(73, 138, 44, 255);

    private readonly Rectangle campoDestino = UILayout.Field;
    private readonly Rectangle panelPregunta = UILayout.QuestionPanel;

    private BaseButton btnHit;
    private BaseButton btnDoble;
    private BaseButton btnTriple;
    private BaseButton btnHomeRun;

    private ScoreBoardPanel scoreBoard;
    private QuestionPanel questionPanel;

    public GameScreen()
    {
        campo = Raylib.LoadTexture("Assets/Images/Campo_Baseball.png");
        fuente = Raylib.LoadFontEx("Assets/Fonts/arial.ttf", 32, null, 0);

        scoreBoard = new ScoreBoardPanel
        {
            EquipoA = 0,
            EquipoB = 0,
            Inning = 1,
            Strikes = 0,
            Outs = 0,
            Turno = "A"
        };

        questionPanel = new QuestionPanel(panelPregunta, fuente);

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
        if (btnHit.IsClicked()) SeleccionarDificultad("Hit");
        if (btnDoble.IsClicked()) SeleccionarDificultad("Doble");
        if (btnTriple.IsClicked()) SeleccionarDificultad("Triple");
        if (btnHomeRun.IsClicked()) SeleccionarDificultad("Home Run");

        int respuestaSeleccionada = questionPanel.GetClickedAnswer(currentQuestion);

        if (respuestaSeleccionada != -1)
        {
            ProcesarRespuesta(respuestaSeleccionada);
        }
    }

    public void Draw()
    {
        Raylib.ClearBackground(colorPasto);

        scoreBoard.Draw();
        DibujarCampo();

        questionPanel.Draw(currentQuestion, pregunta, dificultadSeleccionada);
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

    private void SeleccionarDificultad(string jugada)
    {
        dificultadSeleccionada = jugada;

        string difficultyKey = jugada switch
        {
            "Hit" => "Hit",
            "Doble" => "Doble",
            "Triple" => "Triple",
            "Home Run" => "HomeRun",
            _ => "Hit"
        };

        currentQuestion = questionManager.GetRandomQuestion(difficultyKey);
        pregunta = currentQuestion.Text;
    }

    private void ProcesarRespuesta(int respuestaSeleccionada)
    {
        if (currentQuestion.CorrectAnswer == 0)
            return;

        if (respuestaSeleccionada == currentQuestion.CorrectAnswer)
        {
            SumarPunto();
        }
        else
        {
            scoreBoard.Strikes++;

            if (scoreBoard.Strikes >= 3)
            {
                CambiarTurno();
            }
        }

        LimpiarPregunta();
    }

    private void SumarPunto()
    {
        if (scoreBoard.Turno == "A")
            scoreBoard.EquipoA++;
        else
            scoreBoard.EquipoB++;
    }

    private void CambiarTurno()
    {
        scoreBoard.Strikes = 0;
        scoreBoard.Outs = 0;

        if (scoreBoard.Turno == "A")
        {
            scoreBoard.Turno = "B";
        }
        else
        {
            scoreBoard.Turno = "A";
            scoreBoard.Inning++;
        }
    }

    private void LimpiarPregunta()
    {
        currentQuestion = new Pregunta();
        pregunta = "Selecciona una base para recibir una pregunta.";
        dificultadSeleccionada = "";
    }



}