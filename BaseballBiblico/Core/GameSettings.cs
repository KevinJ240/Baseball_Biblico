using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseballBiblico.Core;

public static class GameSettings
{
    public static string NombreEquipoA { get; set; } = "EQUIPO A";
    public static string NombreEquipoB { get; set; } = "EQUIPO B";

    public static int TotalInnings { get; set; } = 9;
}
