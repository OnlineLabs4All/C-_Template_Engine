/**********************************************************************
 *                                                                    
 *                 webservicedataclasses.cs
 *                         
 **********************************************************************/
/**
 @authors  Thomas Wilhelmer
 @file webservicedataclasses.cs 
 <summary>
 This file holds all data classes where the requests and responses can be stored in
 and also the authentications parameters and restAPI endpoints.
 </summary>

 <remarks> 
 add detailed description for webservicedataclasses.cs  here...
 @cond INTERNAL
 created by t.wilhelmer, 8.11.2015
 @endcond
 </remarks>
*/

namespace WebServiceDataClasses
{

    public sealed class RequestEndpoint
    {
        public static readonly string status = "status";
        public static readonly string experiment = "experiment";
        public static readonly string release = "release";
    }

    public sealed class Authorize
    {
        public static readonly string xapikey = "94b5857d1d464cf28191d126e29e32b2";
        public static readonly string userID = "htl:fhiscool";
    }

    //GET URL/apis/engine/status response
    public class GETStatusResponse
    {
        public string timestamp { get; set; }
        public bool success   { get; set; }
        public string expId     { get; set; }
        public string message   { get; set; }
    }

    //GET URL/apis/engine/experiment response
    public class GETExperimentResponse
    {
        public string timestamp { get; set; }
        public bool success     { get; set; }
        public string expId     { get; set; }
        public string jobStatus { get; set; }
        public string expSpecification { get; set; }
        public string message   { get; set; }
    }

    //POST URL/apis/engine/experiment request
    public class PUTExperimentRequest
    {
        public bool success   { get; set; }
        public string results   { get; set; }
        public string errorReport { get; set; }
    }                 

    //POST URL/apis/engine/experiment response
    public class POSTExperimentResponse
    {
        public string timestamp { get; set; }
        public bool success     { get; set; }
        public string expId     { get; set; }
        public string jobStatus { get; set; }
        public string message   { get; set; }
    }
    
}