﻿using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace VetCore.Anmv.Xml.Data
{
    [XmlRoot("medicinal-product-group")]
    public sealed class MedicinalProductGroupDto
    {
        [XmlElement("Informations")]
        public InformationsDto Informations { get; set; }

        [XmlElement("medicinal-product")]
        public List<MedicinalProductDto> MedicinalProducts { get; set; }
    }

    public sealed class InformationsDto
    {
        [XmlIgnore]
        public DateTime DateJeuDeDonnees { get; set; }

        [XmlElement("date-jeu-de-donnees")]
        public string DateJeuDeDonneesFormatted
        {
            get => DateJeuDeDonnees.ToString(ConstantsAmnv.JEUX_DONNEES_DATE_FORMAT);
            set => DateJeuDeDonnees = DateTime.ParseExact(value, ConstantsAmnv.JEUX_DONNEES_DATE_FORMAT, System.Globalization.CultureInfo.InvariantCulture);
        }
    }

    public sealed class MedicinalProductDto
    {
        /// <summary>
        ///  "src-id" (ex : 447) :
        /// </summary>
        [XmlElement("src-id")]
        public int SrcId { get; set; }

        /// <summary>
        /// "nom" (ex : NEOMYCINE) :  Nom du médicament
        /// </summary>
        [XmlElement("nom")]
        public string Nom { get; set; } // varchar2(255)

        /// <summary>
        /// "num" (ex : 0033630) : N° identification (Remarque 1)
        /// <remarks>
        /// Remarque 1 :
        /// Le N° d’identification du médicament est un n° pérenne et est également disponible dans le jeu de données au format Microsoft Excel.
        /// </remarks>
        /// </summary>
        [XmlElement("num")]
        public string Num { get; set; } // string numérique sur 7 caractères

        /// <summary>
        /// "term-tit" (ex : 294) : Code du titulaire (FK)
        /// </summary>
        [XmlElement("term-tit")]
        public int TermTit { get; set; }

        /// <summary>
        /// "term-nat" (ex : 1) : Code de la nature du médicament (FK)
        /// </summary>
        [XmlElement("term-nat")]
        public int TermNat { get; set; }

        /// <summary>
        /// "term-typ-proc" (ex : 1) : Code du type de procédure (FK)
        /// </summary>
        [XmlElement("term-typ-proc")]
        public int TermTypProc { get; set; }

        /// <summary>
        /// "term-stat-auto" (ex : 4) : Code du statut de l’autorisation (FK)
        /// </summary>
        [XmlElement("term-stat-auto")]
        public int TermStatAuto { get; set; }

        /// <summary>
        /// "date-amm" (ex : 30/06/1992) : Date de la 1ère AMM
        /// </summary>
        [XmlIgnore]
        public DateTime DateAmm { get; set; }

        [XmlElement("date-amm")]
        public string DateAmmFormatted
        {
            get => DateAmm.ToString(ConstantsAmnv.AMNV_DATE_FORMAT);
            set => DateAmm = DateTime.ParseExact(value, ConstantsAmnv.AMNV_DATE_FORMAT, System.Globalization.CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// "term-fp" (ex : 148) : Code de la forme pharmaceutique (FK)
        /// </summary>
        [XmlElement("term-fp")]
        public int TermFp { get; set; }

        /// <summary>
        /// "num-amm" (ex : FR/V/2666590 4/1992) : N° d’AMM (Remarque 2)
        /// <remarks>
        /// Remarque 2 :
        /// Pour les procédures centralisées, un n° d’AMM différent est associé à chacune des présentations du médicament.
        /// Pour les autres procédures, ce n° d’AMM est unique pour un médicament donné quelle que soit la présentation.
        /// </remarks>
        /// </summary>
        [XmlElement("num-amm")]
        public string NumAmm { get; set; } // varchar2(50) (MAY BE EMPTY)

        [XmlElement("perm-id")]
        public string PermId { get; set; } // chaîne numérique sur 12 caractères (600000093156)(MAY BE EMPTY)

        [XmlElement("prod-id")]
        public Guid ProdId { get; set; } // varchar2(50) (969e0ae7-8bce-4e95-8bcd-0505699b165b) (MAY BE EMPTY)

        [XmlIgnore]
        public DateTime MajRcp { get; set; }

        [XmlElement("maj-rcp")]
        public string MajRcpFormatted
        {
            get => MajRcp.ToString(ConstantsAmnv.AMNV_DATE_FORMAT);
            set => MajRcp = DateTime.ParseExact(value, ConstantsAmnv.AMNV_DATE_FORMAT, System.Globalization.CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// "lien_rcp" (ex :
        /// http://www.ircp.anmv.anses.fr/rcp.aspx?NomMedicament=FEVAXYN+PENFOTEL+SOLUTION+INJECTABLE+POUR+CHATS)
        /// : Lien vers le RCP
        /// </summary>
        [XmlElement("lien-rcp")]
        public string LienRcp { get; set; } // varchar2(300)

        [XmlArray("composition")]
        [XmlArrayItem("compo")]
        public List<CompositionDto> Compositions { get; set; }

        [XmlArray("voie-administration")]
        [XmlArrayItem("voie-admin")]
        public List<VoieAdministrationDto> VoiesAdmin { get; set; }

        [XmlArray("modele-destine-vente")]
        [XmlArrayItem("mod-vte")]
        public List<ModeleDestineVenteDto> ModeleDestineVente { get; set; }

        [XmlArray("mdv-codes-gtin")]
        [XmlArrayItem("mod-vte")]
        public List<MdvCodesGtinDto> MdvCodesGtin { get; set; }

        [XmlElement("excipient-qsp")]
        public ExcipientQspDto ExcipientQsp { get; set; }

        [XmlArray("atcvet-code")]
        [XmlArrayItem("code-atcvet")]
        public List<string> AtcvetCode { get; set; } // varchar2(10)

        [XmlElement("paragraphes-rcp")]
        public ParagraphesRcpDto ParagraphesRcp { get; set; }
    }

    public sealed class CompositionDto
    {
        /// <summary>
        /// SubstanceActive
        /// </summary>
        [XmlElement("sa")]
        public SaDto Sa { get; set; }

        /// <summary>
        /// Fraction
        /// </summary>
        [XmlElement("fraction")]
        public FractionDto Fraction { get; set; }


        public bool ShouldSerializeFraction()
        {
            if (Fraction == null)
            {
                return false;
            }

            return Fraction.TermSa.HasValue || !string.IsNullOrWhiteSpace(Fraction.Quantite) || Fraction.TermUnite.HasValue;
        }
    }

    public sealed class SaDto
    {
        /// <summary>
        /// "term-sa" (ex : 1420) : Codes des substances actives
        /// </summary>
        [XmlElement("term-sa")]
        public int? TermSa { get; set; }

        /// <summary>
        /// "quantite" (ex : 56,80 | 164,00 | 2 000 000 | ...) : ???
        /// </summary>
        [XmlElement("quantite")]
        public string Quantite { get; set; } // varchar2(30)

        /// <summary>
        /// "term-unite" (ex : 7225) : Code des unités
        /// </summary>
        [XmlElement("term-unite")]
        public int? TermUnite { get; set; } // Nullable pour valeurs vides
    }

    public sealed class FractionDto
    {
        /// <summary>
        /// "term-sa" (ex : 1420) : Codes des substances actives
        /// </summary>
        [XmlElement("term-sa")]
        public int? TermSa { get; set; }

        /// <summary>
        /// "quantite" (ex : 56,80 | 1,858 |...) : ???
        /// </summary>
        [XmlElement("quantite")]
        public string Quantite { get; set; } // varchar2(30)

        /// <summary>
        /// "term-unite" (ex : 7225) : Code des unités (inféré)
        /// </summary>
        [XmlElement("term-unite")]
        public int? TermUnite { get; set; }
    }

    public sealed class VoieAdministrationDto
    {
        /// <summary>
        /// "term-va" (ex : 24) : Codes des voies d’administration
        /// </summary>
        [XmlElement("term-va")]
        public int TermVa { get; set; }

        /// <summary>
        /// "term-esp" (ex : 22) : Codes des espèces de destination
        /// </summary>
        [XmlElement("term-esp")]
        public int TermEsp { get; set; }

        /// <summary>
        /// "term-denr" (ex : 10) : code des Denrées
        /// </summary>
        [XmlElement("term-denr")]
        public int TermDenr { get; set; }

        [XmlElement("qte-ta")]
        public int QteTa { get; set; }

        /// <summary>
        /// "term-unite" (ex : 22) : Code des unités (inféré)
        /// </summary>
        [XmlElement("term-unite")]
        public int TermUnite { get; set; }
    }

    public sealed class ModeleDestineVenteDto
    {
        /// <summary>
        /// "lib-mod" (ex : Boîte de 10 sachets de 100 g) : Liste des modèles destinés à la vente
        /// </summary>
        [XmlElement("lib-mod")]
        public string LibMod { get; set; } // varchar2(255)

        [XmlElement("nb-unit")]
        public int NbUnit { get; set; } // varchar2(20) => BIZARRE

        [XmlElement("term-pres")]
        public int TermPres { get; set; }

        /// <summary>
        /// "term-cd" (ex : 10 | 3 | ...) : Codes des conditions de délivrances
        /// </summary>
        [XmlElement("term-cd")]
        public int TermCd { get; set; }

        /// <summary>
        /// "lib-condp" (ex : Flacon verre) : Conditionnement (inféré)
        /// </summary>
        [XmlElement("lib-condp")]
        public string LibCondp { get; set; } //varchar2(255)
    }

    public sealed class MdvCodesGtinDto
    {
        /// <summary>
        /// "lib-mod" (ex : Boîte de 10 seringues de 1 dose) : Liste des modèles destinés à la vente
        /// </summary>
        [XmlElement("lib-mod")]
        public string LibMod { get; set; } //varchar2(255)

        /// <summary>
        /// "pack-id" (ex : df3eb1e0-cf0c-40d9-a73c-48ee53fef7e3)
        /// </summary>
        [XmlElement("pack-id")]
        public Guid PackId { get; set; } //varchar2(50)

        /// <summary>
        /// "code-gtin" (ex : 09088881342311) : Code GTIN (Global Trade Item Number) si disponible
        /// </summary>
        [XmlElement("code-gtin")]
        public string CodeGtin { get; set; } //varchar2(50)

        /// <summary>
        /// "num-amm" (ex : EU/2/96/002/001) : N° d’AMM associé (Remarque 2)
        /// <remarks>
        /// Remarque 2 :
        /// Pour les procédures centralisées, un n° d’AMM différent est associé à chacune des présentations du médicament.
        /// Pour les autres procédures, ce n° d’AMM est unique pour un médicament donné quelle que soit la présentation.
        /// </remarks>
        /// </summary>
        [XmlElement("num-amm")]
        public string NumAmm { get; set; } //varchar2(50)
    }

    public sealed class ExcipientQspDto
    {
        [XmlElement("qte-qsp")]
        public int? QteQsp { get; set; } // varchar2(100) => Bizarre

        [XmlElement("term-pres")]
        public int? TermPres { get; set; }

        [XmlElement("term-unite")]
        public string TermUnite { get; set; } //varchar2(100)
    }

    public sealed class ParagraphesRcpDto
    {
        [XmlIgnore]
        public DateTime DateValidation { get; set; }

        [XmlElement("date-validation")]
        public string DateValidationFormatted
        {
            get => DateValidation.ToString(ConstantsAmnv.AMNV_DATE_FORMAT);
            set => DateValidation = DateTime.ParseExact(value, ConstantsAmnv.AMNV_DATE_FORMAT, System.Globalization.CultureInfo.InvariantCulture);
        }


        [XmlElement("lien_rcp")]
        public string LienRcp { get; set; } // varchar2(300)

        [XmlElement("para-rcp")]
        public List<ParaRcpDto> ParaRcp { get; set; }
    }

    public sealed class ParaRcpDto
    {
        [XmlElement("term-titre")]
        public int TermTitre { get; set; }

        [XmlElement("contenu")]
        public string Contenu { get; set; } // CLOB
    }
}