﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace GameOfLife {
    public class Game {
        int _gridSize = 25;
        List<Cell> _cells;

        internal void Start() {
            #region Setup matrix and 'glider' pattern
            var matrix = new int[_gridSize, _gridSize];

            // Start with 'Glider' pattern placed in the middle of the 25x25 grid. 
            // The rest of the cells are 0 (dead) by default
            matrix[11, 12] = 1;
            matrix[12, 13] = 1;
            matrix[13, 11] = 1;
            matrix[13, 12] = 1;
            matrix[13, 13] = 1;
            #endregion

            GenerateCells(matrix);
            PrintGrid(matrix);

            Console.Write("Press any key to start. ");
            Console.ReadLine();

            while (true) {
                GenerateCells(matrix);
                RunGeneration(matrix);
                PrintGrid(matrix);
                Thread.Sleep(200);
            }
        }

        private void PrintGrid(int[,] matrix) {
            var builder = new StringBuilder();

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGreen;

            for (int i = 0; i < _gridSize; i++) {
                for (int j = 0; j < _gridSize; j++) {
                    builder.Append(matrix[i, j] == 1 ? "█" : " "); // alive or dead
                }
                builder.Append("\n");
            }

            Console.Write(builder);
        }

        private void RunGeneration(int[,] matrix) {
            foreach (var cell in _cells) {
                cell.Evaluate();
                matrix[cell.Coordinates.X, cell.Coordinates.Y] = cell.IsAlive ? 1 : 0;
            }
        }

        private void GenerateCells(int[,] matrix) {
            _cells = new List<Cell>();

            for (int i = 0; i < _gridSize; i++) {
                for (int j = 0; j < _gridSize; j++) {

                    var cell = new Cell(isAlive: matrix[i, j] == 1);
                    cell.Coordinates = new Coordinates { X = i, Y = j };

                    // Check 8 neighbour cells exist, and for those alive increment count of alive neighbours.

                    cell.AliveNeighboursCount = (i - 1 >= 0 && j - 1 >= 0) && matrix[i - 1, j - 1] == 1
                        ? cell.AliveNeighboursCount + 1
                        : cell.AliveNeighboursCount;

                    cell.AliveNeighboursCount = (j - 1 >= 0) && matrix[i, j - 1] == 1
                        ? cell.AliveNeighboursCount + 1
                        : cell.AliveNeighboursCount;

                    cell.AliveNeighboursCount = (i + 1 < _gridSize && j - 1 >= 0) && matrix[i + 1, j - 1] == 1
                        ? cell.AliveNeighboursCount + 1
                        : cell.AliveNeighboursCount;


                    cell.AliveNeighboursCount = (i - 1 >= 0) && matrix[i - 1, j] == 1
                        ? cell.AliveNeighboursCount + 1
                        : cell.AliveNeighboursCount;

                    cell.AliveNeighboursCount = (i + 1 < _gridSize) && matrix[i + 1, j] == 1
                        ? cell.AliveNeighboursCount + 1
                        : cell.AliveNeighboursCount;


                    cell.AliveNeighboursCount = (i - 1 >= 0 && j + 1 < _gridSize) && matrix[i - 1, j + 1] == 1
                        ? cell.AliveNeighboursCount + 1
                        : cell.AliveNeighboursCount;

                    cell.AliveNeighboursCount = (j + 1 < _gridSize) && matrix[i, j + 1] == 1
                        ? cell.AliveNeighboursCount + 1
                        : cell.AliveNeighboursCount;

                    cell.AliveNeighboursCount = (i + 1 < _gridSize && j + 1 < _gridSize) && matrix[i + 1, j + 1] == 1
                        ? cell.AliveNeighboursCount + 1
                        : cell.AliveNeighboursCount;

                    //
                    _cells.Add(cell);
                }
            }
        }
    }

    public class Cell {
        public bool IsAlive { get; private set; }
        public Coordinates Coordinates { get; set; }
        public int AliveNeighboursCount { get; set; }
        public Cell(bool isAlive) {
            IsAlive = isAlive;
        }
        public void Die() {
            IsAlive = false;
        }
        public void Activate() {
            IsAlive = true;
        }
        public void Evaluate() {
            if (IsAlive && AliveNeighboursCount < 2)
                Die();
            else if ((IsAlive && AliveNeighboursCount == 2) || (IsAlive && AliveNeighboursCount == 3))
                Activate();
            else if (IsAlive && AliveNeighboursCount > 3)
                Die();
            else if (!IsAlive && AliveNeighboursCount == 3)
                Activate();
        }
    }

    public struct Coordinates {
        public int X { get; set; }
        public int Y { get; set; }
    }
}