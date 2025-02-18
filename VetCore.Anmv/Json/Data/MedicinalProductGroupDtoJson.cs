using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using VetCore.Anmv.Xml.Data;

namespace VetCore.Anmv.Json.Data
{
    public sealed class MedicinalProductGroupDtoJson
    {
        [Required]
        [JsonPropertyName("informations")]
        public InformationsDtoJson Informations { get; set; }

        [Required]
        [JsonPropertyName("medicinalProducts")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        // Doit contenir au moins un élément
        public MedicinalProductDtoJson[] MedicinalProducts { get; set; } = Array.Empty<MedicinalProductDtoJson>();
    }

    public sealed class InformationsDtoJson
    {
        [JsonPropertyName("dateJeuDeDonnees")]
        [Required]
        public DateTime DateJeuDeDonnees { get; set; }
    }

    public sealed class MedicinalProductDtoJson
    {
        [JsonPropertyName("srcId")]
        [Required]
        public int SrcId { get; set; }

        [JsonPropertyName("nom")]
        [Required]
        [MinLength(1)]
        [MaxLength(255)]
        // Chaîne non vide, maximum 255 caractères
        public string Nom { get; set; }

        [JsonPropertyName("num")]
        [Required]
        [RegularExpression(@"^\d{7}$")]
        // Chaîne de 7 chiffres exactement
        public string Num { get; set; }

        [JsonPropertyName("termTit")]
        [Required]
        [Range(0, int.MaxValue)]
        public int TermTit { get; set; }

        [JsonPropertyName("termNat")]
        [Required]
        [Range(0, int.MaxValue)]
        public int TermNat { get; set; }

        [JsonPropertyName("termTypProc")]
        [Required]
        [Range(0, int.MaxValue)]
        public int TermTypProc { get; set; }

        [JsonPropertyName("termStatAuto")]
        [Required]
        [Range(0, int.MaxValue)]
        public int TermStatAuto { get; set; }

        [JsonPropertyName("dateAmm")]
        [Required]
        public DateTime DateAmm { get; set; }

        [JsonPropertyName("termFp")]
        [Required]
        [Range(0, int.MaxValue)]
        public int TermFp { get; set; }

        [JsonPropertyName("numAmm")]
        [MinLength(1)]
        [MaxLength(50)]
        // Optionnel : chaîne non vide maximum 50 caractères si présente
        public string NumAmm { get; set; }

        [JsonPropertyName("permId")]
        [RegularExpression(@"^\d{12}$")]
        // Optionnel : chaîne de 12 chiffres exactement si présente
        public string PermId { get; set; }

        [JsonPropertyName("prodId")]
        // Optionnel : GUID strict
        public Guid? ProdId { get; set; }

        [JsonPropertyName("majRcp")]
        // Optionnel
        public DateTime? MajRcp { get; set; }

        [JsonPropertyName("lienRcp")]
        [MinLength(1)]
        [MaxLength(300)]
        // Optionnel : URI avec 1 à 300 caractères
        public string LienRcp { get; set; }

        [JsonPropertyName("compositions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public CompositionDtoJson[] Compositions { get; set; } = Array.Empty<CompositionDtoJson>();

        [JsonPropertyName("voiesAdmin")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public VoieAdministrationDtoJson[] VoiesAdmin { get; set; } = Array.Empty<VoieAdministrationDtoJson>();

        [JsonPropertyName("modeleDestineVente")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public ModeleDestineVenteDtoJson[] ModeleDestineVente { get; set; } = Array.Empty<ModeleDestineVenteDtoJson>();

        [JsonPropertyName("mdvCodesGtin")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public MdvCodesGtinDtoJson[] MdvCodesGtin { get; set; } = Array.Empty<MdvCodesGtinDtoJson>();

        [JsonPropertyName("excipientQsp")]
        public ExcipientQspDtoJson ExcipientQsp { get; set; }

        [JsonPropertyName("atcvetCodes")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        // Chaque code doit correspondre au pattern : Q[A-Z]\d{2}[A-Z\d]{0,4}
        public string[] AtcvetCodes { get; set; } = Array.Empty<string>();

        [JsonPropertyName("paragraphesRcp")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public ParaRcpDtoJson[] ParagraphesRcp { get; set; } = Array.Empty<ParaRcpDtoJson>();
    }

    public sealed class CompositionDtoJson
    {
        [JsonPropertyName("sa")]
        [Required]
        public SaDtoJson Sa { get; set; }

        [JsonPropertyName("fraction")]
        public FractionDtoJson Fraction { get; set; }
    }

    public sealed class SaDtoJson
    {
        [JsonPropertyName("termSa")]
        [Required]
        [Range(0, int.MaxValue)]
        public int TermSa { get; set; }

        [JsonPropertyName("quantite")]
        [MinLength(1)]
        [MaxLength(30)]
        // Optionnel : chaîne non vide maximum 30 caractères si présente
        public string Quantite { get; set; }

        [JsonPropertyName("termUnite")]
        [Range(0, int.MaxValue)]
        // Optionnel
        public int? TermUnite { get; set; }
    }

    public sealed class FractionDtoJson
    {
        [JsonPropertyName("termSa")]
        [Required]
        [Range(0, int.MaxValue)]
        public int TermSa { get; set; }

        [JsonPropertyName("quantite")]
        [Required]
        [MinLength(1)]
        [MaxLength(30)]
        // Chaîne non vide, maximum 30 caractères
        public string Quantite { get; set; }

        [JsonPropertyName("termUnite")]
        [Required]
        [Range(0, int.MaxValue)]
        public int TermUnite { get; set; }
    }

    public sealed class VoieAdministrationDtoJson
    {
        [JsonPropertyName("termVa")]
        [Required]
        [Range(0, int.MaxValue)]
        public int TermVa { get; set; }

        [JsonPropertyName("termEsp")]
        [Required]
        [Range(0, int.MaxValue)]
        public int TermEsp { get; set; }

        [JsonPropertyName("termDenr")]
        [Range(0, int.MaxValue)]
        // Optionnel
        public int? TermDenr { get; set; }

        [JsonPropertyName("qteTa")]
        [MaxLength(20)] // string due to "log 10 ..." cap to 20 chars
        // Optionnel
        public string QteTa { get; set; }

        [JsonPropertyName("termUnite")]
        [Range(0, int.MaxValue)]
        // Optionnel
        public int? TermUnite { get; set; }
    }

    public sealed class ModeleDestineVenteDtoJson
    {
        [JsonPropertyName("libMod")]
        [Required]
        [MinLength(1)]
        [MaxLength(255)]
        // Chaîne non vide, maximum 255 caractères
        public string LibMod { get; set; }

        [JsonPropertyName("nbUnit")]
        [MinLength(1)]
        [MaxLength(20)]
        // Optionnel : chaîne non vide, maximum 20 caractères si présente
        public string NbUnit { get; set; }

        [JsonPropertyName("termPres")]
        [Range(0, int.MaxValue)]
        // Optionnel
        public int? TermPres { get; set; }

        [JsonPropertyName("termCd")]
        [Range(0, int.MaxValue)]
        // Optionnel
        public int? TermCd { get; set; }

        [JsonPropertyName("libCondp")]
        [MaxLength(255)]
        // Optionnel : maximum 255 caractères
        public string LibCondp { get; set; }
    }

    public sealed class MdvCodesGtinDtoJson
    {
        [JsonPropertyName("libMod")]
        [Required]
        [MinLength(1)]
        [MaxLength(255)]
        // Chaîne non vide, maximum 255 caractères
        public string LibMod { get; set; }

        [JsonPropertyName("packId")]
        // Optionnel : GUID strict
        public Guid? PackId { get; set; }

        [JsonPropertyName("codeGtin")]
        [RegularExpression(@"^\d{8,14}$")]
        // Optionnel : chaîne numérique de 8 à 14 chiffres
        public string CodeGtin { get; set; }

        [JsonPropertyName("numAmm")]
        [MinLength(1)]
        [MaxLength(50)]
        // Optionnel : chaîne non vide, maximum 50 caractères si présente
        public string NumAmm { get; set; }
    }

    public sealed class ExcipientQspDtoJson
    {
        [JsonPropertyName("qteQsp")]
        [Required]
        [MinLength(1)]
        [MaxLength(100)]
        // Chaîne non vide, maximum 100 caractères
        public string QteQsp { get; set; }

        [JsonPropertyName("termPres")]
        [Range(0, int.MaxValue)]
        // Optionnel
        public int? TermPres { get; set; }

        [JsonPropertyName("termUnite")]
        [Range(0, int.MaxValue)]
        public int? TermUnite { get; set; }
    }

    public sealed class ParaRcpDtoJson
    {
        [JsonPropertyName("termTitre")]
        [Required]
        [Range(0, int.MaxValue)]
        public int TermTitre { get; set; }

        [JsonPropertyName("contenu")]
        [Required]
        // Peut être vide
        public string Contenu { get; set; }
    }

    public static class MedicinalProductGroupDtoExtensions
    {
        /// <summary>
        /// Converts a JSON MedicinalProductGroupDtoJson object to an XML MedicinalProductGroupDto object.
        /// </summary>
        public static MedicinalProductGroupDto ToXmlDto(this MedicinalProductGroupDtoJson jsonDto)
        {
            if (jsonDto == null)
                return null;

            return new MedicinalProductGroupDto
            {
                Informations = jsonDto.Informations?.ToXmlDto(),
                MedicinalProducts = jsonDto.MedicinalProducts?.Select(mp => mp.ToXmlDto()).ToList(),
            };
        }

        public static InformationsDto ToXmlDto(this InformationsDtoJson jsonDto)
        {
            if (jsonDto == null)
                return null;

            return new InformationsDto
            {
                DateJeuDeDonnees = jsonDto.DateJeuDeDonnees,
            };
        }

        public static MedicinalProductDto ToXmlDto(this MedicinalProductDtoJson jsonDto)
        {
            if (jsonDto == null)
                return null;

            return new MedicinalProductDto
            {
                SrcId = jsonDto.SrcId,
                Nom = jsonDto.Nom,
                Num = jsonDto.Num,
                TermTit = jsonDto.TermTit,
                TermNat = jsonDto.TermNat,
                TermTypProc = jsonDto.TermTypProc,
                TermStatAuto = jsonDto.TermStatAuto,
                DateAmm = jsonDto.DateAmm,
                TermFp = jsonDto.TermFp,
                NumAmm = jsonDto.NumAmm,
                PermId = jsonDto.PermId,
                ProdId = jsonDto.ProdId,
                MajRcp = jsonDto.MajRcp,
                LienRcp = jsonDto.LienRcp,
                Compositions = jsonDto.Compositions?.Select(c => c.ToXmlDto()).ToList(),
                VoiesAdmin = jsonDto.VoiesAdmin?.Select(va => va.ToXmlDto()).ToList(),
                ModeleDestineVente = jsonDto.ModeleDestineVente?.Select(mdv => mdv.ToXmlDto()).ToList(),
                MdvCodesGtin = jsonDto.MdvCodesGtin?.Select(mdg => mdg.ToXmlDto()).ToList(),
                ExcipientQsp = jsonDto.ExcipientQsp?.ToXmlDto(),
                AtcvetCodes = jsonDto.AtcvetCodes?.ToList(),
                ParagraphesRcp = jsonDto.ParagraphesRcp?.Select(pr => pr.ToXmlDto()).ToList(),
            };
        }

        public static CompositionDto ToXmlDto(this CompositionDtoJson jsonDto)
        {
            if (jsonDto == null)
                return null;

            return new CompositionDto
            {
                Sa = jsonDto.Sa?.ToXmlDto(),
                Fraction = jsonDto.Fraction?.ToXmlDto(),
            };
        }

        public static SaDto ToXmlDto(this SaDtoJson jsonDto)
        {
            if (jsonDto == null)
                return null;

            return new SaDto
            {
                TermSa = jsonDto.TermSa,
                Quantite = jsonDto.Quantite,
                TermUnite = jsonDto.TermUnite,
            };
        }

        public static FractionDto ToXmlDto(this FractionDtoJson jsonDto)
        {
            if (jsonDto == null)
                return null;

            return new FractionDto
            {
                TermSa = jsonDto.TermSa,
                Quantite = jsonDto.Quantite,
                TermUnite = jsonDto.TermUnite,
            };
        }

        public static VoieAdministrationDto ToXmlDto(this VoieAdministrationDtoJson jsonDto)
        {
            if (jsonDto == null)
                return null;

            return new VoieAdministrationDto
            {
                TermVa = jsonDto.TermVa,
                TermEsp = jsonDto.TermEsp,
                TermDenr = jsonDto.TermDenr,
                QteTa = jsonDto.QteTa,
                TermUnite = jsonDto.TermUnite,
            };
        }

        public static ModeleDestineVenteDto ToXmlDto(this ModeleDestineVenteDtoJson jsonDto)
        {
            if (jsonDto == null)
                return null;

            return new ModeleDestineVenteDto
            {
                LibMod = jsonDto.LibMod,
                NbUnit = jsonDto.NbUnit,
                TermPres = jsonDto.TermPres,
                TermCd = jsonDto.TermCd,
                LibCondp = jsonDto.LibCondp,
            };
        }

        public static MdvCodesGtinDto ToXmlDto(this MdvCodesGtinDtoJson jsonDto)
        {
            if (jsonDto == null)
                return null;

            return new MdvCodesGtinDto
            {
                LibMod = jsonDto.LibMod,
                PackId = jsonDto.PackId,
                CodeGtin = jsonDto.CodeGtin,
                NumAmm = jsonDto.NumAmm,
            };
        }

        public static ExcipientQspDto ToXmlDto(this ExcipientQspDtoJson jsonDto)
        {
            if (jsonDto == null)
                return null;

            return new ExcipientQspDto
            {
                QteQsp = jsonDto.QteQsp,
                TermPres = jsonDto.TermPres,
                TermUnite = jsonDto.TermUnite,
            };
        }

        public static ParaRcpDto ToXmlDto(this ParaRcpDtoJson jsonDto)
        {
            if (jsonDto == null)
                return null;

            return new ParaRcpDto
            {
                TermTitre = jsonDto.TermTitre,
                Contenu = jsonDto.Contenu,
            };
        }
    }
}