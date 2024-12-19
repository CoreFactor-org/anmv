using VetCore.Anmv.Xml.Descriptions;

namespace VetCore.Anmv.Utils.Validations;

/// <summary>
/// Group validation of DonneesReferenceGroup Dto related types
/// </summary>
internal static class DonneesReferenceGroupDtoValidator
{
    private const int ENTRY_DESC_MAX_LENGTH = 255;

    public static bool ValidateDto(this DonneesReferenceGroupDto dto, out ValidationErrors errors)
    {
        errors = new ValidationErrors();
        ValidateEntry(dto.TermNat, errors, nameof(dto.TermNat));
        ValidateEntry(dto.TermTit, errors, nameof(dto.TermTit));
        ValidateEntry(dto.TermTypProc, errors, nameof(dto.TermTypProc));
        ValidateEntry(dto.TermStatAuto, errors, nameof(dto.TermStatAuto));
        ValidateEntry(dto.TermFp, errors, nameof(dto.TermFp));
        ValidateEntry(dto.TermEsp, errors, nameof(dto.TermEsp));
        ValidateEntry(dto.TermSa, errors, nameof(dto.TermSa));
        ValidateEntry(dto.TermVa, errors, nameof(dto.TermVa));
        ValidateEntry(dto.TermCd, errors, nameof(dto.TermCd));
        ValidateEntry(dto.TermQsp, errors, nameof(dto.TermQsp));
        ValidateEntry(dto.TermDenr, errors, nameof(dto.TermDenr));
        ValidateEntry(dto.TermPres, errors, nameof(dto.TermPres));
        ValidateEntry(dto.TermUnite, errors, nameof(dto.TermUnite));
        // nothing specific to Ordre to validate here
        ValidateEntry(dto.TermTitre, errors, nameof(dto.TermTitre));
        return errors.Count == 0;
    }

    private static void ValidateEntry<T>(IEnumerable<T> entries, ValidationErrors errors, string tagName)
        where T : EntryDto
    {
        foreach (var entry in entries)
        {
            // an entry should have a description
            if (string.IsNullOrWhiteSpace(entry.SourceDesc))
            {
                errors.Add($"[{tagName}] entry code [{entry.SourceCode}] has source-desc NullOrWhiteSpace.");
            }

            // an entry should have no more than 255 char
            if (entry.SourceDesc.Length > ENTRY_DESC_MAX_LENGTH)
            {
                errors.Add($"[{tagName}] entry code [{entry.SourceCode}] has more than {ENTRY_DESC_MAX_LENGTH} characters.");
            }
        }
    }
}