/**********************************************************************
 *                                                                    
 *                 RestAPI.cs
 *                         
 **********************************************************************/
/**
 @authors  Thomas Wilhelmer
 @file RestAPI.cs 
 <summary>
 This file holds the methods for interacting with the lab server.
 </summary>

 <remarks> 
 add detailed description for RestAPI.cs  here...
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
using System.Net;
using System.IO;
using System.Runtime.Serialization.Json;
using WebServiceDataClasses;


namespace WebService
{
    public enum RequestStyle
    {
        GET,
        POST,
        PUT,
        DELETE
    }

    public class RestAPI
    {
        public RequestStyle Method  { get; set; }
        public string ContentType { get; set; }
        public string PostData  { get; set; }
        private string xapikey;
        private string userID;
        private static readonly string baseURL = "http://dispatcher.onlinelabs4all.org/apis/engine/";
    
        
        /**********************************************************************
         *                                                                    *
         *                 RestAPI()
         *                                                                    *
         **********************************************************************/
        /**
         <summary>
         Constructor of the RestAPI class. X-apikey and userID are passed in case
         the program has to handle multiple users someday.
         </summary>
         <remarks> 
         insert detailed description for RestAPI here...
         @cond INTERNAL
         documented by t.wilhelmer, 8.11.2015
         @endcond
         </remarks> 
         <param name="xapikey">In: X-apikey assigned to the user </param>
         <param name="userID">In: username and password </param>
         <returns> void </returns>
        */
        public RestAPI(string xapikey, string userID)
        {
            this.xapikey = xapikey;
            this.userID = userID;
        }        

        
        /**********************************************************************
         *                                                                    *
         *                 GETRequest()
         *                                                                    *
         **********************************************************************/
        /**
         <summary>
         Performs the GETRequests in order to retrieve information from the lab server.
         </summary>
         <remarks> 
         insert detailed description for GETRequest here...
         @cond INTERNAL
         documented by t.wilhelmer, 8.11.2015
         @endcond
         </remarks> 
         <param name="type">In: Defines if the requeste is for .../experiment or .../status </param>
         <returns>void </returns>
        */
        public Object GETRequest(string type)
        {
            Method = RequestStyle.GET;
            var request = (HttpWebRequest)WebRequest.Create(baseURL + type);            
            request.Method = Method.ToString();
            setAuthorization(ref request);
            request.ContentLength = 0;
            request.ContentType = "text/xml";            

            switch(type)
            { 
                case "status":                   
                    return (GETStatusResponse)getDataFromRequest<GETStatusResponse>(request); 

                case "experiment":
                    return (GETExperimentResponse)getDataFromRequest<GETExperimentResponse>(request);                 

                default: return null;             
           }      
        
        }

        
        /**********************************************************************
         *                                                                    *
         *                 getDataFromRequest()
         *                                                                    *
         **********************************************************************/
        /**
         <summary>
         This method converts the a response from a request into the specified data class. The data
         classes are specified in WebServiceDataClasses.cs.
         </summary>
         <remarks> 
         insert detailed description for getDataFromRequest here...
         @cond INTERNAL
         documented by t.wilhelmer, 8.11.2015
         @endcond
         </remarks> 
         <param name="request">In: description </param>
         <returns> The desired data class. </returns>
        */
        private Object getDataFromRequest<T>(HttpWebRequest request) where T : new()
        {
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    var message = String.Format("Request failed. Received HTTP {0}", response.StatusCode);
                    throw new ApplicationException(message);
                }

                T dataFromServer = new T();
                
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                dataFromServer = (T)serializer.ReadObject(response.GetResponseStream());
                return dataFromServer;
            }
        }


        /**********************************************************************
         *                                                                    *
         *                 PUTrequest()
         *                                                                    *
         **********************************************************************/
        /**
         <summary>
         This method performs the PUTrequest to server.
         </summary>
         <remarks> 
         insert detailed description for POSTrequest here...
         @cond INTERNAL
         documented by t.wilhelmer, 8.11.2015
         @endcond
         </remarks> 
         <param name="type">In: .../experiment or other url endpoints </param>
         <param name="data">In: data which shall be transmitted to the server. JSON syntax!!! </param>
         <returns>Response from the server. </returns>
        */
        public Object PUTrequest(string type, string data)
        {
            Method = RequestStyle.PUT;
            var request = (HttpWebRequest)WebRequest.Create(baseURL + type);
            request.Method = Method.ToString();
            setAuthorization(ref request);
            request.ContentType = "application/json";
            request.Accept = "application/json";
            request.ContentLength = data.Length;            

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(data);
                streamWriter.Flush();
                streamWriter.Close();
            }

            return (POSTExperimentResponse)getDataFromRequest<POSTExperimentResponse>(request);
        }

/*        public Object releaseEngine(string type)
        {
            Method = HttpVerb.PUT;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endPoint + type);
            request.Method = Method.ToString();
            setAuthorization(ref request);

            var httpResponse = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }
            return null;
        }*/

        
        /**********************************************************************
         *                                                                    *
         *                 setAuthorization()
         *                                                                    *
         **********************************************************************/
        /**
         <summary>
         Sets authorization parameters. Gets called with every request.
         </summary>
         <remarks> 
         insert detailed description for setAuthorization here...
         @cond INTERNAL
         documented by t.wilhelmer, 8.11.2015
         @endcond
         </remarks> 
         <param name="request">In: Reference to the actual created request. </param>
         <returns>void</returns>
        */
        private void setAuthorization(ref HttpWebRequest request)
        {
            request.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(userID));//"htl:fhiscool"
            request.Headers["X-apikey"] = xapikey;// "94b5857d1d464cf28191d126e29e32b2";
        }

    }
}

