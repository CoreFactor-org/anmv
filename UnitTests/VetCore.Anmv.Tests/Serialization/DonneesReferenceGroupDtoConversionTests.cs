using VetCore.Anmv.Json.Description;
using VetCore.Anmv.Xml.Descriptions;

namespace VetCore.Anmv.Tests.Serialization
{
    public class DonneesReferenceGroupDtoConversionTests
    {
        [Fact]
        public void XmlToJsonConversion_ShouldConvertCorrectly()
        {
            // Arrange
            var xmlDto = new DonneesReferenceGroupDto
            {
                TermNat = [new EntryDto { SourceCode = 1, SourceDesc = "desc1" }],
                TermTit = [new EntryDto { SourceCode = 2, SourceDesc = "desc2" }],
                TermTypProc = [new EntryDto { SourceCode = 3, SourceDesc = "desc3" }],
                TermStatAuto = [new EntryDto { SourceCode = 4, SourceDesc = "desc4" }],
                TermFp = [new EntryDto { SourceCode = 5, SourceDesc = "desc5" }],
                TermEsp = [new EntryDto { SourceCode = 6, SourceDesc = "desc6" }],
                TermSa = [new EntryDto { SourceCode = 7, SourceDesc = "desc7" }],
                TermVa = [new EntryDto { SourceCode = 8, SourceDesc = "desc8" }],
                TermCd = [new EntryDto { SourceCode = 9, SourceDesc = "desc9" }],
                TermDenr = [new EntryDto { SourceCode = 11, SourceDesc = "desc11" }],
                TermPres = [new EntryDto { SourceCode = 12, SourceDesc = "desc12" }],
                TermUnite = [new EntryDto { SourceCode = 13, SourceDesc = "desc13" }],
                TermTitre = [new EntryOrdreDto { SourceCode = 14, SourceDesc = "desc14", Ordre = 100 }],
            };

            // Act
            var jsonDto = xmlDto.ToJsonDto();

            // Assert
            Assert.NotNull(jsonDto);
            Assert.Equal(xmlDto.TermNat.Count, jsonDto.TermNat.Length);
            Assert.Equal(xmlDto.TermNat.First().SourceCode, jsonDto.TermNat.First().SourceCode);
            Assert.Equal(xmlDto.TermTitre.First().Ordre, jsonDto.TermTitre.First().Ordre);
        }

        [Fact]
        public void JsonToXmlConversion_ShouldConvertCorrectly()
        {
            // Arrange
            var jsonDto = new DonneesReferenceGroupDtoJson
            {
                TermNat =
                [
                    new TermNatDtoJson { SourceCode = 1, SourceDesc = "desc1" },
                ],
                TermTit =
                [
                    new TermTitDtoJson { SourceCode = 2, SourceDesc = "desc2" },
                ],
                TermTypProc =
                [
                    new TermTypProcDtoJson { SourceCode = 3, SourceDesc = "desc3" },
                ],
                TermStatAuto =
                [
                    new TermStatAutoDtoJson { SourceCode = 4, SourceDesc = "desc4" },
                ],
                TermFp =
                [
                    new TermFpDtoJson { SourceCode = 5, SourceDesc = "desc5" },
                ],
                TermEsp =
                [
                    new TermEspDtoJson { SourceCode = 6, SourceDesc = "desc6" },
                ],
                TermSa =
                [
                    new TermSaDtoJson { SourceCode = 7, SourceDesc = "desc7" },
                ],
                TermVa =
                [
                    new TermVaDtoJson { SourceCode = 8, SourceDesc = "desc8" },
                ],
                TermCd =
                [
                    new TermCdDtoJson { SourceCode = 9, SourceDesc = "desc9" },
                ],
                TermDenr =
                [
                    new TermDenrDtoJson { SourceCode = 11, SourceDesc = "desc11" },
                ],
                TermPres =
                [
                    new TermPresDtoJson { SourceCode = 12, SourceDesc = "desc12" },
                ],
                TermUnite =
                [
                    new TermUniteDtoJson { SourceCode = 13, SourceDesc = "desc13" },
                ],
                TermTitre =
                [
                    new TermTitreDtoJson { SourceCode = 14, SourceDesc = "desc14", Ordre = 100 },
                ],
            };

            // Act
            var xmlDto = jsonDto.ToXmlDto();

            // Assert
            Assert.NotNull(xmlDto);
            Assert.Equal(jsonDto.TermNat.Length, xmlDto.TermNat.Count);
            Assert.Equal(jsonDto.TermNat.First().SourceCode, xmlDto.TermNat.First().SourceCode);
            Assert.Equal(jsonDto.TermTitre.First().Ordre, xmlDto.TermTitre.First().Ordre);
        }

        [Fact]
        public void XmlToJsonConversion_NullInput_ShouldReturnNull()
        {
            // Arrange
            DonneesReferenceGroupDto? xmlDto = null;

            // Act
            var jsonDto = xmlDto.ToJsonDto();

            // Assert
            Assert.Null(jsonDto);
        }

        [Fact]
        public void JsonToXmlConversion_NullInput_ShouldReturnNull()
        {
            // Arrange
            DonneesReferenceGroupDtoJson? jsonDto = null;

            // Act
            var xmlDto = jsonDto.ToXmlDto();

            // Assert
            Assert.Null(xmlDto);
        }
    }
}
