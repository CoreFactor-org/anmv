using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using VetCore.Anmv.RepMedVet.Json;

namespace VetCore.Anmv.RepMedVet.Xml
{
    /// <summary>
    /// Fichier REPMEDVET.xml : tous les médicaments vétérinaires autorisés ayant au moins un GTIN déclaré à l'ANMV.
    /// Les noms suivent ceux du fichier, y compris « Medecine ».
    /// </summary>
    [XmlRoot("MedecineRepository")]
    public sealed class MedecineRepositoryDto
    {
        [XmlElement("ACExchangedDocumentSimple")] public ExchangedDocumentDto ExchangedDocument { get; set; }

        [XmlElement("Sender")] public SenderDto Sender { get; set; }

        [XmlElement("Product")] public List<ProductDto> Products { get; set; }
    }

    public sealed class ExchangedDocumentDto
    {
        /// <summary>
        /// "IssueDatetime" (ex : 2026-10-22T05:03:13.1297425+02:00) : date de génération du fichier
        /// </summary>
        [XmlElement("IssueDatetime")]
        public DateTimeOffset IssueDatetime { get; set; }

        [XmlElement("FileName")] public string FileName { get; set; }

        [XmlElement("TypeCode")] public string TypeCode { get; set; }

        /// <summary>
        /// "ExchangeScenario" (ex : MED 0.9) : version du format d'échange
        /// </summary>
        [XmlElement("ExchangeScenario")]
        public string ExchangeScenario { get; set; }
    }

    public sealed class SenderDto
    {
        [XmlElement("IDParty")] public PartyIdDto IdParty { get; set; }

        [XmlElement("Name")] public string Name { get; set; }
    }

    public sealed class PartyIdDto
    {
        /// <summary>
        /// Attribut "TypeID" (ex : SIRET) : nature de l'identifiant
        /// </summary>
        [XmlAttribute("TypeID")]
        public string TypeId { get; set; }

        [XmlText] public string Value { get; set; }
    }

    /// <summary>
    /// Valeur d'un dictionnaire ANMV : son libellé, et son identifiant en attribut "ID".
    /// </summary>
    public sealed class CodedValueDto
    {
        [XmlAttribute("ID")] public int Id { get; set; }

        [XmlText] public string Label { get; set; }
    }

    /// <summary>
    /// Valeur de l'élément "IsSubjectToReporting". Toute autre valeur que 0 ou 1 rend le fichier illisible.
    /// </summary>
    public enum SubjectToReportingDto
    {
        [XmlEnum("0")] No = 0,

        [XmlEnum("1")] Yes = 1,
    }

    public sealed class ProductDto
    {
        /// <summary>
        /// "Product" (ex : ID 85, GLEPTOSIL) : identifiant et nom du médicament
        /// </summary>
        [XmlElement("Product")]
        public CodedValueDto Product { get; set; }

        /// <summary>
        /// "MarketingAuthorisationHolder" : dernier titulaire de l'AMM
        /// </summary>
        [XmlElement("MarketingAuthorisationHolder")]
        public CodedValueDto MarketingAuthorisationHolder { get; set; }

        /// <summary>
        /// "AtcVetCode" (ex : QB03AC90) : un médicament peut en porter plusieurs
        /// </summary>
        [XmlElement("AtcVetCode")]
        public List<string> AtcVetCodes { get; set; }

        /// <summary>
        /// "AuthorisationStatus" (ex : ID 4, AMM illimitée) : statut de l'autorisation (MAY BE NULL)
        /// </summary>
        [XmlElement("AuthorisationStatus")]
        public CodedValueDto AuthorisationStatus { get; set; }

        /// <summary>
        /// "AuthorisationStatusDate" (ex : 2009-07-23) : date du statut de l'autorisation (MAY BE NULL)
        /// </summary>
        [XmlElement("AuthorisationStatusDate", DataType = "date")]
        public DateTime? AuthorisationStatusDate { get; set; }

        /// <summary>
        /// "QspUnit" (ex : ID 11, mL) : unité élémentaire officielle des quantités transmises à CalypsoVet (MAY BE NULL).
        /// <remarks>
        /// Son identifiant vient de deux dictionnaires ANMV, les unités et les présentations : ID 5 désigne g ou Dispositif selon
        /// le cas. Seul le libellé est sans ambiguïté.
        /// </remarks>
        /// </summary>
        [XmlElement("QspUnit")]
        public CodedValueDto QspUnit { get; set; }

        /// <summary>
        /// "IsSubjectToReporting" (0 ou 1) : les cessions de ce médicament doivent être transmises à CalypsoVet
        /// </summary>
        [XmlElement("IsSubjectToReporting")]
        public SubjectToReportingDto? IsSubjectToReporting { get; set; }

        [XmlElement("Drug")] public List<DrugDto> Drugs { get; set; }

        [XmlElement("ActiveSubstance")] public List<ActiveSubstanceDto> ActiveSubstances { get; set; }

        [XmlElement("TargetSpecies")] public List<CodedValueDto> TargetSpecies { get; set; }

        [XmlElement("WithdrawalPeriod")] public List<WithdrawalPeriodDto> WithdrawalPeriods { get; set; }

        [XmlElement("PharmaceuticalForm")] public CodedValueDto PharmaceuticalForm { get; set; }

        /// <summary>
        /// "SummaryOfProductCharacteristics" : RCP, absent pour les médicaments issus d'une procédure centralisée (MAY BE NULL)
        /// </summary>
        [XmlElement("SummaryOfProductCharacteristics")]
        public SummaryOfProductCharacteristicsDto SummaryOfProductCharacteristics { get; set; }

        /// <summary>
        /// Une date absente ne s'écrit pas : sans cela, le sérialiseur produirait un élément xsi:nil étranger au fichier.
        /// </summary>
        public bool ShouldSerializeAuthorisationStatusDate() => AuthorisationStatusDate.HasValue;

        /// <summary>
        /// Même règle : un indicateur absent ne s'écrit pas. Nullable, il reste distinct de « non » : la lecture refuse son absence.
        /// </summary>
        public bool ShouldSerializeIsSubjectToReporting() => IsSubjectToReporting.HasValue;
    }

    /// <summary>
    /// Présentation d'un médicament, identifiée par son GTIN.
    /// </summary>
    public sealed class DrugDto
    {
        [XmlElement("ID")] public GtinDto Id { get; set; }

        /// <summary>
        /// "Name" (ex : GLEPTOSIL - Boîte de 1 flacon rigide de 250 mL) : nom du médicament et de la présentation
        /// </summary>
        [XmlElement("Name")]
        public string Name { get; set; }

        /// <summary>
        /// "AuthorisationNumber" (ex : FR/V/7008561 5/1979) : n° d'AMM, propre à la présentation en procédure centralisée (MAY BE EMPTY)
        /// </summary>
        [XmlElement("AuthorisationNumber")]
        public string AuthorisationNumber { get; set; }

        [XmlElement("Packaging")] public string Packaging { get; set; }
    }

    public sealed class GtinDto
    {
        /// <summary>
        /// Attribut "GTINCode" : GTIN sur 14 chiffres
        /// </summary>
        [XmlAttribute("GTINCode")]
        public string GtinCode { get; set; }
    }

    public sealed class ActiveSubstanceDto
    {
        [XmlElement("ActiveSubstance")] public CodedValueDto ActiveSubstance { get; set; }

        [XmlElement("ActiveSubstanceConcentration")] public ConcentrationDto ActiveSubstanceConcentration { get; set; } // (MAY BE NULL)

        [XmlElement("ActiveFraction")] public CodedValueDto ActiveFraction { get; set; } // (MAY BE NULL)

        [XmlElement("ActiveFractionConcentration")] public ConcentrationDto ActiveFractionConcentration { get; set; } // (MAY BE NULL)
    }

    public sealed class ConcentrationDto
    {
        /// <summary>
        /// "Quantity" (ex : 532,6) : conservée telle que publiée, la source mêlant virgules, points et espaces
        /// </summary>
        [XmlElement("Quantity")]
        public string Quantity { get; set; }

        [XmlElement("Unit")] public CodedValueDto Unit { get; set; }
    }

    /// <summary>
    /// Temps d'attente d'une voie, d'une espèce et d'une denrée. Sans denrée ni quantité, seul le commentaire renseigne.
    /// </summary>
    public sealed class WithdrawalPeriodDto
    {
        [XmlElement("RouteOfAdministration")] public CodedValueDto RouteOfAdministration { get; set; }

        [XmlElement("TargetSpecies")] public CodedValueDto TargetSpecies { get; set; }

        [XmlElement("Tissue")] public CodedValueDto Tissue { get; set; } // (MAY BE NULL)

        [XmlElement("Quantity")] public string Quantity { get; set; } // (MAY BE NULL)

        [XmlElement("Unit")] public CodedValueDto Unit { get; set; } // (MAY BE NULL)

        [XmlElement("Comment")] public string Comment { get; set; } // (MAY BE EMPTY)
    }

    public sealed class SummaryOfProductCharacteristicsDto
    {
        [XmlElement("IndicationsForUse")] public string IndicationsForUse { get; set; }

        [XmlElement("Contraindications")] public string Contraindications { get; set; }

        [XmlElement("SpecialWarnings")] public string SpecialWarnings { get; set; }

        [XmlElement("SpecialPrecautionsForUse")] public SpecialPrecautionsForUseDto SpecialPrecautionsForUse { get; set; }

        [XmlElement("AdverseEvents")] public string AdverseEvents { get; set; }

        [XmlElement("InteractionsWithOtherMedicinalProducts")] public string InteractionsWithOtherMedicinalProducts { get; set; }

        [XmlElement("PosologyAndRoutesOfAdministration")] public string PosologyAndRoutesOfAdministration { get; set; }

        [XmlElement("SpecialPrecautionsForStorage")] public string SpecialPrecautionsForStorage { get; set; }
    }

    public sealed class SpecialPrecautionsForUseDto
    {
        [XmlElement("SpecialPrecautionsForUseInAnimals")] public string SpecialPrecautionsForUseInAnimals { get; set; }

        [XmlElement("SpecialPrecautionsByThePersonAdministering")] public string SpecialPrecautionsByThePersonAdministering { get; set; }

        [XmlElement("OtherPrecautions")] public string OtherPrecautions { get; set; }
    }

    public static class MedecineRepositoryDtoExtensions
    {
        /// <summary>
        /// Converts a MedecineRepositoryDto XML object to a JSON MedecineRepositoryDtoJson object.
        /// </summary>
        public static MedecineRepositoryDtoJson ToJsonDto(this MedecineRepositoryDto xmlDto)
        {
            if (xmlDto == null)
                return null;

            return new MedecineRepositoryDtoJson
            {
                ExchangedDocument = xmlDto.ExchangedDocument?.ToJsonDto(),
                Sender = xmlDto.Sender?.ToJsonDto(),
                Products = xmlDto.Products?.Select(p => p.ToJsonDto()).ToArray() ?? Array.Empty<ProductDtoJson>(),
            };
        }

        public static ExchangedDocumentDtoJson ToJsonDto(this ExchangedDocumentDto xmlDto)
        {
            if (xmlDto == null)
                return null;

            return new ExchangedDocumentDtoJson
            {
                IssueDatetime = xmlDto.IssueDatetime,
                FileName = xmlDto.FileName,
                TypeCode = xmlDto.TypeCode,
                ExchangeScenario = xmlDto.ExchangeScenario,
            };
        }

        public static SenderDtoJson ToJsonDto(this SenderDto xmlDto)
        {
            if (xmlDto == null)
                return null;

            return new SenderDtoJson
            {
                IdParty = xmlDto.IdParty?.ToJsonDto(),
                Name = xmlDto.Name,
            };
        }

        public static PartyIdDtoJson ToJsonDto(this PartyIdDto xmlDto)
        {
            if (xmlDto == null)
                return null;

            return new PartyIdDtoJson
            {
                TypeId = xmlDto.TypeId,
                Value = xmlDto.Value,
            };
        }

        public static CodedValueDtoJson ToJsonDto(this CodedValueDto xmlDto)
        {
            if (xmlDto == null)
                return null;

            return new CodedValueDtoJson
            {
                Id = xmlDto.Id,
                Label = xmlDto.Label,
            };
        }

        public static ProductDtoJson ToJsonDto(this ProductDto xmlDto)
        {
            if (xmlDto == null)
                return null;

            return new ProductDtoJson
            {
                Product = xmlDto.Product?.ToJsonDto(),
                MarketingAuthorisationHolder = xmlDto.MarketingAuthorisationHolder?.ToJsonDto(),
                AtcVetCodes = xmlDto.AtcVetCodes?.ToArray() ?? Array.Empty<string>(),
                AuthorisationStatus = xmlDto.AuthorisationStatus?.ToJsonDto(),
                AuthorisationStatusDate = xmlDto.AuthorisationStatusDate,
                QspUnit = xmlDto.QspUnit?.ToJsonDto(),
                IsSubjectToReporting = xmlDto.IsSubjectToReporting == SubjectToReportingDto.Yes,
                Drugs = xmlDto.Drugs?.Select(d => d.ToJsonDto()).ToArray() ?? Array.Empty<DrugDtoJson>(),
                ActiveSubstances = xmlDto.ActiveSubstances?.Select(s => s.ToJsonDto()).ToArray() ?? Array.Empty<ActiveSubstanceDtoJson>(),
                TargetSpecies = xmlDto.TargetSpecies?.Select(s => s.ToJsonDto()).ToArray() ?? Array.Empty<CodedValueDtoJson>(),
                WithdrawalPeriods = xmlDto.WithdrawalPeriods?.Select(w => w.ToJsonDto()).ToArray() ?? Array.Empty<WithdrawalPeriodDtoJson>(),
                PharmaceuticalForm = xmlDto.PharmaceuticalForm?.ToJsonDto(),
                SummaryOfProductCharacteristics = xmlDto.SummaryOfProductCharacteristics?.ToJsonDto(),
            };
        }

        public static DrugDtoJson ToJsonDto(this DrugDto xmlDto)
        {
            if (xmlDto == null)
                return null;

            return new DrugDtoJson
            {
                GtinCode = xmlDto.Id?.GtinCode,
                Name = xmlDto.Name,
                AuthorisationNumber = xmlDto.AuthorisationNumber,
                Packaging = xmlDto.Packaging,
            };
        }

        public static ActiveSubstanceDtoJson ToJsonDto(this ActiveSubstanceDto xmlDto)
        {
            if (xmlDto == null)
                return null;

            return new ActiveSubstanceDtoJson
            {
                ActiveSubstance = xmlDto.ActiveSubstance?.ToJsonDto(),
                ActiveSubstanceConcentration = xmlDto.ActiveSubstanceConcentration?.ToJsonDto(),
                ActiveFraction = xmlDto.ActiveFraction?.ToJsonDto(),
                ActiveFractionConcentration = xmlDto.ActiveFractionConcentration?.ToJsonDto(),
            };
        }

        public static ConcentrationDtoJson ToJsonDto(this ConcentrationDto xmlDto)
        {
            if (xmlDto == null)
                return null;

            return new ConcentrationDtoJson
            {
                Quantity = xmlDto.Quantity,
                Unit = xmlDto.Unit?.ToJsonDto(),
            };
        }

        public static WithdrawalPeriodDtoJson ToJsonDto(this WithdrawalPeriodDto xmlDto)
        {
            if (xmlDto == null)
                return null;

            return new WithdrawalPeriodDtoJson
            {
                RouteOfAdministration = xmlDto.RouteOfAdministration?.ToJsonDto(),
                TargetSpecies = xmlDto.TargetSpecies?.ToJsonDto(),
                Tissue = xmlDto.Tissue?.ToJsonDto(),
                Quantity = xmlDto.Quantity,
                Unit = xmlDto.Unit?.ToJsonDto(),
                Comment = xmlDto.Comment,
            };
        }

        public static SummaryOfProductCharacteristicsDtoJson ToJsonDto(this SummaryOfProductCharacteristicsDto xmlDto)
        {
            if (xmlDto == null)
                return null;

            return new SummaryOfProductCharacteristicsDtoJson
            {
                IndicationsForUse = xmlDto.IndicationsForUse,
                Contraindications = xmlDto.Contraindications,
                SpecialWarnings = xmlDto.SpecialWarnings,
                SpecialPrecautionsForUse = xmlDto.SpecialPrecautionsForUse?.ToJsonDto(),
                AdverseEvents = xmlDto.AdverseEvents,
                InteractionsWithOtherMedicinalProducts = xmlDto.InteractionsWithOtherMedicinalProducts,
                PosologyAndRoutesOfAdministration = xmlDto.PosologyAndRoutesOfAdministration,
                SpecialPrecautionsForStorage = xmlDto.SpecialPrecautionsForStorage,
            };
        }

        public static SpecialPrecautionsForUseDtoJson ToJsonDto(this SpecialPrecautionsForUseDto xmlDto)
        {
            if (xmlDto == null)
                return null;

            return new SpecialPrecautionsForUseDtoJson
            {
                SpecialPrecautionsForUseInAnimals = xmlDto.SpecialPrecautionsForUseInAnimals,
                SpecialPrecautionsByThePersonAdministering = xmlDto.SpecialPrecautionsByThePersonAdministering,
                OtherPrecautions = xmlDto.OtherPrecautions,
            };
        }
    }
}
