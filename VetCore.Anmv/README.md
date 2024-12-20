# VetCore.Anmv

[![Build and test](https://github.com/CoreFactor-org/anmv/actions/workflows/dotnet-desktop.yml/badge.svg)](https://github.com/CoreFactor-org/anmv/actions/workflows/dotnet-desktop.yml)
[![NuGet](https://img.shields.io/nuget/v/VetCore.Anmv)](https://www.nuget.org/packages/VetCore.Anmv)
[![NuGet Downloads](https://img.shields.io/nuget/dt/VetCore.Anmv)](https://www.nuget.org/packages/VetCore.Anmv)

Une bibliothèque en .NET pour manipuler les données publiques de l'ANMV (Agence Nationale du Médicament Vétérinaire), disponibles sur :

> [Base de données publique des médicaments vétérinaires autorisés en France](https://www.data.gouv.fr/fr/datasets/base-de-donnees-publique-des-medicaments-veterinaires-autorises-en-france-1/)

---

## Fonctionnalités

- **DTO pour les données et descriptions** :
    - Modélisation des fichiers `Data` et `Description` publiés par l'ANMV.
- **Compatibilité étendue** :
    - Développé en .NET Standard 2.0 pour fonctionner avec des projets .NET Framework 4.8 et .NET Core.

---

## Installation

Pour inclure cette bibliothèque dans votre projet, utilisez la commande NuGet :

```bash
dotnet add package VetCore.Anmv
```

---

## Utilisation

### 1. Désérialisation des données XML en DTO

Les fichiers XML publiés par l'ANMV peuvent être transformés en objets DTO grâce à la sérialisation fournie par .NET.

#### Exemple pour un fichier de données (`Data`)

```csharp
using System.IO;
using VetCore.Anmv.Xml.Data;
using System.Xml.Serialization;

// Chemin vers le fichier XML
var dataFilePath = "path/to/data.xml";

// Initialiser le sérialiseur pour le type MedicinalProductGroupDto
var serializer = new XmlSerializer(typeof(MedicinalProductGroupDto));

// Désérialisation du fichier
using var stream = File.OpenRead(dataFilePath);
var dataDto = (MedicinalProductGroupDto)serializer.Deserialize(stream);

// Accéder aux données
Console.WriteLine($"Date du jeu de données : {dataDto.Informations.DateJeuDeDonnees}");
foreach (var product in dataDto.MedicinalProducts)
{
    Console.WriteLine($"Nom : {product.Nom}, Numéro AMM : {product.NumAmm}");
}
```

#### Exemple pour un fichier de description (`Description`)

```csharp
using System.IO;
using VetCore.Anmv.Xml.Descriptions;
using System.Xml.Serialization;

// Chemin vers le fichier XML
var descriptionFilePath = "path/to/description.xml";

// Initialiser le sérialiseur pour le type DonneesReferenceGroupDto
var serializer = new XmlSerializer(typeof(DonneesReferenceGroupDto));

// Désérialisation du fichier
using var stream = File.OpenRead(descriptionFilePath);
var descriptionDto = (DonneesReferenceGroupDto)serializer.Deserialize(stream);

// Accéder aux descriptions
foreach (var nature in descriptionDto.TermNat)
{
    Console.WriteLine($"Code : {nature.SourceCode}, Description : {nature.SourceDesc}");
}
```

---

### 2. Manipulation des DTO

Les DTO offrent une structure typée pour accéder et manipuler les données extraites des fichiers XML.

#### Exemple : Accéder aux produits médicamenteux

```csharp
foreach (var product in dataDto.MedicinalProducts)
{
    Console.WriteLine($"Nom : {product.Nom}, Date AMM : {product.DateAmm}");
    foreach (var voie in product.VoiesAdmin)
    {
        Console.WriteLine($"  - Voie d'administration : {voie.TermVa}, Espèce : {voie.TermEsp}");
    }
}
```

#### Exemple : Accéder aux descriptions des titulaires d'AMM

```csharp
foreach (var titulaire in descriptionDto.TermTit)
{
    Console.WriteLine($"Code titulaire : {titulaire.SourceCode}, Description : {titulaire.SourceDesc}");
}
```

---

### 3. Sérialisation des DTO en XML

Après manipulation des DTO, vous pouvez les transformer à nouveau en XML.

#### Exemple : Sérialiser un DTO en XML

```csharp
using System.IO;
using System.Xml.Serialization;

// Sérialiser un MedicinalProductGroupDto
var serializer = new XmlSerializer(typeof(MedicinalProductGroupDto));
using var stream = File.Create("path/to/output.xml");
serializer.Serialize(stream, dataDto);

Console.WriteLine("Fichier XML généré avec succès.");
```

---

### 4. Exemples avancés

#### Filtrer les produits par critère

```csharp
var produitsFiltrés = dataDto.MedicinalProducts
    .Where(p => p.TermFp == 148 && p.DateAmm.Year > 2000);

foreach (var produit in produitsFiltrés)
{
    Console.WriteLine($"Produit : {produit.Nom}, Date AMM : {produit.DateAmm}");
}
```

#### Construire un dictionnaire à partir des descriptions

```csharp
var titulaireDictionary = descriptionDto.TermTit
    .ToDictionary(t => t.SourceCode, t => t.SourceDesc);

Console.WriteLine($"Description pour le code 294 : {titulaireDictionary[294]}");
```

---

## Historique des versions

Cette bibliothèque suit les versions des données ANMV et garantit la compatibilité avec les fichiers XSD officiels.

- **Dernière version publiée** :
  [![NuGet](https://img.shields.io/nuget/v/VetCore.Anmv)](https://www.nuget.org/packages/VetCore.Anmv)

---

## Remontée de bugs ou remarques

Si vous rencontrez un problème ou souhaitez proposer une amélioration, ouvrez un ticket sur notre [page GitHub](https://github.com/CoreFactor-org/anmv/issues). Veuillez fournir des informations détaillées pour faciliter le diagnostic.

---

## Contributions

Les contributions sont les bienvenues !
- Soumettez une **pull request** pour proposer des améliorations.
- Veillez à tester et documenter votre code avant soumission.

---

## Licence

Ce projet est sous licence **MIT**. Consultez le fichier [LICENSE](https://github.com/CoreFactor-org/anmv/blob/main/LICENSE) pour plus d'informations.