using BaseballBiblico.Entities;

namespace BaseballBiblico.Managers;

public class QuestionManager
{
    private readonly Dictionary<string, List<Pregunta>> questionsByDifficulty = new();
    private readonly Random random = new();

    public QuestionManager()
    {
        LoadQuestions("Hit", "Assets/Preguntas/Hit");
        LoadQuestions("Doble", "Assets/Preguntas/Doble");
        LoadQuestions("Triple", "Assets/Preguntas/Triple");
        LoadQuestions("HomeRun", "Assets/Preguntas/Home Run");
    }

    public Pregunta GetRandomQuestion(string difficulty)
    {
        if (!questionsByDifficulty.ContainsKey(difficulty) ||
            questionsByDifficulty[difficulty].Count == 0)
        {
            return new Pregunta
            {
                Id = 0,
                Text = $"No hay preguntas cargadas para {difficulty}.",
                Answers = ["Respuesta 1", "Respuesta 2", "Respuesta 3", "Respuesta 4"],
                CorrectAnswer = 1
            };
        }

        List<Pregunta> list = questionsByDifficulty[difficulty];
        return list[random.Next(list.Count)];
    }

    private void LoadQuestions(string difficulty, string folderPath)
    {
        string preguntasPath = Path.Combine(folderPath, "Preguntas.txt");
        string respuestasPath = Path.Combine(folderPath, "Respuesta.txt");
        string correctasPath = Path.Combine(folderPath, "Correctas.txt");

        questionsByDifficulty[difficulty] = new List<Pregunta>();

        if (!File.Exists(preguntasPath) ||
            !File.Exists(respuestasPath) ||
            !File.Exists(correctasPath))
        {
            return;
        }

        string[] preguntas = File.ReadAllLines(preguntasPath);
        string[] respuestas = File.ReadAllLines(respuestasPath);
        string[] correctas = File.ReadAllLines(correctasPath);

        int total = Math.Min(preguntas.Length, Math.Min(respuestas.Length, correctas.Length));

        for (int i = 0; i < total; i++)
        {
            string preguntaTexto = RemoveNumberPrefix(preguntas[i]);
            string respuestaLinea = RemoveNumberPrefix(respuestas[i]);
            string correctaLinea = RemoveNumberPrefix(correctas[i]);

            string[] opciones = respuestaLinea
                .Split(',')
                .Select(x => x.Trim())
                .ToArray();

            if (opciones.Length < 2 || opciones.Length > 4)
                continue;

            if (!int.TryParse(correctaLinea, out int correcta))
                continue;

            questionsByDifficulty[difficulty].Add(new Pregunta
            {
                Id = i + 1,
                Text = preguntaTexto,
                Answers = opciones,
                CorrectAnswer = correcta
            });
        }
    }

    private string RemoveNumberPrefix(string line)
    {
        int dotIndex = line.IndexOf('.');

        if (dotIndex >= 0 && dotIndex + 1 < line.Length)
            return line[(dotIndex + 1)..].Trim();

        return line.Trim();
    }
}