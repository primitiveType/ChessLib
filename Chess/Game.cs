/*
ChessLib, a chess data structure library

MIT License

Copyright (c) 2017-2020 Rudy Alex Kohn

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

namespace Rudz.Chess
{
    using Enums;
    using Fen;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Text;
    using Transposition;
    using Types;
    using Piece = Types.Piece;
    using Square = Types.Square;

    public sealed class Game : IGame
    {
        private const int MaxPositions = 256;

        private readonly StringBuilder _output;

        private static readonly IDictionary<(HashKey, int), ulong> _perftCache;
        
        public Game(IPosition pos)
        {
            Pos = pos;
            _output = new StringBuilder(256);
        }

        static Game()
        {
            Table = new TranspositionTable(256);
            _perftCache = new Dictionary<(HashKey, int), ulong>(256);
        }

        public State State => Pos.State;

        public Action<Piece, Square> PieceUpdated => Pos.PieceUpdated;

        public int MoveNumber => 0;//(PositionIndex - 1) / 2 + 1;

        public BitBoard Occupied => Pos.Pieces();

        public IPosition Pos { get; }

        public GameEndTypes GameEndType { get; set; }

        public static TranspositionTable Table { get; set; }

        public void TakeMove()
        {
            Pos.TakeMove(Pos.State.LastMove);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FenError NewGame(string fen = Fen.Fen.StartPositionFen) => Pos.SetFen(new FenData(fen), true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FenData GetFen() => Pos.GenerateFen();

        public void UpdateDrawTypes()
        {
            var gameEndType = GameEndTypes.None;
            if (IsRepetition())
                gameEndType |= GameEndTypes.Repetition;
            if (State.Material[PlayerExtensions.White.Side] <= 300 && State.Material[PlayerExtensions.Black.Side] <= 300 && Pos.BoardPieces[0].Empty && Pos.BoardPieces[8].Empty)
                gameEndType |= GameEndTypes.MaterialDrawn;
            if (State.Rule50 >= 100)
                gameEndType |= GameEndTypes.FiftyMove;

            var moveList = Pos.GenerateMoves().GetMoves();
            foreach (var move in moveList)
            {
                if (Pos.IsLegal(move))
                    continue;
                gameEndType |= GameEndTypes.Pat;
                break;
            }

            GameEndType = gameEndType;
        }

        public override string ToString()
        {
            const string separator = "\n  +---+---+---+---+---+---+---+---+\n";
            const char splitter = '|';
            const char space = ' ';
            _output.Clear();
            _output.Append(separator);
            for (var rank = Ranks.Rank8; rank >= Ranks.Rank1; rank--)
            {
                _output.Append((int)rank + 1);
                _output.Append(space);
                for (var file = Files.FileA; file <= Files.FileH; file++)
                {
                    var piece = Pos.GetPiece(new Square(rank, file));
                    _output.AppendFormat("{0}{1}{2}{1}", splitter, space, piece.GetPieceChar());
                }

                _output.Append(splitter);
                _output.Append(separator);
            }

            _output.AppendLine("    a   b   c   d   e   f   g   h");
            _output.AppendLine($"Zobrist : 0x{State.Key.Key:X}");
            return _output.ToString();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<Piece> GetEnumerator() => Pos.GetEnumerator();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BitBoard OccupiedBySide(Player c) => Pos.OccupiedBySide[c.Side];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Player CurrentPlayer() => Pos.SideToMove;

        public ulong Perft(int depth)
        {
            if (depth == 1)
                return Pos.GenerateMoves().Count;

            var key = Pos.State.Key;

            // var tot = 0ul;
            if (_perftCache.TryGetValue((key, depth), out var tot))
                return tot;

            // var posKey = Pos.State.Key;
            // var (found, entry) = Table.Probe(posKey);
            // if (found && entry.Depth == depth && entry.Key32 == posKey.UpperKey)
            //     return (ulong)entry.Value;

            var move = MoveExtensions.EmptyMove;

            if (depth == 3)
            {
                var f = Pos.GenerateFen().Fen.ToString();
            }


            var ml = Pos.GenerateMoves();
            
            var moves = ml.GetMoves();
            foreach (var m in moves)
            {
                // Console.WriteLine($"{depth}:{m.Move}");
                // Console.WriteLine($"{depth}:Before MakeMove: {Pos.GenerateFen().Fen.ToString()}");
                Pos.MakeMove(m.Move);
                // Console.WriteLine($"{depth}:After MakeMove: {Pos.GenerateFen().Fen.ToString()}");
                move = m;
                tot += Perft(depth - 1);
                // Console.WriteLine($"{depth}:Before TakeMove: {Pos.GenerateFen().Fen.ToString()}");
                Pos.TakeMove(m);
                // Console.WriteLine($"{depth}:After TakeMove: {Pos.GenerateFen().Fen.ToString()}");
            }

            _perftCache.Add((key, depth), tot);
            
            // if (!move.IsNullMove() && tot <= int.MaxValue)
            //     Table.Store(posKey, (int)tot, Bound.Exact, (sbyte)depth, move, 0);

            return tot;
        }

        private bool IsRepetition()
        {
            var repetitionCounter = 0;

            var current = Pos.State;
            var backPos = current?.Previous?.Previous;
            while (backPos != null)
            {
                if (current.Key == backPos.Key && ++repetitionCounter == 3)
                    return true;
                
                backPos = current.Previous?.Previous;
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int HalfMoveCount()
        {
            // TODO : This is WRONG!? :)
            return State.Rule50;
        }
    }
}