# VetCore.Anmv.RepMedVet

[![Build and test](https://github.com/CoreFactor-org/anmv/actions/workflows/dotnet-desktop.yml/badge.svg)](https://github.com/CoreFactor-org/anmv/actions/workflows/dotnet-desktop.yml)
[![NuGet](https://img.shields.io/nuget/v/VetCore.Anmv.RepMedVet)](https://www.nuget.org/packages/VetCore.Anmv.RepMedVet)
[![NuGet Downloads](https://img.shields.io/nuget/dt/VetCore.Anmv.RepMedVet)](https://www.nuget.org/packages/VetCore.Anmv.RepMedVet)

Une bibliothèque .NET pour lire le fichier [REPMEDVET.xml](https://pro.anses.fr/calypso/REPMEDVET.xml), le référentiel des médicaments vétérinaires que l'ANMV publie pour CalypsoVet et les logiciels vétérinaires.

---

## Ce que contient le fichier

Tous les médicaments vétérinaires autorisés ayant au moins un GTIN déclaré, avec notamment :

- `IsSubjectToReporting` : les cessions du médicament doivent être transmises à CalypsoVet ;
- `QspUnit` : l'unité élémentaire officielle des quantités transmises ;
- les présentations (GTIN, nom, numéro d'AMM, conditionnement) ;
- les codes ATCvet, les substances, les espèces cibles et les temps d'attente ;
- le RCP, sauf pour les médicaments issus d'une procédure centralisée.

Le fichier ne remplace pas la [base publique de l'ANMV](https://www.data.gouv.fr/fr/datasets/base-de-donnees-publique-des-medicaments-veterinaires-autorises-en-france-1/), lue par `VetCore.Anmv` : celle-ci couvre aussi les médicaments sans GTIN et les conditions de délivrance, et son RCP est plus complet.

---

## Installation

```bash
dotnet add package VetCore.Anmv.RepMedVet
```

Le package cible .NET Standard 2.0 et ne dépend pas de `VetCore.Anmv`.

---

## Utilisation

### Lecture

```csharp
using VetCore.Anmv.RepMedVet;
using VetCore.Anmv.RepMedVet.Json;
using VetCore.Anmv.RepMedVet.Xml;

// Depuis un fichier, un flux ou une chaîne
MedecineRepositoryDto repository = RepMedVetFileHandler.DeserializeFile(new FileInfo("path/to/REPMEDVET.xml"));

// Les DTO JSON portent les règles de validation et convertissent IsSubjectToReporting en booléen
MedecineRepositoryDtoJson dto = repository.ToJsonDto();
if (!dto.Validate(out var errors))
{
    Console.WriteLine(string.Join(Environment.NewLine, errors));
}

foreach (var product in dto.Products.Where(product => product.IsSubjectToReporting))
{
    Console.WriteLine($"{product.Product.Label} : {product.QspUnit?.Label}");
}
```

Le fichier n'a pas de schéma public. La lecture est donc stricte : un élément, un attribut ou un texte inconnu des DTO, comme une valeur illisible, lève une `RepMedVetFormatException`. Une évolution du format est ainsi vue dès son apparition, au lieu d'être ignorée. Aucune DTD ni ressource externe n'est résolue.

### Écriture et conversions

```csharp
string xml = repository.SerializeToXml(indent: true);
string json = JsonSerializer.Serialize(dto);
MedecineRepositoryDto back = dto.ToXmlDto();
```

---

## À savoir sur les données

- **Quantités :** les concentrations et les temps d'attente restent du texte, tels que publiés : la source mêle virgules, points et espaces.
- **`QspUnit` :** son identifiant vient de deux dictionnaires ANMV, les unités et les présentations. `ID="5"` désigne `g` ou `Dispositif` selon le cas : seul le libellé est sans ambiguïté.
- **`IsSubjectToReporting` :** seule source officielle. La liste de préfixes ATCvet de la spécification ANMV ne suffit pas à la déduire.
- **GTIN :** un même GTIN peut figurer sur deux médicaments.
- **Valeurs vides :** `AuthorisationNumber` et `Comment` peuvent être vides. Ils restent des chaînes vides, sans normalisation.
