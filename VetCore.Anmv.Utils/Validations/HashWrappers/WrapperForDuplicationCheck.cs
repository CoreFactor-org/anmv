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