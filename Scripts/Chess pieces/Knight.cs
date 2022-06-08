
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace felipsteles
{
    public class Knight : ChessPiece
    {
        public override List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board, int tileCountX, int tileCountY)
        {
            List<Vector2Int> aux = new List<Vector2Int>();

            // Top right
            int x = currentX + 1;
            int y = currentY + 2;
            if( x< tileCountX && y < tileCountY)
                if(board[x,y] == null || board[x,y].team != team)
                    aux.Add(new Vector2Int(x,y));

            x = currentX + 2;
            y = currentY + 1;
            if(x < tileCountX && y < tileCountY)
                if(board[x,y] == null || board[x,y].team != team)
                    aux.Add(new Vector2Int(x,y));

            // Top left
            x = currentX - 1;
            y = currentY + 2;
            if( x >= 0 && y < tileCountY)
                if(board[x,y] == null || board[x,y].team != team)
                    aux.Add(new Vector2Int(x,y));

            x = currentX - 2;
            y = currentY + 1;
            if(x >= 0 && y < tileCountY)
                if(board[x,y] == null || board[x,y].team != team)
                    aux.Add(new Vector2Int(x,y));
                    
            // bottom RIGHT
            x = currentX + 1;
            y = currentY - 2;
            if( x< tileCountX && y >= 0)
                if(board[x,y] == null || board[x,y].team != team)
                    aux.Add(new Vector2Int(x,y));

            x = currentX + 2;
            y = currentY - 1;
            if(x < tileCountX && y >= 0)
                if(board[x,y] == null || board[x,y].team != team)
                    aux.Add(new Vector2Int(x,y));
                    
            // bottom LEFT
            x = currentX - 1;
            y = currentY - 2;
            if( x >= 0 && y >= 0)
                if(board[x,y] == null || board[x,y].team != team)
                    aux.Add(new Vector2Int(x,y));
                    
            x = currentX - 2;
            y = currentY - 1;
            if( x >= 0 && y >= 0)
                if(board[x,y] == null || board[x,y].team != team)
                    aux.Add(new Vector2Int(x,y));






            return aux;
        }
    }
}