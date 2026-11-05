# Éléments du chapitre 1 : la Perralière

Source : page publiée du chapitre 1 (v2), https://claude.ai/artifact/7rWyskrP1nNicELmFbLMC5
Rendu automatique des fonctions de dessin du jeu (Canvas 2D), une image par élément, fond transparent.

## Échelle
- Personnages, animaux, véhicules, objets : **4 pixels par unité du jeu**, à la taille où ils apparaissent dans le jeu. Toutes les images sont à la même échelle entre elles. Dans Unity : Pixels Per Unit identique pour tous (ex. 4 ou 100 selon ton choix, mais le même partout).
- Point d'ancrage conseillé : **en bas au centre** (les pieds), comme dans le jeu (tri de profondeur par y).
- Personnages : pose de face, sans ombre. Variantes trottinette de face et de profil pour Ulysse et Sacha.
- Animaux : de profil, regard vers la droite (retourner l'image pour la gauche).
- Objets : l'ombre au sol est comprise dans l'image (comme dans le jeu).
- Portraits : 384 x 384, le carré du dialogue.
- Décor : la carte entière (2720 x 2240 unités), en deux versions : `perraliere_carte.png` (2720 x 2240, sans perte) et `perraliere_carte_x2.jpg` (5440 x 4480, plus net, JPEG qualité 92 pour rester sous 30 Mo). Les bâtiments, chemins, route, tennis, école et aire de jeux font partie de cette carte (dans le jeu, ils sont dessinés d'un bloc).

## Dossiers
- personnages/ : 14 personnages + 4 variantes trottinette
- animaux/ : Bibou, Didou, Ninou, Sacha sur Ninou, Rex, hérisson et rouge-gorge (blessé, guéri, en vol), pigeon, 4 papillons
- vehicules/ : 5 trottinettes, 4 voitures (dont les pompiers)
- objets/ : arbres, araucaria, lampadaire, boîte aux lettres, feux piétons, jets de la fontaine, ronces, fente du local à vélos, tremplin
- portraits/ : visages du dialogue
- decors/ : la carte de la Perralière
- scenes/ : le trajet en métro ligne A (capture 1280 x 720)
- planches/ : toutes les images d'une catégorie côte à côte, pour vérifier d'un coup d'œil
- manifeste.json : liste des fichiers et leurs tailles

Les noms de fichiers reprennent les noms du code (LOOK.ulysse, drawRonce, etc.).
