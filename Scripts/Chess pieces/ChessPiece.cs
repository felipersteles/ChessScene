
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace felipsteles
{
    public enum ChessPieceType
    {
        None = 0,
        Pawn = 1,
        Rook = 2,
        Knight = 3,
        Bishop = 4,
        Queen = 5,
        King = 6
    }

    public class ChessPiece : MonoBehaviour
    {
        public int team; //0 branco 1 preto
        public int currentX;
        public int currentY;
        public ChessPieceType type;

        private Vector3 desiredPosition;
        private Vector3 desiredScale;

        private void Update()
        {
            float delta = 10;

            transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * delta);
            transform.localScale = Vector3.Lerp(transform.localScale, desiredScale, Time.deltaTime * delta);
        }

        //função com argumentos meramente ilustrativos pois é sobrescrito em cada peça
        public virtual List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board, int tileCountX, int tileCountY)
        {
            List<Vector2Int> aux = new List<Vector2Int>();

            aux.Add(new Vector2Int(3,3));
            aux.Add(new Vector2Int(3,4));
            aux.Add(new Vector2Int(4,3));
            aux.Add(new Vector2Int(4,4));

            return aux;
        }

        public virtual void SetPosition(Vector3 position, bool force = false)
        {
            desiredPosition = position;
            if(force)
                transform.position = desiredPosition;
        }
        public virtual void SetScale(Vector3 scale, bool force = false)
        {
            desiredScale = scale;
            if(force)
                transform.localScale = desiredScale;
        }
    }
}