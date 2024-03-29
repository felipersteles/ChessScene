
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace felipsteles
{
    public class Rook : ChessPiece
    {
        public override List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board, int tileCountX, int tileCountY)
        {
            List<Vector2Int> aux = new List<Vector2Int>();

            // Para baixo
            for(int i = currentY - 1; i >= 0; i--)
            {
                if(board[currentX,i] == null)
                    aux.Add(new Vector2Int(currentX, i));

                if(board[currentX, i]!=null)
                {
                    if(board[currentX, i].team != team)
                        aux.Add(new Vector2Int(currentX, i));

                    break;
                }
            }

            // Para cima
            for(int i = currentY + 1; i < tileCountY; i++)
            {
                if(board[currentX,i] == null)
                    aux.Add(new Vector2Int(currentX, i));

                if(board[currentX, i]!=null)
                {
                    if(board[currentX, i].team != team)
                        aux.Add(new Vector2Int(currentX, i));

                    break;
                }
            }

            // Para Esquerda
            for(int i = currentX - 1; i >= 0; i--)
            {
                if(board[i, currentY] == null)
                    aux.Add(new Vector2Int(i, currentY));

                if(board[i, currentY] != null)
                {
                    if(board[i, currentY].team != team)
                        aux.Add(new Vector2Int(i, currentY));

                    break;
                }
            }

            // Para direita
            for(int i = currentX + 1; i < tileCountX; i++)
            {
                if(board[i, currentY] == null)
                    aux.Add(new Vector2Int(i, currentY));

                if(board[i, currentY] != null)
                {
                    if(board[i, currentY].team != team)
                        aux.Add(new Vector2Int(i, currentY));

                    break;
                }
            }

            return aux;
        }
    }
}