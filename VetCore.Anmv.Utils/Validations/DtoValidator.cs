using VetCore.Anmv.Xml.Data;
using VetCore.Anmv.Xml.Descriptions;

namespace VetCore.Anmv.Utils.Validations;

/// <summary>
/// Dto link and structure validation
/// </summary>
internal static class DtoValidator
{
    private const int ENTRY_DESC_MAX_LENGTH = 255;

    public static bool ValidateDto(this MedicinalProductGroupDto data, DonneesReferenceGroupDto reference, out ValidationErrors errors)
    {
        // first validate references :
        if (!ValidateDto(reference, out errors))
        {
            return false;
        }

        // then validate relations between elements
        errors = new ValidationErrors();
        var termNat = reference.TermNat.Select(o => o.SourceCode).ToHashSet();
        var termTit = reference.TermTit.Select(o => o.SourceCode).ToHashSet();
        var termTypProc = reference.TermTypProc.Select(o => o.SourceCode).ToHashSet();
        var termStatAuto = reference.TermStatAuto.Select(o => o.SourceCode).ToHashSet();
        var termFp = reference.TermFp.Select(o => o.SourceCode).ToHashSet();
        var termEsp = reference.TermEsp.Select(o => o.SourceCode).ToHashSet();
        var termSa = reference.TermSa.Select(o => o.SourceCode).ToHashSet();
        var termVa = reference.TermVa.Select(o => o.SourceCode).ToHashSet();
        var termCd = reference.TermCd.Select(o => o.SourceCode).ToHashSet();
        var termDenr = reference.TermDenr.Select(o => o.SourceCode).ToHashSet();
        var termPres = reference.TermPres.Select(o => o.SourceCode).ToHashSet();
        var termUnite = reference.TermUnite.Select(o => o.SourceCode).ToHashSet();
        var termTitre = reference.TermTitre.Select(o => o.SourceCode).ToHashSet();

        foreach (var medProd in data.MedicinalProducts)
        {
            if (!termTit.Contains(medProd.TermTit))
            {
                errors.Add($"[medicalProduct : {medProd.Num}] TermTit id [{medProd.TermTit}] is not within description");
            }

            if (!termNat.Contains(medProd.TermNat))
            {
                errors.Add($"[medicalProduct : {medProd.Num}] TermNat id [{medProd.TermNat}] is not within description");
            }

            if (!termTypProc.Contains(medProd.TermTypProc))
            {
                errors.Add($"[medicalProduct : {medProd.Num}] TermTypProc id [{medProd.TermTypProc}] is not within description");
            }

            if (!termTypProc.Contains(medProd.TermTypProc))
            {
                errors.Add($"[medicalProduct : {medProd.Num}] TermTypProc id [{medProd.TermTypProc}] is not within description");
            }

            if (!termStatAuto.Contains(medProd.TermStatAuto))
            {
                errors.Add($"[medicalProduct : {medProd.Num}] TermStatAuto id [{medProd.TermStatAuto}] is not within description");
            }

            if (!termFp.Contains(medProd.TermFp))
            {
                errors.Add($"[medicalProduct : {medProd.Num}] TermFp id [{medProd.TermFp}] is not within description");
            }

            if (medProd.ProdId == Guid.Empty)
            {
                errors.Add($"[medicalProduct : {medProd.Num}] ProdId ne doit pas être Guid.Empty");
            }

            // --- Compositions ---
            if (medProd.Compositions != null)
            {
                foreach (var composition in medProd.Compositions)
                {
                    if (composition.Sa != null)
                    {
                        if (!termSa.Contains(composition.Sa.TermSa))
                        {
                            errors.Add($"[medicalProduct : {medProd.Num} - compositions SA] TermSa id [{composition.Sa.TermSa}] is not within description");
                        }

                        if (composition.Sa.TermUnite.HasValue && !termUnite.Contains(composition.Sa.TermUnite.Value))
                        {
                            errors.Add($"[medicalProduct : {medProd.Num} - compositions SA] TermUnite id [{composition.Sa.TermUnite}] is not within description");
                        }
                    }

                    if (composition.Fraction != null)
                    {
                        if (!termSa.Contains(composition.Fraction.TermSa))
                        {
                            errors.Add($"[medicalProduct : {medProd.Num} - compositions Fraction] TermSa id [{composition.Fraction.TermSa}] is not within description");
                        }

                        if (!termUnite.Contains(composition.Fraction.TermUnite))
                        {
                            errors.Add($"[medicalProduct : {medProd.Num} - compositions Fraction] TermUnite id [{composition.Fraction.TermUnite}] is not within description");
                        }
                    }
                }
            }

            // --- Voies d'administration ---
            if (medProd.VoiesAdmin != null)
            {
                foreach (var voieAdmin in medProd.VoiesAdmin)
                {
                    if (!termVa.Contains(voieAdmin.TermVa))
                    {
                        errors.Add($"[medicalProduct : {medProd.Num} - voieAdmin ] TermVa id [{voieAdmin.TermVa}] is not within description");
                    }

                    if (!termEsp.Contains(voieAdmin.TermEsp))
                    {
                        errors.Add($"[medicalProduct : {medProd.Num} - voieAdmin ] TermEsp id [{voieAdmin.TermEsp}] is not within description");
                    }

                    if (voieAdmin.TermDenr != null && !termDenr.Contains(voieAdmin.TermDenr.Value))
                    {
                        errors.Add($"[medicalProduct : {medProd.Num} - voieAdmin ] TermDenr id [{voieAdmin.TermDenr}] is not within description");
                    }

                    if (voieAdmin.TermUnite != null && !termUnite.Contains(voieAdmin.TermUnite.Value))
                    {
                        errors.Add($"[medicalProduct : {medProd.Num} - voieAdmin ] TermUnite id [{voieAdmin.TermUnite}] is not within description");
                    }
                }
            }

            // --- Modèles destinés à la vente ---
            if (medProd.ModeleDestineVente != null)
            {
                foreach (var mdv in medProd.ModeleDestineVente)
                {
                    if (mdv.TermPres.HasValue && !termPres.Contains(mdv.TermPres.Value))
                    {
                        errors.Add($"[medicalProduct : {medProd.Num} - ModeleDestineVente ] TermPres id [{mdv.TermPres}] is not within description");
                    }

                    if (mdv.TermCd.HasValue && !termCd.Contains(mdv.TermCd.Value))
                    {
                        errors.Add($"[medicalProduct : {medProd.Num} - ModeleDestineVente ] TermCd id [{mdv.TermCd}] is not within description");
                    }
                }
            }
            // --- MdvCodesGtin ---
            if (medProd.MdvCodesGtin != null)
            {
                foreach (var mdg in medProd.MdvCodesGtin)
                {
                    if (mdg.PackId == Guid.Empty)
                    {
                        errors.Add($"[medicalProduct : {medProd.Num} - MdvCodesGtin] PackId ne doit pas être Guid.Empty");
                    }
                }
            }

            // --- ExcipientQsp ---
            if (medProd.ExcipientQsp != null)
            {
                if (medProd.ExcipientQsp.TermPres.HasValue
                    && !termPres.Contains(medProd.ExcipientQsp.TermPres.Value))
                {
                    errors.Add($"[medicalProduct : {medProd.Num} - ExcipientQsp ] TermPres id [{medProd.ExcipientQsp.TermPres}] is not within description");
                }

                if (medProd.ExcipientQsp.TermUnite.HasValue
                    && !termUnite.Contains(medProd.ExcipientQsp.TermUnite.Value))
                {
                    errors.Add($"[medicalProduct : {medProd.Num} - ExcipientQsp ] TermUnite id [{medProd.ExcipientQsp.TermUnite}] is not within description");
                }
            }

            // --- Paragraphes RCP ---
            if (medProd.ParagraphesRcp != null)
            {
                foreach (var para in medProd.ParagraphesRcp)
                {
                    if (!termTitre.Contains(para.TermTitre))
                    {
                        errors.Add($"[medicalProduct : {medProd.Num} - paraRcp ] TermTitre id [{para.TermTitre}] is not within description");
                    }
                }
            }
        }

        return errors.Count == 0;
    }


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