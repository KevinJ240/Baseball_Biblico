using BaseballBiblico.Core;

try
{
    Directory.SetCurrentDirectory(AppContext.BaseDirectory);

    Game game = new Game();
    game.Run();
}
catch (Exception ex)
{
    string rutaError = Path.Combine(
        AppContext.BaseDirectory,
        "error_inicio.txt"
    );

    File.WriteAllText(
        rutaError,
        $"""
        Fecha: {DateTime.Now:yyyy-MM-dd HH:mm:ss}

        Tipo: {ex.GetType().FullName}

        Mensaje:
        {ex.Message}

        Detalles:
        {ex}
        """
    );
}