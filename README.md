# Finite-State-Machine

Finite State Table:

                                S0             S1               S2  
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
