using Raylib_cs;

namespace BaseballBiblico.UI;

public static class UILayout
{
    // Panel de preguntas
    public static readonly Rectangle QuestionPanel = new(690, 220, 560, 480);

    // Texto del panel de preguntas
    public const float QuestionTitleY = 30;
    public const float QuestionLineY = 95;
    public const float QuestionTextY = 130;
    public const float DifficultyY = 245;

    // Área interna de la pregunta
    public const float QuestionPaddingX = 50;
    public const float QuestionAreaHeight = 90;

    // Botones de respuesta
    public const float AnswerButtonWidth = 220;
    public const float AnswerButtonHeight = 58;
    public const float AnswerButtonSpaceX = 35;
    public const float AnswerButtonSpaceY = 25;
    public const float AnswerButtonStartY = 300;

    // Campo de baseball
    public static readonly Rectangle Field = new(65, 215, 520, 520);


    // Desplazamiento de textos
    public const float TitleOffsetX = -80;
    public const float TitleOffsetY = 0;

    public const float QuestionOffsetX = -250;
    public const float QuestionOffsetY = 0;

    public const float DifficultyOffsetX = -250;
    public const float DifficultyOffsetY = 0;

    public const float AnswerTextOffsetX = -45;
    public const float AnswerTextOffsetY = 0;




}