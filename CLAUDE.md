# Jeu Ulysse, Sacha et Bibou : portage Unity

Tu aides Mich à porter dans Unity un jeu d'aventure pour son fils Ulysse (4 ans), avec son petit frère Sacha (2 ans). Le jeu existe déjà en 9 chapitres HTML (Canvas 2D). Il se passe dans de vrais lieux de Villeurbanne et de Lyon.

## Avant de travailler
- Lis `docs/reprise.md` (état, ce qui reste, leçons) et `docs/bible-du-jeu.md` (univers, chapitres).
- Lis `docs/personnages.md` avant de dessiner ou d'animer un personnage.
- Pour un chapitre, lis son plan dans `docs/plans/` puis sa page HTML dans `reference-html/`. La page HTML est la référence : dialogues exacts, ordre des scènes, positions, couleurs, sons. Le plan dit l'intention et les choix de Mich.
- Ne lis pas les 9 pages HTML d'un coup : une à la fois, et seulement les parties utiles (grep sur le nom de la scène ou du PNJ).
- Avant de construire un chapitre, pose à Mich des questions sur les vrais lieux et sur ce qu'Ulysse aime. Il donne souvent des précisions de terrain.

## Le portage
- Le code JavaScript ne se recopie pas : il sert de spécification. Reproduire le comportement, pas la structure.
- Un moteur commun (déplacement, dialogue, bouton d'action, aide, compagnons qui suivent, balades), puis une scène Unity par lieu. Construire et valider le moteur sur un seul chapitre avant de porter les autres.
- Ordre conseillé : le ch1 v3 en dernier (la résidence est encore en chantier côté HTML, voir reprise.md).
- Garder ce qui marche : voix de synthèse française, bouton unique contextuel, toucher pour marcher, aide en ampoule.

## Univers
- Héros : Ulysse (trottinette verte, casque orange, cape rouge), Sacha (trottinette bleu foncé, casque bleu clair, cape jaune), Bibou (chat van turc), les doudous Didou (renard, à Ulysse) et Ninou (panthère bleue, à Sacha), Papa et Maman.
- Sacha est un garçon : toujours au masculin.
- Rex le Chipeur : petit T-rex qui chipait les jouets parce qu'il n'en avait pas. Jamais effrayant. Il devient ami au ch7 et s'appelle ensuite « Rex ».
- Pas de combat, pas d'échec, pas de chrono stressant. On aide, on répare, on soigne, on trouve.
- Les noms sont dits à voix haute. Un PNJ se présente une seule fois, par « C'est X ! » (pas la famille ni les doudous).
- Lieux et trajets comme en vrai : lignes de métro, tram, bus et TER exactes.

## Règles de conception
- Un enfant de 4 ans comprend seul quoi faire : indices visuels et sonores forts, objectifs proches.
- Les PNJ sont placés AVANT leur quête, sur le chemin d'approche, et parlent quand on approche.
- Ulysse est la vedette : grosses actions (boutons, conduite, réparations, boule de feu, coloriage). Sacha fait les petites (soigner, retrouver, câliner, l'eau). Sacha n'agit jamais seul pendant qu'Ulysse regarde : duo, suite automatique d'une action d'Ulysse, ou geste fait aussi par Ulysse.
- Bibou et Didou ont chacun un moment. Un compagnon bouge d'abord vers l'endroit, ensuite le texte dit ce qu'il a trouvé.
- Objets cachés : indice qui s'intensifie quand on s'approche (étoiles, son plus fort).
- Une salle = un écran plein. L'objectif n'est jamais fait dès l'entrée. Une seule étape mise en avant à la fois.
- Une réplique ne parle jamais d'une chose hors écran ou pas encore visible.
- Niveaux denses et vivants : décor qui bouge, scènes déclenchées par des boutons.
- Balades (funiculaire, téléphérique, train, manège, tram) : en mettre. Pendant une balade, Ulysse regarde, aucune action répétée.
- Mots simples, répliques courtes. Ulysse zappe le blabla.
- Un choix d'Ulysse n'est jamais jugé (« Rex va adorer ! », jamais « bon choix »).
- Pas de cycle jour/nuit.

## Interdits
- Aucune propriété intellectuelle protégée ni sosie : pas de Pokémon, Mario, Totoro, Férociraptors, Petit Pigeon, Guignol, ni de personnage connu. Pas de logo ni de nom de marque dessiné (un McDo se dessine sans logo).
- Jamais d'adresse, de nom de rue ni de nom de tour dans le jeu (les noms d'arrêts sont permis).
- Pas d'images générées par IA sans accord de Mich.

## Tests avant de livrer une build
1. Le projet compile sans erreur ni avertissement nouveau.
2. Partie complète jusqu'à l'écran de fin, au toucher et à la souris.
3. Marche réelle dans toutes les zones (une téléportation ne voit pas les collisions).
4. Captures en portrait téléphone et sur PC.
5. Relire la liste des dialogues : aucun qui s'empile, aucun qui parle d'une chose invisible.

## Façon de travailler avec Mich
- Corriger ce qu'il signale sans tout regénérer quand une petite correction suffit.
- « Ne génère rien » ou « réponds d'abord » : répondre seulement, sans créer de fichier.
- Il surveille ses crédits : étapes courtes. Prévenir si une tâche risque de ne pas tenir dans la session.
- Après chaque étape validée, mettre à jour `docs/reprise.md`.

## Format des réponses
- Français, phrases courtes, Markdown, direct.
- Jamais de tiret cadratin.
- Pas de phrase d'introduction ni de conclusion.

## Hypothèses à confirmer avec Mich
- Unity 2D, cibles PC (manette PS5, souris) et téléphone.
- Arborescence : `docs/` (reprise, bible, personnages, plans), `reference-html/` (les 9 chapitres exportés).
