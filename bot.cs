using System;
using System.Windows.Forms;

namespace TaTeTi_1._0
{
    public abstract class Bot
    {
        // Declarar el array sin inicializar
        protected bool?[,] game;
        // Pasar el estado del game completo y no solo la modificación.
        public bool?[,] GameState
        {
            get 
            { 
                return game; 
            }
            set 
            { 
                if (value.GetLength(0) == 3 && value.GetLength(1) == 3) // Validar que la matriz sea 3x3
                {
                    game = value; 
                }
                else
                {
                    throw new ArgumentException("Error, la matriz debe ser de 3x3.");
                }
            } 
        }


        // Constructor que recibe la instancia de Game
        public Bot()
        {
            game = new bool?[3, 3];
        }



        // Método abstracto que deberá ser implementado por las clases derivadas, tranformar gameOver a false antes de finalizar
        public abstract byte[] playing(bool player);
    }
}
