# Plan : test Unity du ch1 (validé par Mich le 5 novembre 2026)

Cibles : Android, paysage. Voix : préparées une fois (synthèse française de qualité), identiques partout.

1. **Décor en calques** (fait) : sol 2 px/unité en 9 tuiles, 9 bâtiments découpés avec pivot au pied de façade, positions du code dans `Assets/Data/ch1_layout.json`, scène générée par `Ulysse > Reconstruire la scène du ch1`. Outils : `tools/export-ch1/`.
2. Moteur de base : toucher pour marcher (recherche de chemin), clavier, manette PS5, caméra qui suit.
3. Compagnons en file : Sacha sur Ninou, Bibou, Didou (Papa et Maman suivent aussi).
4. Bouton unique contextuel, dialogue avec portrait et voix ; le bouton ne couvre jamais le texte.
5. Quête d'Iris : ronces du portail (boule de feu, embrasement, vibration manette), cour, trottinette, retour à Iris.
6. Build web sur lien privé, puis APK Android.

Écarts assumés avec le HTML : « Casino » devient « Épicerie » (règle : pas de marque dessinée).
