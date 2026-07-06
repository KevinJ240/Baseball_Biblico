using Raylib_cs;
using System.Numerics;

namespace BaseballBiblico.Managers;

public class RunnerAnimationManager
{
    private class Runner
    {
        public int BaseActual;
        public int BaseDestino;
        public int PasosRestantes;
        public Vector2 Position;
        public Vector2 From;
        public Vector2 To;
        public float Progress;
        public bool Finished;
    }

    private readonly List<Runner> runners = new();
    private readonly Rectangle fieldRect;

    private bool isAnimating = false;
    private int pendingRuns = 0;

    private const float Speed = 2.8f;

    public bool IsAnimating => isAnimating;

    public RunnerAnimationManager(Rectangle fieldRect)
    {
        this.fieldRect = fieldRect;
    }

    public void ClearBases()
    {
        runners.Clear();
        isAnimating = false;
        pendingRuns = 0;
    }

    public void AdvanceRunners(int amount)
    {
        if (isAnimating)
            return;

        pendingRuns = 0;

        foreach (Runner runner in runners)
        {
            runner.BaseDestino = runner.BaseActual + amount;
            runner.PasosRestantes = amount;
            runner.From = GetBasePosition(runner.BaseActual);
            runner.To = GetBasePosition(GetNextBase(runner.BaseActual));
            runner.Progress = 0f;
            runner.Finished = false;
        }

        Runner batter = new Runner
        {
            BaseActual = 0,
            BaseDestino = amount,
            PasosRestantes = amount,
            Position = GetBasePosition(0),
            From = GetBasePosition(0),
            To = GetBasePosition(GetNextBase(0)),
            Progress = 0f,
            Finished = false
        };

        runners.Add(batter);

        isAnimating = true;
    }

    public void Update(float deltaTime)
    {
        if (!isAnimating)
            return;

        bool allFinished = true;

        foreach (Runner runner in runners)
        {
            if (runner.Finished)
                continue;

            runner.Progress += deltaTime * Speed;

            if (runner.Progress >= 1f)
            {
                runner.Position = runner.To;
                runner.BaseActual = GetNextBase(runner.BaseActual);
                runner.PasosRestantes--;

                if (runner.PasosRestantes <= 0)
                {
                    if (runner.BaseDestino >= 4)
                    {
                        pendingRuns++;
                        runner.Finished = true;
                    }
                    else
                    {
                        runner.Finished = true;
                    }
                }
                else
                {
                    runner.From = runner.Position;
                    runner.To = GetBasePosition(GetNextBase(runner.BaseActual));
                    runner.Progress = 0f;
                }
            }
            else
            {
                runner.Position = Vector2.Lerp(runner.From, runner.To, runner.Progress);
            }

            if (!runner.Finished)
                allFinished = false;
        }

        if (allFinished)
        {
            runners.RemoveAll(r => r.BaseDestino >= 4);
            isAnimating = false;
        }
    }

    public int GetPendingRuns()
    {
        int runs = pendingRuns;
        pendingRuns = 0;
        return runs;
    }

    public void Draw(Texture2D token)
    {
        if (token.Id <= 0)
            return;

        foreach (Runner runner in runners)
        {
            DrawToken(runner.Position, token);
        }
    }

    private void DrawToken(Vector2 position, Texture2D token)
    {
        const float size = 70;

        Raylib.DrawTexturePro(
            token,
            new Rectangle(0, 0, token.Width, token.Height),
            new Rectangle(position.X - size / 2, position.Y - size / 2, size, size),
            Vector2.Zero,
            0,
            Color.White
        );
    }

    private int GetNextBase(int baseNumber)
    {
        return baseNumber switch
        {
            0 => 1,
            1 => 2,
            2 => 3,
            3 => 0,
            _ => 0
        };
    }

    private Vector2 GetBasePosition(int baseNumber)
    {
        return baseNumber switch
        {
            0 => new Vector2(fieldRect.X + fieldRect.Width * 0.50f, fieldRect.Y + fieldRect.Height * 0.82f),
            1 => new Vector2(fieldRect.X + fieldRect.Width * 0.80f, fieldRect.Y + fieldRect.Height * 0.45f),
            2 => new Vector2(fieldRect.X + fieldRect.Width * 0.50f, fieldRect.Y + fieldRect.Height * 0.14f),
            3 => new Vector2(fieldRect.X + fieldRect.Width * 0.20f, fieldRect.Y + fieldRect.Height * 0.45f),
            _ => new Vector2(fieldRect.X + fieldRect.Width * 0.50f, fieldRect.Y + fieldRect.Height * 0.82f)
        };
    }
}