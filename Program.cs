using System;

namespace Connect_4_Bot
{
    class Program
    {

        /**
         * Converts the given string board to our board representation.
         * This function is quite expensive and could be improved, although I am
         * not sure if it would make it possible to increase the search depth to make
         * it worth it.
         */
        private static Board getBoardFromInput(String boardInput, char me)
        {
            boardInput = boardInput.Replace(",", "");
            Board b = new Board();
            int x = 6;
            for (int i = 0; i < boardInput.Length; i++)
            {
                if (i % 7 == 0) x--;
                if (boardInput[i] == me) { b.Get[x][i % 7] = -1; b.LastPiece[i % 7]++; }
                else if (boardInput[i] == '.')
                    b.Get[x][i % 7] = 0;
                else { b.Get[x][i % 7] = 1; b.LastPiece[i % 7]++; }
            }
            return b;
        }

        static void Main(string[] args)
        {
            Minimax<Board> minimax = new Minimax<Board>(new Connect4GameRules());
            Board currentBoard = new Board();
            char me = '0';
            int remainingTime = 1000;

            // Set the size of the standard input:
            Console.OpenStandardInput(512);
            String cmd = "";
            while ((cmd = Console.ReadLine()) != null)
            {
                String[] cmds = cmd.Split(' ');
                switch (cmds[0])
                {
                    case "settings":
                        // The only settings we really care about is our ID in the board:
                        if (cmds[1] == "your_botid") me = char.Parse(cmds[2]);
                        break;
                    case "update":
                        // The only update we care about is the current state of the board:
                        if (cmds[1] == "game" && cmds[2] == "field") currentBoard = getBoardFromInput(cmds[3], me);
                        break;
                    case "action":
                        if (cmds[1] == "move")
                        {
                            remainingTime = int.Parse(cmds[2]);
                            // The engine is requesting a move:
                            int[] boardBefore = currentBoard.LastPiece;

                            // Play the next move with search depth of 6, but we could use the remainingTime variable to 
                            // improve the depth with something like (remainingTime > [value] ? 8 : 6)
                            currentBoard = minimax.PlayNextMove(currentBoard, 6);

                            // We have to figure out what position changed =/
                            for (int i = 0; i < boardBefore.Length; i++)
                                if (boardBefore[i] != currentBoard.LastPiece[i])
                                {
                                    Console.WriteLine("place_disc " + i);
                                    break;
                                }
                        }
                        break;
                }
            }
        }
    }
}
