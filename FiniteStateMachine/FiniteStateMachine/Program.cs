using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Threading;
using System.Transactions;
using System.Xml.Serialization;
using System.Dynamic;

namespace FiniteStateMachine
{

    public class FiniteStateTable
{

    private int State = 0; //Default state is set to 0, but it can be changed by constructor
    private int Event = 0; //Default event is set to 0, but its changes later depending on keyboard input.
    public delegate void Do();
    private struct cell_FST { public int index; public Do[] List_Action; }   //Each FST cell consists of and index and a list of delagates pointing to the action functions.
    private cell_FST[,] FST;


    //Constructors
    public FiniteStateTable() { }  //Constructor 1: It takes no arguments and is made to prevent errors. Other than that, it doesnt have any functional use.


    public FiniteStateTable(int Events, int States)
    {
        FST = new cell_FST[Events, States];     //Constructor 2: This constructor assumes an intial state of 0.
    }


    public FiniteStateTable(int Events, int States, int InitialState)
    {
        FST = new cell_FST[Events, States];       //Constructor 3: This constructor does allow the user to set an intial state.
        State = InitialState;
    }


    //Methods
    public void SetNextState(int Input_Event, int Input_State, int nextState) { FST[Input_Event, Input_State].index = nextState; } //Sets the index of a particular cell that the user's choice.
    public int GetNextState() { return FST[Event, State].index; }  //Gets next state of current cell
    public Do[] GetActions() { return FST[Event, State].List_Action; } //Gets actions of current cell
    public void SetActions(int Input_Event, int Input_State, Do[] Actions_List) { FST[Input_Event, Input_State].List_Action = Actions_List; }    //Sets actions of a particular cell that the user's choice.


    //Additional methods
    public void UpdateState(int newState) { this.State = newState; }  //Updates the State variable
    public void UpdateEvent(int newEvent) { this.Event = newEvent; }  //Updates the Event variable
    public int GetCurrentState() { return State; }  //Returns the State variable


}



class Program
{
    static void Main(string[] args)
    {


        FST_functions Func = new FST_functions();

        //MACHINE 1

        //Making arrays of action delagates for each cell, with the required corrospoding functions (in accordance with the finite state diagram for machine 1).
        var S0_a = new FiniteStateTable.Do[2] { Func.X, Func.Y };
        var S0_b = new FiniteStateTable.Do[1] { Func.Do_Nothing };
        var S0_c = new FiniteStateTable.Do[1] { Func.Do_Nothing };
        var S1_a = new FiniteStateTable.Do[1] { Func.W };
        var S1_b = new FiniteStateTable.Do[2] { Func.X, Func.Z };
        var S1_c = new FiniteStateTable.Do[1] { Func.Do_Nothing };
        var S2_a = new FiniteStateTable.Do[1] { Func.W };
        var S2_b = new FiniteStateTable.Do[1] { Func.Do_Nothing };
        var S2_c = new FiniteStateTable.Do[2] { Func.X, Func.Y };

        //A 3x3 finite state table called FSM_1 is initialized using Constructor 3
        var FSM_1 = new FiniteStateTable(3, 3, 0);



        //Each cell is assigned and index and actions in accordance with the finite state diagram of machine 1.
        FSM_1.SetNextState(0, 0, 1); FSM_1.SetActions(0, 0, S0_a);
        FSM_1.SetNextState(1, 0, 0); FSM_1.SetActions(1, 0, S0_b);
        FSM_1.SetNextState(2, 0, 0); FSM_1.SetActions(2, 0, S0_c);
        FSM_1.SetNextState(0, 1, 0); FSM_1.SetActions(0, 1, S1_a);
        FSM_1.SetNextState(1, 1, 2); FSM_1.SetActions(1, 1, S1_b);
        FSM_1.SetNextState(2, 1, 1); FSM_1.SetActions(2, 1, S1_c);
        FSM_1.SetNextState(0, 2, 0); FSM_1.SetActions(0, 2, S2_a);
        FSM_1.SetNextState(1, 2, 2); FSM_1.SetActions(1, 2, S2_b);
        FSM_1.SetNextState(2, 2, 1); FSM_1.SetActions(2, 2, S2_c);



        //MACHINE 2

        //Making arrays of action delagates for each cell, with the required corrospoding functions (in accordance with the finite state diagram for machine 2).
        var SA_a = new FiniteStateTable.Do[1] { Func.Do_Nothing };
        var SB_S1 = new FiniteStateTable.Do[3] { Func.J, Func.K, Func.L };
        var SA_S1 = new FiniteStateTable.Do[1] { Func.Do_Nothing };
        var SB_a = new FiniteStateTable.Do[1] { Func.Do_Nothing };
        var FSM_2 = new FiniteStateTable(2, 2, 1);

        //Each cell is assigned and index and actions in accordance with the finite state diagram of machine 2.
        FSM_2.SetNextState(0, 0, 1); FSM_2.SetActions(0, 0, SA_a);
        FSM_2.SetNextState(0, 1, 1); FSM_2.SetActions(0, 1, SB_a);
        FSM_2.SetNextState(1, 0, 0); FSM_2.SetActions(1, 0, SA_S1);
        FSM_2.SetNextState(1, 1, 0); FSM_2.SetActions(1, 1, SB_S1);



        var logData = new List<string>();   // This list stores all the log data which is then transferred to a text file at the end of the program.

        //The code below is printing the intial states of the machines to the user and logging this to the logData variable. Som extra logData.Add() have been used for nice formatting of the text file.
        logData.Add("/n");
        Console.WriteLine("Machine 1 is in state S" + FSM_1.GetCurrentState() as String); logData.Add(DateTime.Now + " Machine 1 is in state S" + FSM_1.GetCurrentState() as String); logData.Add("/n");
        Console.WriteLine("Machine 2 is in state SB"); logData.Add(DateTime.Now + " Machine 2 is in state SB"); logData.Add("/n");
        logData.Add("------------------------------------------------------------------------------------------------------------------------------------------------"); logData.Add("/n");


        while (true)
        {



            bool ErrorKey = false; //Is set to true if the input key presssed is not 'a,b,c or q'


            Console.Write("Press key a,b,c or q: ");
            var key = Console.ReadKey().Key;

            logData.Add(DateTime.Now + " Key: " + key.ToString());
            logData.Add("/n"); //This is used to move to a new line in the text file thats created in the end.
            Console.WriteLine();       // <-- This writes a newline.

            //If else statements checking which key has been pressed by the user to update the event variable accordingly.
            if (key == ConsoleKey.A) { FSM_1.UpdateEvent(0); }
            else if (key == ConsoleKey.B) { FSM_1.UpdateEvent(1); }
            else if (key == ConsoleKey.C) { FSM_1.UpdateEvent(2); }
            else if (key == ConsoleKey.Q) { logData.Add("/n"); break; } //Exits the loop and proceeds to creating the log text file.
            else { ErrorKey = true; }  //If none of the above if statements are satisfied, then its an error key.


            //If an error key is not pressed, perform the associated actions with that cell and update the state.
            //if ((FSM_1.GetNextState() != FSM_1.GetCurrentState()) && !ErrorKey)
            if (!ErrorKey)
            {
                var actions = FSM_1.GetActions();
                foreach (FiniteStateTable.Do i in actions) { i(); logData.Add(DateTime.Now + " " + Func.GetAction_in_String_Form()); logData.Add("/n"); }
                FSM_1.UpdateState(FSM_1.GetNextState());
                Console.WriteLine("Machine 1 is in state S" + FSM_1.GetCurrentState() as String);
                logData.Add(DateTime.Now + " Machine 1 is in state S" + FSM_1.GetCurrentState() as String);
                logData.Add("/n");
            }

            //otherwise tell the user that its an invalid key and loop from the start again.
            else { Console.WriteLine("Please Press a valid Key. Try again."); logData.Add(DateTime.Now + " Please Press a valid Key. Try again."); logData.Add("/n"); }



            Console.WriteLine();       // <-- This writes a newline.

            //If the key input was valid for machine 1, go ahead with the if statement. Otherwise, dont bother running machine 2.
            if (!ErrorKey)
            {

                //MACHINE 2 STUFF
                bool ErrorKey2 = false; //Error key for Machine 2 will be true if the key input is not 'a' or if Machine 1 is not in state S1.

                //Check for what the key input is and update the event variable accoridngly.
                if (key.ToString() == ConsoleKey.A.ToString() && FSM_1.GetCurrentState() != 1) { FSM_2.UpdateEvent(0); }
                else if (FSM_1.GetCurrentState() == 1) { FSM_2.UpdateEvent(1); }
                else { ErrorKey2 = true; } //If none of the above statemnts are satisfied, then the input must be an error key for machine 2.


                //if ((FSM_2.GetNextState() != FSM_2.GetCurrentState()) && !ErrorKey2)
                if (!ErrorKey2)
                {
                    var actions2 = FSM_2.GetActions();
                    Thread[] ThreadList = new Thread[actions2.Length];   //Creating a list of threads.
                    int index = 0;
                    foreach (FiniteStateTable.Do i in actions2)
                    {
                        ThreadStart threadDelegate = new ThreadStart(i);
                        ThreadList[index] = new Thread(threadDelegate);  //Each element of the array is assigned a thread.
                        ThreadList[index].Start(); //Each thread is started. They all run in parallel.
                        index = index + 1;  //Index variable updated.
                    }
                    foreach (var i in ThreadList) { i.Join(); }  // The join() method is used to ensure that all the actions are performed before the program asks the user for another input.
                    logData.Add(DateTime.Now + " Action K"); logData.Add("/n");
                    logData.Add(DateTime.Now + " Action J"); logData.Add("/n");
                    logData.Add(DateTime.Now + " Action L"); logData.Add("/n");
                    FSM_2.UpdateState(FSM_2.GetNextState()); //State is updated.


                }

                //These two lines print the state of the machine and logs tha data.
                if (FSM_2.GetCurrentState() == 0) { Console.WriteLine("Machine 2 is in state SA"); logData.Add(DateTime.Now + " Machine 2 is in state SA"); logData.Add("/n"); }
                else { Console.WriteLine("Machine 2 is in state SB"); logData.Add(DateTime.Now + " Machine 2 is in state SB"); logData.Add("/n"); }

                Console.WriteLine();       // <-- This writes a newline.

            }

            logData.Add("------------------------------------------------------------------------------------------------------------------------------------------------");
            logData.Add("/n");

        }



        string FNAME;  //This variable takes the path that the user inputs.
        FileInfo fi;  //This is used to check if the file already exists.
        while (true)
        {

            //This loop runs till the user confirms that they are happy with the location the file will be saved in. 
            while (true)
            {
                bool valid = true;
                Console.Write("Input your path: ");
                FNAME = Console.ReadLine();
                try { if (FNAME.Substring(FNAME.Length - 3, 3) != "txt") { Console.WriteLine("The file name should have a .txt extentsion."); valid = false; } }  //Ensures that its a text file.
                catch (Exception ex) { Console.WriteLine("File name cannot be smaller than 4 characters."); valid = false; }   //Catches the exception incase the input is smaller than 4 characters.
                if (valid == true)
                {
                    fi = new FileInfo(FNAME);
                    Console.WriteLine("File will be saved on path: " + fi.FullName + " Is this okay? (Enter 'Yes' or 'No': )");  //Displays the path to the user to double check things.
                    var user_input = Console.ReadLine();
                    if (user_input == "Yes" | user_input == "yes") { break; }
                }

            }

            bool isOkToCreateFile = false; // This variable indicated to the following loops if its ok to create a file with this path or not.

            //If a file with this name already exists on the same location, the program offers them a choice to overwrite this file or change the location.
            if (fi.Exists)
            {

                Console.WriteLine("A file with this path already exists. Would you like to overwrite it? (Enter 'Yes' Or 'No'): ");
                var user_input = Console.ReadLine();
                if (user_input == "Yes" | user_input == "yes")
                {
                    fi.Delete();
                    isOkToCreateFile = true;
                }
                else { Console.WriteLine("No problem, lets do this again."); }
            }
            else { isOkToCreateFile = true; }


            //If its ok to create a file, the system tries to make one and add log text to it. In case there are any exceptions, the program will catch them 
            //and print out what the exception was and ask the user to input a path again.
            if (isOkToCreateFile == true)
            {

                try
                {
                    string[] createText = { "Welcome to Log of Task 2" };
                    File.WriteAllLines(FNAME, createText);
                    logData.ForEach(delegate (String i)
                    {
                        if (i == "/n") { System.IO.File.AppendAllText(FNAME, Environment.NewLine); }
                        else { System.IO.File.AppendAllText(FNAME, i + " "); }

                    });
                    break;


                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }




            }


        }

        Environment.Exit(0);

    }





}

//This class is used to collect all the action functions in one location for convinience.
public class FST_functions
{

    string Func_text = "";
    public void Do_Nothing() { Func_text = "Do Nothing"; }
    public void X() { Console.WriteLine("Action X"); Func_text = "Action X"; }
    public void Y() { Console.WriteLine("Action Y"); Func_text = "Action Y"; }
    public void W() { Console.WriteLine("Action W"); Func_text = "Action W"; }
    public void Z() { Console.WriteLine("Action Z"); Func_text = "Action Z"; }
    public void J() { Console.WriteLine("Action J"); Func_text = "Action J"; }
    public void K() { Console.WriteLine("Action K"); Func_text = "Action K"; }
    public void L() { Console.WriteLine("Action L"); Func_text = "Action L"; }
    public string GetAction_in_String_Form() { return Func_text; }

}


/*                                     S0             S1               S2  
                     -------------------------------------------------------
                     |     {S1,(X,Y)}      {S0,(W)}          {S0,(W)}       | a
                     |                                                      |    
                     |  {S0,do_nothing}   {S2,(X,Z)}      {S2,do_nothing}   | b             
                     |                                                      |
                     |  {S0,do_nothing} {S1,do_nothing}     {S1,(X,Y)}      | c
                     |                                                      |
                      ------------------------------------------------------
                                     Finite State Table 1

--------------------------------------------------------------------------------------------------------------

                                          SA             SB                
                            ------------------------------------------
                            |                                         | 
                            |     {SB,do_nothing}     {SB,Do nothing} | a
                            |                                         |
                            |                                         |
                            |     {SA,do_Nothing}    SA,(J,K,L)       | if (S1 == true)
                             -----------------------------------------
                                       Finite State Table 2




*/








}





