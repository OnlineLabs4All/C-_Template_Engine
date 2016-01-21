/**********************************************************************
 *                                                                    
 *                 logger.cs
 *                         
 **********************************************************************/
/**
 @authors  Thomas Wilhelmer
 @file logger.cs 
 <summary>
 This file implements the logging functionality.
 </summary>

 <remarks> 
 add detailed description for logger.cs  here...
 @cond INTERNAL
 created by t.wilhelmer, 8.11.2015
 @endcond
 </remarks>
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebService
{
    public class Logger
    {
        private static bool createOnce = true;
        private static System.IO.StreamWriter errorFile;
        private static System.IO.StreamWriter evaluationLog;


        /**********************************************************************
         *                                                                    *
         *                 Logger()
         *                                                                    *
         **********************************************************************/
        /**
         <summary>
         Constructor.
         </summary>
         <remarks> 
         @cond INTERNAL
         documented by t.wilhelmer, 8.11.2015
         @endcond
         </remarks> 
         <returns>  </returns>
        */
        public Logger()
        {
            if (createOnce == true)
            {
                errorFile = new System.IO.StreamWriter("ErrorLog.txt");
                errorFile.WriteLine("======================================================================");
                errorFile.WriteLine("=                           WebService Error Log                    =");
                errorFile.WriteLine("======================================================================\n");
                errorFile.Close();
                evaluationLog = new System.IO.StreamWriter("EvaluationLog.txt");
                evaluationLog.WriteLine("======================================================================");
                evaluationLog.WriteLine("=                           WebService Log                           =");
                evaluationLog.WriteLine("======================================================================\n");
                evaluationLog.Close();
                createOnce = false;
            }
        }

        /**********************************************************************
         *                                                                    *
         *                 ~Logger()
         *                                                                    *
         **********************************************************************/
        /**
         <summary>
         Destructor. Closes file streams.
         </summary>
         <remarks> 
         insert detailed description for ~Logger here...
         @cond INTERNAL
         documented by t.wilhelmer, 8.11.2015
         @endcond
         </remarks> 
         <returns> return value description here </returns>
        */
        ~Logger()
        {
            errorFile.Close();
        }

        /**********************************************************************
         *                                                                    *
         *                 errorLog()
         *                                                                    *
         **********************************************************************/
        /**
         <summary>
         Writes the message into the error log file.
         </summary>
         <remarks> 
         @cond INTERNAL
         documented by t.wilhelmer, 8.11.2015
         @endcond
         </remarks> 
         <param name="message">In: Error message </param>
         <returns> void </returns>
        */
        public void errorLog(string message)
        {
            errorFile = new System.IO.StreamWriter("ErrorLog.txt", true);
            errorFile.WriteLine(message);
            errorFile.Close();
        }


        /**********************************************************************
         *                                                                    *
         *                 log()
         *                                                                    *
         **********************************************************************/
        /**
         <summary>
         Writes messages to EvaluationLog.txt.
         </summary>
         <remarks> 
         @cond INTERNAL
         documented by t.wilhelmer, 8.11.2015
         @endcond
         </remarks> 
         <param name="message">In: Message for file </param>
         <returns> void </returns>
        */
        public void log(string message)
        {
            evaluationLog = new System.IO.StreamWriter("EvaluationLog.txt", true);
            evaluationLog.WriteLine(message);
            evaluationLog.Close();
        }

    }
        
    
}
