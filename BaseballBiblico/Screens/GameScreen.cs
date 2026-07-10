using Raylib_cs;
using System.Numerics;
using BaseballBiblico.UI;
using BaseballBiblico.Managers;
using BaseballBiblico.Entities;
using BaseballBiblico.Core;

namespace BaseballBiblico.Screens;

public class GameScreen
{
    private Texture2D campo;
    private Texture2D fichaAzul;
    private Texture2D fichaRoja;
    private Font fuente;

    private readonly QuestionManager questionManager = new();
    private RunnerAnimationManager runnerManager;

    private Pregunta currentQuestion = new()
    {
        Id = 0,
        Text = "",
        Answers = Array.Empty<string>(),
        CorrectAnswer = 0
    };

    private GamePlayState estado = GamePlayState.EsperandoJugada;

    private bool mostrandoResultado = false;
    private float tiempoResultado = 0f;
    private int respuestaSeleccionada = -1;
    private bool respuestaFueCorrecta = false;

    private const float TIEMPO_MOSTRAR_RESULTADO = 2.0f;

    private string pregunta = "Selecciona una base para recibir una pregunta.";
    private string dificultadSeleccionada = "";
    private string dificultadKeyActual = "";
    private int preguntaAtacanteId = 0;
    private int avancePendiente = 0;

    private readonly Color colorPasto = new(73, 138, 44, 255);

    private readonly Rectangle campoDestino = UILayout.Field;
    private readonly Rectangle panelPregunta = UILayout.QuestionPanel;

    private BaseButton btnHit;
    private BaseButton btnDoble;
    private BaseButton btnTriple;
    private BaseButton btnHomeRun;

    private GameButton btnIntentarOut;
    private GameButton btnContinuar;

    private ScoreBoardPanel scoreBoard;
    private QuestionPanel questionPanel;

    public GameScreen()
    {
        campo = Raylib.LoadTexture("Assets/Images/Campo_Baseball.png");
        fichaAzul = Raylib.LoadTexture("Assets/Images/fichaAzul.png");
        fichaRoja = Raylib.LoadTexture("Assets/Images/fichaRoja.png");
        fuente = FontManager.CargarFuenteEspanol(
            "Assets/Fonts/PatuaOne-Regular.ttf",
            40
        );

        runnerManager = new RunnerAnimationManager(campoDestino);

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

        btnDoble = new BaseButton(new Vector2(campoDestino.X + campoDestino.Width * 0.50f, campoDestino.Y + campoDestino.Height * 0.14f), 35, "Doble", "Media");
        btnTriple = new BaseButton(new Vector2(campoDestino.X + campoDestino.Width * 0.20f, campoDestino.Y + campoDestino.Height * 0.45f), 35, "Triple", "Dificil");
        btnHit = new BaseButton(new Vector2(campoDestino.X + campoDestino.Width * 0.80f, campoDestino.Y + campoDestino.Height * 0.45f), 35, "Hit", "Facil");
        btnHomeRun = new BaseButton(new Vector2(campoDestino.X + campoDestino.Width * 0.50f, campoDestino.Y + campoDestino.Height * 0.82f), 35, "Home Run", "Muy dificil");

        btnIntentarOut = new GameButton(760, 560, 210, 60, "INTENTAR OUT");
        btnContinuar = new GameButton(1000, 560, 180, 60, "CONTINUAR");
    }

    public void Update()
    {
        runnerManager.Update(Raylib.GetFrameTime());

        if (!runnerManager.IsAnimating)
        {
            int carrerasPendientes = runnerManager.GetPendingRuns();

            if (carrerasPendientes > 0)
            {
                if (scoreBoard.Turno == "A")
                    scoreBoard.EquipoA += carrerasPendientes;
                else
                    scoreBoard.EquipoB += carrerasPendientes;
            }
        }


        if (mostrandoResultado)
        {
            tiempoResultado += Raylib.GetFrameTime();

            if (tiempoResultado >= TIEMPO_MOSTRAR_RESULTADO)
                ResolverResultadoDespuesDeMostrar();

            return;
        }

        switch (estado)
        {
            case GamePlayState.EsperandoJugada:
                UpdateEsperandoJugada();
                break;

            case GamePlayState.PreguntaAtacante:
            case GamePlayState.PreguntaBloqueo:
                UpdatePregunta();
                break;

            case GamePlayState.DecidirBloqueo:
                UpdateDecidirBloqueo();
                break;
        }
    }

    public void Draw()
    {
        Raylib.ClearBackground(colorPasto);

        scoreBoard.Draw();
        DibujarCampo();

        questionPanel.Draw(
            currentQuestion,
            pregunta,
            dificultadSeleccionada,
            mostrandoResultado,
            respuestaSeleccionada
        );

        if (estado == GamePlayState.DecidirBloqueo)
            DibujarPanelBloqueo();
    }

    private void UpdateEsperandoJugada()
    {
        if (btnHit.IsClicked()) SeleccionarDificultad("Hit");
        if (btnDoble.IsClicked()) SeleccionarDificultad("Doble");
        if (btnTriple.IsClicked()) SeleccionarDificultad("Triple");
        if (btnHomeRun.IsClicked()) SeleccionarDificultad("Home Run");
    }

    private void UpdatePregunta()
    {
        int respuesta = questionPanel.GetClickedAnswer(currentQuestion);

        if (respuesta != -1)
        {
            respuestaSeleccionada = respuesta;
            respuestaFueCorrecta = respuesta == currentQuestion.CorrectAnswer;
            mostrandoResultado = true;
            tiempoResultado = 0f;
        }
    }

    private void UpdateDecidirBloqueo()
    {
        if (btnIntentarOut.IsClicked())
            CargarPreguntaBloqueo();

        if (btnContinuar.IsClicked())
        {
            AvanzarCorredores();
            LimpiarPregunta();
        }
    }

    private void ResolverResultadoDespuesDeMostrar()
    {
        mostrandoResultado = false;
        tiempoResultado = 0f;

        if (estado == GamePlayState.PreguntaAtacante)
        {
            if (respuestaFueCorrecta)
            {
                estado = GamePlayState.DecidirBloqueo;
                pregunta = $"Equipo {EquipoDefensor()} puede intentar hacer OUT.";
                currentQuestion = CrearPreguntaVacia();
            }
            else
            {
                MarcarStrike();
                LimpiarPregunta();
            }
        }
        else if (estado == GamePlayState.PreguntaBloqueo)
        {
            if (respuestaFueCorrecta)
                MarcarOut();
            else
                AvanzarCorredores();

            LimpiarPregunta();
        }

        respuestaSeleccionada = -1;
        respuestaFueCorrecta = false;
    }

    private void SeleccionarDificultad(string jugada)
    {
        dificultadSeleccionada = jugada;

        dificultadKeyActual = jugada switch
        {
            "Hit" => "Hit",
            "Doble" => "Doble",
            "Triple" => "Triple",
            "Home Run" => "HomeRun",
            _ => "Hit"
        };

        avancePendiente = jugada switch
        {
            "Hit" => 1,
            "Doble" => 2,
            "Triple" => 3,
            "Home Run" => 4,
            _ => 1
        };

        currentQuestion = questionManager.GetRandomQuestion(dificultadKeyActual);
        preguntaAtacanteId = currentQuestion.Id;
        pregunta = currentQuestion.Text;

        estado = GamePlayState.PreguntaAtacante;
    }

    private void CargarPreguntaBloqueo()
    {
        currentQuestion = questionManager.GetRandomQuestionExcept(dificultadKeyActual, preguntaAtacanteId);
        pregunta = currentQuestion.Text;
        dificultadSeleccionada = $"Bloqueo - {dificultadSeleccionada}";
        estado = GamePlayState.PreguntaBloqueo;
    }

    private void AvanzarCorredores()
    {
        runnerManager.AdvanceRunners(avancePendiente);
    }

    private void MarcarOut()
    {
        scoreBoard.Outs++;

        if (scoreBoard.Outs >= 3)
        {
            scoreBoard.Outs = 0;
            MarcarStrike();
        }
    }

    private void MarcarStrike()
    {
        scoreBoard.Strikes++;

        if (scoreBoard.Strikes >= 3)
            CambiarTurno();
    }

    private void CambiarTurno()
    {
        scoreBoard.Strikes = 0;
        scoreBoard.Outs = 0;
        runnerManager.ClearBases();

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

    private string EquipoDefensor()
    {
        return scoreBoard.Turno == "A" ? "B" : "A";
    }

    private Pregunta CrearPreguntaVacia()
    {
        return new Pregunta
        {
            Id = 0,
            Text = "",
            Answers = Array.Empty<string>(),
            CorrectAnswer = 0
        };
    }

    private void LimpiarPregunta()
    {
        currentQuestion = CrearPreguntaVacia();

        pregunta = "Selecciona una base para recibir una pregunta.";
        dificultadSeleccionada = "";

        mostrandoResultado = false;
        tiempoResultado = 0f;
        respuestaSeleccionada = -1;
        respuestaFueCorrecta = false;

        estado = GamePlayState.EsperandoJugada;
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

        Texture2D fichaActual = scoreBoard.Turno == "A" ? fichaAzul : fichaRoja;
        runnerManager.Draw(fichaActual);

        btnHit.Draw();
        btnDoble.Draw();
        btnTriple.Draw();
        btnHomeRun.Draw();
    }

    private void DibujarPanelBloqueo()
    {
        Raylib.DrawRectangle(735, 505, 495, 150, new Color(245, 245, 245, 245));
        Raylib.DrawRectangleLines(735, 505, 495, 150, Color.Black);

        string texto = $"Equipo {EquipoDefensor()}, ¿desea intentar OUT?";
        Raylib.DrawTextEx(fuente, texto, new Vector2(790, 520), 22, 1, Color.Black);

        btnIntentarOut.Draw();
        btnContinuar.Draw();
    }
}