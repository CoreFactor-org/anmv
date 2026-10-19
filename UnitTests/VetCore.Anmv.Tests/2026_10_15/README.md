# Données ANMV du 15 octobre 2026

Archive ZIP dérivée du fichier ANMV, conservée sans modification du XML. Le jeu de données indique `2026-10-15T09:30:04` et contient 3 221 médicaments.

L'archive de descriptions `amm-vet-fr-v2-d.zip` contient le XML téléchargé le 19 octobre 2026 depuis `https://pro.anses.fr/RCP/amm-vet-fr-v2-d.xml`, compressé sans modification. Il s'agit du thésaurus publié disponible à cette date, pas d'une version reconstruite pour compléter les références manquantes du fichier produits.

Le conditionnement `EU/2/21/270/041` du médicament `0888699` porte le pack-id source `1EC2116F-84FF-AEBA-E063-0E2A11ACBC`. Ce texte ne doit être ni complété ni normalisé.

Le test vérifie tous les pack-id du fichier contre le XML source et couvre les conversions XML/JSON. La validation XSD ciblée porte sur la fiche concernée : six autres fiches du fichier complet contiennent un élément `atcvet-code` vide, refusé par une règle indépendante du schéma actuel.

Le schéma embarqué `VetCore.Anmv.Utils/Xsd/amm-vet-fr-v2-d.xsd`, utilisé par les tests, a été aligné sur le [XSD officiel du thésaurus](https://pro.anses.fr/RCP/amm-vet-fr-v2-d.xsd) consulté le 19 octobre 2026 : il accepte désormais l'élément facultatif `term-qsp` présent dans cette archive. Le rejet de cet élément provenait du schéma embarqué incomplet.
