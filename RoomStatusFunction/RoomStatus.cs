﻿using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.Azure.WebJobs.Host;
using System.Threading.Tasks;
using RoomStatusFunction.Processors;

namespace RoomStatusFunction
{
    public class RoomStatus
    {
        public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
        {
            log.Info($"C# HTTP trigger function processed a request. RequestUri={req.RequestUri}");

            // parse query parameter
            string spaceid = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "spaceid", true) == 0)
                .Value;
            // Get request body
            dynamic data = await req.Content.ReadAsAsync<object>();
            // Set name to query string or body data
            spaceid = spaceid ?? data?.body;
            if (spaceid == null)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a name on the query string or in the request body");
            }
            else
            {
                var value = RoomStatusProcessor.GetStatus(spaceid);
                return req.CreateResponse(HttpStatusCode.OK, value.ToString());
            }                        
        }
    }
}