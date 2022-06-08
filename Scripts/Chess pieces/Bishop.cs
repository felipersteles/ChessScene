
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace felipsteles
{
    public class Bishop : ChessPiece
    {
        public override List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board, int tileCountX, int tileCountY)
        {
            List<Vector2Int> aux = new List<Vector2Int>();

            // Top RIGHT 
            for( int i = currentX + 1, j = currentY + 1; i < tileCountX && j < tileCountY; i++, j++)
            {
                if(board[i,j] == null)
                    aux.Add(new Vector2Int(i,j));
                else
                {
                    if(board[i,j].team != team)
                        aux.Add(new Vector2Int(i,j));
                }
            }

            // Top LEFT 
            for( int i = currentX - 1, j = currentY + 1; i >= 0 && j < tileCountY; i--, j++)
            {
                if(board[i,j] == null)
                    aux.Add(new Vector2Int(i,j));
                else
                {
                    if(board[i,j].team != team)
                        aux.Add(new Vector2Int(i,j));
                }
            }

            // Bottom LEFT  
            for( int i = currentX - 1, j = currentY - 1; i >= 0 && j >= 0; i--, j--)
            {
                if(board[i,j] == null)
                    aux.Add(new Vector2Int(i,j));
                else
                {
                    if(board[i,j].team != team)
                        aux.Add(new Vector2Int(i,j));
                }
            }

            //Bottom RIGHT
            for( int i = currentX + 1, j = currentY - 1; i < tileCountX && j >= 0; i++, j--)
            {
                if(board[i,j] == null)
                    aux.Add(new Vector2Int(i,j));
                else
                {
                    if(board[i,j].team != team)
                        aux.Add(new Vector2Int(i,j));
                }
            }

            
            
            return aux;
        }
    }
}