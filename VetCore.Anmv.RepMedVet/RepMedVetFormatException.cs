using System;

namespace VetCore.Anmv.RepMedVet
{
    /// <summary>
    /// Le document n'a pas la forme attendue d'un fichier REPMEDVET : XML invalide, valeur illisible, ou élément inconnu des DTO.
    /// </summary>
    public sealed class RepMedVetFormatException : Exception
    {
        public RepMedVetFormatException(string message, Exception innerException = null)
            : base(message, innerException)
        {
        }
    }
}
