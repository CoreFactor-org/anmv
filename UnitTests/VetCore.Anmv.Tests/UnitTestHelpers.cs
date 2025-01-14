using VetCore.Anmv.Xml.Descriptions;
using Xunit.Abstractions;

namespace VetCore.Anmv.Tests;

public static class UnitTestHelpers
{
    public static void CompareAndLogDifferences(this ITestOutputHelper testOutputHelper, List<EntryDto> previous, List<EntryDto> current, string listName)
    {
        var previousCodes = previous.Select(e => e.SourceCode).ToHashSet();
        var currentCodes = current.Select(e => e.SourceCode).ToHashSet();

        var missingInCurrent = previousCodes.Except(currentCodes).ToList();
        var missingInPrevious = currentCodes.Except(previousCodes).ToList();

        testOutputHelper.WriteLine($"--- Comparaison de {listName} Nombre d'éléments: AVANT={previous.Count} | ACTUEL={previous.Count}---");
        if (missingInCurrent.Count != 0)
        {
            testOutputHelper.WriteLine("Éléments dans AVANT mais manquants dans ACTUEL:");
            foreach (var code in missingInCurrent)
            {
                var entry = previous.First(e => e.SourceCode == code);
                testOutputHelper.WriteLine($"  - SourceCode: {entry.SourceCode}, SourceDesc: {entry.SourceDesc}");
            }
        }

        if (missingInPrevious.Count != 0)
        {
            testOutputHelper.WriteLine("Éléments dans ACTUEL mais manquants dans AVANT:");
            foreach (var entry in missingInPrevious.Select(o => current.First(e => e.SourceCode == o)))
            {
                testOutputHelper.WriteLine($"  - SourceCode: {entry.SourceCode}, SourceDesc: {entry.SourceDesc}");
            }
        }
        testOutputHelper.WriteLine("");
    }

}