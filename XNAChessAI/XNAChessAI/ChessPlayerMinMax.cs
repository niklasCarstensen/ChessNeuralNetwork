﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace XNAChessAI
{
    class ChessPlayerMinMax : ChessPlayer
    {
        static int[] pawnScores = {
0,  0,  0,  0,  0,  0,  0,  0,
50, 50, 50, 50, 50, 50, 50, 50,
10, 10, 20, 30, 30, 20, 10, 10,
 5,  5, 10, 25, 25, 10,  5,  5,
 0,  0,  0, 20, 20,  0,  0,  0,
 5, -5,-10,  0,  0,-10, -5,  5,
 5, 10, 10,-20,-20, 10, 10,  5,
 0,  0,  0,  0,  0,  0,  0,  0 };
        static int[] knightScores = {
-50,-40,-30,-30,-30,-30,-40,-50,
-40,-20,  0,  0,  0,  0,-20,-40,
-30,  0, 10, 15, 15, 10,  0,-30,
-30,  5, 15, 20, 20, 15,  5,-30,
-30,  0, 15, 20, 20, 15,  0,-30,
-30,  5, 10, 15, 15, 10,  5,-30,
-40,-20,  0,  5,  5,  0,-20,-40,
-50,-40,-30,-30,-30,-30,-40,-50 };
        static int[] bishopScores = {
-20,-10,-10,-10,-10,-10,-10,-20,
-10,  0,  0,  0,  0,  0,  0,-10,
-10,  0,  5, 10, 10,  5,  0,-10,
-10,  5,  5, 10, 10,  5,  5,-10,
-10,  0, 10, 10, 10, 10,  0,-10,
-10, 10, 10, 10, 10, 10, 10,-10,
-10,  5,  0,  0,  0,  0,  5,-10,
-20,-10,-10,-10,-10,-10,-10,-20 };
        static int[] rookScores = {
 0,  0,  0,  0,  0,  0,  0,  0,
 5, 10, 10, 10, 10, 10, 10,  5,
-5,  0,  0,  0,  0,  0,  0, -5,
-5,  0,  0,  0,  0,  0,  0, -5,
-5,  0,  0,  0,  0,  0,  0, -5,
-5,  0,  0,  0,  0,  0,  0, -5,
-5,  0,  0,  0,  0,  0,  0, -5,
 0,  0,  0,  5,  5,  0,  0,  0 };
        static int[] queenScores = {
-20,-10,-10, -5, -5,-10,-10,-20,
-10,  0,  0,  0,  0,  0,  0,-10,
-10,  0,  5,  5,  5,  5,  0,-10,
 -5,  0,  5,  5,  5,  5,  0, -5,
  0,  0,  5,  5,  5,  5,  0, -5,
-10,  5,  5,  5,  5,  5,  0,-10,
-10,  0,  5,  0,  0,  0,  0,-10,
-20,-10,-10, -5, -5,-10,-10,-20 };
        static int[] kingScores = {
-30,-40,-40,-50,-50,-40,-40,-30,
-30,-40,-40,-50,-50,-40,-40,-30,
-30,-40,-40,-50,-50,-40,-40,-30,
-30,-40,-40,-50,-50,-40,-40,-30,
-20,-30,-30,-40,-40,-30,-30,-20,
-10,-20,-20,-20,-20,-20,-20,-10,
 20, 20,  0,  0,  0,  0, 20, 20,
 20, 30, 10,  0,  0, 10, 30, 20 };
        
        public Move[] GetAllMoves(ChessBoard Board, ChessPlayer Player)
        {
            List<Move> moves = new List<Move>(8 * 8);
            for (int x = 0; x < 8; x++)
                for (int y = 0; y < 8; y++)
                {
                    if (Board.GetChessPieceFromPoint(x, y) != null && Board.GetChessPieceFromPoint(x, y).Parent == Player)
                    {
                        Point[] targets = Board.GetAllPossibleMovesForPiece(x, y);
                        for (int i = 0; i < targets.Length; i++)
                            moves.Add(new Move(new Point(x, y), targets[i]));
                    }
                }
            return moves.ToArray();
        }
        public int EvaluationFunction(ChessBoard Board)
        {
            int re = 0;
            for (int x = 0; x < 8; x++)
                for (int y = 0; y < 8; y++)
                {
                    ChessPiece Piece = Board.GetChessPieceFromPoint(x, y);
                    if (Piece != null)
                    {
                        if (Piece.Parent == this)
                        {
                            if (this == Board.PlayerBottom)
                            {
                                switch (Piece.Type)
                                {
                                    case ChessPieceType.Pawn:
                                        re += 10 + pawnScores[x + y * 8];
                                        break;
                                    case ChessPieceType.Knight:
                                        re += 30 + knightScores[x + y * 8];
                                        break;
                                    case ChessPieceType.Rook:
                                        re += 50 + rookScores[x + y * 8];
                                        break;
                                    case ChessPieceType.Bishop:
                                        re += 30 + bishopScores[x + y * 8];
                                        break;
                                    case ChessPieceType.Queen:
                                        re += 90 + queenScores[x + y * 8];
                                        break;
                                    case ChessPieceType.King:
                                        re += 900 + kingScores[x + y * 8];
                                        break;
                                }
                            }
                            else
                            {
                                switch (Piece.Type)
                                {
                                    case ChessPieceType.Pawn:
                                        re += 10 + pawnScores[x + (7 - y) * 8];
                                        break;
                                    case ChessPieceType.Knight:
                                        re += 30 + knightScores[x + (7 - y) * 8];
                                        break;
                                    case ChessPieceType.Rook:
                                        re += 50 + rookScores[x + (7 - y) * 8];
                                        break;
                                    case ChessPieceType.Bishop:
                                        re += 30 + bishopScores[x + (7 - y) * 8];
                                        break;
                                    case ChessPieceType.Queen:
                                        re += 90 + queenScores[x + (7 - y) * 8];
                                        break;
                                    case ChessPieceType.King:
                                        re += 900 + kingScores[x + (7 - y) * 8];
                                        break;
                                }
                            }
                        }
                        else
                        {
                            if (Board.GetOponent(this) == Board.PlayerBottom)
                            {
                                switch (Piece.Type)
                                {
                                    case ChessPieceType.Pawn:
                                        re -= 10 + pawnScores[x + y * 8];
                                        break;
                                    case ChessPieceType.Knight:
                                        re -= 30 + knightScores[x + y * 8];
                                        break;
                                    case ChessPieceType.Rook:
                                        re -= 50 + rookScores[x + y * 8];
                                        break;
                                    case ChessPieceType.Bishop:
                                        re -= 30 + bishopScores[x + y * 8];
                                        break;
                                    case ChessPieceType.Queen:
                                        re -= 90 + queenScores[x + y * 8];
                                        break;
                                    case ChessPieceType.King:
                                        re -= 900 + kingScores[x + y * 8];
                                        break;
                                }
                            }
                            else
                            {
                                switch (Piece.Type)
                                {
                                    case ChessPieceType.Pawn:
                                        re -= 10 + pawnScores[x + (7 - y) * 8];
                                        break;
                                    case ChessPieceType.Knight:
                                        re -= 30 + knightScores[x + (7 - y) * 8];
                                        break;
                                    case ChessPieceType.Rook:
                                        re -= 50 + rookScores[x + (7 - y) * 8];
                                        break;
                                    case ChessPieceType.Bishop:
                                        re -= 30 + bishopScores[x + (7 - y) * 8];
                                        break;
                                    case ChessPieceType.Queen:
                                        re -= 90 + queenScores[x + (7 - y) * 8];
                                        break;
                                    case ChessPieceType.King:
                                        re -= 900 + kingScores[x + (7 - y) * 8];
                                        break;
                                }
                            }
                        }
                    }
                }
            return re;
        }
        public Move MiniMax(int depth, ChessBoard Board, Move lastMove, bool maximising)
        {
            if (depth == 0)
            {
                Move m = new Move();
                m.rating = EvaluationFunction(Board);
                return m;
            }
            Move[] Moves;
            if (maximising)
            {
                Moves = GetAllMoves(Board, this);
                Move bestMove = new Move();
                bestMove.rating = int.MinValue;
                for (int i = 0; i < Moves.Length; i++)
                {
                    ChessBoard Clone = (ChessBoard)Board.Clone();
                    Clone.MovePiece(Moves[i].From, Moves[i].To);
                    if (Clone.GameEnded && Clone.Winner != this)
                        continue;
                    Moves[i].rating = EvaluationFunction(Clone);
                    if (Moves[i].rating < lastMove.rating)
                        continue;
                    if (Clone.Winner == this)
                        return new Move(Moves[i].From, Moves[i].To, int.MaxValue);
                    Move minimax = MiniMax(depth - 1, Clone, Moves[i], !maximising);
                    if (minimax.rating > bestMove.rating)
                        bestMove = Moves[i];
                }
                if (bestMove.rating == 0)
                    bestMove = Moves.OrderBy(x => Values.RDM.Next(int.MaxValue)).OrderByDescending(x => x.rating).First();
                return bestMove;
            }
            else
            { 
                Moves = GetAllMoves(Board, Board.GetOponent(this));
                Move bestMove = new Move();
                bestMove.rating = int.MaxValue;
                for (int i = 0; i < Moves.Length; i++)
                {
                    ChessBoard Clone = (ChessBoard)Board.Clone();
                    Clone.MovePiece(Moves[i].From, Moves[i].To);
                    if (Clone.GameEnded && Clone.Winner != this)
                        return new Move(Moves[i].From, Moves[i].To, int.MinValue);
                    Moves[i].rating = EvaluationFunction(Clone);
                    if (Moves[i].rating > lastMove.rating)
                        continue;
                    Move minimax = MiniMax(depth - 1, Clone, Moves[i], !maximising);
                    if (minimax.rating < bestMove.rating)
                        bestMove = Moves[i];
                }
                if (bestMove.rating == 0)
                    bestMove = Moves.OrderBy(x => Values.RDM.Next(int.MaxValue)).OrderByDescending(x => -x.rating).First();
                return bestMove;
            }
        }
        
        public override void Update()
        {
            Move minimax = MiniMax(3, Parent, new Move(), true);
            if (minimax.From == Point.Zero && minimax.To == Point.Zero)
            {
                Debug.WriteLine("I dunnu wat im doin!");
                Move[] moves = GetAllMoves(Parent, this);
                minimax = moves[Values.RDM.Next(moves.Length)];
            }
            Parent.MovePiece(minimax);
        }
    }
}