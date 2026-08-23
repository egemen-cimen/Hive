# Hive Game Engine and AI Player

This project contains a C# engine and a Minimax with alpha-beta pruning AI for [the tabletop strategy game Hive](https://en.wikipedia.org/wiki/Hive_(game)), I built this project from scratch to explore advanced graph traversal and state management.

Only the base game pieces are implemented:

- Ant
- Beetle
- Grasshopper
- Queen Bee
- Spider

## The Architecture

The architecture was specifically designed to support AI player evaluating thousands of game states per second. To achieve this, the engine separates data and logic.

The `GameState` is designed to be a pure data container which holds the coordinate system, past player actions, current player color, and the turn number. It has no knowledge of how to play Hive. This allows AI player to quickly traverse mutate, and restore the states without expensive object instantiation.

The `GameRules` is designed as a stateless collection of rules that operate on the `GameState`. This make testing the engine easier (not need for complex mocks) and ensures that validating moves on the board have no side effects.

Game pieces and piece movement rules are abstracted behind `IPiece` and `IMovementRules` interfaces respectively. Each piece has it's own movement validation rules. This allows me to add expansion pieces (like mosquito or the ladybug) easier in the future without having to make changes to existing rules.

All the code is tested with a coverage over 90%.

## The Data Structures & Algorithms

I implemented 3D axial coordinate system for this. The beetle piece can go on top of other pieces, I used a stack for the pieces. And since the game area does not have any limits I chose to store the column (q), and row (r) coordinates and the stack of pieces in a dictionary.

To implement [the base game rules](https://hivegame.com/download/rules.pdf), I utilized graph traversal algorithms and various data structures.

In order to accommodate the AI player's exploration of game states, I implemented the make/unmake pattern.

## The AI

The AI uses Minimax algorithm with alpha-beta pruning and simple heuristics:

- Playing own queen piece is an advantage
- Every piece next to own queen is a disadvantage
- Every piece next to opponent queen is an advantage
- Own queen being pinned in a disadvantage
- Pinning the opponent queen in an advantage

## Known Issues & Future Development

- The "draw after repeating the same moves three times" rule is not implemented
- Since there's no visualization for the game, the game is not played stand alone. Some sort of UI could be implemented in the future

## How to Run the Project and Play the Game

Currently there's no visualization. So in order to play the game it's recommended to have the physical game with you. Run the console project and synchronize your board with the console.

To do this, build and run the project:

```ps1
dotnet build
dotnet run --project Hive.ConsoleRunner
```

Start a new game and:

- when you make a move on the physical game, input your move into the console
- when the AI player makes a move, apply the console output to your physical game
