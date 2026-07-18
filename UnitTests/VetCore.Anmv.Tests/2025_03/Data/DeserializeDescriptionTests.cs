using System.IO.Compression;
using PRF.Utils.CoreComponents.Extensions;
using VetCore.Anmv.Tests.utils;
using VetCore.Anmv.Utils;

namespace VetCore.Anmv.Tests._2025_03.Data;

public sealed class DeserializeDataTests
{
    [Fact]
    public void Deserialize_data_into_DTO()
    {
        //Arrange
        var zipFile = UnitTestFileAccessor
            .GetFile(AmnvFilesUnitTest.XML_AMM_Data_2025_03)
            .ToFileInfo();

        using var zipStream = zipFile.OpenRead();
        using var archive = new ZipArchive(zipStream, ZipArchiveMode.Read);
        var xmlContent = archive.ReadEntryAsString("amm-vet-fr-v2-v.xml");

        //Act
        var res = AnmvFileHandler.DeserializeDataXmlString(xmlContent);

        //Assert
        Assert.NotNull(res);
        Assert.Equal(3105, res.MedicinalProducts.Count);
        Assert.Equal(DateTime.Parse("2025-04-10T15:20:03.0000000"), res.Informations.DateJeuDeDonnees);
        var aggregated = res.MedicinalProducts.Aggregate(
            new
            {
                CompositionCount = 0,
                AtcvetCodeCount = 0,
                ParagraphesRcpCount = 0,
                VoiesAdminCount = 0,
                MdvCodesGtinCount = 0,
                ModeleDestineVenteCount = 0,
            },
            (acc, product) => new
            {
                CompositionCount = acc.CompositionCount + product.Compositions.Count,
                AtcvetCodeCount = acc.AtcvetCodeCount + product.AtcvetCodes.Count,
                ParagraphesRcpCount = acc.ParagraphesRcpCount + product.ParagraphesRcp.Count,
                VoiesAdminCount = acc.VoiesAdminCount + product.VoiesAdmin.Count,
                MdvCodesGtinCount = acc.MdvCodesGtinCount + product.MdvCodesGtin.Count,
                ModeleDestineVenteCount = acc.ModeleDestineVenteCount + product.ModeleDestineVente.Count,
            });

        // count aggregated total
        Assert.Equal(4615, aggregated.CompositionCount);
        Assert.Equal(3145, aggregated.AtcvetCodeCount);
        Assert.Equal(88927, aggregated.ParagraphesRcpCount);
        Assert.Equal(9532, aggregated.VoiesAdminCount);
        Assert.Equal(14966, aggregated.MdvCodesGtinCount);
        Assert.Equal(14965, aggregated.ModeleDestineVenteCount);
    }


    [Fact]
    public void Validate_Commentaire_extraction()
    {
        //Arrange
        var xmlContent =
            """
            <?xml version="1.0" encoding="utf-8" standalone="no"?>
            <!--Médicaments vétérinaires autorisés en France-->
            <!--Jeu de données du 2025-03-12T14:30:38-->
            <medicinal-product-group>
              <Informations>
                <date-jeu-de-donnees>2025-03-12T14:30:38</date-jeu-de-donnees>
              </Informations>
            <medicinal-product>
              <src-id>373</src-id>
              <nom>TRINODOL</nom>
              <num>0317700</num>
              <term-tit>165</term-tit>
              <term-nat>1</term-nat>
              <term-typ-proc>1</term-typ-proc>
              <term-stat-auto>4</term-stat-auto>
              <date-amm>1986-04-23</date-amm>
              <term-fp>113</term-fp>
              <num-amm>FR/V/4228406 4/1986</num-amm>
              <perm-id>600000032161</perm-id>
              <prod-id>8feb8355-17de-4447-87f4-30bf875c7703</prod-id>
              <maj-rcp>2022-02-11</maj-rcp>
              <lien-rcp>http://www.ircp.anmv.anses.fr/rcp.aspx?NomMedicament=TRINODOL</lien-rcp>
              <!--Composition-->
              <composition>
                <compo>
                  <sa>
                    <term-sa>4378</term-sa>
                    <quantite>2</quantite>
                    <term-unite>10</term-unite>
                  </sa>
                  <fraction>
                    <term-sa>1667</term-sa>
                    <quantite>1,8</quantite>
                    <term-unite>10</term-unite>
                  </fraction>
                </compo>
                <compo>
                  <sa>
                    <term-sa>1243</term-sa>
                    <quantite>8,7</quantite>
                    <term-unite>10</term-unite>
                  </sa>
                </compo>
              </composition>
              <!--Voies d'administration-->
              <voie-administration>
                <voie-admin>
                  <term-va>2</term-va>
                  <term-esp>18</term-esp>
                  <term-denr>14</term-denr>
                  <qte-ta>10</qte-ta>
                  <term-unite>22</term-unite>
                  <commentaire>Ne pas utiliser chez les animaux producteurs de lait destiné à la consommation humaine.</commentaire>
                </voie-admin>
              </voie-administration>
              <!--Modeles destines a la vente-->
              <modele-destine-vente>
                <mod-vte>
                  <lib-mod>Boîte de 1 flacon de 125 mL</lib-mod>
                  <nb-unit>1</nb-unit>
                  <term-pres>7</term-pres>
                  <term-cd>7094</term-cd>
                  <lib-condp>Flacon verre type III brun</lib-condp>
                </mod-vte>
              </modele-destine-vente>
              <!--Codes GTIN-->
              <mdv-codes-gtin>
                <mod-vte>
                  <lib-mod>Boîte de 1 flacon de 125 mL</lib-mod>
                  <pack-id>6fce84a5-37ef-4b71-975c-e28aebf19dcb</pack-id>
                  <code-gtin>03515656835809</code-gtin>
                  <num-amm>FR/V/4228406 4/1986</num-amm>
                </mod-vte>
              </mdv-codes-gtin>
              <!--excipient qsp-->
              <excipient-qsp>
                <qte-qsp>1</qte-qsp>
                <term-unite>11</term-unite>
              </excipient-qsp>
              <!--codes atcvet-->
              <atcvet-code>
                <code-atcvet>QM02AX53</code-atcvet>
              </atcvet-code>
              <!--RCP-->
              <paragraphes-rcp>
                <para-rcp>
                  <term-titre>1</term-titre>
                  <contenu><![CDATA[TRINODOL]]></contenu>
                </para-rcp>
                <para-rcp>
                  <term-titre>2</term-titre>
                  <contenu><![CDATA[\nUn mL contient&nbsp;:&nbsp;\n&nbsp;&nbsp;\nSubstance(s) active(s)&nbsp;:&nbsp;\nPrednisolone ..&hellip;&hellip;&hellip;&hellip;&hellip;&hellip;&hellip;&hellip;&hellip;&hellip;&hellip;&hellip;&hellip;&hellip;&hellip;&hellip;&hellip;1,8 mg\n(sous forme d&rsquo;acétate)&nbsp;\n(équivalant &agrave; 2 mg d&rsquo;acétate de prednisolone)&nbsp;\nLidoca&iuml;ne &hellip;..&hellip;&hellip;&hellip;&hellip;&hellip;&hellip;&hellip;&hellip;&hellip;&hellip;&hellip;&hellip;&hellip;&hellip;&hellip;&hellip;&hellip;..8,7 mg\n(sous forme de chlorhydrate monohydraté)&nbsp;\n&nbsp;&nbsp;\nExcipient(s) :&nbsp;\nDiméthylsulfoxyde ..&hellip;&hellip;&hellip;&hellip;&hellip;&hellip;&hellip;&hellip;&hellip;&hellip;&hellip;&hellip;&hellip;&hellip;&hellip;968,0 mg\nPour la liste compl&egrave;te des excipients, voir rubrique &laquo;&nbsp;Liste des Excipients&nbsp;&raquo;.]]></contenu>
                </para-rcp>
                <para-rcp>
                  <term-titre>3</term-titre>
                  <contenu><![CDATA[Gel.Liquide visqueux, clair.]]></contenu>
                </para-rcp>
                <para-rcp>
                  <term-titre>5</term-titre>
                  <contenu><![CDATA[Chevaux.]]></contenu>
                </para-rcp>
                <para-rcp>
                  <term-titre>6</term-titre>
                  <contenu><![CDATA[Chez les chevaux&nbsp;:-&nbsp;Réduction de la douleur et de l&rsquo;inflammation associées aux troubles musculo-squelettiques localisés.]]></contenu>
                </para-rcp>
                <para-rcp>
                  <term-titre>7</term-titre>
                  <contenu><![CDATA[Ne pas utiliser en cas d'hypersensibilité aux substances actives ou &agrave; l'un des excipients.Voir la rubrique &laquo; Utilisation en cas de gestation, de lactation ou de ponte &raquo;.Ne pas utiliser chez les chevaux atteints d'une maladie hépatique ou rénale.Ne pas utiliser chez les chevaux atteints d'infections virales ou fongiques ou chez les chevaux immunodéprimés.]]></contenu>
                </para-rcp>
                <para-rcp>
                  <term-titre>8</term-titre>
                  <contenu><![CDATA[Aucune.]]></contenu>
                </para-rcp>
                <para-rcp>
                  <term-titre>10</term-titre>
                  <contenu><![CDATA[Ce produit ne doit pas être utilisé sur une peau irritée ou présentant des lésions.&Eacute;viter toute ingestion orale du produit par les animaux traités ou les animaux ayant été en contact avec des animaux traités.]]></contenu>
                </para-rcp>
                <para-rcp>
                  <term-titre>11</term-titre>
                  <contenu><![CDATA[-&nbsp;Ce produit peut provoquer des réactions allergiques. Les personnes présentant une hypersensibilité connue &agrave; la prednisolone, &agrave; la lidoca&iuml;ne, &agrave; d&rsquo;autres anesthésiques locaux ou &agrave; l'un des excipients ne doivent pas manipuler le produit.-&nbsp;La prednisolone peut être nocive pour le f&oelig;tus. Il est donc déconseillé aux femmes enceintes de manipuler ce produit.-&nbsp;Ce produit peut-être nocif apr&egrave;s exposition cutanée et orale. La lidoca&iuml;ne peut former des métabolites génotoxiques chez les humains. Une étude toxicologique &agrave; long terme chez le rat a montré que ces métabolites peuvent également induire des effets cancérog&egrave;nes &agrave; fortes doses. Le produit est également irritant pour la peau (réactions incluant éryth&egrave;me et prurit) et pour les yeux.-&nbsp;&Eacute;viter tout contact avec la peau, les yeux et la bouche, y compris le contact main-bouche et le contact main-&agrave;-yeux. Se laver les mains apr&egrave;s usage. En cas de contact accidentel avec la peau ou les yeux, rincer abondamment &agrave; l'eau.-&nbsp;Porter un équipement de protection individuelle composé de gants de protection imperméables &agrave; usage unique pour manipuler le médicament vétérinaire ou toucher la zone traitée.-&nbsp;Empêcher les enfants de toucher le cheval traité pendant la période de traitement et 12&nbsp;jours apr&egrave;s la fin du traitement.-&nbsp;Ne pas toucher la zone traitée. Si cela s'av&egrave;re nécessaire pour prodiguer les soins au cheval, porter des gants de protection imperméables &agrave; usage unique.-&nbsp;&nbsp;En cas d'ingestion accidentelle ou d'irritation prolongée de la peau ou des yeux, consulter immédiatement un médecin et lui montrer la notice ou l'étiquette.-&nbsp;Le matériel ou les instruments supplémentaires utilisés pour appliquer le produit, comme un pinceau, doivent être nettoyés minutieusement ou éliminés conformément aux exigences locales.-&nbsp;Conserver le flacon avec la pompe doseuse dans l&rsquo;emballage extérieur et dans un endroit s&ucirc;r, hors de la vue et de la portée des enfants, jusqu'au moment de l'utilisation. Le flacon doit être verrouillé apr&egrave;s chaque utilisation (voir détails &agrave; la rubrique &laquo; Posologie et voie d'administration &raquo;).]]></contenu>
                </para-rcp>
                <para-rcp>
                  <term-titre>12</term-titre>
                  <contenu><![CDATA[Aucune.]]></contenu>
                </para-rcp>
                <para-rcp>
                  <term-titre>13</term-titre>
                  <contenu><![CDATA[Des réactions locales (douleur, chaleur, perte de poils, squamosis, br&ucirc;lures, gonflement) ont été tr&egrave;s rarement rapportées.&nbsp;&nbsp;La fréquence des effets indésirables est définie comme suit :- tr&egrave;s fréquent (effets indésirables chez plus d'1 animal sur 10 animaux traités)- fréquent (entre 1 et 10 animaux sur 100 animaux traités)- peu fréquent (entre 1 et 10 animaux sur 1&nbsp;000 animaux traités)- rare (entre 1 et 10 animaux sur 10&nbsp;000 animaux traités)- tr&egrave;s rare (moins d'un animal sur 10&nbsp;000 animaux traités, y compris les cas isolés).]]></contenu>
                </para-rcp>
                <para-rcp>
                  <term-titre>14</term-titre>
                  <contenu><![CDATA[Des études sur des animaux de laboratoire ont mis en évidence les effets embryotoxiques de la prednisolone.La lidoca&iuml;ne pén&egrave;tre la barri&egrave;re placentaire et peut avoir des effets neurotoxiques et cardiorespiratoires chez le f&oelig;tus et le nouveau-né. L'innocuité du produit pour les animaux cibles n'a pas été évaluée pendant la gestation ni la lactation.Ne pas utiliser le produit chez les juments en gestation ou en lactation.]]></contenu>
                </para-rcp>
                <para-rcp>
                  <term-titre>15</term-titre>
                  <contenu><![CDATA[Ne pas utiliser avec d'autres produits, notamment des produits topiques, sur la zone traitée.]]></contenu>
                </para-rcp>
                <para-rcp>
                  <term-titre>16</term-titre>
                  <contenu><![CDATA[Voie cutanée. Appliquer le produit sur une zone localisée de la lésion sous-jacente &agrave; l&rsquo;aide d&rsquo;un pinceau (ou assimilé). Au besoin, un pansement non compressif peut être appliqué pour couvrir la zone traitée. Appliquer 10 &agrave; 30&nbsp;mL deux fois par jour, soit&nbsp;6 &agrave; 18 doses de la pompe doseuse, selon la nature de la lésion.&nbsp;La pompe doit être amorcée deux fois avant utilisation.Poursuivre le traitement jusqu'&agrave; la guérison sans toutefois le prolonger au-del&agrave; de 12 jours.Pour ouvrir le flacon, tourner le bec distributeur comme indiqué sur le dessus. Apr&egrave;s chaque utilisation, fermer le flacon en tournant le bec distributeur dans le sens opposé.&nbsp;]]></contenu>
                </para-rcp>
                <para-rcp>
                  <term-titre>17</term-titre>
                  <contenu><![CDATA[Aucune information disponible.]]></contenu>
                </para-rcp>
                <para-rcp>
                  <term-titre>18</term-titre>
                  <contenu><![CDATA[Viande et abats&nbsp;: 10 jours.Ne pas utiliser chez les animaux producteurs de lait destiné &agrave; la consommation humaine.]]></contenu>
                </para-rcp>
                <para-rcp>
                  <term-titre>19</term-titre>
                  <contenu><![CDATA[Groupe pharmacothérapeutique&nbsp;: &laquo;&nbsp;Autre produit topique pour douleurs articulaires et musculaires, associations&nbsp;&raquo;.Code ATC-vet : QM02AX99.]]></contenu>
                </para-rcp>
                <para-rcp>
                  <term-titre>20</term-titre>
                  <contenu><![CDATA[La prednisolone est un glucocortico&iuml;de de synth&egrave;se &agrave; action anti-inflammatoire. Elle a des propriétés anti-exsudatives, une action anti-granulomateuse et elle diminue la réaction fibroblastique en stabilisant les membranes cellulaires, elle empêche la destruction cellulaire et donc l'inflammation de la zone considérée. De plus, elle augmente le tonus vasculaire local et produit une diminution de l'&oelig;d&egrave;me. Enfin, elle prévient la dépolymérisation des mucopolysaccharides.La lidoca&iuml;ne est un anesthésique local.Le diméthylsulfoxyde (DMSO) favorise la pénétration transcutanée des substances actives en augmentant la perméabilité cellulaire.]]></contenu>
                </para-rcp>
                <para-rcp>
                  <term-titre>21</term-titre>
                  <contenu><![CDATA[Aucune information spécifique n'est disponible concernant l'application cutanée du produit chez le cheval.&nbsp;Appliquée localement sur la peau intacte, la lidoca&iuml;ne est absorbée de mani&egrave;re limitée et retardée. Une absorption plus importante de lidoca&iuml;ne est probable lorsque la fonction de la barri&egrave;re cutanée est compromise. La lidoca&iuml;ne est éliminée par métabolisme hépatique en métabolites actifs et inactifs, puis excrétée par les reins. La demi-vie terminale est inférieure &agrave; 2&nbsp;heures pour la plupart des esp&egrave;ces animales.&nbsp;Appliquée localement sur la peau intacte, la prednisolone est absorbée de mani&egrave;re limitée et retardée. Une absorption plus importante de prednisolone est probable lorsque la fonction de la barri&egrave;re cutanée est compromise. La prednisolone est métabolisée au niveau hépatique et extra-hépatique (notamment les reins). La demi-vie terminale pour les chevaux est d'environ 3&nbsp;heures. La molécule m&egrave;re et ses métabolites sont excrétés dans l'urine.]]></contenu>
                </para-rcp>
                <para-rcp>
                  <term-titre>23</term-titre>
                  <contenu><![CDATA[DiméthylsulfoxydeHydroxyéthylcelluloseEau purifiée]]></contenu>
                </para-rcp>
                <para-rcp>
                  <term-titre>24</term-titre>
                  <contenu><![CDATA[En l'absence d'études de compatibilité, ce médicament ne doit pas être mélangé &agrave; d'autres médicaments.]]></contenu>
                </para-rcp>
                <para-rcp>
                  <term-titre>25</term-titre>
                  <contenu><![CDATA[Durée de conservation du médicament vétérinaire tel que conditionné pour la vente&nbsp;: 18 mois.Durée de conservation apr&egrave;s premi&egrave;re ouverture du conditionnement primaire&nbsp;: 30 jours.]]></contenu>
                </para-rcp>
                <para-rcp>
                  <term-titre>26</term-titre>
                  <contenu><![CDATA[&Agrave; conserver &agrave; une température ne dépassant pas 30°C.Conserver dans l&rsquo;emballage extérieur de fa&ccedil;on &agrave; protéger de la lumi&egrave;re.]]></contenu>
                </para-rcp>
                <para-rcp>
                  <term-titre>27</term-titre>
                  <contenu><![CDATA[Flacon verre type III brun\nPompe doseuse polyéthyl&egrave;ne haute densité/polypropyl&egrave;ne munie d'un tube plongeur en polyéthyl&egrave;ne basse densité et polypropyl&egrave;ne\nBouchon &agrave; vis polypropyl&egrave;ne]]></contenu>
                </para-rcp>
                <para-rcp>
                  <term-titre>28</term-titre>
                  <contenu><![CDATA[Les conditionnements vides et tout reliquat de produit doivent être éliminés suivant les pratiques en vigueur régies par la réglementation sur les déchets.]]></contenu>
                </para-rcp>
                <para-rcp>
                  <term-titre>29</term-titre>
                  <contenu><![CDATA[AUDEVARD\n37-39 RUE DE NEUILLY\n92110 CLICHY \nFRANCE]]></contenu>
                </para-rcp>
                <para-rcp>
                  <term-titre>30</term-titre>
                  <contenu><![CDATA[FR/V/4228406 4/1986\n\nBo&icirc;te de 1 flacon de 125 mL\n\nToutes les présentations peuvent ne pas être commercialisées.]]></contenu>
                </para-rcp>
                <para-rcp>
                  <term-titre>31</term-titre>
                  <contenu><![CDATA[23/04/1986 - 26/04/2011]]></contenu>
                </para-rcp>
                <para-rcp>
                  <term-titre>32</term-titre>
                  <contenu><![CDATA[11/02/2022]]></contenu>
                </para-rcp>
              </paragraphes-rcp>
            </medicinal-product>
            
            </medicinal-product-group>
            """;

        //Act
        var res = AnmvFileHandler.DeserializeDataXmlString(xmlContent);

        //Assert
        Assert.NotNull(res);
        Assert.Equal("Ne pas utiliser chez les animaux producteurs de lait destiné à la consommation humaine.", res.MedicinalProducts.Single().VoiesAdmin.Single().Commentaire);
    }
}