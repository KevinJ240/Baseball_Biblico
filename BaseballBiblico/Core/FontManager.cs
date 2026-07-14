using Raylib_cs;

namespace BaseballBiblico.Core;

public static class FontManager
{
    public static Font CargarFuenteEspanol(
        string ruta,
        int tamaño)
    {
        if (!File.Exists(ruta))
        {
            throw new FileNotFoundException(
                $"No se encontró la fuente: {ruta}"
            );
        }

        int[] caracteres = CrearCaracteresEspanol();

        Font fuente = Raylib.LoadFontEx(
            ruta,
            tamaño,
            caracteres,
            caracteres.Length
        );

        if (fuente.Texture.Id <= 0)
        {
            throw new InvalidOperationException(
                $"No se pudo cargar la fuente: {ruta}"
            );
        }

        Raylib.SetTextureFilter(
            fuente.Texture,
            TextureFilter.Bilinear
        );

        return fuente;
    }

    private static int[] CrearCaracteresEspanol()
    {
        List<int> caracteres = new();

        // Caracteres ASCII normales.
        for (int i = 32; i <= 126; i++)
        {
            caracteres.Add(i);
        }

        // Caracteres utilizados en español.
        string adicionales =
            "áéíóúÁÉÍÓÚñÑüÜ¿¡";

        foreach (char caracter in adicionales)
        {
            if (!caracteres.Contains(caracter))
            {
                caracteres.Add(caracter);
            }
        }

        return caracteres.ToArray();
    }
}