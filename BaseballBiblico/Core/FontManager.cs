using Raylib_cs;
using System.Text;

namespace BaseballBiblico.Core;

public static class FontManager
{
    public static Font CargarFuenteEspanol(
        string rutaRelativa,
        int tamaño)
    {
        string rutaPrincipal =
            ObtenerRutaAbsoluta(rutaRelativa);

        string rutaAlternativa =
            ObtenerRutaAbsoluta(
                "Assets/Fonts/ari-bold.ttf"
            );

        Font fuente = IntentarCargarFuente(
            rutaPrincipal,
            tamaño
        );

        if (FuenteValida(fuente))
        {
            ConfigurarFuente(fuente);
            return fuente;
        }

        fuente = IntentarCargarFuente(
            rutaAlternativa,
            tamaño
        );

        if (FuenteValida(fuente))
        {
            ConfigurarFuente(fuente);
            return fuente;
        }

        throw new InvalidOperationException(
            "No se pudo cargar ninguna fuente del juego.\n\n" +
            $"Fuente principal:\n{rutaPrincipal}\n\n" +
            $"Fuente alternativa:\n{rutaAlternativa}\n\n" +
            "Comprueba que ambos archivos existan, tengan contenido " +
            "y se encuentren dentro de Assets/Fonts."
        );
    }

    private static Font IntentarCargarFuente(
        string ruta,
        int tamaño)
    {
        if (!File.Exists(ruta))
        {
            return default;
        }

        FileInfo archivo = new(ruta);

        if (archivo.Length <= 0)
        {
            return default;
        }

        int[] caracteres =
            CrearCaracteresEspanol();

        try
        {
            return Raylib.LoadFontEx(
                ruta,
                tamaño,
                caracteres,
                caracteres.Length
            );
        }
        catch
        {
            return default;
        }
    }

    private static bool FuenteValida(Font fuente)
    {
        return fuente.Texture.Id > 0 &&
               fuente.GlyphCount > 0;
    }

    private static void ConfigurarFuente(Font fuente)
    {
        Raylib.SetTextureFilter(
            fuente.Texture,
            TextureFilter.Bilinear
        );
    }

    private static string ObtenerRutaAbsoluta(
        string rutaRelativa)
    {
        string rutaNormalizada =
            rutaRelativa.Replace(
                '/',
                Path.DirectorySeparatorChar
            );

        return Path.GetFullPath(
            Path.Combine(
                AppContext.BaseDirectory,
                rutaNormalizada
            )
        );
    }

    private static int[] CrearCaracteresEspanol()
    {
        HashSet<int> caracteres = new();

        // ASCII imprimible.
        for (int codigo = 32; codigo <= 126; codigo++)
        {
            caracteres.Add(codigo);
        }

        // Caracteres en español.
        string adicionales =
            "áéíóúÁÉÍÓÚñÑüÜ¿¡";

        foreach (Rune rune in adicionales.EnumerateRunes())
        {
            caracteres.Add(rune.Value);
        }

        return caracteres.ToArray();
    }
}