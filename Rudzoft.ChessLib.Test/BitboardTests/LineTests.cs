﻿using Rudzoft.ChessLib.Types;

namespace Rudzoft.ChessLib.Test.BitboardTests;

public sealed class LineTests
{
    [Theory]
    [InlineData(Squares.a1, Squares.a1, 0)]
    [InlineData(Squares.a1, Squares.a2, 8)]
    [InlineData(Squares.a1, Squares.a3, 8)]
    [InlineData(Squares.a1, Squares.a4, 8)]
    [InlineData(Squares.a1, Squares.a5, 8)]
    [InlineData(Squares.a1, Squares.a6, 8)]
    [InlineData(Squares.a1, Squares.a7, 8)]
    [InlineData(Squares.a1, Squares.a8, 8)]
    [InlineData(Squares.a1, Squares.b1, 8)]
    [InlineData(Squares.a1, Squares.b2, 8)]
    [InlineData(Squares.a1, Squares.b3, 0)]
    [InlineData(Squares.a1, Squares.b4, 0)]
    [InlineData(Squares.a1, Squares.b5, 0)]
    [InlineData(Squares.a1, Squares.b6, 0)]
    [InlineData(Squares.a1, Squares.b7, 0)]
    [InlineData(Squares.a1, Squares.b8, 0)]
    [InlineData(Squares.a1, Squares.c1, 8)]
    [InlineData(Squares.a1, Squares.c2, 0)]
    [InlineData(Squares.a1, Squares.c3, 8)]
    [InlineData(Squares.a1, Squares.c4, 0)]
    [InlineData(Squares.a1, Squares.c5, 0)]
    [InlineData(Squares.a1, Squares.c6, 0)]
    [InlineData(Squares.a1, Squares.c7, 0)]
    [InlineData(Squares.a1, Squares.c8, 0)]
    [InlineData(Squares.a1, Squares.d1, 8)]
    [InlineData(Squares.a1, Squares.d2, 0)]
    [InlineData(Squares.a1, Squares.d3, 0)]
    [InlineData(Squares.a1, Squares.d4, 8)]
    [InlineData(Squares.a1, Squares.d5, 0)]
    [InlineData(Squares.a1, Squares.d6, 0)]
    [InlineData(Squares.a1, Squares.d7, 0)]
    [InlineData(Squares.a1, Squares.d8, 0)]
    [InlineData(Squares.a1, Squares.e1, 8)]
    [InlineData(Squares.a1, Squares.e2, 0)]
    [InlineData(Squares.a1, Squares.e3, 0)]
    [InlineData(Squares.a1, Squares.e4, 0)]
    [InlineData(Squares.a1, Squares.e5, 8)]
    [InlineData(Squares.a1, Squares.e6, 0)]
    [InlineData(Squares.a1, Squares.e7, 0)]
    [InlineData(Squares.a1, Squares.e8, 0)]
    [InlineData(Squares.a1, Squares.f1, 8)]
    [InlineData(Squares.a1, Squares.f2, 0)]
    [InlineData(Squares.a1, Squares.f3, 0)]
    [InlineData(Squares.a1, Squares.f4, 0)]
    [InlineData(Squares.a1, Squares.f5, 0)]
    [InlineData(Squares.a1, Squares.f6, 8)]
    [InlineData(Squares.a1, Squares.f7, 0)]
    [InlineData(Squares.a1, Squares.f8, 0)]
    [InlineData(Squares.a1, Squares.g1, 8)]
    [InlineData(Squares.a1, Squares.g2, 0)]
    [InlineData(Squares.a1, Squares.g3, 0)]
    [InlineData(Squares.a1, Squares.g4, 0)]
    [InlineData(Squares.a1, Squares.g5, 0)]
    [InlineData(Squares.a1, Squares.g6, 0)]
    [InlineData(Squares.a1, Squares.g7, 8)]
    [InlineData(Squares.a1, Squares.g8, 0)]
    [InlineData(Squares.a1, Squares.h1, 8)]
    [InlineData(Squares.a1, Squares.h2, 0)]
    [InlineData(Squares.a1, Squares.h3, 0)]
    [InlineData(Squares.a1, Squares.h4, 0)]
    [InlineData(Squares.a1, Squares.h5, 0)]
    [InlineData(Squares.a1, Squares.h6, 0)]
    [InlineData(Squares.a1, Squares.h7, 0)]
    [InlineData(Squares.a1, Squares.h8, 8)]
    public void LineCount(Squares s1, Squares s2, int expected)
    {
        var sq1 = new Square(s1);
        var sq2 = new Square(s2);

        var line = sq1.Line(sq2);

        var actual = line.Count;

        Assert.Equal(expected, actual);
    }
}