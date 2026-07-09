using Raylib_cs;

namespace BaseballBiblico.UI;

public static class UILayout
{
    public static readonly Rectangle QuestionPanel = new(690, 220, 560, 480);

    public const float QuestionTitleY = 120;
    public const float QuestionLineY = 95;
    public const float QuestionTextY = 130;
    public const float DifficultyY = 285;

    public const float QuestionPaddingX = 120;
    public const float QuestionAreaHeight = 135;

    public const float AnswerButtonWidth = 220;
    public const float AnswerButtonHeight = 58;
    public const float AnswerButtonSpaceX = 35;
    public const float AnswerButtonSpaceY = 25;
    public const float AnswerButtonStartY = 320;

    public static readonly Rectangle Field = new(65, 215, 520, 520);
}