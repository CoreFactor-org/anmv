# REPMEDVET du 22 octobre 2026

Archive ZIP du référentiel [REPMEDVET.xml](https://pro.anses.fr/calypso/REPMEDVET.xml), publié en accès libre par l'ANMV pour CalypsoVet et les logiciels vétérinaires. Le fichier a été récupéré le 2 novembre 2026. Il indique une génération le `2026-10-22T05:03:13.1297425+02:00` et le scénario `MED 0.9`. Il est recompressé en ZIP sans modification de ses octets.

Empreinte SHA-256 du XML décompressé : `323b4162c1669adf581234517b74a9dd41459d30bb43dddb3df15e2376837eb0`.

Le fichier contient 2 368 médicaments et 4 258 présentations, tous avec un GTIN à 14 chiffres, dont 614 médicaments soumis à déclaration.

## Absence de schéma

Aucun XSD n'accompagne ce fichier. Les tests compensent de deux façons :

- `Serialize_should_keep_every_element_of_the_file` réécrit le fichier depuis les DTO et compare tous les éléments, attributs et textes à l'original ;
- la lecture refuse tout élément, attribut ou texte inconnu des DTO.

## Comparaison avec l'archive ANMV du 15 octobre 2026

- `QspUnit` a le même libellé que `excipient-qsp` sur tous les GTIN communs. Quatre écarts apparents viennent de GTIN partagés par GUMBOHATCH et EVANOVO, deux vaccins non déclarables.
- `IsSubjectToReporting` n'existe que dans ce fichier. Il vaut 1 pour quatre médicaments que la liste de préfixes ATCvet de la spécification ANMV ne désigne pas : CORTIZEME, PREDNIDERM, CORTANMYCETINE (`QD07CA03`) et ORIBIOTIC (`QS02DC`).
- Il ne remplace pas l'archive : celle-ci couvre aussi les médicaments sans GTIN et les conditions de délivrance.
- Son RCP est moins complet que celui de l'archive.
