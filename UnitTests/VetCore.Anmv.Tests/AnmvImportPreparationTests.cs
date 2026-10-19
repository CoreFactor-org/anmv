using System.Security.Cryptography;
using System.Xml.Linq;
using VetCore.Anmv.Tests.utils;
using VetCore.Anmv.Utils;
using VetCore.Anmv.Utils.Validations;
using Xunit.Abstractions;

namespace VetCore.Anmv.Tests;

public sealed class AnmvImportPreparationTests(ITestOutputHelper output)
{
    private const string PRODUCT_XML = """
        <medicinal-product-group>
          <Informations><date-jeu-de-donnees>2026-10-15T09:30:04</date-jeu-de-donnees></Informations>
          <medicinal-product>
            <src-id>1</src-id><nom>Médicament test</nom><num>0000001</num>
            <term-tit>1</term-tit><term-nat>1</term-nat><term-typ-proc>1</term-typ-proc><term-stat-auto>1</term-stat-auto>
            <date-amm>2020-01-01</date-amm><term-fp>1</term-fp>
            <composition><compo><sa><term-sa>1</term-sa><quantite>2</quantite><term-unite>1</term-unite></sa><fraction><term-sa>1</term-sa><quantite>1</quantite><term-unite>1</term-unite></fraction></compo></composition>
            <voie-administration><voie-admin><term-va>1</term-va><term-esp>1</term-esp><term-denr>1</term-denr><qte-ta>5</qte-ta><term-unite>1</term-unite><commentaire>Source</commentaire></voie-admin></voie-administration>
            <modele-destine-vente><mod-vte><lib-mod>Boîte</lib-mod><nb-unit>1</nb-unit><term-pres>1</term-pres><term-cd>1</term-cd><lib-condp>Carton</lib-condp></mod-vte></modele-destine-vente>
            <mdv-codes-gtin><mod-vte><lib-mod>Boîte</lib-mod><pack-id>1EC2116F-84FF-AEBA-E063-0E2A11ACBC</pack-id><code-gtin>0123456789012</code-gtin><num-amm>AMM-1</num-amm></mod-vte></mdv-codes-gtin>
            <atcvet-code><code-atcvet>QA01</code-atcvet></atcvet-code>
            <paragraphes-rcp><para-rcp><term-titre>1</term-titre><contenu>Contenu source</contenu></para-rcp></paragraphes-rcp>
          </medicinal-product>
        </medicinal-product-group>
        """;

    [Fact]
    public void ValidSource_IsValidAndImportable()
    {
        var result = AnmvFileHandler.PrepareImport(CreateDescriptions().ToString(), PRODUCT_XML);

        Assert.True(result.IsValid, string.Join(Environment.NewLine, result.SourceErrors));
        Assert.True(result.IsImportable);
        Assert.Empty(result.RemovedDuplicates);
    }

    [Theory]
    [InlineData("composition", "Compositions")]
    [InlineData("voie-administration", "VoiesAdmin")]
    [InlineData("modele-destine-vente", "ModeleDestineVente")]
    [InlineData("mdv-codes-gtin", "MdvCodesGtin")]
    [InlineData("atcvet-code", "AtcvetCodes")]
    [InlineData("paragraphes-rcp", "ParagraphesRcp")]
    public void ExactDuplicates_AreInvalidButImportableWithoutChangingSource(string collection, string reportName)
    {
        var source = XDocument.Parse(PRODUCT_XML);
        var group = source.Descendants(collection).Single();
        group.Add(new XElement(group.Elements().First()));
        group.Add(new XElement(group.Elements().First()));
        var originalXml = source.ToString();

        var result = AnmvFileHandler.PrepareImport(CreateDescriptions().ToString(), originalXml);

        Assert.False(result.IsValid);
        Assert.True(result.IsImportable, string.Join(Environment.NewLine, result.BlockingErrors));
        Assert.Equal(new AnmvRemovedDuplicates("0000001", reportName, 2), Assert.Single(result.RemovedDuplicates));
        Assert.Contains(result.SourceErrors, issue => issue.Message.Contains("Duplicate"));
        Assert.Equal(originalXml, source.ToString());
        Assert.Equal(3, XDocument.Parse(AnmvFileHandler.DeserializeDataXmlString(originalXml)!.SerializeAmnvToXml())
            .Descendants(collection).Single().Elements().Count());
        Assert.NotNull(result.Products);
        var prepared = XDocument.Parse(result.Products.SerializeAmnvToXml());
        Assert.Single(prepared.Descendants(collection).Single().Elements());
        Assert.Equal("1EC2116F-84FF-AEBA-E063-0E2A11ACBC", Assert.Single(result.Products.MedicinalProducts).MdvCodesGtin[0].PackId);
    }

    [Theory]
    [InlineData("composition", "quantite", "3")]
    [InlineData("voie-administration", "commentaire", "Source ")]
    [InlineData("voie-administration", "qte-ta", "6")]
    [InlineData("modele-destine-vente", "lib-condp", "carton")]
    [InlineData("modele-destine-vente", "nb-unit", "2")]
    [InlineData("mdv-codes-gtin", "pack-id", "1ec2116f-84ff-aeba-e063-0e2a11acbc")]
    [InlineData("mdv-codes-gtin", "num-amm", "AMM-2")]
    [InlineData("paragraphes-rcp", "contenu", "Contenu source ")]
    public void DifferentValues_AreNeverDeduplicated(string collection, string field, string value)
    {
        var source = XDocument.Parse(PRODUCT_XML);
        var group = source.Descendants(collection).Single();
        var different = new XElement(group.Elements().First());
        different.Descendants(field).First().Value = value;
        group.Add(different);

        var result = AnmvFileHandler.PrepareImport(CreateDescriptions().ToString(), source.ToString());

        Assert.Empty(result.RemovedDuplicates);
        var prepared = XDocument.Parse(result.Products!.SerializeAmnvToXml());
        Assert.Equal(2, prepared.Descendants(collection).Single().Elements().Count());
        Assert.Contains(prepared.Descendants(collection).Single().Descendants(field), item => item.Value == value);
    }

    [Theory]
    [InlineData("term-tit", false)]
    [InlineData("term-titre", false)]
    [InlineData("term-tit", true)]
    [InlineData("term-titre", true)]
    public void ReferenceDuplicates_OnlyIdenticalEntriesAreImportable(string collection, bool conflicting)
    {
        var descriptions = CreateDescriptions();
        var group = descriptions.Element(collection)!;
        var duplicate = new XElement(group.Elements().Single());
        if (conflicting)
        {
            duplicate.Element("source-desc")!.Value = "Autre signification";
        }
        group.Add(duplicate);

        var result = AnmvFileHandler.PrepareImport(descriptions.ToString(), PRODUCT_XML);

        Assert.False(result.IsValid);
        Assert.Equal(!conflicting, result.IsImportable);
        if (conflicting)
        {
            Assert.Empty(result.RemovedDuplicates);
            Assert.Contains(result.BlockingErrors, item => item.Message.Contains("conflicting values"));
        }
        else
        {
            Assert.Equal(1, Assert.Single(result.RemovedDuplicates).Count);
        }
    }

    [Fact]
    public void Deduplication_DoesNotHideMissingReferencesOrSchemaErrors()
    {
        var source = XDocument.Parse(PRODUCT_XML);
        var group = source.Descendants("voie-administration").Single();
        group.Add(new XElement(group.Elements().Single()));
        source.Descendants("term-tit").Single().Value = "999";
        source.Descendants("atcvet-code").Single().RemoveNodes();

        var result = AnmvFileHandler.PrepareImport(CreateDescriptions().ToString(), source.ToString());

        Assert.False(result.IsValid);
        Assert.False(result.IsImportable);
        Assert.Single(result.RemovedDuplicates);
        Assert.Equal(AnmvImportStage.XmlSchema, result.BlockingErrors[0].Stage);
        Assert.Contains(result.BlockingErrors, item => item.Message.Contains("TermTit id [999]"));
    }

    [Theory]
    [InlineData("num-amm", "AMM-2")]
    [InlineData("lib-mod", "Autre présentation")]
    public void SamePackageIdentifierWithDifferentValues_IsNeverMerged(string field, string value)
    {
        var source = XDocument.Parse(PRODUCT_XML);
        var group = source.Descendants("mdv-codes-gtin").Single();
        var conflicting = new XElement(group.Elements().Single());
        conflicting.Element(field)!.Value = value;
        group.Add(conflicting);

        var result = AnmvFileHandler.PrepareImport(CreateDescriptions().ToString(), source.ToString());

        Assert.Empty(result.RemovedDuplicates);
        Assert.Equal(2, Assert.Single(result.Products!.MedicinalProducts).MdvCodesGtin.Count);
    }

    [Fact]
    public void DifferentReferenceOrder_IsAConflictNotADuplicate()
    {
        var descriptions = CreateDescriptions();
        var group = descriptions.Element("term-titre")!;
        var conflicting = new XElement(group.Elements().Single());
        conflicting.Element("ordre")!.Value = "2";
        group.Add(conflicting);

        var result = AnmvFileHandler.PrepareImport(descriptions.ToString(), PRODUCT_XML);

        Assert.False(result.IsImportable);
        Assert.Empty(result.RemovedDuplicates);
        Assert.Contains(result.BlockingErrors, item => item.Message.Contains("conflicting values"));
    }

    [Fact]
    public void DuplicateProductReference_IsNotSilentlySelectedOrMerged()
    {
        var source = XDocument.Parse(PRODUCT_XML);
        var conflicting = new XElement(source.Root!.Element("medicinal-product")!);
        conflicting.Element("nom")!.Value = "Autre médicament";
        source.Root.Add(conflicting);

        var result = AnmvFileHandler.PrepareImport(CreateDescriptions().ToString(), source.ToString());

        Assert.False(result.IsValid);
        Assert.False(result.IsImportable);
        Assert.Empty(result.RemovedDuplicates);
        Assert.Contains(result.BlockingErrors, item => item.Message.Contains("Duplicate product reference"));
        Assert.Equal(2, result.Products!.MedicinalProducts.Count);
    }

    [Fact]
    public void MissingActiveSubstance_IsReportedAsSchemaErrorWithoutCrashingDeduplication()
    {
        var source = XDocument.Parse(PRODUCT_XML);
        source.Descendants("sa").Single().Remove();

        var result = AnmvFileHandler.PrepareImport(CreateDescriptions().ToString(), source.ToString());

        Assert.False(result.IsValid);
        Assert.False(result.IsImportable);
        Assert.Contains(result.BlockingErrors, item => item.Stage == AnmvImportStage.XmlSchema);
    }

    [Fact]
    public void DifferentCompositionFraction_IsPreserved()
    {
        var source = XDocument.Parse(PRODUCT_XML);
        var group = source.Descendants("composition").Single();
        var different = new XElement(group.Elements().Single());
        different.Element("fraction")!.Element("quantite")!.Value = "9";
        group.Add(different);

        var result = AnmvFileHandler.PrepareImport(CreateDescriptions().ToString(), source.ToString());

        Assert.True(result.IsImportable);
        Assert.Empty(result.RemovedDuplicates);
        Assert.Equal(new[] { "1", "9" }, Assert.Single(result.Products!.MedicinalProducts).Compositions.Select(item => item.Fraction.Quantite));
    }

    [Fact]
    public void DuplicateAmongDifferentValues_PreservesFirstOccurrenceAndOrder()
    {
        var source = XDocument.Parse(PRODUCT_XML);
        var group = source.Descendants("paragraphes-rcp").Single();
        var first = group.Elements().Single();
        var second = new XElement(first);
        second.Element("contenu")!.Value = "Deuxième contenu";
        group.Add(second, new XElement(first), new XElement(second));

        var result = AnmvFileHandler.PrepareImport(CreateDescriptions().ToString(), source.ToString());

        Assert.False(result.IsValid);
        Assert.True(result.IsImportable);
        Assert.Equal(2, Assert.Single(result.RemovedDuplicates).Count);
        Assert.Equal(new[] { "Contenu source", "Deuxième contenu" }, Assert.Single(result.Products!.MedicinalProducts).ParagraphesRcp.Select(item => item.Contenu));
    }

    [Fact]
    public void MissingDescription_IsReportedWithoutThrowing()
    {
        var descriptions = CreateDescriptions();
        descriptions.Element("term-tit")!.Element("entry")!.Element("source-desc")!.Remove();

        var result = AnmvFileHandler.PrepareImport(descriptions.ToString(), PRODUCT_XML);

        Assert.False(result.IsValid);
        Assert.False(result.IsImportable);
        Assert.Contains(result.BlockingErrors, item => item.Stage == AnmvImportStage.DataValidation && item.Message.Contains("NullOrWhiteSpace"));
    }

    [Fact]
    public void MalformedXml_IsNotImportable()
    {
        var result = AnmvFileHandler.PrepareImport(CreateDescriptions().ToString(), "<medicinal-product-group>");

        Assert.False(result.IsValid);
        Assert.False(result.IsImportable);
        Assert.Contains(result.BlockingErrors, item => item.Stage == AnmvImportStage.Deserialization);
    }

    [Fact]
    public void OctoberArchive_ReportsRemainingBlockersAndPreservesRawBytes()
    {
        var file = UnitTestFileAccessor.GetFile(AmnvFilesUnitTest.XML_AMM_Data_2026_10_15);
        using var beforeStream = file.OpenRead();
        var before = SHA256.HashData(beforeStream);
        var products = UnitTestFileAccessor.GetXmlContent(AmnvFilesUnitTest.XML_AMM_Data_2026_10_15);
        var descriptions = UnitTestFileAccessor.GetXmlContent(AmnvFilesUnitTest.XML_AMM_Descriptions_2026_10_15);

        var result = AnmvFileHandler.PrepareImport(descriptions, products);

        Assert.False(result.IsValid);
        Assert.False(result.IsImportable);
        Assert.Equal(3221, result.Products!.MedicinalProducts.Count);
        Assert.Equal(6, result.RemovedDuplicates.Sum(item => item.Count));
        using var afterStream = file.OpenRead();
        Assert.Equal(before, SHA256.HashData(afterStream));
        Assert.Contains(result.BlockingErrors, item => item.Stage == AnmvImportStage.XmlSchema && item.Message.Contains("atcvet-code"));
        Assert.Contains(result.BlockingErrors, item => item.Message.Contains("TermTit id [1709]"));
        output.WriteLine($"IsValid={result.IsValid}; IsImportable={result.IsImportable}; removed={result.RemovedDuplicates.Sum(item => item.Count)}");
        foreach (var duplicate in result.RemovedDuplicates)
        {
            output.WriteLine($"DEDUP {duplicate.ProductReference} {duplicate.Collection}: {duplicate.Count}");
        }
        foreach (var issue in result.BlockingErrors)
        {
            output.WriteLine($"BLOCK {issue.Stage}: {issue.Message}");
        }
    }

    private static XElement CreateDescriptions()
    {
        var root = new XElement("donnees-reference-group");
        foreach (var name in new[] { "term-nat", "term-tit", "term-typ-proc", "term-stat-auto", "term-fp", "term-esp", "term-sa", "term-va", "term-cd", "term-denr", "term-pres", "term-unite", "term-titre" })
        {
            var entry = new XElement("entry", new XElement("source-code", 1), new XElement("source-desc", "Source"));
            if (name == "term-titre")
            {
                entry.Add(new XElement("ordre", 1));
            }
            root.Add(new XElement(name, entry));
        }
        return root;
    }
}
