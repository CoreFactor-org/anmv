# VetCore.Anmv.Utils

[![Build and test](https://github.com/CoreFactor-org/anmv/actions/workflows/dotnet-desktop.yml/badge.svg)](https://github.com/CoreFactor-org/anmv/actions/workflows/dotnet-desktop.yml)
[![NuGet](https://img.shields.io/nuget/v/VetCore.Anmv.Utils)](https://www.nuget.org/packages/VetCore.Anmv.Utils)
[![NuGet Downloads](https://img.shields.io/nuget/dt/VetCore.Anmv.Utils)](https://www.nuget.org/packages/VetCore.Anmv.Utils)

Une bibliothèque utilitaire en .NET pour valider, sérialiser, et manipuler les fichiers XML de l'ANMV (Agence Nationale du Médicament Vétérinaire). Elle inclut les XSD officiels intégrés pour garantir la conformité.

---

## Fonctionnalités principales

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

---

### 2. Validation des fichiers XML avec un XSD

#### Exemple : Valider un fichier XML `Data`

```csharp
using System.IO;
using VetCore.Anmv.Utils;
using VetCore.Anmv.Utils.Xsd;

var xmlFile = new FileInfo("path/to/data.xml");
var xsdContent = AmnvFilesKey.Data_XSD_AMM.GetXsdContent();

// Valider le fichier XML avec le XSD
var result = AnmvFileHandler.ValidateXmlWithXsd(xmlFile, xsdContent);

if (result.Errors.Count == 0)
{
    Console.WriteLine("Validation réussie !");
}
else
{
    Console.WriteLine("Erreurs de validation :");
    Console.WriteLine(result.PrintErrorsAndWarnings());
}
```

---

### 3. Génération de XSD depuis un type DTO

Générez un fichier XSD à partir de n'importe quel type DTO pour des besoins spécifiques :

```csharp
using VetCore.Anmv.Utils;

var xsd = AnmvFileHandler.GenerateXsdFromDtoType(typeof(MedicinalProductGroupDto));
Console.WriteLine($"XSD généré :\n{xsd}");
```

---

### 4. Sérialisation des DTO en XML

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

### 5. Accès aux fichiers XSD intégrés

Les XSD officiels sont embarqués dans la bibliothèque et accessibles via `AmnvFilesKey` et `XsdAnmvFileAccessor`.

#### Exemple : Obtenir un fichier XSD en tant que contenu texte

```csharp
using VetCore.Anmv.Utils.Xsd;

var xsdContent = AmnvFilesKey.Data_XSD_AMM.GetXsdContent();
Console.WriteLine($"Contenu du XSD Data :\n{xsdContent}");
```

---

## Contributions

Les contributions sont les bienvenues ! Soumettez une pull request ou ouvrez une issue sur notre [GitHub](https://github.com/CoreFactor-org/anmv) pour proposer des améliorations ou signaler des problèmes.

---

## Licence

Ce projet est sous licence **MIT**. Consultez le fichier [LICENSE](https://github.com/CoreFactor-org/anmv/blob/main/LICENSE) pour plus d'informations.

---