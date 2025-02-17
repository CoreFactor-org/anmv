# VetCore.Anmv

[![Build and test](https://github.com/CoreFactor-org/anmv/actions/workflows/dotnet-desktop.yml/badge.svg)](https://github.com/CoreFactor-org/anmv/actions/workflows/dotnet-desktop.yml)


Une bibliothèque en .NET pour manipuler les données publiques de l'ANMV (Agence Nationale du Médicament Vétérinaire) disponibles sur data.gouv à l'adresse : 

> [https://www.data.gouv.fr/fr/datasets/base-de-donnees-publique-des-medicaments-veterinaires-autorises-en-france-1/](https://www.data.gouv.fr/fr/datasets/base-de-donnees-publique-des-medicaments-veterinaires-autorises-en-france-1/).

---

Cette bibliothèque propose :

- Des DTO pour les données (`Data`) et la description (`Description`) fournies par l'ANMV.
- Des utilitaires pour valider, sérialiser, et désérialiser les fichiers XML au format officiel de l'ANMV.
- Un projet de tests pour valider la conformité avec les XSD officiels.

## Projets

### VetCore.Anmv [![NuGet](https://img.shields.io/nuget/v/VetCore.Anmv)](https://www.nuget.org/packages/VetCore.Anmv)
- **Type** : Bibliothèque .NET Standard 2.0
- **Contenu** : DTO pour les fichiers `Data` et `Description`.
- **Objectif** : Compatible avec les projets .NET Framework 4.8 et .NET Core.
- **Readme** : [README détaillé VetCore.Anmv](VetCore.Anmv/README.md)


### VetCore.Anmv.Utils [![NuGet](https://img.shields.io/nuget/v/VetCore.Anmv.Utils)](https://www.nuget.org/packages/VetCore.Anmv.Utils)
- **Type** : Bibliothèque utilitaire.
- **Contenu** :
  - Classes principales : `AnmvFileHandler` et `XsdAnmvFileAccessor`.
  - Validation et manipulation des fichiers XML basés sur les XSD officiels.
- **Readme** : [README  détaillé VetCore.Anmv.Utils](VetCore.Anmv.Utils/README.md)

### VetCore.Anmv.Tests
- **Type** : Projet de tests unitaires
- **Contenu** :
  - Tests de validation des fichiers XML.
  - Historique des fichiers XML pour tests.

## Utilisation

### 1. Désérialisation des fichiers XML

```csharp
using VetCore.Anmv.Utils;

var dataFile = new FileInfo("path/to/data.xml");
var descriptionFile = new FileInfo("path/to/description.xml");

var dataDto = AnmvFileHandler.DeserializeDataFile(dataFile);
var descriptionDto = AnmvFileHandler.DeserializeDescriptionFile(descriptionFile);
```

### 2. Sérialisation des DTO vers XML

```csharp
var xmlData = dataDto.SerializeAmnvToXml(indent: true);
var xmlDescription = descriptionDto.SerializeAmnvToXml(indent: true);
```

### 2.1 Conversion entre DTO XML et DTO JSON

La bibliothèque offre également la possibilité de convertir les DTO XML en DTO JSON et inversement grâce à des méthodes d’extension dédiées. 

Par exemple, pour convertir un objet XML en JSON, il suffit d’appeler la méthode `ToJsonDto()` directement sur l’instance XML :

```csharp
// Exemple de conversion XML → JSON
MedicinalProductGroupDto xmlDto = AnmvFileHandler.DeserializeDataFile(new FileInfo("path/to/data.xml"));
var jsonDto = xmlDto.ToJsonDto();
```

De même, pour reconvertir un DTO JSON en XML, vous pouvez utiliser la méthode `ToXmlDto()` :

```csharp
// Exemple de conversion JSON → XML
MedicinalProductGroupDtoJson jsonDto = /* votre DTO JSON déjà obtenu */;
var xmlDto = jsonDto.ToXmlDto();
```

Ces conversions bidirectionnelles facilitent l’intégration et la manipulation des données.

---

### 3. Validation d'un fichier XML avec un XSD

Ces fichiers peuvent être utilisés pour valider des fichiers XML via la classe `AnmvFileHandler` :

#### Validation d'un fichier XML Description:

```csharp
using VetCore.Anmv.Utils;
using VetCore.Anmv.Utils.Xsd;

FileInfo xmlFile = new FileInfo("path/to/file.xml");

// Valider le fichier XML avec le XSD de Description
var validationResult = AnmvFileHandler.ValidateXml(xmlFile, AmnvFilesKey.Descriptions_XSD_AMM);

if (validationResult.Errors.Count == 0)
{
    Console.WriteLine("Validation réussie !");
}
else
{
    Console.WriteLine(validationResult.PrintErrorsAndWarnings(Environment.NewLine));
}
```

#### Validation d'un fichier XML Données :

```csharp
using VetCore.Anmv.Utils;
using VetCore.Anmv.Utils.Xsd;

FileInfo xmlFile = new FileInfo("path/to/file.xml");

// Valider le fichier XML avec le XSD de DATA (changement de la clé AmnvFilesKey utilisée)
var validationResult = AnmvFileHandler.ValidateXml(xmlFile, AmnvFilesKey.Data_XSD_AMM);

if (validationResult.Errors.Count == 0)
{
    Console.WriteLine("Validation réussie !");
}
else
{
    Console.WriteLine(validationResult.PrintErrorsAndWarnings(Environment.NewLine));
}
```

En cas d'erreur de configuration (par exemple, si le fichier XSD n'est pas trouvé), une exception `FileNotFoundException` sera levée avec un message explicatif. Merci d'alerter les mainteneurs.

## Structure des fichiers XSD

Les fichiers XSD officiels utilisés pour valider les fichiers XML de l'ANMV sont intégrés dans la bibliothèque et accessibles via l'enum `AmnvFilesKey` et la classe `XsdAnmvFileAccessor`. Les clés disponibles sont :

- **`AmnvFilesKey.Data_XSD_AMM`** : Correspond au fichier XSD pour les données (`amm-vet-fr-v2-v.xsd`).
- **`AmnvFilesKey.Descriptions_XSD_AMM`** : Correspond au fichier XSD pour la description des termes de référence (`amm-vet-fr-v2-d.xsd`).

### Accès aux fichiers XSD

Pour accéder aux fichiers XSD, utilisez la méthode `GetXsdContent` de la classe `XsdAnmvFileAccessor`. Voici un exemple de code :

```csharp
using VetCore.Anmv.Utils.Xsd;

// Récupérer le contenu XSD pour les données (en string)
string dataXsd = AmnvFilesKey.Data_XSD_AMM.GetXsdContent();

// Récupérer le contenu XSD pour la description (en string)
string descriptionXsd = AmnvFilesKey.Descriptions_XSD_AMM.GetXsdContent();
```

## Historique des fichiers

Les fichiers XML et XSD utilisés pour les tests sont maintenus dans le projet `VetCore.Anmv.Tests` pour garantir la compatibilité avec les versions futures des données officielles.

---

## Exemples de tests unitaires

Les tests unitaires incluent des validations des fichiers XML historisés avec les XSD officiels, tels que :

```csharp
[Fact]
    public void AMNV_DESCRIPTIONS_Validate_xml_with_official_xsd()
    {
        //Arrange
        var xml = UnitTestFileAccessor.GetFile(AmnvFilesUnitTest.XML_AMM_Descriptions_2025_01_14);

        //Act
        var res = AnmvFileHandler.ValidateXml(xml.ToFileInfo(), AmnvFilesKey.Data_XSD_AMM);

        //Assert
        Assert.True(res.Errors.Count == 0 && res.Warnings.Count == 0, res.PrintErrorsAndWarnings(Environment.NewLine));
    }
```

Dans ce cas pour le xml de description de début 2025.

## Remontée de bugs ou remarques

Si vous rencontrez un problème ou souhaitez proposer une amélioration, n'hésitez pas à ouvrir un ticket dans ce dépôt. Veuillez fournir des détails précis sur le problème ou la suggestion pour faciliter la résolution.

## Contributions

Les contributions sont les bienvenues ! Si vous souhaitez améliorer le projet, vous pouvez soumettre une pull request. Assurez-vous que votre code est testé et conforme aux standards du projet avant de proposer une modification.
