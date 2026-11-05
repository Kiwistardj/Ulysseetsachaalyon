# Analyse du ch1 (v2 jouable) et apport de Unity

Source : `reference-html/ch1-perraliere-v2.html` (2 120 lignes, un seul fichier, tout dessiné en code).
`ch1-perraliere-v3-chantier.html` = résidence seule, sans quête : pas une référence de gameplay.

## Ce que fait le ch1
**Boucle** : 3 trottinettes de copines volées par Rex. Chacune se récupère avec un pouvoir différent, puis se rend à sa propriétaire.

| Trottinette | Mécanique | Héros mis en avant |
|---|---|---|
| Iris (violette) | Ronces sur le portail de l'école : boule de feu | Ulysse |
| Victorine (orange) | Bibou flaire le local à vélos, Didou se faufile | Bibou, Didou |
| Léonie (rose) | Hérisson blessé : câlin guérisseur, il montre la trottinette | Sacha |

**Final** : Rex se montre, se moque, file au métro Flachet. On va chercher nos trottinettes à la maison, poursuite en trottinette avec tremplins, puis balade en métro ligne A.

**À côté** : passage piéton avec bonhomme rouge/vert (Papa et Maman expliquent), voitures qui klaxonnent, camion de pompiers « pin-pon », pigeons qui s'envolent, commerces qui répondent, rouge-gorge à soigner, ronces « pour le plaisir », 10 PNJ.

**Systèmes** : déplacement (joystick, flèches, manette, toucher), compagnons en file sur la trace du joueur, bouton unique contextuel (feu, parler, câlin, patte, klaxon, sonner), dialogue avec portrait et voix de synthèse, aide (ampoule, et Bibou miaule avec une flèche après 55 s sans progrès), musique synthétisée en direct.

## Points forts à garder
- Trois quêtes = trois personnages qui brillent. Bonne répartition Ulysse / Sacha / animaux.
- Guidage sans texte : flèche, miaulement, objectifs proches.
- Monde vivant et réactif : presque tout répond au bouton.
- Le code ne pèse que 140 Ko, se lance tout de suite sur téléphone, et se modifie en quelques minutes.

## Faiblesses relevées (indépendantes du moteur)
- Joystick à l'écran, alors que `reprise.md` demande toucher pour marcher et pas de croix.
- Voix de synthèse : la voix change selon le téléphone, et parfois il n'y en a pas du tout.
- Personnages animés par simple balancement (pas de vrais pas, pas d'expressions).
- Carte = une image unique : on ne passe jamais derrière un immeuble ou un arbre du décor.
- Tout est en coordonnées écrites à la main (`RECTS`, `NPCS`...) : déplacer un objet oblige à toucher au code.

## Ce que Unity apporte vraiment
1. **Édition visuelle des niveaux** : chaque PNJ, objet et collision est un objet qu'on déplace à la souris. Les 9 chapitres partagent un moteur, et les données (dialogues, quêtes) vivent dans des fichiers séparés.
2. **Profondeur** : le décor découpé en calques passe devant ou derrière les personnages selon leur position. Il faut exporter les bâtiments et les arbres séparément de la carte.
3. **Animation 2D par os** (package 2D Animation) : marche, cape qui flotte, clignement, expressions. Il faut découper chaque personnage en parties (tête, bras, cape).
4. **Effets** : particules pour les boules de feu, l'embrasement des ronces, les cœurs du câlin. Lumières 2D (lampadaires, flammes qui éclairent).
5. **Caméra Cinemachine et Timeline** : zoom sur les moments clés, scènes jouées (la fuite de Rex, l'arrivée du métro) sans code.
6. **Manette PS5** : vibration à chaque boule de feu, couleur de la barre lumineuse (rouge Ulysse, jaune Sacha).
7. **Son** : voix enregistrées ou générées une fois pour toutes, donc identiques sur tous les appareils. Mixage propre de la musique et des effets.
8. **Plus tard** : application Android ou iPhone, hors ligne et en vrai plein écran.

## Ce que Unity coûte
- **Poids** : 20 à 40 Mo à charger dans le navigateur au lieu de 140 Ko, avec un démarrage lent sur téléphone et des limites de mémoire sur iPhone.
- **Voix** : la synthèse vocale du navigateur ne passe pas directement dans Unity. Il faut un pont JavaScript, ou des voix préparées à l'avance.
- **Dessins figés** : les PNG exportés perdent les animations du code. Tant que l'animation par os n'est pas faite, ça bouge moins que le HTML.
- **Vitesse d'itération** : chaque essai demande compilation et build WebGL, plusieurs minutes, donc plus de crédits. Le HTML se corrige plus vite.
- **Tout est à réécrire** : 2 000 lignes de comportements à refaire en C#.

## Proposition de test (ch1 seul)
Une tranche jouable, pour comparer sur téléphone avec le HTML :
1. Carte en calques (export refait ici avec le script de Mich adapté) et profondeur réelle.
2. Toucher pour marcher (recherche de chemin), manette, clavier.
3. Sacha, Ninou, Bibou, Didou en file.
4. Bouton unique et dialogue avec portrait et voix.
5. Une quête complète : les ronces du portail et la trottinette d'Iris, avec boule de feu en particules, ronces qui brûlent et vibration de la manette.
6. Build WebGL publiée sur un lien privé.

Ensuite seulement, on décide si on continue (animation par os, autres quêtes) ou si on reste en HTML.
