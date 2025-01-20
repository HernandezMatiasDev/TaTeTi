using System;
using System.Configuration;
using System.Linq;

namespace TaTeTi_1._0
{
        public class BotEasy : Bot
    {
        const byte COL = 2, ROW = 1, DIAGONAL = 3, UPWARD = 1, DOWNWARD = 2;
        

        public override byte[] playing(bool player)
        {
            byte[,] possibleLine = new byte[8,3];
            byte nullPlays;

            possibleLine = lineFuncion(player);
            possibleLine = sortList(possibleLine);
            nullPlays = countNullsPlasy(possibleLine);

            byte[,] cleanPossibleLine = new byte[8 - nullPlays,3];

            cleanPossibleLine = clearList(possibleLine, nullPlays);

            byte?[,] plays = new byte?[9,2];
            plays = bestPlays(cleanPossibleLine);

            byte countPlaysNull = countNulls(plays);

            byte[,] clearPlays = new byte[9-countPlaysNull,2];
            clearPlays = clearNulls(plays, countPlaysNull);

            Random random = new Random();
            byte randomBestPlay;
            randomBestPlay = (byte)random.Next(0, 9-countPlaysNull);
            
            byte[] palyReturn = new byte[2];
            palyReturn[0] = clearPlays[randomBestPlay, 0];
            palyReturn[1] = clearPlays[randomBestPlay, 1];


            return palyReturn; // Retorna una jugada predecible
        }

        private byte countNulls(byte?[,] plays)
        {
            byte count = 0;
            for(byte i = 0; i < plays.GetLength(0); i++)
            {
                if (plays[i,0] == null || plays[i,1] == null)
                {
                    count += 1;
                }
            }
            return count;
        }
        private byte[,] clearNulls(byte?[,] plays, byte count)
        {
            byte[,] clearPlays = new byte[9-count,2];
            byte AuxiliaryCount = 0;

            for(byte i = 0; i < plays.GetLength(0); i++)
            {
                if (plays[i,0] != null && plays[i,1] != null)
                {
                    clearPlays[AuxiliaryCount, 0] = (byte)(plays[i,0]);
                    clearPlays[AuxiliaryCount, 1] = (byte)(plays[i,1]);
                    AuxiliaryCount += 1;
                }
            }
            return clearPlays;
        }
        
        
        private byte?[,] bestPlays(byte[,] lines)
        {
            byte?[,] listBestPlays = new byte?[9,2];
            byte?[,] listCoordinatePositions = new byte?[3, 2];
            byte[] listAuxiliary = new byte[2];

            byte count = 0;
            
            bool twoToken = false;
            byte twoTokenCount = 0;
            bool oneToken = false;
            byte oneTokenCount = 0;
            bool zeroToken = false;
            byte zerTokenCount = 0;
    
            for(byte i = 0; i < lines.GetLength(0); i++)
            {
                if (lines[i, 2] == 2)
                {
                    twoToken = true;
                    twoTokenCount += 1;
                }
                else if (lines[i, 2] == 1)
                {
                    oneToken = true;
                    oneTokenCount += 1;
                }
                else if (lines[i, 2] == 0)
                {
                    zeroToken = true;
                    zerTokenCount += 1;
                }
            }
            
            if (twoToken)
            { 
                for(byte i = 0; i < twoTokenCount; i++)
                {
                    listAuxiliary[0] = lines[i,0];
                    listAuxiliary[1] = lines[i,1];
                    listCoordinatePositions = cordenatePositions(listAuxiliary);
                    for (byte x = 0; x<3; x++)
                    {
                        if (!savedMove(listBestPlays,listCoordinatePositions[x,0],listCoordinatePositions[x,1]))
                        {
                            listBestPlays[count,0] = listCoordinatePositions[x,0];
                            listBestPlays[count,1] = listCoordinatePositions[x,1];
                            count += 1;
                        }
                    }

                }
            }
            else if (oneToken)
            {
                for(byte i = 0; i < oneTokenCount; i++)
                {
                    listAuxiliary[0] = lines[i,0];
                    listAuxiliary[1] = lines[i,1];
                    listCoordinatePositions = cordenatePositions(listAuxiliary);
                    for (byte x = 0; x<3; x++)
                    {
                        if (!savedMove(listBestPlays,listCoordinatePositions[x,0],listCoordinatePositions[x,1]))
                        {
                            listBestPlays[count,0] = listCoordinatePositions[x,0];
                            listBestPlays[count,1] = listCoordinatePositions[x,1];
                            count += 1;
                        }
                    }
                }
            }
            else if (zeroToken)
            {
                for(byte i = 0; i < zerTokenCount; i++)
                {
                    listAuxiliary[0] = lines[i,0];
                    listAuxiliary[1] = lines[i,1];
                    listCoordinatePositions = cordenatePositions(listAuxiliary);
                    for (byte x = 0; x<3; x++)
                    {
                        if (!savedMove(listBestPlays,listCoordinatePositions[x,0],listCoordinatePositions[x,1]))
                        {   
                            listBestPlays[count,0] = listCoordinatePositions[x,0];
                            listBestPlays[count,1] = listCoordinatePositions[x,1];
                            count += 1;
                        }
                    }
                }
            }
            return listBestPlays;

        }
            private bool savedMove(byte?[,] matriz, byte? valor1, byte? valor2)
        {
            return Enumerable.Range(0, matriz.GetLength(0)).Any(i => matriz[i, 0] == valor1 && matriz[i, 1] == valor2);
        }

        private byte?[,] cordenatePositions(byte[] line)
        {
            byte?[,] listCordenatePositions = new byte?[3, 2];
            byte count = 0;
            
            if (line[1] == ROW)
            {
                for (byte i = 0; i < 3; i++)
                {
                    if (GameState[line[0], i] == null)
                    {
                        listCordenatePositions[count, 0] = line[0];
                        listCordenatePositions[count, 1] = i;
                        count += 1;
                    }
                }
            }
            else if(line[1] == COL)
            {
                for (byte i = 0; i < 3; i++)
                {
                    if (GameState[i, line[0]] == null)
                    {
                        listCordenatePositions[count, 0] = line[0];
                        listCordenatePositions[count, 1] = i;
                        count += 1;
                    }
                }
            }else if(line[1] == DIAGONAL)
            {
                if (line[0] == DOWNWARD)
                {
                    for (byte i=0; i<3; i++)
                    {
                        if (GameState[i, i] == null)
                        {
                            listCordenatePositions[count, 0] = i;
                            listCordenatePositions[count, 1] = i;
                            count += 1;
                        }
                    }
                }
                else if (line[0] == UPWARD)
                {
                    for (byte i = 0, z = 2; i < 3 ; i++, z--)
                    {
                        if (GameState[i, z] == null)
                        {
                            listCordenatePositions[count, 0] = i;
                            listCordenatePositions[count, 1] = z;
                            count += 1;
                        }
                    }
                }
            }
            return listCordenatePositions;

        }

        //sort the list from largest to smallest
        private byte[,] sortList(byte[,] lines) 
        {
            for(byte x = 0; x < lines.GetLength(0); x++)
            {
                for(byte y = 0; y < lines.GetLength(0) - x - 1; y++)
                {
                    if (lines[y, 2] < lines[y + 1, 2])
                    {
                        for (byte i = 0; i < 3; i++)
                        {
                            lines[y + 1, i] = (byte)(lines[y, i] + lines[y + 1, i]);
                            lines[y, i] = (byte)(lines[y + 1, i] - lines[y, i]);
                            lines[y + 1, i] = (byte)(lines[y + 1, i] - lines[y, i]);
                        }

                    }

                }
            }
            return lines;
        }

        //determines how many lines are not available
        private byte countNullsPlasy(byte[,] lines)
        {
            byte count = 0;
            for (byte i = 0; i < lines.GetLength(0); i++)
            {
                if (lines[i, 1] == 0) //since place 1 can never be 0, if it is 0 it is because it is not a possible line
                {
                    count += 1;
                }
            }
            return count;
        }
        private byte[,] clearList(byte[,] lines, byte count)
        {
             
            byte[,] newListClear = new byte[8 - count,3];
            for (byte i = 0; i < newListClear.GetLength(0); i++)
            {
                for(byte x = 0; x<3; x++)
                {
                    newListClear[i, x] = lines[i, x];
                }
            }
            return newListClear;
        }
        private bool checkline(bool? cell1, bool? cell2, bool? cell3, bool player)
        {
            if (cell1 != !player  && cell2 != !player && cell3 != !player)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //This function checks for clean lines 
        private byte[,] lineFuncion(bool player)  //returns a list of possible lines

        {       
            byte[,] possibleLine = new byte[8,3];

            byte cont = 0;
            byte playerQuantity;

            for(byte x=0; x<3; x++)   //Check for possible vertical lines
            {
                if (checkline(GameState[0,x],GameState[1,x],GameState[2,x], player)) 
                {
                    playerQuantity = 0;

                    for (byte i=0; i<3; i++) // count how many pieces you have on that line

                    {
                        if (GameState[x,i] == player)
                        {
                            playerQuantity += 1;
                        }
                    }

                    possibleLine[cont, 0] = x;
                    possibleLine[cont, 1] = COL;
                    possibleLine[cont, 2] = playerQuantity;
                    cont += 1;
                }
                if (checkline(GameState[x,0],GameState[x,1],GameState[x,2], player)) //Check for possible horizontal lines
                {
                    playerQuantity = 0;

                    for (byte i=0; i<3; i++)
                    {
                        if (GameState[x,i] == player)
                        {
                            playerQuantity += 1;
                        }
                    }

                    possibleLine[cont, 0] = x;
                    possibleLine[cont, 1] = ROW;
                    possibleLine[cont, 2] = playerQuantity;

                    cont+= 1;
                }
            }
            if (checkline(GameState[0,0],GameState[1,1],GameState[2,2], player)) //Check for possible diagonals
            {
                playerQuantity = 0;

                for (byte i=0; i<3; i++)
                    {
                        if (GameState[i,i] == player)
                        {
                            playerQuantity += 1;
                        }
                    }

                possibleLine[cont, 0] = DOWNWARD;
                possibleLine[cont, 1] = DIAGONAL;
                possibleLine[cont, 2] = playerQuantity;
                cont+= 1;
            }

             if (checkline(GameState[0,2],GameState[1,1],GameState[2,0], player))//Check for possible diagonals
            {
                playerQuantity = 0;

                for (byte i = 0, z = 2; i < 3 ; i++, z--)
                    {
                        if (GameState[i,z] == player)
                        {
                            playerQuantity += 1;
                        }
                    }

                possibleLine[cont, 0] = UPWARD;
                possibleLine[cont, 1] = DIAGONAL;
                possibleLine[cont, 2] = playerQuantity;
                cont+= 1;
            }
            
            return possibleLine;
        }
    }

}