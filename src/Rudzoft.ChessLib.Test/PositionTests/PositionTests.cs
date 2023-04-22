﻿/*
ChessLib, a chess data structure library

MIT License

Copyright (c) 2017-2022 Rudy Alex Kohn

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

using Rudzoft.ChessLib.Factories;
using Rudzoft.ChessLib.Types;
using Xunit.Abstractions;

namespace Rudzoft.ChessLib.Test.PositionTests;

public sealed class PositionTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    public PositionTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void AddPieceTest()
    {
        var pieceValue = new PieceValue();
        var board = new Board();
        var position = new Position(board, pieceValue);

        position.AddPiece(Pieces.WhiteKing, Square.A7);
        var pieces = position.Pieces();
        var square = pieces.Lsb();
        Assert.Equal(square, Square.A7);

        var piece = position.GetPiece(square);
        Assert.Equal(Pieces.WhiteKing, piece.Value);

        // test overload
        pieces = position.Pieces(piece);
        Assert.False((pieces & square).IsEmpty);

        // Test piece type overload

        board = new Board();
        position = new Position(board, pieceValue);

        position.AddPiece(PieceTypes.Knight.MakePiece(Player.Black), Square.D5);
        pieces = position.Pieces();
        square = pieces.Lsb();
        Assert.Equal(Square.D5, square.Value);

        piece = position.GetPiece(square);
        Assert.Equal(Pieces.BlackKnight, piece.Value);
    }

    [Fact]
    public void KingBlocker()
    {
        // black has an absolute pinned piece at e6
        const string fen = "1rk5/8/4n3/5B2/1N6/8/8/1Q1K4 b - - 3 53";

        const int expected = 1;
        var expectedSquare = new Square(Ranks.Rank6, Files.FileE);

        var game = GameFactory.Create(fen);
        var pos = game.Pos;

        var b = pos.KingBlockers(Player.Black);

        // b must contain one square at this point
        var pinnedCount = b.Count;

        Assert.Equal(expected, pinnedCount);

        // test for correct square
        var actual = b.Lsb();

        Assert.Equal(expectedSquare, actual);
    }

    [Fact]
    public void PieceMovedGeneratesEventWithPieceInfo()
    {
        int moveEventsReceived = 0;
        int removeEventsReceived = 0;
        int addEventsReceived = 0;
        var moves = new[]
        {
            Move.Create(Square.F2, Square.F3),
            Move.Create(Square.E7, Square.E5),
            Move.Create(Square.G2, Square.G4),
            Move.Create(Square.D7, Square.D6),
            Move.Create(Square.H2, Square.H4),
            Move.Create(Square.D8, Square.H4)
        };

        // construct game and start a new game
        var game = GameFactory.Create(Fen.Fen.StartPositionFen);
        game.Pos.IsProbing = false;
        game.Pos.PieceMoved += PieceUpdated;
        game.Pos.PieceAdded += PieceAdded;
        game.Pos.PieceRemoved += PieceRemoved;

        var position = game.Pos;

        // make the moves necessary to create a mate
        foreach (var move in moves)
            position.MakeMove(move, game.Pos.State);

        //We should have received 6 move events and one remove.
        Assert.Equal(6, moveEventsReceived);
        Assert.Equal(1, removeEventsReceived);
        Assert.Equal(0, addEventsReceived);

        void PieceUpdated(object sender, PieceMovedEventArgs args)
        {
            moveEventsReceived++;
            Assert.NotEqual(Pieces.NoPiece, args.MovedPiece.Value);
            _testOutputHelper.WriteLine($"{args.MovedPiece.Value} moved from {args.From} to {args.To}.");
        }
        void PieceRemoved(object sender, PieceRemovedEventArgs args)
        {
            removeEventsReceived++;
            Assert.NotEqual(Pieces.NoPiece, args.RemovedPiece.Value);
            _testOutputHelper.WriteLine($"{args.RemovedPiece.Value} removed from {args.EmptiedSquare}.");
        }

        void PieceAdded(object sender, PieceAddedEventArgs args)
        {
            addEventsReceived++;
        }
    }


}
