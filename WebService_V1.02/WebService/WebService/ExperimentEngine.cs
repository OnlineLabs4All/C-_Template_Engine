/**********************************************************************
 *                                                                    
 *                 ExperimentEngine.cs
 *                         
 **********************************************************************/
/**
 @authors  Thomas Wilhelmer
 @file ExperimentEngine.cs 
 <summary>
 This file holds the ExperimentEngine class. It manages the interaction with
 the RestAPI class.
 </summary>

 <remarks> 
 add detailed description for ExperimentEngine.cs  here...
 @cond INTERNAL
 created by t.wilhelmer, 10.11.2015
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
using Experiment;

namespace WebService
{    
    public class ExperimentEngine
    {
        private static PUTExperimentRequest postExperimentRequest = new PUTExperimentRequest();
        private static GETExperimentResponse expData = new GETExperimentResponse();
        private static DummyExperiment dummyExperiment = new DummyExperiment();

        private static Logger logger = new Logger();
         /**********************************************************************
         *                                                                    *
         *                 checkForNewExperiment()
         *                                                                    *
         **********************************************************************/
        /**
         <summary>
         Sends a request to the server if an experiment is queued.
         </summary>
         <remarks> 
         insert detailed description for checkForNewExperiment here...
         @cond INTERNAL
         documented by t.wilhelmer, 8.11.2015
         @endcond
         </remarks> 
         <param name="restAPI">In: RESTapi object with personalized x-apikey and userID. </param>
         <returns>New status for state machine.  </returns>
        */
        public Status checkForNewExperiment(RestAPI restAPI)
        {
            GETStatusResponse expResponse = new GETStatusResponse();
            try
            {
                expResponse = (GETStatusResponse)restAPI.GETRequest(RequestEndpoint.status);
            }
            catch(Exception e)
            {
                logger.errorLog("An exception occurred in checkForNewExperiment() using apis/engine/experiment/" + RequestEndpoint.status);
                logger.errorLog("Exception message:");
                logger.errorLog(e.Message);
                return Status.EXCEPTION;
            }

            
            if (expResponse.success == true)
            {
                Console.WriteLine("\nExperiment found!");
                logger.log("checkForNewExperiment()");
                logger.log("(" + expResponse.timestamp + ") - Experiment found : ExpId = " + expResponse.expId + "\n");
                return Status.DEQUEUEEXPERIMENT;
            }
            else
            {
                Console.WriteLine("No experiment found!");
                return Status.CHECKFOREXPERIMENT;
            }
        } 

        
        /**********************************************************************
         *                                                                    *
         *                 dequeueExperiment()
         *                                                                    *
         **********************************************************************/
        /**
         <summary>
         Sends request to server i order to retrieve experiment specification.
         </summary>
         <remarks>          
         @cond INTERNAL
         documented by t.wilhelmer, 8.11.2015
         @endcond
         </remarks> 
         <param name="restAPI">In: RESTapi object with personalized x-apikey and userID. </param>
         <returns>New status for state machine. </returns>
        */
        public  Status dequeueExperiment(RestAPI restAPI)
        {
            try
            {
                expData = (GETExperimentResponse)restAPI.GETRequest(RequestEndpoint.experiment);
            }
            catch(Exception e)
            {
                logger.errorLog("An exception occurred in dequeueExperiment() using apis/engine/experiment/" + RequestEndpoint.experiment);
                logger.errorLog("Exception message:");
                logger.errorLog(e.Message);
                return Status.EXCEPTION;
            }

            logger.log("dequeueExperiment()");
            logger.log("(" + expData.timestamp + ") - " + expData.message + "\n");

            if (expData.success == true)
                return Status.PERFORMEXPERIMENT;
            else
            {
                errorReport = postExperimentRequest.errorReport;
                return Status.ERROR;
            }
        }

        
        /**********************************************************************
         *                                                                    *
         *                 returnData()
         *                                                                    *
         **********************************************************************/
        /**
         <summary>
         Sends experiment data to the server.
         </summary>
         <remarks> 
         insert detailed description for returnData here...
         @cond INTERNAL
         documented by t.wilhelmer, 8.11.2015
         @endcond
         </remarks> 
         <param name="restAPI">In: RESTapi object with personalized x-apikey and userID </param>
         <returns>New status for state machine. </returns>
        */
        public Status returnData(RestAPI restAPI)
        {
            POSTExperimentResponse postResponse = new POSTExperimentResponse();
            
            string data =   "{ \"success\":" + postExperimentRequest.success + "," 
                            + "\"results\":" + postExperimentRequest.results
                            + "\"rerrorReport\":" + postExperimentRequest.errorReport + "}";
            try
            {
                postResponse = (POSTExperimentResponse)restAPI.PUTrequest(RequestEndpoint.experiment, data);
            }
            catch(Exception e)
            {
                logger.errorLog("An exception occurred in returnData() using apis/engine/experiment/" + RequestEndpoint.experiment);
                logger.errorLog("Exception message:");
                logger.errorLog(e.Message);
                return Status.EXCEPTION;
            }

            logger.log("returnData()");
            logger.log("(" + postResponse.timestamp + ") - " + postResponse.message + "\n");

            if (postResponse.success == true)
            {
                Console.WriteLine("Experiment data successfully transmitted to the lab server!\n");
                return Status.CHECKFOREXPERIMENT;
            }
            else
            {
                errorReport = postExperimentRequest.errorReport;
                return Status.ERROR;
            }
        }

        
        /**********************************************************************
         *                                                                    *
         *                 performExperiment()
         *                                                                    *
         **********************************************************************/
        /**
         <summary>
         Here is the interface to the dummy experiment. If a real experiment will be implemented, you
         should pass its date inside this method.
         </summary>
         <remarks> 
         insert detailed description for performExperiment here...
         @cond INTERNAL
         documented by t.wilhelmer, 8.11.2015
         @endcond
         </remarks> 
         <returns>New status for state machine. </returns>
        */
        public Status performExperiment()
        {
            logger.log("performExperiment()");

            try
            {
                postExperimentRequest = (PUTExperimentRequest)dummyExperiment.performExperiment<GETExperimentResponse>(/*expData*/);
            }
            catch (Exception e)
            {
                logger.errorLog("An exception occurred in performExperiment()");
                logger.errorLog("Exception message:");
                logger.errorLog(e.Message);
                return Status.EXCEPTION;
            }

            logger.log("Experiment performed: " + postExperimentRequest.success);
            logger.log("Experiment report: " + postExperimentRequest.errorReport + "\n");

            if (postExperimentRequest.success == true)
                return Status.RETURNDATA;
            else
            {
                errorReport = postExperimentRequest.errorReport;
                return Status.ERROR;
            }
        }




        /**********************************************************************
         *                                                                    *
         *                 abortExperiment()
         *                                                                    *
         **********************************************************************/
        /**
         <summary>
         This method gets called if a success parameter is "false". It forwards
         the error report to server.
         </summary>
         <remarks> 
         insert detailed description for abortExperiment here...
         @cond INTERNAL
         documented by t.wilhelmer, 8.11.2015
         @endcond
         </remarks> 
         <param name="restAPI">In: RESTapi object with personalized x-apikey and userID </param>        
         <param name="error">In: error report of concerning request/response </param>
         <returns>New status for state machine. </returns>
        */
        public Status abortExperiment(RestAPI restAPI, string error)
        {
            POSTExperimentResponse postResponse = new POSTExperimentResponse();

            string data = "{ \"success\": false,"
                            + "\"results\": \"An error occurred.\","
                            + "\"rerrorReport\":"  + error + "}";
            try
            {
                postResponse = (POSTExperimentResponse)restAPI.PUTrequest(RequestEndpoint.experiment, data);
            }
            catch (Exception e)
            {
                logger.errorLog("An exception occurred in abortExperiment() using apis/engine/experiment/" + RequestEndpoint.experiment);
                logger.errorLog("Exception message:");
                logger.errorLog(e.Message);
                return Status.EXCEPTION;
            }

            return Status.CHECKFOREXPERIMENT;
        }

        public string errorReport{get;set;}

        //private static Status releaseEngine(RestAPI client)
        //{
        //    client.releaseEngine(Requests.release);
        //    return Status.CHECKFOREXPERIMENT;
        //}

    }
    
}


/**
@mainpage Overview
V1.02
 
@par
This an experiment engine coded in c#. It is implemented as a state machine.
The machine has following states:
    - CHECKFOREXPERIMENT
    - DEQUEUEEXPERIMENT
    - PERFORMEXPERIMENT
    - RETURNDATA
    - RELEASEENGINE
    - ERROR
    - EXCEPTION

@par
The normal operation steps are logged in EvaluationLog.txt.
In case of an exception, more details can be found in the ErrorLog.txt.

@par
CHECKFOREXPERIMENT:
The initial state. It sends a request to server in order to see if there is an experiment in the queue.
If so, the new status will be DEQUEUEEXPERIMENT. In case there is no experiment, it keeps on polling.
The polling will only get aborted if an exception rises.

@par
DEQUEUEEXPERIMENT:
This state retrieves the experiment specification from the server and stores the data for the further
usage in the dummy experiment.
  
@par
PERFORMEXPERIMENT:
Here is the interface for the dummy experiment or the "real experiment". The gained data from the former
state gets passed to the experiment and results are added to the concerning experiment request class. 

@par
RETURNDATA:
This state forms a request for the server. The data for this request comes form the experiment request class
which was gained in the former state.
 
@par
RELEASEENGINE:
------ not used yet ------
 
@par
ERROR:
The error states becomes active if the "success" parameter of a response is "false". If this state occurs, the
state will be set back to CHECKFOREXPERIMENT.
 
@par
EXCEPTION:
If an exception occurs, this state will abort the polling and writes a info messages in the console.
More details can be found in the ErrorLog.txt.
*/