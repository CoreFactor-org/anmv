# Données ANMV du 15 octobre 2026

Archive ZIP dérivée du fichier ANMV, conservée sans modification du XML. Le jeu de données indique `2026-10-15T09:30:04` et contient 3 221 médicaments.

L'archive de descriptions `amm-vet-fr-v2-d.zip` contient le XML téléchargé le 19 octobre 2026 depuis `https://pro.anses.fr/RCP/amm-vet-fr-v2-d.xml`, compressé sans modification. Il s'agit du thésaurus publié disponible à cette date, pas d'une version reconstruite pour compléter les références manquantes du fichier produits.

Le conditionnement `EU/2/21/270/041` du médicament `0888699` porte le pack-id source `1EC2116F-84FF-AEBA-E063-0E2A11ACBC`. Ce texte ne doit être ni complété ni normalisé.

Le test vérifie tous les pack-id du fichier contre le XML source et couvre les conversions XML/JSON. La validation XSD ciblée porte sur la fiche concernée : six autres fiches du fichier complet contiennent un élément `atcvet-code` vide, refusé par une règle indépendante du schéma actuel.

## Préparation à l'import

`AnmvImportPreparationTests` prépare les deux documents bruts avec `AnmvFileHandler.PrepareImport` et vérifie que l'archive des produits reste identique par SHA-256. Résultat : `IsValid = false`, `IsImportable = false`.

La préparation retire six occurrences strictement identiques, sans supprimer de médicament : une voie d'administration pour chacune des fiches `0973310`, `0055040` et `0434889`, deux pour `0316829`, et un modèle destiné à la vente pour `0726799`. Les 3 221 médicaments sont conservés.

Le schéma embarqué `VetCore.Anmv.Utils/Xsd/amm-vet-fr-v2-d.xsd`, utilisé par les tests, a été aligné sur le [XSD officiel du thésaurus](https://pro.anses.fr/RCP/amm-vet-fr-v2-d.xsd) consulté le 19 octobre 2026 : il accepte désormais l'élément facultatif `term-qsp` présent dans cette archive. Le rejet de cet élément provenait du schéma embarqué incomplet.

Résultat après tolérance des listes ATCvet vides :

1. **XSD des produits, anomalie tolérée à l'import** : six éléments `atcvet-code` sont vides alors que le schéma strict exige un enfant `code-atcvet`. Ces erreurs restent dans `SourceErrors` et maintiennent `IsValid = false`, mais ne figurent plus dans `BlockingErrors`. Les DTO conservent des listes vides.
2. **Références croisées, toujours bloquantes** : 78 codes distincts utilisés par 292 médicaments sont absents du thésaurus publié fourni : 19 titulaires, 54 substances, une forme pharmaceutique, un conditionnement, une espèce, une unité et un type de procédure. `IsImportable` reste donc faux pour ce jeu complet.

Aucun code manquant n'est inventé et aucune correction n'est écrite dans les fichiers. Le XSD strict reste inchangé ; seule sa variante de validation d'import en mémoire tolère les listes ATCvet vides. Ce diagnostic porte uniquement sur la bibliothèque ANMV, sans exécution des conversions métier de VCS.
