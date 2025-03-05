// using System;

// namespace TicTacToe{
    
//     public class TicTacToeEnv{
//     private readonly Char[,] board;
//     private readonly Char empty = '-';
//     private readonly Char PlayerX = 'X';
//     private readonly Char PlayerO = 'O';
    
//     private char currentPlayer;

//     public TicTacToeEnv(){
//         board = new char[3,3];
//         for(int i=0; i<3;i++){
//             for(int j=0;j<3;j++){
//                 board[i,j]=empty;
//             }
//         }
//         currentPlayer = PlayerX;
//     }

//     public void DisplayBoard(){
//         for(int i=0;i<3;i++){
//             for(int j=0;j<3;j++){
//                 Console.Write(board[i,j]+" ");
//             }
//             Console.WriteLine();
//         }
//     }

//     public bool MakeMove(int row, int col){
//         if(row >= 0 && row <3 && col >= 0 && col <3 && board[row,col]==empty){
//             board[row,col]=currentPlayer;
//             currentPlayer = currentPlayer == PlayerX ? PlayerO : PlayerX;
//             return true;
//         }
//         return false;
//     }

//     public bool CheckWin(char player){
//         for(int i=0;i<3;i++){
//             if((board[i,0] == player && board[i,1] == player && board[i,2] == player) || 
//             (board[0,i] == player && board[1,i] == player && board[2,i] == player)){
//                 return true;
//             }
//         }
//         return(board[0,0] == player && board[1,1] == player && board[2,2] == player) || 
//         (board[0,2] == player && board[1,1] == player && board[2,0] == player);
//     }

//     public bool checkDraw(){
//         foreach(var cell in board){
//             if(cell == empty){
//                 return false;
//             }
//         }
//         return true;
//     }

//     public char[,] GetBoard(){
//         return board;
//     }
//     }

//     public class QLearningAgent{
//         private readonly Dictionary<string, double[]> qTable;
//         private readonly double learningRate;
//         private readonly double discountFactor;
//         private readonly double explorationRate;
//         private readonly Random random;

//         public QLearningAgent(double learningRate, double discountFactor, double explorationRate){
//             qTable = new Dictionary<string, double[]>();
//             this.learningRate = learningRate;
//             this.discountFactor = discountFactor;
//             this.explorationRate = explorationRate;
//             random = new Random();
//         }

//         public int ChooseAction(char[,] board){
//             var state = GetState(board);
//             if(!qTable.ContainsKey(state) || random.NextDouble() < explorationRate){
//                 return random.Next(9);
//             }

//             var qValues = qTable[state];
//             double maxQValue = double.MinValue;
//             int action =0;
//             for(int i=0; i<qValues.Length;i++){
//                 if(qValues[i]>maxQValue){
//                     maxQValue = qValues[i];
//                     action = i;
//                 }
//             }
//             return action;
//         }

//         public void UpdateQTable(char[,] board, int action, double reward, char[,] nextBoard){
//             var state = GetState(board);
//             var nextState = GetState(nextBoard);
//             if(!qTable.ContainsKey(state)){
//                 qTable[state] = new double[9];
//             }
//             if(!qTable.ContainsKey(nextState)){
//                 qTable[nextState] = new double[9];
//             }
//             double maxNextQValue = double.MinValue;
//             foreach(var qValues in qTable[nextState]){
//                 if(qValues > maxNextQValue){
//                     maxNextQValue = qValues;
//                 }
//             }
//             qTable[state][action] = (1-learningRate) * qTable[state][action] + learningRate * (reward + discountFactor * maxNextQValue);

//         }

//         private string GetState(char[,] board){
//             char[] state = new char[9];
//             int index =0;
//             for(int i=0;i<3;i++){
//                 for(int j=0;j<3;j++){
//                     state[index++] = board[i,j];
//                 }
//             }
//             return new string(state);
//         }
//     }

//     class Program{
//         static void Main(string[] args){
//             var env = new TicTacToeEnv();
//             var agent = new QLearningAgent(learningRate:0.1, discountFactor:0.9, explorationRate:0.1);

//             for(int episode =0; episode<10000;episode++){
//                 env = new TicTacToeEnv();
//                 char winner = '-';
//                 while(winner =='-'){
//                     var board = env.GetBoard();
//                     int action = agent.ChooseAction(board);
//                     int row = action/3;
//                     int col = action%3;
//                     if(env.MakeMove(row,col)){
//                         var nextBoard = env.GetBoard();
//                         if(env.CheckWin('X')){
//                             agent.UpdateQTable(board,action,1,nextBoard);
//                             winner = 'X';
//                         }else if(env.checkDraw()){
//                             agent.UpdateQTable(board, action, 0, nextBoard);
//                             winner = 'D';
//                         }else{
//                             while(true){
//                                 int opponentAction = new Random().Next(9);
//                                 int opponentRow = opponentAction/3;
//                                 int opponentCol = opponentAction%3;
//                                 if(env.MakeMove(opponentRow, opponentCol)){
//                                     break;
//                                 }
//                             }
//                             nextBoard = env.GetBoard();
//                             if(env.CheckWin('0')){
//                                 agent.UpdateQTable(board, action, -1, nextBoard);
//                                 winner = 'O';
//                             }else if(env.checkDraw()){
//                                 agent.UpdateQTable(board, action, 0, nextBoard);
//                                 winner = 'D';
//                             }else{
//                                 agent.UpdateQTable(board, action, 0, nextBoard);
//                             }
//                         }
//                     }
//                 }
//             }
//             var testEnv = new TicTacToeEnv();
//             while(true){
//                 testEnv.DisplayBoard();
//                 var testBoard = testEnv.GetBoard();
//                 int testAction = agent.ChooseAction(testBoard);
//                 int testRow = testAction/3;
//                 int testCol = testAction%3;
//                 testEnv.MakeMove(testRow, testCol);
//                 if(testEnv.CheckWin('X') || testEnv.checkDraw()){
//                     testEnv.DisplayBoard();
//                     break;
//                 }
//             }
//             while(true){
//                 int opponentAction = new Random().Next(9);
//                 int opponentRow = opponentAction/3;
//                 int opponentCol = opponentAction%3;
//                 if(testEnv.MakeMove(opponentRow, opponentCol)){
//                     break;
//                 }   
//                  if(testEnv.CheckWin('O') || testEnv.checkDraw()){
//                     testEnv.DisplayBoard();
//                     break;
//                 }                          
//             }
//         }
//     }
// }
using System;
using System.Collections.Generic;

namespace TicTacToe
{
    public class TicTacToeEnv
    {
        private readonly char[,] board;
        private const char Empty = '-';
        private const char PlayerX = 'X';
        private const char PlayerO = 'O';
        private char currentPlayer;

        public TicTacToeEnv()
        {
            board = new char[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    board[i, j] = Empty;
                }
            }
            currentPlayer = PlayerX;
        }

        public void DisplayBoard()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Console.Write(board[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        public bool MakeMove(int row, int col)
        {
            if (row >= 0 && row < 3 && col >= 0 && col < 3 && board[row, col] == Empty)
            {
                board[row, col] = currentPlayer;
                Console.WriteLine($"Turn {GetMoveCount()}: {currentPlayer} plays at ({row}, {col})");
                currentPlayer = currentPlayer == PlayerX ? PlayerO : PlayerX;
                return true;
            }
            return false;
        }

        public bool CheckWin(char player)
        {
            for (int i = 0; i < 3; i++)
            {
                if ((board[i, 0] == player && board[i, 1] == player && board[i, 2] == player) ||
                    (board[0, i] == player && board[1, i] == player && board[2, i] == player))
                {
                    return true;
                }
            }
            return (board[0, 0] == player && board[1, 1] == player && board[2, 2] == player) ||
                   (board[0, 2] == player && board[1, 1] == player && board[2, 0] == player);
        }

        public bool CheckDraw()
        {
            foreach (var cell in board)
            {
                if (cell == Empty)
                {
                    return false;
                }
            }
            return true;
        }

        public char[,] GetBoard()
        {
            return board;
        }

        private int GetMoveCount()
        {
            int count = 0;
            foreach (var cell in board)
            {
                if (cell != Empty)
                    count++;
            }
            return count;
        }
    }

    public class QLearningAgent
    {
        private readonly Dictionary<string, double[]> qTable;
        private readonly double learningRate;
        private readonly double discountFactor;
        private readonly double explorationRate;
        private readonly Random random;

        public QLearningAgent(double learningRate, double discountFactor, double explorationRate)
        {
            qTable = new Dictionary<string, double[]>();
            this.learningRate = learningRate;
            this.discountFactor = discountFactor;
            this.explorationRate = explorationRate;
            random = new Random();
        }

        public int ChooseAction(char[,] board)
        {
            var state = GetState(board);
            List<int> availableActions = new List<int>();
            
            for (int i = 0; i < 9; i++)
            {
                int row = i / 3;
                int col = i % 3;
                if (board[row, col] == '-')
                {
                    availableActions.Add(i);
                }
            }

            if (!qTable.ContainsKey(state) || random.NextDouble() < explorationRate)
            {
                return availableActions[random.Next(availableActions.Count)];
            }

            var qValues = qTable[state];
            int bestAction = availableActions[0];
            double maxQValue = double.MinValue;

            foreach (int action in availableActions)
            {
                if (qValues[action] > maxQValue)
                {
                    maxQValue = qValues[action];
                    bestAction = action;
                }
            }
            return bestAction;
        }

        public void UpdateQTable(char[,] board, int action, double reward, char[,] nextBoard)
        {
            var state = GetState(board);
            var nextState = GetState(nextBoard);

            if (!qTable.ContainsKey(state)) qTable[state] = new double[9];
            if (!qTable.ContainsKey(nextState)) qTable[nextState] = new double[9];

            double maxNextQValue = double.MinValue;
            foreach (var q in qTable[nextState])
            {
                if (q > maxNextQValue) maxNextQValue = q;
            }
            qTable[state][action] = (1 - learningRate) * qTable[state][action] + learningRate * (reward + discountFactor * maxNextQValue);
        }

        private string GetState(char[,] board)
        {
            char[] state = new char[9];
            int index = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    state[index++] = board[i, j];
                }
            }
            return new string(state);
        }
    }

    class Program
    {
        static void Main()
        {
            var env = new TicTacToeEnv();
            var agent = new QLearningAgent(0.1, 0.9, 0.1);

            for (int episode = 0; episode < 10000; episode++)
            {
                env = new TicTacToeEnv();
                char winner = '-';

                while (winner == '-')
                {
                    env.DisplayBoard();
                    var board = env.GetBoard();
                    int action = agent.ChooseAction(board);
                    int row = action / 3, col = action % 3;
                    
                    if (env.MakeMove(row, col))
                    {
                        var nextBoard = env.GetBoard();
                        if (env.CheckWin('X')) 
                        {
                            winner = 'X';
                            Console.WriteLine("Player X Wins! 🎉");
                            env.DisplayBoard();
                        }
                        else if (env.CheckWin('O'))
                        {
                            winner = 'O';
                            Console.WriteLine("Player O Wins! 🎉");
                            env.DisplayBoard();
                        }
                        else if (env.CheckDraw()) 
                        {
                            winner = 'D';
                            Console.WriteLine("Game is a Draw! 🤝");
                            env.DisplayBoard();
                        }
                    }
                }
            }
        }
    }
}
