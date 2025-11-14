# GÉNÉRATION PROCÉDURALE

## Introduction
Ceci est un projet de cours où plusieurs techniques de génération procédurale nous ont été présentées :
- Simple Placement Room  
- Binary Space Partitioning (BSP)  
- Cellular Automata  
- Noise (Perlin / Simplex)

L'objectif est d'apprendre les bases de la génération procédurale et de comprendre comment ces techniques peuvent être utilisées pour créer automatiquement des niveaux variés.

---

## Structure du Projet
- `assets/components/ProceduralGeneration` : Contient les différents algorithmes et leurs scripts.
- `README.md` : Présentation du cours et synthèse des algorithmes étudiés.

---

# Simple Placement Room

## Principe
Placement aléatoire de salles dans une grille donnée.  
Chaque salle possède une taille et des coordonnées déterminées de manière aléatoire.

## Algorithme
1. Définir une zone (une grille)
2. Générer plusieurs salles avec des dimensions aléatoires
3. Tenter de placer chaque salle dans la grille
4. Vérifier si la salle entre en collision avec une autre
5. Accepter ou rejeter la salle si elle se trouve sur un autre
6. Ajouter les salles valides à la carte

```markdown
![Simple Placement Room](assets/image/SPM.gif)


---

# BSP (Binary Space Partitioning)

## Principe
Le BSP découpe RÉCURSIVEMENT( c'est important car il nous as bien fait galérer) un espace en sous-régions, puis place une salle dans chaque partition.

## Algorithme
1. Diviser l'espace initial en deux sous-espaces
2. Répéter la division récursivement selon les paramètres données
3. Générer une salle dans chaque partition
4. Connecter les salles avec des corridors
5. Construire la carte finale à partir de l’arborescence de partitions

![BSP Example](assets/images/BSP.png)


---

# Cellular Automata

## Principe
Utilisation d’un automate cellulaire pour générer différent type de paysage, carte ...

## Algorithme
1. Générer une grille initiale de cellules (mur / vide)
2. Appliquer les règles
3. Répéter le processus durant plusieurs itérations
4. Lisser ou nettoyer les zones pour obtenir un meilleur rendu
5. Produire la carte finale

```markdown
![Simple Placement Room](assets/image/CellularAutomata.gif)

---

# Noise (Perlin / Simplex)

## Principe
On utilise des fonctions de Noise pour générer des valeurs continues, permettant de créer des terrains, biomes, hauteurs, etc.

## Algorithme
1. Générer une carte de valeurs via le noise
2. Appliquer des seuils ou des masques (ex : altitude, biome)
3. Ajouter éventuellement plusieurs couches grace aux octaves
4. Composer un rendu final

```markdown
![Simple Placement Room](assets/image/Noise.gif)

---


