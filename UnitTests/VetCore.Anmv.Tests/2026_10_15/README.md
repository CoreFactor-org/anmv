# Données ANMV du 15 octobre 2026

Archive ZIP dérivée du fichier ANMV, conservée sans modification du XML. Le jeu de données indique `2026-10-15T09:30:04` et contient 3 221 médicaments.

Les deux XML proviennent de la même [archive officielle ANMV XML V2](https://pro.anses.fr/RCP/amm-vet-fr-v2-v.7z), référencée par la [ressource data.gouv.fr](https://www.data.gouv.fr/api/1/datasets/r/6e88f8e0-6089-445b-924d-012ee750027a), téléchargée le 19 octobre 2026. Chaque XML est recompressé en ZIP sans modification de ses octets. Le fichier produits existant est identique à celui de cette archive.

Empreintes SHA-256 des XML décompressés :

- Produits : `b9e73cb5c5181b592938df2e968112a72b66c6b81a0a960e5791b5ca0f4271bd`.
- Thésaurus : `726c1b44477c36354622f211cd645ba99daae4a9218ea2129121a1ac052d95d8`.

Le conditionnement `EU/2/21/270/041` du médicament `0888699` porte le pack-id source `1EC2116F-84FF-AEBA-E063-0E2A11ACBC`. Ce texte ne doit être ni complété ni normalisé.

Le test vérifie tous les pack-id du fichier contre le XML source et couvre les conversions XML/JSON. La validation XSD ciblée porte sur la fiche concernée : six autres fiches du fichier complet contiennent un élément `atcvet-code` vide, refusé par une règle indépendante du schéma actuel.

## Préparation à l'import

`AnmvImportPreparationTests.OctoberArchive_IsImportableWithPairedThesaurusAndPreservesRawBytes` prépare les deux documents bruts avec `AnmvFileHandler.PrepareImport` et vérifie que les deux archives restent identiques par SHA-256. Résultat vérifié : `IsValid = false`, `IsImportable = true`, aucun `BlockingErrors`.

La préparation retire six occurrences strictement identiques, sans supprimer de médicament : une voie d'administration pour chacune des fiches `0973310`, `0055040` et `0434889`, deux pour `0316829`, et un modèle destiné à la vente pour `0726799`. Les 3 221 médicaments sont conservés.

Le schéma embarqué `VetCore.Anmv.Utils/Xsd/amm-vet-fr-v2-d.xsd`, utilisé par les tests, a été aligné sur le [XSD officiel du thésaurus](https://pro.anses.fr/RCP/amm-vet-fr-v2-d.xsd) consulté le 19 octobre 2026 : il accepte désormais l'élément facultatif `term-qsp` présent dans cette archive. Le rejet de cet élément provenait du schéma embarqué incomplet.

Résultat après tolérance des listes ATCvet vides :

1. **XSD des produits, anomalie tolérée à l'import** : six éléments `atcvet-code` sont vides alors que le schéma strict exige un enfant `code-atcvet`. Ces erreurs restent dans `SourceErrors` et maintiennent `IsValid = false`, mais ne figurent plus dans `BlockingErrors`. Les DTO conservent des listes vides.
2. **Références croisées résolues** : les 78 codes signalés sont tous présents dans le thésaurus de la même archive, notamment l'unité `11648`, définie comme « log10 particule ARN ». Aucune fusion ni création de référence n'est nécessaire.

Le diagnostic précédent associait incorrectement les produits d'octobre au XML de thésaurus publié séparément à l'URL `https://pro.anses.fr/RCP/amm-vet-fr-v2-d.xml`, resté daté du 15 janvier 2025. Notre ancienne copie était fidèle à cette URL mais ne formait pas la bonne paire avec les produits. Les 78 références concernaient 283 médicaments distincts, et non 292 : l'ancien comptage séparait à tort plusieurs contextes d'erreur d'une même fiche. `term-cd` désigne les conditions de délivrance, pas le conditionnement.

Aucun code manquant n'est inventé et aucune correction n'est écrite dans les fichiers. Le XSD strict reste inchangé ; seule sa variante de validation d'import en mémoire tolère les listes ATCvet vides. Ce diagnostic porte uniquement sur la bibliothèque ANMV, sans exécution des conversions métier de VCS.
