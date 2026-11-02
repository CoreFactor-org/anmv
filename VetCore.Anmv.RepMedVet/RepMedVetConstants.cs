namespace VetCore.Anmv.RepMedVet
{
    /// <summary>
    /// Bornes des chaînes des DTO. REPMEDVET n'a pas de schéma public : elles reprennent celles du XSD ANMV pour les mêmes données,
    /// issues de la même base. Les champs sans équivalent connu, dont les textes du RCP, ne sont pas bornés.
    /// </summary>
    public static class RepMedVetConstants
    {
        public const string GTIN_PATTERN = @"^\d{14}$";
        public const int NAME_MAX_LENGTH = 255;
        public const int LABEL_MAX_LENGTH = 255;
        public const int PACKAGING_MAX_LENGTH = 255;
        public const int AUTHORISATION_NUMBER_MAX_LENGTH = 50;
        public const int QUANTITY_MAX_LENGTH = 30;
        public const int COMMENT_MAX_LENGTH = 4000;
    }
}
