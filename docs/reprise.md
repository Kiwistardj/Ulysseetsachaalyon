# Reprise : état au 5 novembre 2026

Le jeu est complet en HTML (9 chapitres). Le travail en cours est le portage dans Unity. Les détails de chaque chapitre sont dans `bible-du-jeu.md` et dans `plans/`.

## Les 9 chapitres HTML (références dans `reference-html/`)

| Ch | Lieu | Version de référence | Plan |
|---|---|---|---|
| 1 | La Perralière | v3 jouable (6 novembre 2026), v2 en référence | dans reprise.md |
| 2 | Parc de la Tête d'Or | v5 | dans la bible |
| 3 | Les Gratte-Ciel | v5 | dans la bible |
| 4 | Mini World | v3, passes 1 à 3 | plan-mini-world-v3.md |
| 5 | Raptor Parc | v3.1 | plan-ch5.md |
| 6 | Anse | passes 1 à 3 | plan-ch6.md |
| 7 | Musée des Confluences | v10 (fin « Rex devient ami » refaite, jeu des ombres réparé sur PC) | plan-ch7.md |
| 8 | Chez Julien et la MLIS | v2, passes A, B, C (livre musical, morceau Kiwistar) | plan-ch8.md puis plan-ch8-v2.md |
| 9 | La grande fête à la Perralière | passes 1 à 3, chapitre final, renvoie au ch1 | plan-ch9-v1.md |

## Fil de l'histoire
1. Perralière : Rex chipe les trottinettes et le cochonnet. Poursuite jusqu'au métro Flachet (ligne A).
2. Tête d'Or (métro A jusqu'à Masséna) : les pièces du petit train. Bus vers les Gratte-Ciel.
3. Gratte-Ciel : 4 sujets du manège volés. Métro A jusqu'à La Soie.
4. Mini World : tampons du passeport. Voiture vers les Échets.
5. Raptor Parc : 4 œufs, éclosion des bébés dinosaures. Rex part en moto vers Anse.
6. Anse : 3 tickets dorés de la fête foraine, petit train. TER jusqu'à Perrache.
7. Confluences (tram T1) : Rex pleure, Ulysse lui donne un jouet, Rex devient ami. Retour T1 puis trambus TB12 jusqu'à Maisons Neuves.
8. Chez Julien : bibliothèque (bus 69), parc, livre colorié offert à Rex. Rex part en C27 préparer la fête.
9. Fête : C27 jusqu'à Grandclément, préparatifs dans la résidence, magasin, boulangerie, fête à la pétanque, photo de groupe, dernière page du livre.

## Ce qui reste côté HTML (hors Unity)
- Lien du ch8 vers le ch9 : pas encore publié (l'écran de fin du ch8 dit « À suivre » sans lien).
- Ch6 : écran de fin à refaire ; vérifier le lien vers le ch7.
- Chapitre « bords de Saône » (chez Judith à Fontaines-sur-Saône) envisagé entre Mini World et le Raptor Parc. Pas décidé.
- Mini-jeux à part : Paris, Champagne-en-Valromey.

## Reprises à intégrer pendant le portage
Le portage est l'occasion de corriger les anciens chapitres :
1. Mini World : blocage en bas du funiculaire sur téléphone.
2. Toucher pour marcher et gros bouton unique dans tous les chapitres (pas de croix directionnelle, ni sur téléphone ni sur PC).
3. Plus de ronces partout (Ulysse adore la boule de feu et l'explosion).
4. Ch1 : la Perralière au vrai plan (voir plan-ch1-v3.md).
5. Ch2 vers ch3 : bus 69 au lieu d'un bus sans numéro.
6. Partout : un compagnon bouge d'abord, ensuite le texte dit ce qu'il a trouvé (ex. ch1 : Nanou parle de Bibou sous les vélos avant qu'il bouge).
7. Rex caché : jamais trop évident (au Raptor Parc, Ulysse l'a deviné de loin).
8. Partout : aucun objectif fait dès l'entrée d'un lieu ; Sacha n'agit jamais seul pendant qu'Ulysse regarde.
9. Ch8 : salon plus large que long (demander à Mich où sont les choses avant).

## Leçons (retours de Mich après test avec Ulysse)
- Chercher le contenu réel d'un lieu avant de dessiner. Orientation comprise.
- Une salle = un écran plein ; sur PC, pas de bandes vides, le décor continue sur les côtés.
- Un moment clé (amitié, pardon) se montre et se joue : action d'Ulysse, changement visible, mots simples. Le dire ne suffit pas.
- Rex : vu au début et à la fin ; ses empreintes vertes guident entre les deux.
- Un PNJ ne parle d'une chose qu'une fois qu'elle est visible.
- Coloriages pour Ulysse, rapides (un toucher par zone), couleurs vives.
- Les choix d'Ulysse comptent et reviennent plus tard.
- Balades courtes (bus : environ 12 s à l'aller, 6 s au retour) et toujours avec quelque chose à voir. Prévoir « encore un tour ».
- Ulysse aime les chapitres longs où il se concentre ; il zappe les dialogues trop longs.
- Sur téléphone : le gros bouton ne doit jamais couvrir le texte.

## Mécaniques à reproduire dans le moteur Unity
- Déplacement : toucher ou clic pour marcher (A* sur grille), flèches et manette sur PC.
- Dialogue en bande en bas, voix de synthèse française (narrateur adulte pour enfants et animaux, cri de l'animal avant sa réplique).
- Bouton d'action unique et contextuel : l'icône change (feu, glace, parler, câlin, bouton, patte, klaxon, « doucement », « chut » à l'intérieur).
- À l'intérieur d'un bâtiment, jamais de feu : le bouton fait « Chut ! ».
- Aide : ampoule discrète ; aide automatique (Bibou miaule, flèche) si Ulysse tourne en rond.
- Compagnons qui suivent en file (Sacha, Bibou, Didou, Ninou, copains, Rex à partir du ch8).
- Ascenseurs, balades (tram, bus, funiculaire, téléphérique, petit train conduit par Ulysse), fenêtres plein écran (loupe, coloriage, livre).
- Écran de fin de chapitre avec les portraits et le lien vers le chapitre suivant.

## Son
- Morceau de Mich (Kiwistar, électro swing) : boucle de 8 mesures, 105 BPM, fa dièse mineur, départ à 30,302 s du fichier « LANDR-looty_beats_mix_v2-Open-High.wav ». Utilisé à la trompette (ch8), au poste d'écoute (ch8), sur la page bibliothèque du livre et à la fête (ch9). Repartir du fichier original de Mich plutôt que du mp3 compressé dans la page HTML.
- Berceuse du livre : « Au clair de la lune » (air traditionnel, libre).

## Lignes vérifiées (à garder exactes)
- Métro A : Flachet, Gratte-Ciel, République, Charpennes, Masséna ; terminus Vaulx-en-Velin La Soie.
- Tram T1 : Musée des Confluences, Perrache, Gare Part-Dieu.
- Trambus TB12 (deux perches, ouvert le 28 février 2026) : Gare Part-Dieu V. Merle, Archives - Parc Mandela, Maisons Neuves.
- Bus 69 (bleu violet foncé) : Maisons Neuves, Marengo, Verlaine - Racine, Mairie de Villeurbanne, Gratte-Ciel, Rue de France - IUT.
- Bus C27 : Maisons Neuves, Florian, Arago, Perrin - le Rize, Grandclément. Couleur verte à vérifier.
- TER : Anse vers Lyon Perrache, le long de la Saône.

## Règles de travail avec Mich
- Français, phrases courtes, Markdown, jamais de tiret cadratin, pas d'intro ni de conclusion.
- « Ne génère rien » ou « réponds d'abord » : répondre seulement.
- Poser des questions avant de construire ; il ne lance une étape que si le plan est parfait.
- Tout doit ressembler au maximum à la réalité. Pas de logos, pas de personnages protégés, pas d'adresse.

## Assemblage (6 novembre 2026)
- Décision : on garde les 9 chapitres en entier (histoire, scènes, répliques). Pas de Unity, pas de pixel art : style album, HTML.
- `app/` : le jeu complet en une app. `app/index.html` = carte de Lyon (menu), sauvegarde dans le navigateur, enchaînement des chapitres. `app/chapitres/` = les chapitres copiés tels quels depuis `reference-html/` (ch1 = v2 jouable), avec un petit relais ajouté à la fin (liens internes, détection de l'écran de fin).
- Régénérer : `python3 -I tools/assemble/build.py`.
- Lien de test : https://claude.ai/artifact/C7o38JdvU7MYijXW3CFHSg
- Vérifié : chapitres 2 à 6 du dépôt identiques aux versions publiées ; 7 et 8 non vérifiés un par un.
- Cahier des charges (refonte des décors au vrai plan, harmonisation, enrichissements) : https://claude.ai/artifact/4Z2ikyhGQEyjND8HVoNEdb
- Prochaines étapes : décors chapitre par chapitre (ch1 fait, voir plus bas). L'application viendra quand le jeu sera fini.

## Ch1 v3 (6 novembre 2026)
- Fichier : `reference-html/ch1-perraliere-v3.html`. Lien de test : https://claude.ai/artifact/LeuAv4ZfocjY77vYV4DZgw
- Base : moteur et résidence du ch9 (sans la fête, sans les intérieurs des commerces, sans le mp3). Histoire, répliques et ordre : ceux de la v2.
- Nord ajouté (Mich) : carrefour du Casino (passage sur la route, puis sur la rue), parc de l'Espace des Droits de l'Homme au nord-ouest, en diagonale du Casino. Entrée au sud : zone des petits avec sable à l'ouest, bateau (tronc, pont de corde, grand toboggan) à l'est, 3 balançoires nord-sud au sud-est du bateau, esplanade, kiosque, zone du fond à gauche sans sable (toboggan, 2 balançoires). Passage au nord sur la rue, puis le cours (2 voies, parallèle à la route du Casino) avec un passage : bouche Flachet direction Perrache au sud du cours, direction Vaulx-en-Velin La Soie au nord, sur le trottoir est. En face du Casino : la crèche et le relais assistantes maternelles.
- Feux : rouge 7 s, vert vite quand on attend au bord ; Ulysse s'arrête au rouge ; les voitures s'arrêtent au vert.
- Quêtes : Iris (Prune au carrefour des chemins de l'école, au bout de sa barre, Iris dans l'allée ouest ; trottinette d'Iris bleue (Mich) ; ronces du portail de la cour, trottinette près du toboggan, Mahé dans la cour) ; Victorine (haie de la placette devant chez Nanou à la place du local à vélos : Bibou flaire, Didou se faufile) ; Léonie (elle attend à l'angle du Casino et ne suit pas la bande ; hérisson dans la zone du fond, trottinette derrière la haie, qu'on lui rapporte à l'angle du Casino). Puis Rex, trottinettes au hall, poursuite (couloir, galerie, passage, trottoir est, tremplins), métro A jusqu'à Masséna.
- Répliques modifiées (validées) : « local à vélos » devient « haie » (Nanou, Victorine, Didou, aide) ; Léonie : « vers l'aire de jeux du parc, de l'autre côté de la route ». Ajouts validés : Prune « Je viens avec vous ! », « Vas-y Ulysse ! » ; Iris « Je viens avec toi ! », « Bravo Sacha ! ».
- Retouches à faire valider par Mich : Didou « trois trottinettes à retrouver autour de la résidence » (au lieu de « dans la résidence ») ; « la trottinette d'Iris » (au lieu de « de Iris ») ; « trottinette bleue » au lieu de « violette » (Iris) ; nouvelle aide « On rapporte sa trottinette rose à Léonie, à l'angle du Casino ! ».
- On joue au parc (Mich : sinon c'est décevant) : le bateau (on monte par le tronc contre la proue, on passe sur le pont et le pont de corde, on glisse au grand toboggan, Sacha et les compagnons suivent), les 2 toboggans des petits et celui de la cour (après les ronces), les balançoires (Ulysse et Sacha ensemble). Rejouables, étoiles quand on approche, bouton étoile « Jouer ».
- Hypothèses : fontaine, araucaria, ping-pong placés par Claude ; tremplins et cheminement de Rex inventés.
- Testé : partie complète automatique jusqu'à l'écran de fin (clavier), toucher pour marcher sur téléphone, captures PC et téléphone, aucune erreur.
- À faire : partie complète à la main sur téléphone ; toit de la galerie qui cache le bas du passage du Casino ; toit gris uni de la crèche.
- Décision de Mich (6 novembre) : pas d'application (Android, .exe) tant que le jeu n'est pas fini ; on la fera en une fois à la fin.
