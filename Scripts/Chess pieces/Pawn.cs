
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace felipsteles
{
    public class Pawn : ChessPiece
    {
        public override List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board, int tileCountX, int tileCountY)
        {
            List<Vector2Int> aux = new List<Vector2Int>();

                            //if        true false(else)
            int direction = (team == 0) ? 1 : -1;

            // Um pra frente
            if(board[currentX, currentY + direction] == null)
                aux.Add(new Vector2Int(currentX, currentY + direction));

            // Dois pra frente
            if(board[currentX, currentY + direction] == null)
            {
                //White team
                if(team == 0 && currentY == 1 && board[currentX, currentY + (direction*2)] == null)
                    aux.Add(new Vector2Int(currentX, currentY + (direction*2)));

                //Black team
                if(team == 1 && currentY == 6 && board[currentX, currentY + (direction*2)] == null)
                    aux.Add(new Vector2Int(currentX, currentY + (direction*2)));
            }

            // Diagonal (kill move)
            if(currentX != tileCountX - 1)
                if(board[currentX + 1, currentY + direction] != null && board[currentX + 1, currentY + direction].team != team )
                    aux.Add(new Vector2Int(currentX + 1, currentY + direction)); 
            if(currentX != 0)
                if(board[currentX - 1, currentY + direction] != null && board[currentX - 1, currentY + direction].team != team )
                    aux.Add(new Vector2Int(currentX - 1, currentY + direction)); 

            return aux;
        }
    }
}