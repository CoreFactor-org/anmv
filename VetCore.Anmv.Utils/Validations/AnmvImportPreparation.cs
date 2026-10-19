using VetCore.Anmv.Xml.Data;
using VetCore.Anmv.Xml.Descriptions;

namespace VetCore.Anmv.Utils.Validations;

/// <summary>Ordre des contrôles source, conservé dans les diagnostics de préparation.</summary>
public enum AnmvImportStage
{
    XmlSchema,
    Deserialization,
    DataValidation,
}

/// <summary>Anomalie du fichier source ou blocage restant après dédoublonnage.</summary>
public sealed record AnmvImportIssue(AnmvImportStage Stage, string Message);

/// <summary>Nombre d'occurrences strictement identiques retirées dans une collection d'une fiche ou du thésaurus.</summary>
public sealed record AnmvRemovedDuplicates(string? ProductReference, string Collection, int Count);

/// <summary>
/// Distingue la validité brute de l'importabilité selon les contrôles ANMV.
/// Le consommateur doit encore appliquer ses propres contrôles de conversion métier.
/// </summary>
public sealed class AnmvImportPreparation
{
    internal AnmvImportPreparation(DonneesReferenceGroupDto? descriptions, MedicinalProductGroupDto? products,
        IReadOnlyList<AnmvImportIssue> sourceErrors, IReadOnlyList<AnmvImportIssue> blockingErrors,
        IReadOnlyList<AnmvRemovedDuplicates> removedDuplicates)
    {
        Descriptions = descriptions;
        Products = products;
        SourceErrors = sourceErrors;
        BlockingErrors = blockingErrors;
        RemovedDuplicates = removedDuplicates;
    }

    public bool IsValid => SourceErrors.Count == 0;
    public bool IsImportable => Descriptions is not null && Products is not null && BlockingErrors.Count == 0;
    public IReadOnlyList<AnmvImportIssue> SourceErrors { get; }
    public IReadOnlyList<AnmvImportIssue> BlockingErrors { get; }
    public IReadOnlyList<AnmvRemovedDuplicates> RemovedDuplicates { get; }
    public DonneesReferenceGroupDto? Descriptions { get; }
    public MedicinalProductGroupDto? Products { get; }
}
