using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using VetCore.Anmv.RepMedVet.Xml;

namespace VetCore.Anmv.RepMedVet.Json
{
    public sealed class MedecineRepositoryDtoJson
    {
        [Required]
        [JsonPropertyName("exchangedDocument")]
        public ExchangedDocumentDtoJson ExchangedDocument { get; set; }

        [Required]
        [JsonPropertyName("sender")]
        public SenderDtoJson Sender { get; set; }

        [Required]
        [MinLength(1)]
        [JsonPropertyName("products")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        // Doit contenir au moins un élément
        public ProductDtoJson[] Products { get; set; } = Array.Empty<ProductDtoJson>();
    }

    public sealed class ExchangedDocumentDtoJson
    {
        [Required]
        [JsonPropertyName("issueDatetime")]
        public DateTimeOffset IssueDatetime { get; set; }

        [Required]
        [JsonPropertyName("fileName")]
        public string FileName { get; set; }

        [Required]
        [JsonPropertyName("typeCode")]
        public string TypeCode { get; set; }

        [Required]
        [JsonPropertyName("exchangeScenario")]
        public string ExchangeScenario { get; set; }
    }

    public sealed class SenderDtoJson
    {
        [Required]
        [JsonPropertyName("idParty")]
        public PartyIdDtoJson IdParty { get; set; }

        [Required]
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public sealed class PartyIdDtoJson
    {
        [Required]
        [JsonPropertyName("typeId")]
        public string TypeId { get; set; }

        [Required]
        [JsonPropertyName("value")]
        public string Value { get; set; }
    }

    /// <summary>
    /// Valeur d'un dictionnaire ANMV.
    /// </summary>
    public sealed class CodedValueDtoJson
    {
        [Required]
        [Range(0, int.MaxValue)]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(RepMedVetConstants.LABEL_MAX_LENGTH)]
        [JsonPropertyName("label")]
        // Chaîne non vide, maximum 255 caractères
        public string Label { get; set; }
    }

    public sealed class ProductDtoJson
    {
        [Required]
        [JsonPropertyName("product")]
        public CodedValueDtoJson Product { get; set; }

        [Required]
        [JsonPropertyName("marketingAuthorisationHolder")]
        public CodedValueDtoJson MarketingAuthorisationHolder { get; set; }

        [JsonPropertyName("atcVetCodes")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string[] AtcVetCodes { get; set; } = Array.Empty<string>();

        [JsonPropertyName("authorisationStatus")]
        // Optionnel
        public CodedValueDtoJson AuthorisationStatus { get; set; }

        [JsonPropertyName("authorisationStatusDate")]
        // Optionnel
        public DateTime? AuthorisationStatusDate { get; set; }

        /// <summary>
        /// Unité élémentaire officielle. Son identifiant vient de deux dictionnaires ANMV : seul le libellé est sans ambiguïté.
        /// </summary>
        [JsonPropertyName("qspUnit")]
        // Optionnel
        public CodedValueDtoJson QspUnit { get; set; }

        /// <summary>
        /// Les cessions de ce médicament doivent être transmises à CalypsoVet. Obligatoire en JSON : une absence ne vaut pas « non ».
        /// </summary>
        [JsonRequired]
        [JsonPropertyName("isSubjectToReporting")]
        public bool IsSubjectToReporting { get; set; }

        [Required]
        [MinLength(1)]
        [JsonPropertyName("drugs")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        // Le fichier ne contient que des médicaments ayant au moins un GTIN
        public DrugDtoJson[] Drugs { get; set; } = Array.Empty<DrugDtoJson>();

        [JsonPropertyName("activeSubstances")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public ActiveSubstanceDtoJson[] ActiveSubstances { get; set; } = Array.Empty<ActiveSubstanceDtoJson>();

        [JsonPropertyName("targetSpecies")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public CodedValueDtoJson[] TargetSpecies { get; set; } = Array.Empty<CodedValueDtoJson>();

        [JsonPropertyName("withdrawalPeriods")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public WithdrawalPeriodDtoJson[] WithdrawalPeriods { get; set; } = Array.Empty<WithdrawalPeriodDtoJson>();

        [Required]
        [JsonPropertyName("pharmaceuticalForm")]
        public CodedValueDtoJson PharmaceuticalForm { get; set; }

        [JsonPropertyName("summaryOfProductCharacteristics")]
        // Optionnel : absent pour les médicaments issus d'une procédure centralisée
        public SummaryOfProductCharacteristicsDtoJson SummaryOfProductCharacteristics { get; set; }
    }

    public sealed class DrugDtoJson
    {
        [Required]
        [RegularExpression(RepMedVetConstants.GTIN_PATTERN)]
        [JsonPropertyName("gtinCode")]
        // Chaîne de 14 chiffres exactement
        public string GtinCode { get; set; }

        [Required]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [MaxLength(RepMedVetConstants.AUTHORISATION_NUMBER_MAX_LENGTH)]
        [JsonPropertyName("authorisationNumber")]
        // Optionnel : parfois vide dans le fichier
        public string AuthorisationNumber { get; set; }

        [Required]
        [MaxLength(RepMedVetConstants.PACKAGING_MAX_LENGTH)]
        [JsonPropertyName("packaging")]
        public string Packaging { get; set; }
    }

    public sealed class ActiveSubstanceDtoJson
    {
        [Required]
        [JsonPropertyName("activeSubstance")]
        public CodedValueDtoJson ActiveSubstance { get; set; }

        [JsonPropertyName("activeSubstanceConcentration")]
        // Optionnel
        public ConcentrationDtoJson ActiveSubstanceConcentration { get; set; }

        [JsonPropertyName("activeFraction")]
        // Optionnel
        public CodedValueDtoJson ActiveFraction { get; set; }

        [JsonPropertyName("activeFractionConcentration")]
        // Optionnel
        public ConcentrationDtoJson ActiveFractionConcentration { get; set; }
    }

    public sealed class ConcentrationDtoJson
    {
        [Required]
        [MaxLength(RepMedVetConstants.QUANTITY_MAX_LENGTH)]
        [JsonPropertyName("quantity")]
        // Texte de la source : virgules, points et espaces y coexistent
        public string Quantity { get; set; }

        [Required]
        [JsonPropertyName("unit")]
        public CodedValueDtoJson Unit { get; set; }
    }

    public sealed class WithdrawalPeriodDtoJson
    {
        [Required]
        [JsonPropertyName("routeOfAdministration")]
        public CodedValueDtoJson RouteOfAdministration { get; set; }

        [Required]
        [JsonPropertyName("targetSpecies")]
        public CodedValueDtoJson TargetSpecies { get; set; }

        [JsonPropertyName("tissue")]
        // Optionnel
        public CodedValueDtoJson Tissue { get; set; }

        [MaxLength(RepMedVetConstants.QUANTITY_MAX_LENGTH)]
        [JsonPropertyName("quantity")]
        // Optionnel
        public string Quantity { get; set; }

        [JsonPropertyName("unit")]
        // Optionnel
        public CodedValueDtoJson Unit { get; set; }

        [MaxLength(RepMedVetConstants.COMMENT_MAX_LENGTH)]
        [JsonPropertyName("comment")]
        // Optionnel : souvent vide dans le fichier
        public string Comment { get; set; }
    }

    public sealed class SummaryOfProductCharacteristicsDtoJson
    {
        [JsonPropertyName("indicationsForUse")]
        public string IndicationsForUse { get; set; }

        [JsonPropertyName("contraindications")]
        public string Contraindications { get; set; }

        [JsonPropertyName("specialWarnings")]
        public string SpecialWarnings { get; set; }

        [JsonPropertyName("specialPrecautionsForUse")]
        public SpecialPrecautionsForUseDtoJson SpecialPrecautionsForUse { get; set; }

        [JsonPropertyName("adverseEvents")]
        public string AdverseEvents { get; set; }

        [JsonPropertyName("interactionsWithOtherMedicinalProducts")]
        public string InteractionsWithOtherMedicinalProducts { get; set; }

        [JsonPropertyName("posologyAndRoutesOfAdministration")]
        public string PosologyAndRoutesOfAdministration { get; set; }

        [JsonPropertyName("specialPrecautionsForStorage")]
        public string SpecialPrecautionsForStorage { get; set; }
    }

    public sealed class SpecialPrecautionsForUseDtoJson
    {
        [JsonPropertyName("specialPrecautionsForUseInAnimals")]
        public string SpecialPrecautionsForUseInAnimals { get; set; }

        [JsonPropertyName("specialPrecautionsByThePersonAdministering")]
        public string SpecialPrecautionsByThePersonAdministering { get; set; }

        [JsonPropertyName("otherPrecautions")]
        public string OtherPrecautions { get; set; }
    }

    public static class MedecineRepositoryDtoExtensions
    {
        /// <summary>
        /// Converts a JSON MedecineRepositoryDtoJson object to a MedecineRepositoryDto XML object.
        /// </summary>
        public static MedecineRepositoryDto ToXmlDto(this MedecineRepositoryDtoJson jsonDto)
        {
            if (jsonDto == null)
                return null;

            return new MedecineRepositoryDto
            {
                ExchangedDocument = jsonDto.ExchangedDocument?.ToXmlDto(),
                Sender = jsonDto.Sender?.ToXmlDto(),
                Products = jsonDto.Products?.Select(p => p.ToXmlDto()).ToList(),
            };
        }

        public static ExchangedDocumentDto ToXmlDto(this ExchangedDocumentDtoJson jsonDto)
        {
            if (jsonDto == null)
                return null;

            return new ExchangedDocumentDto
            {
                IssueDatetime = jsonDto.IssueDatetime,
                FileName = jsonDto.FileName,
                TypeCode = jsonDto.TypeCode,
                ExchangeScenario = jsonDto.ExchangeScenario,
            };
        }

        public static SenderDto ToXmlDto(this SenderDtoJson jsonDto)
        {
            if (jsonDto == null)
                return null;

            return new SenderDto
            {
                IdParty = jsonDto.IdParty?.ToXmlDto(),
                Name = jsonDto.Name,
            };
        }

        public static PartyIdDto ToXmlDto(this PartyIdDtoJson jsonDto)
        {
            if (jsonDto == null)
                return null;

            return new PartyIdDto
            {
                TypeId = jsonDto.TypeId,
                Value = jsonDto.Value,
            };
        }

        public static CodedValueDto ToXmlDto(this CodedValueDtoJson jsonDto)
        {
            if (jsonDto == null)
                return null;

            return new CodedValueDto
            {
                Id = jsonDto.Id,
                Label = jsonDto.Label,
            };
        }

        public static ProductDto ToXmlDto(this ProductDtoJson jsonDto)
        {
            if (jsonDto == null)
                return null;

            return new ProductDto
            {
                Product = jsonDto.Product?.ToXmlDto(),
                MarketingAuthorisationHolder = jsonDto.MarketingAuthorisationHolder?.ToXmlDto(),
                AtcVetCodes = jsonDto.AtcVetCodes?.ToList(),
                AuthorisationStatus = jsonDto.AuthorisationStatus?.ToXmlDto(),
                AuthorisationStatusDate = jsonDto.AuthorisationStatusDate,
                QspUnit = jsonDto.QspUnit?.ToXmlDto(),
                IsSubjectToReporting = jsonDto.IsSubjectToReporting ? SubjectToReportingDto.Yes : SubjectToReportingDto.No,
                Drugs = jsonDto.Drugs?.Select(d => d.ToXmlDto()).ToList(),
                ActiveSubstances = jsonDto.ActiveSubstances?.Select(s => s.ToXmlDto()).ToList(),
                TargetSpecies = jsonDto.TargetSpecies?.Select(s => s.ToXmlDto()).ToList(),
                WithdrawalPeriods = jsonDto.WithdrawalPeriods?.Select(w => w.ToXmlDto()).ToList(),
                PharmaceuticalForm = jsonDto.PharmaceuticalForm?.ToXmlDto(),
                SummaryOfProductCharacteristics = jsonDto.SummaryOfProductCharacteristics?.ToXmlDto(),
            };
        }

        public static DrugDto ToXmlDto(this DrugDtoJson jsonDto)
        {
            if (jsonDto == null)
                return null;

            return new DrugDto
            {
                Id = new GtinDto { GtinCode = jsonDto.GtinCode },
                Name = jsonDto.Name,
                AuthorisationNumber = jsonDto.AuthorisationNumber,
                Packaging = jsonDto.Packaging,
            };
        }

        public static ActiveSubstanceDto ToXmlDto(this ActiveSubstanceDtoJson jsonDto)
        {
            if (jsonDto == null)
                return null;

            return new ActiveSubstanceDto
            {
                ActiveSubstance = jsonDto.ActiveSubstance?.ToXmlDto(),
                ActiveSubstanceConcentration = jsonDto.ActiveSubstanceConcentration?.ToXmlDto(),
                ActiveFraction = jsonDto.ActiveFraction?.ToXmlDto(),
                ActiveFractionConcentration = jsonDto.ActiveFractionConcentration?.ToXmlDto(),
            };
        }

        public static ConcentrationDto ToXmlDto(this ConcentrationDtoJson jsonDto)
        {
            if (jsonDto == null)
                return null;

            return new ConcentrationDto
            {
                Quantity = jsonDto.Quantity,
                Unit = jsonDto.Unit?.ToXmlDto(),
            };
        }

        public static WithdrawalPeriodDto ToXmlDto(this WithdrawalPeriodDtoJson jsonDto)
        {
            if (jsonDto == null)
                return null;

            return new WithdrawalPeriodDto
            {
                RouteOfAdministration = jsonDto.RouteOfAdministration?.ToXmlDto(),
                TargetSpecies = jsonDto.TargetSpecies?.ToXmlDto(),
                Tissue = jsonDto.Tissue?.ToXmlDto(),
                Quantity = jsonDto.Quantity,
                Unit = jsonDto.Unit?.ToXmlDto(),
                Comment = jsonDto.Comment,
            };
        }

        public static SummaryOfProductCharacteristicsDto ToXmlDto(this SummaryOfProductCharacteristicsDtoJson jsonDto)
        {
            if (jsonDto == null)
                return null;

            return new SummaryOfProductCharacteristicsDto
            {
                IndicationsForUse = jsonDto.IndicationsForUse,
                Contraindications = jsonDto.Contraindications,
                SpecialWarnings = jsonDto.SpecialWarnings,
                SpecialPrecautionsForUse = jsonDto.SpecialPrecautionsForUse?.ToXmlDto(),
                AdverseEvents = jsonDto.AdverseEvents,
                InteractionsWithOtherMedicinalProducts = jsonDto.InteractionsWithOtherMedicinalProducts,
                PosologyAndRoutesOfAdministration = jsonDto.PosologyAndRoutesOfAdministration,
                SpecialPrecautionsForStorage = jsonDto.SpecialPrecautionsForStorage,
            };
        }

        public static SpecialPrecautionsForUseDto ToXmlDto(this SpecialPrecautionsForUseDtoJson jsonDto)
        {
            if (jsonDto == null)
                return null;

            return new SpecialPrecautionsForUseDto
            {
                SpecialPrecautionsForUseInAnimals = jsonDto.SpecialPrecautionsForUseInAnimals,
                SpecialPrecautionsByThePersonAdministering = jsonDto.SpecialPrecautionsByThePersonAdministering,
                OtherPrecautions = jsonDto.OtherPrecautions,
            };
        }
    }
}
