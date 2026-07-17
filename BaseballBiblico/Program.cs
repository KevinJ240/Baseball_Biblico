using BaseballBiblico.Core;

try
{
    Directory.SetCurrentDirectory(
        AppContext.BaseDirectory
    );

    Game game = new();
    game.Run();
}
catch (Exception ex)
{
    string rutaError = Path.Combine(
        AppContext.BaseDirectory,
        "error_inicio.txt"
    );

    string contenido =
        $"Fecha: {DateTime.Now:yyyy-MM-dd HH:mm:ss}\n\n" +
        $"Directorio base:\n{AppContext.BaseDirectory}\n\n" +
        $"Directorio actual:\n{Directory.GetCurrentDirectory()}\n\n" +
        $"Tipo: {ex.GetType().FullName}\n\n" +
        $"Mensaje:\n{ex.Message}\n\n" +
        $"Detalles:\n{ex}";

    File.WriteAllText(
        rutaError,
        contenido
    );
}