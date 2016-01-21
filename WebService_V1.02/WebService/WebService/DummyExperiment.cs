/**********************************************************************
 *                                                                    
 *                 dummyexperiment.cs
 *                         
 **********************************************************************/
/**
 @authors  Thomas Wilhelmer
 @file dummyexperiment.cs 
 <summary>
 This file might be exchange with a real experiment.
 </summary>

 <remarks> 
 add detailed description for dummyexperiment.cs  here...
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
using WebServiceDataClasses;
using WebService;

namespace Experiment
{
    class DummyExperiment
    {
        private Logger logger = new Logger();

        
        /**********************************************************************
         *                                                                    *
         *                 performExperiment()
         *                                                                    *
         **********************************************************************/
        /**
         <summary>
         This is the entry point to the experiment. This methods gets called in the 
         state machines state PERFORMEXPERIMENT.
         </summary>
         <remarks> 
         insert detailed description for performExperiment here...
         @cond INTERNAL
         documented by t.wilhelmer, 8.11.2015
         @endcond
         </remarks> 
         <returns> Response from the server. </returns>
        */
        public Object performExperiment<T>(/*T data*/) where T : new()
        {
            Console.WriteLine("Performing dummy experiment.");
            logger.log("------ performing dummy experiment ------");

            string testResults =  @"<?xml version='1.0' encoding='utf-8' standalone='no' ?>
                                    <!DOCTYPE experimentResult SYSTEM 'http://exp01.cti.ac.at/elvis/xml/experimentResult.dtd'>
                                    <experimentResult lab='MIT NI-ELVIS Weblab' specversion='0.1'>
                                    <datavector name='TIME'>0.000000 0.010000 0.020000 0.030000 0.040000 0.050000 0.060000 0.070000 0.080000 0.090000 0.100000 0.110000 0.120000 0.130000 0.140000 0.150000 0.160000 0.170000 0.180000 0.190000 0.200000 0.210000 0.220000 0.230000 0.240000 0.250000 0.260000 0.270000 0.280000 0.290000 0.300000 0.310000 0.320000 0.330000 0.340000 0.350000 0.360000 0.370000 0.380000 0.390000 0.400000 0.410000 0.420000 0.430000 0.440000 0.450000 0.460000 0.470000 0.480000 0.490000 0.500000</datavector>
                                    <datavector name='VOUT'>-5.280159 -5.280159 -3.892919 -0.396190 3.251519 5.276399 5.276399 3.845766 0.337853 -3.302879 -5.280159 -5.280159 -3.820593 -0.300831 3.333028 5.276399 5.276399 3.770699 0.243782 -3.382614 -5.280159 -5.280159 -3.746496 -0.205954 3.411638 5.276399 5.276399 3.694183 0.147778 -3.462671 -5.280159 -5.280159 -3.667083 -0.109306 3.489926 5.276399 5.276399 3.618795 0.053063 -3.539990 -5.280159 -5.280159 -3.589764 -0.013624 3.568375 5.276399 5.276399 3.540346 -0.044713 -3.617148 -5.280159</datavector>
                                    <datavector name='VIN'>1.858303 1.943677 1.279856 0.130865 -1.067571 -1.858796 -1.935792 -1.266344 -0.110595 1.083659 1.867323 1.935300 1.257627 0.098327 -1.095277 -1.868622 -1.928382 -1.242182 -0.079184 1.111526 1.877149 1.925474 1.235076 0.068043 -1.122500 -1.879414 -1.917912 -1.219148 -0.049062 1.140198 1.887298 1.915648 1.209947 0.036955 -1.149400 -1.890851 -1.906636 -1.193697 -0.018296 1.166294 1.899218 1.905178 1.183368 0.007316 -1.175334 -1.901643 -1.897294 -1.166958 0.011504 1.189167 1.911460</datavector>
                                    </experimentResult>";
            
            PUTExperimentRequest testData = new PUTExperimentRequest();
            testData.success = true;
            testData.results = "\"" + testResults + "\",";
            testData.errorReport = "\"" + "No error detected" + "\"";

            return testData;
        }
    }
}
