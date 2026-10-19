using System.Xml.Linq;
using VetCore.Anmv.Utils.Validations.HashWrappers;
using VetCore.Anmv.Utils.Xsd;
using VetCore.Anmv.Xml.Data;
using VetCore.Anmv.Xml.Descriptions;

namespace VetCore.Anmv.Utils.Validations;

/// <summary>Prépare uniquement des DTO nouvellement désérialisés, sans réécriture des fichiers sources.</summary>
internal static class AnmvImportPreparer
{
    public static AnmvImportPreparation Prepare(string descriptionsXml, string productsXml)
    {
        var sourceErrors = new List<AnmvImportIssue>();
        AddSchemaErrors(AnmvFileHandler.ValidateXml(descriptionsXml, AmnvFilesKey.Descriptions_XSD_AMM), "Descriptions", sourceErrors);
        var blockingErrors = new List<AnmvImportIssue>(sourceErrors);
        var productValidation = AnmvFileHandler.ValidateXml(productsXml, AmnvFilesKey.Data_XSD_AMM);
        AddSchemaErrors(productValidation, "Products", sourceErrors);
        if (productValidation.Errors.Count > 0)
        {
            // Seule la cardinalité de la liste ATCvet est tolérée à l'import ; le XML brut reste intact.
            var importValidation = XsdValidator.ValidateXmlWithXsd(productsXml, CreateImportProductsSchema());
            AddSchemaErrors(importValidation, "Products", blockingErrors);
        }
        DonneesReferenceGroupDto? descriptions;
        MedicinalProductGroupDto? products;
        try
        {
            descriptions = AnmvFileHandler.DeserializeDescriptionXmlString(descriptionsXml);
            products = AnmvFileHandler.DeserializeDataXmlString(productsXml);
        }
        catch (InvalidOperationException exception)
        {
            var issue = new AnmvImportIssue(AnmvImportStage.Deserialization, exception.InnerException?.Message ?? exception.Message);
            sourceErrors.Add(issue);
            blockingErrors.Add(issue);
            return new(null, null, sourceErrors.ToArray(), blockingErrors.ToArray(), []);
        }
        if (descriptions is null || products is null)
        {
            var issue = new AnmvImportIssue(AnmvImportStage.Deserialization, "Les deux documents ANMV sont nécessaires.");
            sourceErrors.Add(issue);
            blockingErrors.Add(issue);
            return new(descriptions, products, sourceErrors.ToArray(), blockingErrors.ToArray(), []);
        }

        products.ValidateDataWithRelatedDescription(descriptions, out var originalErrors);
        AddDataErrors(originalErrors, sourceErrors);
        var removed = new List<AnmvRemovedDuplicates>();
        DeduplicateDescriptions(descriptions, removed);
        foreach (var product in products.MedicinalProducts ?? [])
        {
            product.AtcvetCodes = Deduplicate(product.AtcvetCodes, code => code, product.Num, nameof(product.AtcvetCodes), removed);
            product.Compositions = Deduplicate(product.Compositions, item => new CompositionDtoWrapper(item), product.Num, nameof(product.Compositions), removed);
            product.VoiesAdmin = Deduplicate(product.VoiesAdmin, item => new VoieAdministrationDtoWrapper(item), product.Num, nameof(product.VoiesAdmin), removed);
            product.ModeleDestineVente = Deduplicate(product.ModeleDestineVente, item => new ModeleDestineVenteDtoWrapper(item), product.Num, nameof(product.ModeleDestineVente), removed);
            product.MdvCodesGtin = Deduplicate(product.MdvCodesGtin, item => new MdvCodesGtinDtoWrapper(item), product.Num, nameof(product.MdvCodesGtin), removed);
            product.ParagraphesRcp = Deduplicate(product.ParagraphesRcp, item => new ParaRcpDtoWrapper(item), product.Num, nameof(product.ParagraphesRcp), removed);
        }
        products.ValidateDataWithRelatedDescription(descriptions, out var remainingErrors);
        AddDataErrors(remainingErrors, blockingErrors);
        return new(descriptions, products, sourceErrors.ToArray(), blockingErrors.ToArray(), removed.ToArray());
    }

    private static string CreateImportProductsSchema()
    {
        var schema = XDocument.Parse(AmnvFilesKey.Data_XSD_AMM.GetXsdContent());
        XNamespace xs = "http://www.w3.org/2001/XMLSchema";
        var atcvetList = schema.Descendants(xs + "element").Single(element => (string?)element.Attribute("name") == "atcvet-code");
        var atcvetCode = atcvetList.Descendants(xs + "element").Single(element => (string?)element.Attribute("name") == "code-atcvet");
        atcvetCode.SetAttributeValue("minOccurs", "0");
        return schema.ToString();
    }

    private static void AddSchemaErrors(XsdValidationResult validation, string document, List<AnmvImportIssue> issues)
    {
        foreach (var error in validation.Errors)
        {
            issues.Add(new(AnmvImportStage.XmlSchema, $"{document}: {error}"));
        }
    }

    private static void AddDataErrors(ValidationErrors errors, List<AnmvImportIssue> issues)
    {
        foreach (var error in errors.GetErrors())
        {
            issues.Add(new(AnmvImportStage.DataValidation, error));
        }
    }

    private static List<T> Deduplicate<T, TKey>(List<T>? items, Func<T, TKey> key,
        string? product, string collection, List<AnmvRemovedDuplicates> removed)
    {
        if (items is null)
        {
            return null!;
        }
        var seen = new HashSet<TKey>();
        List<T>? unique = null;
        for (var index = 0; index < items.Count; index++)
        {
            var item = items[index];
            if (seen.Add(key(item)))
            {
                unique?.Add(item);
            }
            else
            {
                unique ??= items.GetRange(0, index);
            }
        }
        if (unique is null)
        {
            return items;
        }
        removed.Add(new(product, collection, items.Count - unique.Count));
        return unique;
    }

    private static void DeduplicateDescriptions(DonneesReferenceGroupDto descriptions, List<AnmvRemovedDuplicates> removed)
    {
        descriptions.TermNat = Deduplicate(descriptions.TermNat, item => new EntryDtoWrapper(item), null, nameof(descriptions.TermNat), removed);
        descriptions.TermTit = Deduplicate(descriptions.TermTit, item => new EntryDtoWrapper(item), null, nameof(descriptions.TermTit), removed);
        descriptions.TermTypProc = Deduplicate(descriptions.TermTypProc, item => new EntryDtoWrapper(item), null, nameof(descriptions.TermTypProc), removed);
        descriptions.TermStatAuto = Deduplicate(descriptions.TermStatAuto, item => new EntryDtoWrapper(item), null, nameof(descriptions.TermStatAuto), removed);
        descriptions.TermFp = Deduplicate(descriptions.TermFp, item => new EntryDtoWrapper(item), null, nameof(descriptions.TermFp), removed);
        descriptions.TermEsp = Deduplicate(descriptions.TermEsp, item => new EntryDtoWrapper(item), null, nameof(descriptions.TermEsp), removed);
        descriptions.TermSa = Deduplicate(descriptions.TermSa, item => new EntryDtoWrapper(item), null, nameof(descriptions.TermSa), removed);
        descriptions.TermVa = Deduplicate(descriptions.TermVa, item => new EntryDtoWrapper(item), null, nameof(descriptions.TermVa), removed);
        descriptions.TermCd = Deduplicate(descriptions.TermCd, item => new EntryDtoWrapper(item), null, nameof(descriptions.TermCd), removed);
        descriptions.TermDenr = Deduplicate(descriptions.TermDenr, item => new EntryDtoWrapper(item), null, nameof(descriptions.TermDenr), removed);
        descriptions.TermPres = Deduplicate(descriptions.TermPres, item => new EntryDtoWrapper(item), null, nameof(descriptions.TermPres), removed);
        descriptions.TermUnite = Deduplicate(descriptions.TermUnite, item => new EntryDtoWrapper(item), null, nameof(descriptions.TermUnite), removed);
        descriptions.TermTitre = Deduplicate(descriptions.TermTitre, item => new EntryOrdreDtoWrapper(item), null, nameof(descriptions.TermTitre), removed);
    }
}
