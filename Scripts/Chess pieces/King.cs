
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace felipsteles
{
    public class King : ChessPiece
    {
        public override List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board, int tileCountX, int tileCountY)
        {
            List<Vector2Int> aux = new List<Vector2Int>();

            // Right
            if(currentX + 1 < tileCountX)
            {
                //RIGHT
                if(board[currentX + 1, currentY] == null)
                    aux.Add(new Vector2Int(currentX + 1, currentY));
                else if(board[currentX + 1, currentY].team != team) //comer as peças
                    aux.Add(new Vector2Int(currentX + 1, currentY));

                // Top right
                if (currentY + 1 < tileCountY)
                    if (board[currentX + 1, currentY + 1] == null)
                        aux.Add(new Vector2Int(currentX + 1, currentY + 1));
                    else if (board[currentX + 1, currentY + 1].team != team)
                        aux.Add(new Vector2Int(currentX + 1, currentY + 1));

                // bottom right
                if(currentY - 1 >= 0)
                    if(board[currentX + 1, currentY - 1] == null)
                        aux.Add(new Vector2Int(currentX + 1, currentY - 1));
                    else if (board[currentX + 1, currentY - 1].team != team)
                        aux.Add(new Vector2Int(currentX + 1, currentY - 1));

            }

            //Left
            if(currentX - 1 >= 0)
            {
                //LEFT
                if(board[currentX - 1, currentY] == null)
                    aux.Add(new Vector2Int(currentX - 1, currentY));
                else if(board[currentX + 1, currentY].team != team) //comer as peças
                    aux.Add(new Vector2Int(currentX - 1, currentY));

                // TOP left
                if (currentY + 1 < tileCountY)
                    if (board[currentX - 1, currentY + 1] == null)
                        aux.Add(new Vector2Int(currentX - 1, currentY + 1));
                    else if (board[currentX - 1, currentY + 1].team != team)
                        aux.Add(new Vector2Int(currentX - 1, currentY + 1));

                // BOTTOM left
                if(currentY - 1 >= 0)
                    if(board[currentX - 1, currentY - 1] == null)
                        aux.Add(new Vector2Int(currentX - 1, currentY - 1));
                    else if (board[currentX - 1, currentY - 1].team != team)
                        aux.Add(new Vector2Int(currentX - 1, currentY - 1));

            }

            //TOP
            if(currentY + 1 < tileCountY)
            {
                if(board[currentX, currentY + 1] == null)
                    aux.Add(new Vector2Int(currentX, currentY + 1));
                else if(board[currentX, currentY + 1].team != team) //comer as peças
                    aux.Add(new Vector2Int(currentX, currentY + 1));
            }

            //BOTTOM
            if(currentY - 1 >= 0)
            {
                if(board[currentX, currentY - 1] == null)
                    aux.Add(new Vector2Int(currentX, currentY - 1));
                else if(board[currentX, currentY - 1].team != team) //comer as peças
                    aux.Add(new Vector2Int(currentX, currentY - 1));
            }

            
            return aux;
        }
    }
}