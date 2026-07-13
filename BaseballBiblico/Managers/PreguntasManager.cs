using BaseballBiblico.Entities;
using System.Text;
namespace BaseballBiblico.Managers;

public class QuestionManager
{
    private readonly Dictionary<string, List<Pregunta>> questionsByDifficulty = new();
    private readonly Dictionary<string, HashSet<int>> usedQuestionsByDifficulty = new();
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
        if (!HasQuestions(difficulty))
            return EmptyQuestion($"No hay preguntas cargadas para {difficulty}.");

        EnsureUsedSet(difficulty);

        List<Pregunta> disponibles = questionsByDifficulty[difficulty]
            .Where(q => !usedQuestionsByDifficulty[difficulty].Contains(q.Id))
            .ToList();

        if (disponibles.Count == 0)
            return EmptyQuestion($"Ya no hay preguntas disponibles para {difficulty}.");

        Pregunta seleccionada = disponibles[random.Next(disponibles.Count)];
        usedQuestionsByDifficulty[difficulty].Add(seleccionada.Id);

        return seleccionada;
    }

    public Pregunta GetRandomQuestionExcept(string difficulty, int excludedId)
    {
        if (!HasQuestions(difficulty))
            return EmptyQuestion($"No hay preguntas cargadas para {difficulty}.");

        EnsureUsedSet(difficulty);

        List<Pregunta> disponibles = questionsByDifficulty[difficulty]
            .Where(q =>
                q.Id != excludedId &&
                !usedQuestionsByDifficulty[difficulty].Contains(q.Id))
            .ToList();

        if (disponibles.Count == 0)
            return EmptyQuestion($"Ya no hay preguntas disponibles para {difficulty}.");

        Pregunta seleccionada = disponibles[random.Next(disponibles.Count)];
        usedQuestionsByDifficulty[difficulty].Add(seleccionada.Id);

        return seleccionada;
    }

    private void LoadQuestions(string difficulty, string folderPath)
    {
        string preguntasPath = Path.Combine(folderPath, "Preguntas.txt");
        string respuestasPath = Path.Combine(folderPath, "Respuesta.txt");
        string correctasPath = Path.Combine(folderPath, "Correctas.txt");

        questionsByDifficulty[difficulty] = new List<Pregunta>();
        usedQuestionsByDifficulty[difficulty] = new HashSet<int>();

        if (!File.Exists(preguntasPath) ||
            !File.Exists(respuestasPath) ||
            !File.Exists(correctasPath))
        {
            return;
        }

        string[] preguntas = File.ReadAllLines(preguntasPath, Encoding.UTF8);
        string[] respuestas = File.ReadAllLines(respuestasPath, Encoding.UTF8);
        string[] correctas = File.ReadAllLines(correctasPath, Encoding.UTF8);

        int total = Math.Min(preguntas.Length, Math.Min(respuestas.Length, correctas.Length));

        for (int i = 0; i < total; i++)
        {
            string preguntaTexto = RemoveNumberPrefix(preguntas[i]);
            string respuestaLinea = RemoveNumberPrefix(respuestas[i]);
            string correctaLinea = RemoveNumberPrefix(correctas[i]);

            string[] opciones = respuestaLinea
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToArray();

            if (opciones.Length < 2 || opciones.Length > 4)
                continue;

            if (!int.TryParse(correctaLinea, out int correcta))
                continue;

            if (correcta < 1 || correcta > opciones.Length)
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

    private bool HasQuestions(string difficulty)
    {
        return questionsByDifficulty.ContainsKey(difficulty) &&
               questionsByDifficulty[difficulty].Count > 0;
    }

    private void EnsureUsedSet(string difficulty)
    {
        if (!usedQuestionsByDifficulty.ContainsKey(difficulty))
            usedQuestionsByDifficulty[difficulty] = new HashSet<int>();
    }

    private Pregunta EmptyQuestion(string message)
    {
        return new Pregunta
        {
            Id = 0,
            Text = message,
            Answers = Array.Empty<string>(),
            CorrectAnswer = 0
        };
    }

    private string RemoveNumberPrefix(string line)
    {
        int dotIndex = line.IndexOf('.');

        if (dotIndex >= 0 && dotIndex + 1 < line.Length)
            return line[(dotIndex + 1)..].Trim();

        return line.Trim();
    }

    public void Reset()
    {
        foreach (
            HashSet<int> preguntasUsadas
            in usedQuestionsByDifficulty.Values)
        {
            preguntasUsadas.Clear();
        }
    }

}