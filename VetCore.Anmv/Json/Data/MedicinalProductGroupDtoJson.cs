using System;
using System.Linq;
using System.Text.Json.Serialization;
using VetCore.Anmv.Xml.Data;

namespace VetCore.Anmv.Json.Data
{
    public sealed class MedicinalProductGroupDtoJson
    {
        [JsonPropertyName("informations")]
        public InformationsDtoJson Informations { get; set; }

        [JsonPropertyName("medicinalProducts")]
        public MedicinalProductDtoJson[] MedicinalProducts { get; set; }
    }

    public sealed class InformationsDtoJson
    {
        [JsonPropertyName("dateJeuDeDonnees")]
        public DateTime DateJeuDeDonnees { get; set; }
    }

    public sealed class MedicinalProductDtoJson
    {
        [JsonPropertyName("srcId")]
        public int SrcId { get; set; }

        [JsonPropertyName("nom")]
        public string Nom { get; set; }

        [JsonPropertyName("num")]
        public string Num { get; set; }

        [JsonPropertyName("termTit")]
        public int TermTit { get; set; }

        [JsonPropertyName("termNat")]
        public int TermNat { get; set; }

        [JsonPropertyName("termTypProc")]
        public int TermTypProc { get; set; }

        [JsonPropertyName("termStatAuto")]
        public int TermStatAuto { get; set; }

        [JsonPropertyName("dateAmm")]
        public DateTime DateAmm { get; set; }

        [JsonPropertyName("termFp")]
        public int TermFp { get; set; }

        [JsonPropertyName("numAmm")]
        public string NumAmm { get; set; }

        [JsonPropertyName("permId")]
        public string PermId { get; set; }

        [JsonPropertyName("prodId")]
        public Guid? ProdId { get; set; }

        [JsonPropertyName("majRcp")]
        public DateTime MajRcp { get; set; }

        [JsonPropertyName("lienRcp")]
        public string LienRcp { get; set; }

        [JsonPropertyName("compositions")]
        public CompositionDtoJson[] Compositions { get; set; }

        [JsonPropertyName("voiesAdmin")]
        public VoieAdministrationDtoJson[] VoiesAdmin { get; set; }

        [JsonPropertyName("modeleDestineVente")]
        public ModeleDestineVenteDtoJson[] ModeleDestineVente { get; set; }

        [JsonPropertyName("mdvCodesGtin")]
        public MdvCodesGtinDtoJson[] MdvCodesGtin { get; set; }

        [JsonPropertyName("excipientQsp")]
        public ExcipientQspDtoJson ExcipientQsp { get; set; }

        [JsonPropertyName("atcvetCode")]
        public string[] AtcvetCode { get; set; }

        [JsonPropertyName("paragraphesRcp")]
        public ParaRcpDtoJson[] ParagraphesRcp { get; set; }
    }

    public sealed class CompositionDtoJson
    {
        [JsonPropertyName("sa")]
        public SaDtoJson Sa { get; set; }

        [JsonPropertyName("fraction")]
        public FractionDtoJson Fraction { get; set; }
    }

    public sealed class SaDtoJson
    {
        [JsonPropertyName("termSa")]
        public int TermSa { get; set; }

        [JsonPropertyName("quantite")]
        public string Quantite { get; set; }

        [JsonPropertyName("termUnite")]
        public int TermUnite { get; set; }
    }

    public sealed class FractionDtoJson
    {
        [JsonPropertyName("termSa")]
        public int TermSa { get; set; }

        [JsonPropertyName("quantite")]
        public string Quantite { get; set; }

        [JsonPropertyName("termUnite")]
        public int TermUnite { get; set; }
    }

    public sealed class VoieAdministrationDtoJson
    {
        [JsonPropertyName("termVa")]
        public int TermVa { get; set; }

        [JsonPropertyName("termEsp")]
        public int TermEsp { get; set; }

        [JsonPropertyName("termDenr")]
        public int TermDenr { get; set; }

        [JsonPropertyName("qteTa")]
        public string QteTa { get; set; }

        [JsonPropertyName("termUnite")]
        public int? TermUnite { get; set; }
    }

    public sealed class ModeleDestineVenteDtoJson
    {
        [JsonPropertyName("libMod")]
        public string LibMod { get; set; }

        [JsonPropertyName("nbUnit")]
        public string NbUnit { get; set; }

        [JsonPropertyName("termPres")]
        public int TermPres { get; set; }

        [JsonPropertyName("termCd")]
        public int TermCd { get; set; }

        [JsonPropertyName("libCondp")]
        public string LibCondp { get; set; }
    }

    public sealed class MdvCodesGtinDtoJson
    {
        [JsonPropertyName("libMod")]
        public string LibMod { get; set; }

        [JsonPropertyName("packId")]
        public Guid PackId { get; set; }

        [JsonPropertyName("codeGtin")]
        public string CodeGtin { get; set; }

        [JsonPropertyName("numAmm")]
        public string NumAmm { get; set; }
    }

    public sealed class ExcipientQspDtoJson
    {
        [JsonPropertyName("qteQsp")]
        public string QteQsp { get; set; }

        [JsonPropertyName("termPres")]
        public int? TermPres { get; set; }

        [JsonPropertyName("termUnite")]
        public string TermUnite { get; set; }
    }

    public sealed class ParaRcpDtoJson
    {
        [JsonPropertyName("termTitre")]
        public int TermTitre { get; set; }

        [JsonPropertyName("contenu")]
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
                AtcvetCode = jsonDto.AtcvetCode?.ToList(),
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