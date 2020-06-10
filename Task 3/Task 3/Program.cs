using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Threading;
using System.Transactions;
using System.Xml.Serialization;

namespace Task_3
{

    public class FiniteStateTable
    {

        private int State = 0; //Default state is set to 0, but it can be changed by constructor
        private int Event = 0; //Default event is set to 0, but its changes later depending on keyboard input.
        public delegate void Do();
        private struct cell_FST { public int index; public Do[] List_Action; }
        private cell_FST[,] FST;


        //Constructors
        public FiniteStateTable()
        {
            //Not sure what to add in here
        }

        public FiniteStateTable(int Events, int States)
        {
            FST = new cell_FST[Events, States];
        }

        public FiniteStateTable(int Events, int States, int InitialState)
        {
            FST = new cell_FST[Events, States];
            State = InitialState;
        }


        //Methods
        public void SetNextState(int Input_Event, int Input_State, int nextState) { FST[Input_Event, Input_State].index = nextState; }
        public int GetNextState() { return FST[Event, State].index; }  //Gets next state of current celll
        public Do[] GetActions() { return FST[Event, State].List_Action; } //Gets actions of current cell
        public void SetActions(int Input_Event, int Input_State, Do[] Actions_List) { FST[Input_Event, Input_State].List_Action = Actions_List; }
        public void UpdateState(int newState) { this.State = newState; }
        public void UpdateEvent(int newEvent) { this.Event = newEvent; }
        public int GetCurrentState() { return State; }

        /*These methods below are used for testing purposes only
         
        public Do[] GetActions(int Input_Event, int Input_State) { return FST[Input_Event, Input_State].List_Action; } //Gets actions of specified cell
        public int GetNextState(int Input_Event, int Input_State) { return FST[Input_Event, Input_State].index; } //Gets next state of specified cell

        */


    }

    /*        S0             S1               S2  
-------------------------------------------------------
|     {S1,(X,Y)}      {S0,(W)}          {S0,(W)}       | a
|                                                      |    
|  {S0,do_nothing}   {S2,(X,Z)}      {S2,do_nothing}   | b
|                                                      |
|  {S0,do_nothing} {S1,do_nothing}     {S1,(X,Y)}      | c
|                                                      |
 ------------------------------------------------------

 */




    class Program
    {
        static void Main(string[] args)
        {
            FST_functions Func = new FST_functions();

            var S0_a = new FiniteStateTable.Do[2] { Func.X, Func.Y };
            var S0_b = new FiniteStateTable.Do[1] { Func.Do_Nothing };
            var S0_c = new FiniteStateTable.Do[1] { Func.Do_Nothing };
            var S1_a = new FiniteStateTable.Do[1] { Func.W };
            var S1_b = new FiniteStateTable.Do[2] { Func.X, Func.Z };
            var S1_c = new FiniteStateTable.Do[1] { Func.Do_Nothing };
            var S2_a = new FiniteStateTable.Do[1] { Func.W };
            var S2_b = new FiniteStateTable.Do[1] { Func.Do_Nothing };
            var S2_c = new FiniteStateTable.Do[2] { Func.X, Func.Y };

            var FSM_1 = new FiniteStateTable(3, 3, 0);

            //Format -> ( Event,  State , input value)
            FSM_1.SetNextState(0, 0, 1); FSM_1.SetActions(0, 0, S0_a);
            FSM_1.SetNextState(1, 0, 0); FSM_1.SetActions(1, 0, S0_b);
            FSM_1.SetNextState(2, 0, 0); FSM_1.SetActions(2, 0, S0_c);
            FSM_1.SetNextState(0, 1, 0); FSM_1.SetActions(0, 1, S1_a);
            FSM_1.SetNextState(1, 1, 2); FSM_1.SetActions(1, 1, S1_b);
            FSM_1.SetNextState(2, 1, 1); FSM_1.SetActions(2, 1, S1_c);
            FSM_1.SetNextState(0, 2, 0); FSM_1.SetActions(0, 2, S2_a);
            FSM_1.SetNextState(1, 2, 2); FSM_1.SetActions(1, 2, S2_b);
            FSM_1.SetNextState(2, 2, 1); FSM_1.SetActions(2, 2, S2_c);



            Console.WriteLine("Now in state " + FSM_1.GetCurrentState() as String);
            var logData = new List<string>();
            while (true)
            {

                bool ErrorKey = false;

                Console.Write("Press key a,b,c or q: ");
                var key = Console.ReadKey().Key;
                logData.Add(DateTime.Now + " Key: " + key.ToString());
                Console.WriteLine();       // <-- This writes a newline.

                if (key == ConsoleKey.A) { FSM_1.UpdateEvent(0); }
                else if (key == ConsoleKey.B) { FSM_1.UpdateEvent(1); }
                else if (key == ConsoleKey.C) { FSM_1.UpdateEvent(2); }
                else if (key == ConsoleKey.Q) { break; }
                else { ErrorKey = true; }

                if ((FSM_1.GetNextState() != FSM_1.GetCurrentState()) && !ErrorKey)
                {
                    var actions = FSM_1.GetActions();
                    foreach (FiniteStateTable.Do i in actions) { i(); logData.Add(DateTime.Now + " " + Func.GetAction_in_String_Form()); }
                    FSM_1.UpdateState(FSM_1.GetNextState());
                    Console.WriteLine("Now in state S" + FSM_1.GetCurrentState() as String);
                    logData.Add(DateTime.Now + " Now in state S" + FSM_1.GetCurrentState() as String);
                }

                if (ErrorKey) { Console.WriteLine("Please Press a valid Key. Try again."); logData.Add("Invalid Key press"); }
                logData.Add("/n");

                Console.WriteLine();       // <-- This writes a newline.
                Console.WriteLine();       // <-- This writes a newline.

            }

            Console.Write("Input your path: ");
            string FNAME = Console.ReadLine();

            //need to add path validation
            //string FNAME = @"C:\Users\sahil\Documents\University\Year 3\bob.txt";

            if (!File.Exists(FNAME))
            {
                // Create a file to write to.
                string[] createText = { "Welcome to Log of Task 2" };
                File.WriteAllLines(FNAME, createText);
            }
            logData.ForEach(delegate (String i)
            {
                if (i == "/n") { System.IO.File.AppendAllText(FNAME, Environment.NewLine); }
                else { System.IO.File.AppendAllText(FNAME, i + " "); }



            });


            Environment.Exit(0);

        }
    }


    public class FST_functions
    {

        string Func_text = "";
        public void Do_Nothing() { Func_text = "Do Nothing"; }
        public void X() { Console.WriteLine("Action X"); Func_text = "Action X"; }
        public void Y() { Console.WriteLine("Action Y"); Func_text = "Action Y"; }
        public void W() { Console.WriteLine("Action W"); Func_text = "Action W"; }
        public void Z() { Console.WriteLine("Action Z"); Func_text = "Action Z"; }
        public string GetAction_in_String_Form() { return Func_text; }

    }

}




