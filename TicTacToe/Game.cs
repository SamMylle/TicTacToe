using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TicTacToe {
    public class Game {
        private List<List<Field> > _board;
        private readonly Marker _player1;
        private readonly Marker _player2;
        private bool _firstPlayer;
        private bool _gameOver;

        private Game(char p1, char p2) {
            // Create a new game where p1 will ben the mark of player one and p2 the mark of player 2
            
            this._player1 = new Marker(p1);
            this._player2 = new Marker(p2);
            this.ResetBoard();
        }

        public override string ToString() {
            // Print the game in the console
            
            var lineSeparator = "   ------------";
            var topLegend = "    1   2   3";
            var boardArr = (from row in this._board select string.Join("", from field in row select field.ToString()) + "|").ToList();
            var legendArr = new List<string>(new string[] {"a", "b", "c"});
            var totalArr = boardArr.Zip(legendArr, (x, y) => y + " " + x);
            return topLegend + "\n" + lineSeparator + "\n" + string.Join("\n" + lineSeparator + "\n", totalArr) + "\n" + lineSeparator + "\n";
        }

        private void CurrentPlayerMessage() {
            // Instruct the current player to play (prints this message in the Console)
            
            var marker = _firstPlayer ? _player1 : _player2;
            Console.Write("Your turn to play {0}: ", marker);
        }

        private bool MakeMove(char xco, char yco) {
            // Given the coordinates (as given by xco and yco), make a move for the current player (player 1 starts)
            // Returns a boolean indicating whether or not the move was valid
            // Notice that xco must be a, b or c and yco must be 1, 2 or 3
            // xco will be mapped to 0, 1, 2 respectively as well as yco
            // E.g. this means that c1 will map to this._board[2][0]
            
            if (_gameOver) {
                return false;
            }

            ushort item1;
            ushort item2;
            try {
                (item1, item2) = TranslateCoordinates(xco.ToString().ToLower()[0], yco);
            }
            catch (Exception) {
                return false;
            }
            
            var marker = _firstPlayer ? _player1 : _player2;

            if (this._board[item1][item2].IsEmpty()) {
                this._board[item1][item2].Mark(marker);

                this._gameOver = this.PlayerWon(marker);

                if (this._gameOver) {
                    Console.WriteLine(this.ToString());
                    Console.WriteLine("Congratulations player " + marker + "! You won!");
                }

                if (!this._gameOver && this.IsDraw()) {
                    Console.WriteLine(this.ToString());
                    this._gameOver = true;
                    Console.WriteLine("It's a draw, everyone wins! :D");
                }
                
                this._firstPlayer = !_firstPlayer;
                return true;
            } else
                return false;
        }

        private bool IsDraw() {
            // Returns a boolean indicating whether or not the game ended in a draw
            return this._board.SelectMany(x => x).Where(x => x.IsEmpty()).ToList().Count == 0;
        }

        private bool PlayerWon(Marker marker) {
            // Returns a boolean indicating whether or not the player with the given marker is victorious
            bool allMarked;
            
            // Vertical line check
            for (ushort xco = 0; xco < 3; xco++) {
                allMarked = true;
                for (ushort yco = 0; yco < 3; yco++) {
                    if (!this._board[xco][yco].Marked(marker)) {
                        allMarked = false;
                    }
                }

                if (allMarked)
                    return true;
            }
            
            // Horizontal line check
            for (ushort yco = 0; yco < 3; yco++) {
                allMarked = true;
                for (ushort xco = 0; xco < 3; xco++) {
                    if (!this._board[xco][yco].Marked(marker)) {
                        allMarked = false;
                    }
                }

                if (allMarked)
                    return true;
            }

            // Main diagonal check
            allMarked = true;
            for (ushort co = 0; co < 3; co++) {
                if (!this._board[co][co].Marked(marker)) {
                    allMarked = false;
                    break;
                }
            }
            
            if (allMarked)
                return true;
            
            // Anti-diagonal check
            allMarked = true;
            for (ushort co = 0; co < 3; co++) {
                if (!this._board[co][2 - co].Marked(marker)) {
                    allMarked = false;
                    break;
                }
            }
            
            return allMarked;
        }

        static Tuple<ushort, ushort> TranslateCoordinates(char xco, char yco) {
            // Translate the coordinates into indices in this._board
            // Notice that xco must be a, b or c and yco must be 1, 2 or 3
            // xco will be mapped to 0, 1, 2 respectively as well as yco
            // E.g. this means that c1 will map to this._board[2][0]
            // The result is stored in a Tuple
            
            if (!"abc".Contains(xco) || !"123".Contains(yco)) {
                throw new InvalidDataException("(" + xco + ", " + yco + ") Is not a valid coordinate");
            }

            const ushort baseX = 'a';
            ushort x = xco;
            var y = Convert.ToUInt16(ushort.Parse(yco.ToString()) - 1);
            return new Tuple<ushort, ushort>(Convert.ToUInt16(x - baseX), y);
        }

        private bool GameRunning() {
            // Return a boolean indicating whether or not the game is still being played
            return !this._gameOver;
        }

        private void ResetBoard() {
            // Reset the board (clear all markers and make sure player one gets to move)
            
            this._board = (from i in Enumerable.Range(0, 3) select (from j in Enumerable.Range(0, 3) select new Field()).ToList()).ToList();
            this._firstPlayer = true;
            this._gameOver = false;
        }

        public static bool GameLoop() {
            // Starts the actual game using a command line interface
            
            Game game = new Game('X', 'O');

            Console.WriteLine("Welcome to TicTacToe!\nType \"help\" for instructions at any time during the game." +
                              "To restart the game, simply type \"restart\" whenever you like." +
                              "When the game is finished, the program closes.");
            
            // The game loop with the CLI
            while (game.GameRunning()) {
                Console.WriteLine(game);
                game.CurrentPlayerMessage();

                var validMove = false; 
                do {
                    var inp = Console.ReadLine();

                    if (inp == "help") {
                        Console.WriteLine("There are two players X and O that alternatively make a move.\n" +
                                          "You can place your mark on the board by entering the coordinates whenever asked to (e.g. a2 or B3).\n" +
                                          "The goal is to get your markers on a straight line of length 3. This can diagonal as well.\n" +
                                          "To quit the game, type \"exit\".");
                        game.CurrentPlayerMessage();
                    }
                    else if (inp == "restart")
                        return true;
                    else if (inp == "exit") {
                        Console.WriteLine("Goodbye! Thank you for playing! :D");
                        return false;
                    } else if (inp.Length != 2)
                        Console.Write("Sadly \"{0}\" is not a valid move :-(, try again: ", inp);
                    else {
                        validMove = game.MakeMove(inp[0], inp[1]);
                        if (!validMove)
                            Console.Write("Sadly \"{0}\" is not a valid move/command :-(, try again: ", inp);
                    }
                } while (!validMove);
            }

            // Post-game CLI allowing to exit or restart
            while (true) {
                Console.WriteLine("Do you wish to restart? (y/n): ");
                var restart = Console.ReadLine();

                if (restart == "y")
                    return true;
                else if (restart == "n") {
                    Console.WriteLine("Goodbye! Thank you for playing! :D");
                    return false;
                } else
                    Console.WriteLine("\"{0}\" is not a valid command, only use y/n.", restart);
            }
        }
    }
}