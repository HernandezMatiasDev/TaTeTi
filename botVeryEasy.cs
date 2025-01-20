using System;

namespace TaTeTi_1._0
{
    public class BotVeryEasy : Bot
    {

        public override byte[] playing(bool player)
        {

            Random random = new Random();
            byte row, col;

            do
            {
                row = (byte)random.Next(0, 3);
                col = (byte)random.Next(0, 3);
            } while (GameState[row, col] != null);


            return new byte[] { row, col }; //ver si tengo que hacer algo mas al terminar el juego
        }
    }
}   