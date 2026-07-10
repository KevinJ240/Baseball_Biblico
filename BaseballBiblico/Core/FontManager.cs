using Raylib_cs;

namespace BaseballBiblico.Core;

public static class FontManager
{
    public static Font CargarFuenteEspanol(string ruta, int tamano)
    {
        // Caracteres desde espacio (32) hasta ÿ (255).
        // Incluye: ¿ ¡ á é í ó ú ñ Á É Í Ó Ú Ñ ü Ü, etc.
        const int primerCodigo = 32;
        const int ultimoCodigo = 255;

        int cantidad = ultimoCodigo - primerCodigo + 1;
        int[] codepoints = new int[cantidad];

        for (int i = 0; i < cantidad; i++)
        {
            codepoints[i] = primerCodigo + i;
        }

        Font fuente = Raylib.LoadFontEx(
            ruta,
            tamano,
            codepoints,
            codepoints.Length
        );

        if (fuente.Texture.Id == 0)
        {
            Console.WriteLine($"ERROR: No se pudo cargar la fuente: {ruta}");
        }

        return fuente;
    }
}