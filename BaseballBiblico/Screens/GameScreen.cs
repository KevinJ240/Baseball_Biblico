using Raylib_cs;
using System.Numerics;

namespace BaseballBiblico.Screens;

public class GameScreen
{
    private int equipoA = 0;
    private int equipoB = 0;
    private int inning = 1;
    private int outs = 0;

    private string pregunta = "Selecciona una base para recibir una pregunta.";
    private string dificultadSeleccionada = "";

    private readonly Rectangle respuesta1 = new Rectangle(310, 285, 230, 65);
    private readonly Rectangle respuesta2 = new Rectangle(560, 285, 230, 65);
    private readonly Rectangle respuesta3 = new Rectangle(310, 375, 230, 65);
    private readonly Rectangle respuesta4 = new Rectangle(560, 375, 230, 65);

    public void Update()
    {
        Vector2 mouse = Raylib.GetMousePosition();

        if (Raylib.IsMouseButtonPressed(MouseButton.Left))
        {
            if (Raylib.CheckCollisionPointCircle(mouse, new Vector2(1110, 300), 35))
                SeleccionarDificultad("Hit");

            if (Raylib.CheckCollisionPointCircle(mouse, new Vector2(1000, 110), 35))
                SeleccionarDificultad("Doble");

            if (Raylib.CheckCollisionPointCircle(mouse, new Vector2(890, 300), 35))
                SeleccionarDificultad("Triple");

            if (Raylib.CheckCollisionPointCircle(mouse, new Vector2(1000, 570), 35))
                SeleccionarDificultad("Home Run");
        }
    }

    public void Draw()
    {
        Raylib.ClearBackground(new Color(38, 118, 25, 255));

        DibujarMarcador();
        DibujarPanelPregunta();
        DibujarCampo();
    }

    private void DibujarMarcador()
    {
        Raylib.DrawRectangle(15, 20, 260, 300, Color.RayWhite);
        Raylib.DrawRectangleLines(15, 20, 260, 300, Color.Black);

        Raylib.DrawText($"Equipo A: {equipoA}", 30, 45, 30, Color.Black);
        Raylib.DrawText($"Equipo B: {equipoB}", 30, 90, 30, Color.Black);

        Raylib.DrawText($"Inning: {inning}", 30, 230, 30, Color.Black);
        Raylib.DrawText($"Outs: {outs}", 30, 270, 30, Color.Black);
    }

    private void DibujarPanelPregunta()
    {
        Raylib.DrawRectangle(300, 20, 540, 660, Color.RayWhite);
        Raylib.DrawRectangleLines(300, 20, 540, 660, Color.Black);

        Raylib.DrawText("Pregunta:", 500, 45, 32, Color.Black);
        Raylib.DrawText(pregunta, 345, 100, 24, Color.Black);

        if (!string.IsNullOrWhiteSpace(dificultadSeleccionada))
        {
            Raylib.DrawText($"Dificultad: {dificultadSeleccionada}", 410, 180, 26, Color.DarkBlue);
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

        int anchoTexto = Raylib.MeasureText(texto, 28);
        int x = (int)(rect.X + rect.Width / 2 - anchoTexto / 2);
        int y = (int)(rect.Y + rect.Height / 2 - 14);

        Raylib.DrawText(texto, x, y, 28, Color.Black);
    }

    private void DibujarCampo()
    {
        Vector2 home = new Vector2(1000, 570);
        Vector2 first = new Vector2(1110, 300);
        Vector2 second = new Vector2(1000, 110);
        Vector2 third = new Vector2(890, 300);

        Raylib.DrawTriangle(home, first, second, new Color(214, 145, 145, 255));
        Raylib.DrawTriangle(home, second, third, new Color(214, 145, 145, 255));

        Raylib.DrawLineEx(home, first, 6, Color.White);
        Raylib.DrawLineEx(first, second, 6, Color.White);
        Raylib.DrawLineEx(second, third, 6, Color.White);
        Raylib.DrawLineEx(third, home, 6, Color.White);

        Raylib.DrawCircle(1000, 340, 120, new Color(35, 150, 25, 255));
        Raylib.DrawCircleLines(1000, 340, 120, Color.DarkGreen);

        DibujarBase(first, "Hit");
        DibujarBase(second, "Doble");
        DibujarBase(third, "Triple");
        DibujarBase(home, "Home Run");
    }

    private void DibujarBase(Vector2 posicion, string texto)
    {
        Vector2 mouse = Raylib.GetMousePosition();
        bool hover = Raylib.CheckCollisionPointCircle(mouse, posicion, 35);

        Color colorBase = hover ? Color.Gold : Color.White;

        Raylib.DrawCircleV(posicion, 28, colorBase);
        Raylib.DrawCircleLines((int)posicion.X, (int)posicion.Y, 28, Color.Black);

        int anchoTexto = Raylib.MeasureText(texto, 28);
        Raylib.DrawText(texto, (int)(posicion.X - anchoTexto / 2), (int)(posicion.Y + 40), 28, Color.Black);
    }

    private void SeleccionarDificultad(string dificultad)
    {
        dificultadSeleccionada = dificultad;
        pregunta = dificultad switch
        {
            "Hit" => "Pregunta facil: ¿Cuantos discipulos tuvo Jesus?",
            "Doble" => "Pregunta media: ¿Quien construyo el arca?",
            "Triple" => "Pregunta dificil: ¿Quien interpreto sueños en Egipto?",
            "Home Run" => "Pregunta muy dificil: ¿Cuantos libros tiene la Biblia?",
            _ => "Selecciona una base."
        };
    }
}