# VetCore.Anmv.Utils

[![Build and test](https://github.com/CoreFactor-org/anmv/actions/workflows/dotnet-desktop.yml/badge.svg)](https://github.com/CoreFactor-org/anmv/actions/workflows/dotnet-desktop.yml)
[![NuGet](https://img.shields.io/nuget/v/VetCore.Anmv.Utils)](https://www.nuget.org/packages/VetCore.Anmv.Utils)
[![NuGet Downloads](https://img.shields.io/nuget/dt/VetCore.Anmv.Utils)](https://www.nuget.org/packages/VetCore.Anmv.Utils)

Une bibliothèque utilitaire en .NET pour valider, sérialiser, et manipuler les fichiers XML de l'ANMV (Agence Nationale du Médicament Vétérinaire). Elle inclut les XSD officiels intégrés pour garantir la conformité.

---

## Fonctionnalités principales

### Migration vers 2.0.0

Cette version utilise `VetCore.Anmv` 2.0.0 : `PackId` devient une chaîne facultative conservant la valeur source. La validation des DTO et le schéma XSD embarqué ne lui imposent plus le format GUID ; le XSD conserve une longueur maximale de 50 caractères. `ProdId` reste inchangé. Le thésaurus `term-qsp` est conservé dans les DTO XML/JSON et suit les mêmes contrôles et dédoublonnages stricts que les autres listes de références. Ce schéma adapté ne remplace pas la spécification de l'éditeur.

### Validité du brut et préparation à l'import

`AnmvFileHandler.PrepareImport(descriptionsXml, productsXml)` retourne :

- `IsValid` : validation XSD et validation des DTO du fichier brut, doublons compris ;
- `IsImportable` : absence de blocage ANMV après dédoublonnage strict et tolérance des listes ATCvet vides ; les contrôles métier propres au consommateur restent nécessaires ;
- `SourceErrors`, `BlockingErrors` : diagnostics ordonnés par étape (XSD, désérialisation, validation des données) ;
- `RemovedDuplicates` : médicament, collection et nombre d'occurrences retirées ;
- `Descriptions`, `Products` : DTO préparés, disponibles pour inspection même si des blocages subsistent, sauf échec de désérialisation.

Seuls les blocs exactement identiques des compositions, voies d'administration, modèles de vente, codes GTIN/PackId, codes ATCvet, paragraphes RCP et entrées du thésaurus sont dédoublonnés. La première occurrence et l'ordre des éléments restants sont conservés. Aucune normalisation de casse, d'espaces, d'identifiant ou de quantité n'est appliquée. Les fichiers bruts ne sont jamais réécrits.

Un même code du thésaurus avec des valeurs contradictoires reste bloquant. Deux fiches portant le même numéro de médicament ne sont pas fusionnées. Deux blocs de conditionnement qui diffèrent sont conservés distincts, même s'ils partagent un PackId ou un GTIN : ces identifiants seuls ne sont jamais une clé de dédoublonnage. Une référence absente reste bloquante, même si des doublons ont été retirés.

Un conteneur `atcvet-code` présent mais vide est invalide selon le XSD strict : `IsValid` reste faux et `SourceErrors` conserve le diagnostic. Il est toutefois importable comme une liste vide. Pour les blocages d'import, le XML original est revalidé avec un schéma dérivé en mémoire autorisant uniquement zéro enfant `code-atcvet` ; le XSD embarqué et le brut restent inchangés. Un enfant `code-atcvet` vide ou mal formé, un conteneur répété, un élément ou attribut inattendu, ainsi que toute autre erreur XSD restent bloquants.

```csharp
var preparation = AnmvFileHandler.PrepareImport(descriptionsXml, productsXml);
if (!preparation.IsImportable)
{
    // Présenter BlockingErrors ; ne pas appliquer les DTO préparés.
    return;
}
// Utiliser preparation.Products et preparation.Descriptions pour la suite de l'import.
// IsValid peut rester false : SourceErrors conserve les anomalies du brut.
```

- **Désérialisation facile** : Transformez les fichiers XML `Data` et `Description` en objets DTO typés.
- **Validation XML avec XSD** : Validez vos fichiers XML à l'aide des XSD officiels fournis.
- **Génération de XSD** : Génération de fichiers XSD à partir de types DTO.
- **Sérialisation en XML** : Transformez vos DTO en chaînes XML.
- **Accès aux XSD officiels intégrés** : Les fichiers XSD `Data` et `Description` sont directement accessibles depuis les ressources embarquées.

---

## Installation

Ajoutez cette bibliothèque à votre projet via NuGet :

```bash
dotnet add package VetCore.Anmv.Utils
```

---

## Utilisation

### 1. Désérialisation des fichiers XML

#### Exemple pour un fichier `Data`

```csharp
using System.IO;
using VetCore.Anmv.Utils;
using VetCore.Anmv.Xml.Data;

var dataFile = new FileInfo("path/to/data.xml");

// Désérialiser un fichier Data
var dataDto = AnmvFileHandler.DeserializeDataFile(dataFile);

Console.WriteLine($"Jeu de données du : {dataDto?.Informations.DateJeuDeDonnees}");
foreach (var product in dataDto?.MedicinalProducts ?? Enumerable.Empty<MedicinalProductDto>())
{
    Console.WriteLine($"Produit : {product.Nom}, Numéro AMM : {product.NumAmm}");
}
```

#### Exemple pour un fichier `Description`

```csharp
using System.IO;
using VetCore.Anmv.Utils;
using VetCore.Anmv.Xml.Descriptions;

var descriptionFile = new FileInfo("path/to/description.xml");

// Désérialiser un fichier Description
var descriptionDto = AnmvFileHandler.DeserializeDescriptionFile(descriptionFile);

foreach (var entry in descriptionDto?.TermNat ?? Enumerable.Empty<EntryDto>())
{
    Console.WriteLine($"Nature : {entry.SourceCode} - {entry.SourceDesc}");
}
```

> Il est également possible de désérializer directement un xml sous forme de string via les méthodes `AnmvFileHandler.DeserializeDescriptionXmlString(xmlString)` et `AnmvFileHandler.DeserializeDataXmlString(xmlString)`

---

### 2. Validation des fichiers XML avec un XSD

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

---

### 3. Sérialisation des DTO en XML

#### Exemple pour un fichier `Data`

```csharp
using VetCore.Anmv.Utils;
using VetCore.Anmv.Xml.Data;

var dto = new MedicinalProductGroupDto
{
    Informations = new InformationsDto { DateJeuDeDonnees = DateTime.Now },
    MedicinalProducts = new List<MedicinalProductDto>
    {
        new MedicinalProductDto { Nom = "NEOMYCINE", NumAmm = "0033630" }
    }
};

var xml = dto.SerializeAmnvToXml(indent: true);
Console.WriteLine($"XML généré :\n{xml}");
```

---

### 4. Accès aux fichiers XSD intégrés

Les XSD officiels sont embarqués dans la bibliothèque et accessibles via `AmnvFilesKey` et `XsdAnmvFileAccessor`.

#### Exemple : Obtenir un fichier XSD en tant que contenu texte

```csharp
using VetCore.Anmv.Utils.Xsd;

// Récupérer le contenu XSD pour les données (en string)
string dataXsd = AmnvFilesKey.Data_XSD_AMM.GetXsdContent();

// Récupérer le contenu XSD pour la description (en string)
string descriptionXsd = AmnvFilesKey.Descriptions_XSD_AMM.GetXsdContent();
```

---

## Contributions

Les contributions sont les bienvenues ! Soumettez une pull request ou ouvrez une issue sur notre [GitHub](https://github.com/CoreFactor-org/anmv) pour proposer des améliorations ou signaler des problèmes.

---

## Licence

Ce projet est sous licence **MIT**. Consultez le fichier [LICENSE](https://github.com/CoreFactor-org/anmv/blob/main/LICENSE) pour plus d'informations.

---
