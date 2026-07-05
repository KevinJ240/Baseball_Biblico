using Raylib_cs;
using System.Numerics;

namespace BaseballBiblico.Managers;

public class BaseRunnerManager
{
    private readonly bool[] bases = new bool[4];

    public void ClearBases()
    {
        bases[1] = false;
        bases[2] = false;
        bases[3] = false;
    }

    public int AdvanceRunners(int amount)
    {
        int runs = 0;

        for (int i = 3; i >= 1; i--)
        {
            if (!bases[i])
                continue;

            bases[i] = false;

            int newBase = i + amount;

            if (newBase >= 4)
                runs++;
            else
                bases[newBase] = true;
        }

        if (amount >= 4)
            runs++;
        else
            bases[amount] = true;

        return runs;
    }

    public void Draw(Rectangle fieldRect, Texture2D token)
    {
        if (token.Id <= 0)
            return;

        DrawTokenIfOccupied(
            1,
            new Vector2(fieldRect.X + fieldRect.Width * 0.80f,
                        fieldRect.Y + fieldRect.Height * 0.45f),
            token);

        DrawTokenIfOccupied(
            2,
            new Vector2(fieldRect.X + fieldRect.Width * 0.50f,
                        fieldRect.Y + fieldRect.Height * 0.14f),
            token);

        DrawTokenIfOccupied(
            3,
            new Vector2(fieldRect.X + fieldRect.Width * 0.20f,
                        fieldRect.Y + fieldRect.Height * 0.45f),
            token);
    }

    private void DrawTokenIfOccupied(int baseNumber, Vector2 position, Texture2D token)
    {
        if (!bases[baseNumber])
            return;

        const float size = 90;

        Raylib.DrawTexturePro(
            token,
            new Rectangle(0, 0, token.Width, token.Height),
            new Rectangle(position.X - size / 2,
                          position.Y - size / 2,
                          size,
                          size),
            Vector2.Zero,
            0,
            Color.White);
    }
}