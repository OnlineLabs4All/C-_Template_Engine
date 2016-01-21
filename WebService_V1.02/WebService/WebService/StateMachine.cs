/**********************************************************************
 *                                                                    
 *                 ExperimentEngine.cs
 *                         
 **********************************************************************/
/**
 @authors  Thomas Wilhelmer
 @file ExperimentEngine.cs 
 <summary>
 This file holds the state machine which uses the experiment engine.
 </summary>

 <remarks> 
 add detailed description for ExperimentEngine.cs  here...
 @cond INTERNAL
 created by t.wilhelmer, 8.11.2015
 @endcond
 </remarks>
*/

using System;
using System.IO;
using System.Text;
using System.Net;
using System.Threading;
using System.Runtime.Serialization.Json;
using WebServiceDataClasses;

namespace WebService
{
    public enum Status
    {
        CHECKFOREXPERIMENT,
        DEQUEUEEXPERIMENT,
        PERFORMEXPERIMENT,
        RETURNDATA,
        RELEASEENGINE,
        ERROR,
        EXCEPTION
    }

    public class StateMachine
    {        
        /**********************************************************************
         *                                                                    *
         *                 Main()
         *                                                                    *
         **********************************************************************/
        /**
         <summary>
         Entry point of the experiment engine.
         </summary>
         <remarks> 
         insert detailed description for Main here...
         @cond INTERNAL
         documented by t.wilhelmer, 8.11.2015
         @endcond
         </remarks> 
         <param name="args">In/Out: description </param>
         <returns> return value description here </returns>
        */
        public static void Main(string[] args)
        {
            ExperimentEngine engine = new ExperimentEngine();
            bool noException = true;
            RestAPI restAPI = new RestAPI(Authorize.xapikey, Authorize.userID);
            Logger logger = new Logger();

            Status status = Status.CHECKFOREXPERIMENT;
            while (noException == true)
            {
                switch (status)
                {
                    case Status.CHECKFOREXPERIMENT:
                        logger.log("Checking for a new experiment");
                        while (status == Status.CHECKFOREXPERIMENT)
                        {
                            Console.WriteLine("Waiting for experiment.");
                            Thread.Sleep(3000);
                            status = engine.checkForNewExperiment(restAPI);
                        }
                        break;
                    case Status.DEQUEUEEXPERIMENT:
                        Console.WriteLine("Dequeue experiment.");
                        status = engine.dequeueExperiment(restAPI);
                        break;

                    case Status.PERFORMEXPERIMENT:
                        Console.WriteLine("Perform experiment.");
                        status = engine.performExperiment();
                        //Entering point for experiment calculations etc.
                        break;

                    case Status.RETURNDATA:
                        Console.WriteLine("Return experiment data.");
                        status = engine.returnData(restAPI);
                        break;

                    //case Status.RELEASEENGINE:
                    //    status = releaseEngine(client);
                    //    break;

                    case Status.ERROR:
                        Console.WriteLine("A \"success\" parameter was returned as \"false\"");
                        logger.log("An error occurred: errorReport:" + engine.errorReport);
                        logger.log("Restarting state machine. \n");
                        logger.errorLog("An internal error occurred. Please see EvaluationLog.txt for more details.");
                        status = engine.abortExperiment(restAPI, engine.errorReport);
                        break;

                    case Status.EXCEPTION:
                        noException = false;
                        Console.WriteLine("An exception occurred. Please check the ErrorLog.txt for more details.");
                        break;

                    default:
                        status = Status.EXCEPTION;
                        logger.errorLog("A undefined state of the state machine has been called.");
                        break;
                }
            }
        }

    }
}
