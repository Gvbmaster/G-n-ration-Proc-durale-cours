# GÉNÉRATION PROCÉDURALE


## Introduction
Ce est un projet de cours où il nous a été présenté plusieurs techniques de génération procédurale :
- Simple Placement Room
- Binary Space Partitioning (BSP)
- Cellular Automata
- Noise (Perlin / Simplex)

L'objectif est de nous apprendre les bases de la génération procédurale.

---

## Structure du Projet
- `assets/components/ProceduralGeneration` : Ici sont stockés les différents algorithme et leurs scripts.
- `README.md` : Présentation du cours.

---

# Simple Placement Room

## Principe
Placement aléatoire de salles dans une grid donnée.  
Chaque salle possède une taille et des coordonnées déterminées de manière aléatoire.

## Algorithme
1. Définir une zone ( Une grid)
2. Générer plusieurs salles avec des dimensions aléatoires
3. Placer les salles dans la grid
4. Tenter de placer chaque salle à un emplacement valide
5. Vérifier la salle entre en collision avec une autre salle
6. Ajouter les salles valides à la carte