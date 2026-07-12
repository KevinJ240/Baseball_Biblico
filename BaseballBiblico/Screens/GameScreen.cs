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

    private Texture2D botonRespuestaNormal;
    private Texture2D botonRespuestaHover;

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

    private bool mostrandoResultado;
    private float tiempoResultado;
    private int respuestaSeleccionada = -1;
    private bool respuestaFueCorrecta;

    private bool partidaTerminada;

    private const float TIEMPO_MOSTRAR_RESULTADO = 2.0f;

    private string pregunta =
        "Selecciona una base para recibir una pregunta.";

    private string dificultadSeleccionada = "";
    private string dificultadKeyActual = "";

    private int preguntaAtacanteId;
    private int avancePendiente;

    private readonly Color colorPasto =
        new(73, 138, 44, 255);

    private readonly Rectangle campoDestino =
        UILayout.Field;

    private readonly Rectangle panelPregunta =
        UILayout.QuestionPanel;

    private BaseButton btnHit;
    private BaseButton btnDoble;
    private BaseButton btnTriple;
    private BaseButton btnHomeRun;

    private ImageButton btnIntentarOut;
    private ImageButton btnContinuar;

    private ScoreBoardPanel scoreBoard;
    private QuestionPanel questionPanel;

    public GameScreen()
    {
        CargarRecursos();
        CrearMarcador();
        CrearPanelPregunta();
        CrearBotonesBases();
        CrearBotonesBloqueo();
    }

    private void CargarRecursos()
    {
        campo = Raylib.LoadTexture(
            "Assets/Images/Campo_Baseball.png"
        );

        fichaAzul = Raylib.LoadTexture(
            "Assets/Images/fichaAzul.png"
        );

        fichaRoja = Raylib.LoadTexture(
            "Assets/Images/fichaRoja.png"
        );

        botonRespuestaNormal = Raylib.LoadTexture(
            "Assets/Images/Boton1.png"
        );

        botonRespuestaHover = Raylib.LoadTexture(
            "Assets/Images/Boton2.png"
        );

        fuente = FontManager.CargarFuenteEspanol(
            "Assets/Fonts/PatuaOne-Regular.ttf",
            40
        );

        runnerManager =
            new RunnerAnimationManager(campoDestino);
    }

    private void CrearMarcador()
    {
        scoreBoard = new ScoreBoardPanel
        {
            EquipoA = 0,
            EquipoB = 0,
            Inning = 1,
            Strikes = 0,
            Outs = 0,
            Turno = "A"
        };
    }

    private void CrearPanelPregunta()
    {
        questionPanel = new QuestionPanel(
            panelPregunta,
            fuente,
            botonRespuestaNormal,
            botonRespuestaHover
        );
    }

    private void CrearBotonesBases()
    {
        btnDoble = new BaseButton(
            new Vector2(
                campoDestino.X + campoDestino.Width * 0.50f,
                campoDestino.Y + campoDestino.Height * 0.14f
            ),
            35,
            "Doble",
            "Media"
        );

        btnTriple = new BaseButton(
            new Vector2(
                campoDestino.X + campoDestino.Width * 0.20f,
                campoDestino.Y + campoDestino.Height * 0.45f
            ),
            35,
            "Triple",
            "Difícil"
        );

        btnHit = new BaseButton(
            new Vector2(
                campoDestino.X + campoDestino.Width * 0.80f,
                campoDestino.Y + campoDestino.Height * 0.45f
            ),
            35,
            "Hit",
            "Fácil"
        );

        btnHomeRun = new BaseButton(
            new Vector2(
                campoDestino.X + campoDestino.Width * 0.50f,
                campoDestino.Y + campoDestino.Height * 0.82f
            ),
            35,
            "Home Run",
            "Muy difícil"
        );
    }

    private void CrearBotonesBloqueo()
    {
        btnIntentarOut = new ImageButton(
            new Rectangle(750, 565, 220, 64),
            botonRespuestaNormal,
            botonRespuestaHover,
            fuente,
            "INTENTAR OUT",
            19
        );

        btnContinuar = new ImageButton(
            new Rectangle(990, 565, 200, 64),
            botonRespuestaNormal,
            botonRespuestaHover,
            fuente,
            "CONTINUAR",
            19
        );
    }

    public void Update()
    {
        if (partidaTerminada)
            return;

        ActualizarCorredores();

        if (mostrandoResultado)
        {
            tiempoResultado += Raylib.GetFrameTime();

            if (tiempoResultado >= TIEMPO_MOSTRAR_RESULTADO)
            {
                ResolverResultadoDespuesDeMostrar();
            }

            return;
        }

        if (runnerManager.IsAnimating)
            return;

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

    private void ActualizarCorredores()
    {
        runnerManager.Update(Raylib.GetFrameTime());

        if (runnerManager.IsAnimating)
            return;

        int carrerasPendientes =
            runnerManager.GetPendingRuns();

        if (carrerasPendientes <= 0)
            return;

        if (scoreBoard.Turno == "A")
            scoreBoard.EquipoA += carrerasPendientes;
        else
            scoreBoard.EquipoB += carrerasPendientes;
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

        if (estado == GamePlayState.DecidirBloqueo &&
            !partidaTerminada)
        {
            DibujarPanelBloqueo();
        }

        if (partidaTerminada)
        {
            DibujarResultadoFinal();
        }
    }

    private void UpdateEsperandoJugada()
    {
        if (btnHit.IsClicked())
        {
            SeleccionarDificultad("Hit");
            return;
        }

        if (btnDoble.IsClicked())
        {
            SeleccionarDificultad("Doble");
            return;
        }

        if (btnTriple.IsClicked())
        {
            SeleccionarDificultad("Triple");
            return;
        }

        if (btnHomeRun.IsClicked())
        {
            SeleccionarDificultad("Home Run");
        }
    }

    private void UpdatePregunta()
    {
        int respuesta =
            questionPanel.GetClickedAnswer(currentQuestion);

        if (respuesta == -1)
            return;

        respuestaSeleccionada = respuesta;

        respuestaFueCorrecta =
            respuesta == currentQuestion.CorrectAnswer;

        mostrandoResultado = true;
        tiempoResultado = 0f;
    }

    private void UpdateDecidirBloqueo()
    {
        if (btnIntentarOut.IsClicked())
        {
            CargarPreguntaBloqueo();
            return;
        }

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
            ResolverPreguntaAtacante();
        }
        else if (estado == GamePlayState.PreguntaBloqueo)
        {
            ResolverPreguntaBloqueo();
        }

        respuestaSeleccionada = -1;
        respuestaFueCorrecta = false;
    }

    private void ResolverPreguntaAtacante()
    {
        if (respuestaFueCorrecta)
        {
            estado = GamePlayState.DecidirBloqueo;

            pregunta =
                $"{NombreEquipoDefensor()}, ¿desea intentar hacer OUT?";

            dificultadSeleccionada = "";
            currentQuestion = CrearPreguntaVacia();
        }
        else
        {
            MarcarStrike();

            if (!partidaTerminada)
                LimpiarPregunta();
        }
    }

    private void ResolverPreguntaBloqueo()
    {
        if (respuestaFueCorrecta)
        {
            MarcarOut();
        }
        else
        {
            AvanzarCorredores();
        }

        if (!partidaTerminada)
            LimpiarPregunta();
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

        currentQuestion =
            questionManager.GetRandomQuestion(
                dificultadKeyActual
            );

        preguntaAtacanteId = currentQuestion.Id;
        pregunta = currentQuestion.Text;

        if (currentQuestion.Id <= 0)
        {
            dificultadSeleccionada = "";
            estado = GamePlayState.EsperandoJugada;
            return;
        }

        estado = GamePlayState.PreguntaAtacante;
    }

    private void CargarPreguntaBloqueo()
    {
        currentQuestion =
            questionManager.GetRandomQuestionExcept(
                dificultadKeyActual,
                preguntaAtacanteId
            );

        pregunta = currentQuestion.Text;

        if (currentQuestion.Id <= 0)
        {
            dificultadSeleccionada = "";
            estado = GamePlayState.EsperandoJugada;
            return;
        }

        dificultadSeleccionada =
            $"Bloqueo - {dificultadSeleccionada}";

        estado = GamePlayState.PreguntaBloqueo;
    }

    private void AvanzarCorredores()
    {
        if (avancePendiente <= 0 ||
            runnerManager.IsAnimating)
        {
            return;
        }

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
        {
            CambiarTurno();
        }
    }

    private void CambiarTurno()
    {
        scoreBoard.Strikes = 0;
        scoreBoard.Outs = 0;

        runnerManager.ClearBases();

        if (scoreBoard.Turno == "A")
        {
            scoreBoard.Turno = "B";
            return;
        }

        // El Equipo B acaba de terminar su turno.
        // Aquí se decide si terminó la partida.
        if (scoreBoard.Inning >= GameSettings.TotalInnings)
        {
            FinalizarPartida();
            return;
        }

        scoreBoard.Turno = "A";
        scoreBoard.Inning++;
    }

    private void FinalizarPartida()
    {
        partidaTerminada = true;

        currentQuestion = CrearPreguntaVacia();

        pregunta = "";
        dificultadSeleccionada = "";
        dificultadKeyActual = "";

        mostrandoResultado = false;
        tiempoResultado = 0f;
        respuestaSeleccionada = -1;
        respuestaFueCorrecta = false;

        preguntaAtacanteId = 0;
        avancePendiente = 0;

        runnerManager.ClearBases();
    }

    private string EquipoDefensor()
    {
        return scoreBoard.Turno == "A"
            ? "B"
            : "A";
    }

    private string NombreEquipoDefensor()
    {
        return scoreBoard.Turno == "A"
            ? GameSettings.NombreEquipoB
            : GameSettings.NombreEquipoA;
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

        pregunta =
            "Selecciona una base para recibir una pregunta.";

        dificultadSeleccionada = "";
        dificultadKeyActual = "";

        mostrandoResultado = false;
        tiempoResultado = 0f;

        respuestaSeleccionada = -1;
        respuestaFueCorrecta = false;

        preguntaAtacanteId = 0;
        avancePendiente = 0;

        estado = GamePlayState.EsperandoJugada;
    }

    private void DibujarCampo()
    {
        Raylib.DrawTexturePro(
            campo,
            new Rectangle(
                0,
                0,
                campo.Width,
                campo.Height
            ),
            campoDestino,
            Vector2.Zero,
            0,
            Color.White
        );

        Texture2D fichaActual =
            scoreBoard.Turno == "A"
                ? fichaAzul
                : fichaRoja;

        runnerManager.Draw(fichaActual);

        if (!partidaTerminada)
        {
            btnHit.Draw();
            btnDoble.Draw();
            btnTriple.Draw();
            btnHomeRun.Draw();
        }
    }

    private void DibujarPanelBloqueo()
    {
        Rectangle panelBloqueo = new(
            720,
            500,
            515,
            155
        );

        Raylib.DrawRectangleRec(
            panelBloqueo,
            new Color(245, 245, 245, 250)
        );

        Raylib.DrawRectangleLinesEx(
            panelBloqueo,
            3,
            Color.Black
        );

        string texto =
            $"{NombreEquipoDefensor()}, ¿desea intentar OUT?";

        DibujarTextoCentradoEnRectangulo(
            texto,
            new Rectangle(
                panelBloqueo.X + 20,
                panelBloqueo.Y + 12,
                panelBloqueo.Width - 40,
                42
            ),
            20,
            Color.Black
        );

        btnIntentarOut.Draw();
        btnContinuar.Draw();
    }

    private void DibujarResultadoFinal()
    {
        Raylib.DrawRectangle(
            0,
            0,
            1280,
            720,
            new Color(0, 0, 0, 150)
        );

        Rectangle panelFinal = new(
            325,
            220,
            630,
            310
        );

        Raylib.DrawRectangleRec(
            panelFinal,
            new Color(245, 245, 245, 255)
        );

        Raylib.DrawRectangleLinesEx(
            panelFinal,
            5,
            Color.Black
        );

        string titulo = "PARTIDA TERMINADA";

        DibujarTextoCentradoEnRectangulo(
            titulo,
            new Rectangle(
                panelFinal.X,
                panelFinal.Y + 25,
                panelFinal.Width,
                50
            ),
            34,
            Color.DarkBlue
        );

        string ganador = ObtenerTextoGanador();

        DibujarTextoCentradoEnRectangulo(
            ganador,
            new Rectangle(
                panelFinal.X + 20,
                panelFinal.Y + 95,
                panelFinal.Width - 40,
                55
            ),
            30,
            Color.Black
        );

        string marcador =
            $"{GameSettings.NombreEquipoA}: {scoreBoard.EquipoA}    " +
            $"{GameSettings.NombreEquipoB}: {scoreBoard.EquipoB}";

        DibujarTextoCentradoEnRectangulo(
            marcador,
            new Rectangle(
                panelFinal.X + 20,
                panelFinal.Y + 165,
                panelFinal.Width - 40,
                50
            ),
            24,
            Color.DarkGray
        );

        string innings =
            $"Partida de {GameSettings.TotalInnings} inning(s)";

        DibujarTextoCentradoEnRectangulo(
            innings,
            new Rectangle(
                panelFinal.X + 20,
                panelFinal.Y + 225,
                panelFinal.Width - 40,
                45
            ),
            20,
            Color.DarkGray
        );
    }

    private string ObtenerTextoGanador()
    {
        if (scoreBoard.EquipoA > scoreBoard.EquipoB)
        {
            return $"GANADOR: {GameSettings.NombreEquipoA}";
        }

        if (scoreBoard.EquipoB > scoreBoard.EquipoA)
        {
            return $"GANADOR: {GameSettings.NombreEquipoB}";
        }

        return "EMPATE";
    }

    private void DibujarTextoCentradoEnRectangulo(
        string texto,
        Rectangle rect,
        int tamaño,
        Color color)
    {
        Vector2 medida = Raylib.MeasureTextEx(
            fuente,
            texto,
            tamaño,
            1
        );

        while (medida.X > rect.Width - 20 &&
               tamaño > 13)
        {
            tamaño--;

            medida = Raylib.MeasureTextEx(
                fuente,
                texto,
                tamaño,
                1
            );
        }

        float x =
            rect.X +
            (rect.Width - medida.X) / 2f;

        float y =
            rect.Y +
            (rect.Height - medida.Y) / 2f;

        Raylib.DrawTextEx(
            fuente,
            texto,
            new Vector2(x, y),
            tamaño,
            1,
            color
        );
    }

    public void Reset()
    {
        scoreBoard.EquipoA = 0;
        scoreBoard.EquipoB = 0;
        scoreBoard.Inning = 1;
        scoreBoard.Strikes = 0;
        scoreBoard.Outs = 0;
        scoreBoard.Turno = "A";

        runnerManager.ClearBases();

        partidaTerminada = false;

        currentQuestion = CrearPreguntaVacia();

        pregunta =
            "Selecciona una base para recibir una pregunta.";

        dificultadSeleccionada = "";
        dificultadKeyActual = "";

        mostrandoResultado = false;
        tiempoResultado = 0f;
        respuestaSeleccionada = -1;
        respuestaFueCorrecta = false;

        preguntaAtacanteId = 0;
        avancePendiente = 0;

        estado = GamePlayState.EsperandoJugada;
    }

    public void Unload()
    {
        if (campo.Id > 0)
            Raylib.UnloadTexture(campo);

        if (fichaAzul.Id > 0)
            Raylib.UnloadTexture(fichaAzul);

        if (fichaRoja.Id > 0)
            Raylib.UnloadTexture(fichaRoja);

        if (botonRespuestaNormal.Id > 0)
            Raylib.UnloadTexture(botonRespuestaNormal);

        if (botonRespuestaHover.Id > 0)
            Raylib.UnloadTexture(botonRespuestaHover);

        if (fuente.Texture.Id > 0)
            Raylib.UnloadFont(fuente);
    }
}