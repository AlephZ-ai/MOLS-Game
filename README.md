# Mutually-Orthogonal-Latin-Square-15-Puzzle-Solution-Finder

## What is an MOLS?

A [Mutually Orthogonal Latin Square (MOLS)](https://en.wikipedia.org/wiki/Mutually_orthogonal_Latin_squares) is a set of [Latin squares](https://en.wikipedia.org/wiki/Latin_square) where each pair of squares is orthogonal, meaning that when any two squares are superimposed, each ordered pair of symbols appears exactly once. For example, playing cards can be a form of MOLS if they are arranged into squares where one Latin square represents suits (♠, ♥, ♦, ♣) and another represents values (A, K, Q, J). You create an MOLS by ensuring that when the two Latin squares are superimposed, each combination of suit and rank (e.g., ♠A, ♥2) is unique and occurs exactly once across the two squares.

## About the Puzzle

The MOLS-15-Puzzle is a twist on the classic [15-puzzle](https://en.wikipedia.org/wiki/15_Puzzle), where the goal is not simply to order numbers from 1 to 15 but to achieve an MOLS configuration. This adds a twist to the traditional puzzle, challenging players to think about the game in a fundamentally new way. It also dramatically increases the number of valid solutions, as there are multiple MOLS configurations in a 4 by 4 grid compared to the 15-puzzle's one valid solution.

## Purpose

The primary aim of this project is to explore the properties and playability of this puzzle. We are particularly focused on discovering if any scrambled permutation of the puzzle is "playable," meaning that a valid solution can be reached in a reasonable number of moves through intuition. This means a minimal use of 15-puzzle algorithms. Additionally, we are interested in finding the average smallest distance possible between two MOLS, as well as the two MOLS with the largest non-infinite distance between each other. Distance is defined as the number of valid moves from one MOLS to another.

## Current Features

As of now, we have a playable version of the game, along with various algorithms to discover paths leading to valid solutions. Here's a brief overview of the algorithms we currently support:

### Best Algorithms:

- **Depth Modifier (DMOD):** Leverages a depth-weighted heuristic to find a solution, typically within 25-50 moves.
  
- **Breadth-First Search (BFS):** Implements a true breadth-first search to accurately determine the shortest path to a solution, viable up to a depth of 22 moves.

### Deprecated or Experimental:

- **Heuristic (H):** A basic heuristic approach that quickly finds a solution, albeit in a longer path ranging from 100 to 300 moves.

- **Breadth-First Search + Heuristic (BFS+H):** An enhanced heuristic that, while slower than the basic heuristic, yields a shorter path of 60-90 moves.


## Installation

To download, make sure you have Visual Studio 2022. Clone this repository and run it on your local host.

## How to Play

First, navigate to the Counter tab. Then click on the "Click to Use Arrow Keys" button. Use the arrow keys on your keyboard to move the free square on the puzzle. 

## License

MIT

## Acknowledgments

A special thank you to Professor Ivan Cherednik.
