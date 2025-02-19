using VetCore.Anmv.Xml.Data;
using VetCore.Anmv.Xml.Descriptions;

namespace VetCore.Anmv.Utils.Validations.HashWrappers;


internal readonly record struct EntryDtoWrapper(int SourceCode, string SourceDesc)
{
    public EntryDtoWrapper(EntryDto dto) : this(dto.SourceCode, dto.SourceDesc) { }
}

internal readonly record struct EntryOrdreDtoWrapper(int SourceCode, string SourceDesc, int Ordre)
{
    public EntryOrdreDtoWrapper(EntryOrdreDto dto) : this(dto.SourceCode, dto.SourceDesc, dto.Ordre) { }
}

public readonly record struct VoieAdministrationDtoWrapper(int TermVa, int TermEsp, int? TermDenr, string QteTa, int? TermUnite)
{
    public VoieAdministrationDtoWrapper(VoieAdministrationDto dto) : this(dto.TermVa, dto.TermEsp, dto.TermDenr, dto.QteTa, dto.TermUnite) { }
}

public readonly record struct ModeleDestineVenteDtoWrapper(string LibMod, string NbUnit, int? TermPres, int? TermCd, string LibCondp)
{
    public ModeleDestineVenteDtoWrapper(ModeleDestineVenteDto dto) : this(dto.LibMod, dto.NbUnit, dto.TermPres, dto.TermCd, dto.LibCondp) { }
}

public readonly record struct MdvCodesGtinDtoWrapper(string LibMod, Guid? PackId, string CodeGtin, string NumAmm)
{
    public MdvCodesGtinDtoWrapper(MdvCodesGtinDto dto) : this(dto.LibMod, dto.PackId, dto.CodeGtin, dto.NumAmm) { }
}

public readonly record struct ParaRcpDtoWrapper(int TermTitre, string Contenu)
{
    public ParaRcpDtoWrapper(ParaRcpDto dto) : this(dto.TermTitre, dto.Contenu) { }
}

public readonly record struct CompositionDtoWrapper(int TermSa, string QuantiteSa, int? TermUniteSa, int? TermSaFraction, string? QuantiteFraction, int? TermUniteFraction)
{
    public CompositionDtoWrapper(CompositionDto dto)
        : this(
            dto.Sa.TermSa,
            dto.Sa.Quantite,
            dto.Sa.TermUnite,
            dto.Fraction?.TermSa,
            dto.Fraction?.Quantite,
            dto.Fraction?.TermUnite
        ) { }
}
