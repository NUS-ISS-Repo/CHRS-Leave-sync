using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Web.Script.Serialization;
using System.Data;
using System.Net.Http.Headers;
using System.Web.Helpers;
using Json.Net;

namespace CHRS_V1
{
    class Program
    {
        static void Main(string[] args)
        {
            string uri = ConfigurationManager.AppSettings["CHRS_Conn"];
            string staff_api_key = ConfigurationManager.AppSettings["X-STAFF-API"];
            string app_api_key = ConfigurationManager.AppSettings["X-APP-API"];
            //var json = @"{ "params": [ { "field": "staff_n", "operator": "in", "value": ["100164"] } ] }"";

            // string str = "{ 'context_name': { 'lower_bound': 'value', 'pper_bound': 'value', 'values': [ 'value1', 'valueN' ] } }";
            string x = "{ \"params\": [ { \"field\": \"staff_n\", \"operator\": \"in\", \"value\": [\"1022\"] } ] }";
            JavaScriptSerializer j = new JavaScriptSerializer();
            var json = j.Deserialize(x, typeof(object));
            // JavaScriptSerializer j = new JavaScriptSerializer();
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-STAFF-API", staff_api_key);
            client.DefaultRequestHeaders.Add("X-APP-API", app_api_key);
            var content = new StringContent(x, Encoding.UTF8, "application/json");
            var result = client.PutAsync(uri, content).Result;
            string status = result.StatusCode.ToString();
            var stream = result.Content.ReadAsStreamAsync().Result;
            string currentLine = "";
            using (var reader = new StreamReader(stream))
            {

                while (!reader.EndOfStream)
                {
                    currentLine = reader.ReadLine();

                    // var json2 = jss.DeserializeObject(currentLine);

                    //string st2 = json2["responseMessage"];
                    //object[] parameters = new Object[3];
                    //parameters[0] = EnrolmentID;
                    //parameters[1] = st1;
                    //parameters[2] = st2;
                    // UpdateEnrolmentStatus(parameters);
                }
            }
            //var root = new Root();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            //Response oRootObject = new Response();
            // root = jss.DeserializeObject<Root>(currentLine);
            //Root json1 = (Root)jss.Deserialize(currentLine, typeof(Root));
            dynamic ter = jss.DeserializeObject(currentLine);
            //string st1 = json2["responseCode"];
            //var jobject = JsonConvert.DeserializeObject<RootObject>(jsonstring);

            //foreach (var item in ter)
            //{

            //    Console.WriteLine(item);
            //    // Console.Write();


            //}
            TransformJson(currentLine);

            //var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(currentLine);
            //foreach (var kv in dict)
            //{
            //    Console.WriteLine(kv.Key + ":" + kv.Value);
            //}

        }

        /// <summary>
        ///  nm.,/l,m
        /// </summary>
        /// <param name="inputPayload"></param>
        static void TransformJson(string inputPayload)
        {
            DateTime CurrTime = DateTime.Now;
            clsSapData s;
            s = new clsSapData();
            DataTable dt;

            JObject obj = JObject.Parse(inputPayload);
            ////var manipulatedObj = obj.DeepClone();
            JArray array = (JArray)obj["data"];
            //Console.WriteLine(array.Count);
            //foreach (var item in array.Children())
            //{            
            //    var itemProperties = item.Children<JProperty>(); 
            //    //you could do a foreach or a linq here depending on what you need to do exactly with the value
            //    var myElement = itemProperties.FirstOrDefault(x => x.Name == "LeaveInfo");
            //}

            // string json = "[{\"id\":1,\"name\":\"Section1\",\"project_id\":100,\"configs\":[{\"id\":1000,\"name\":\"myItem1\",\"group_id\":1}]},{\"id\":2,\"name\":\"Section2\",\"project_id\":100,\"configs\":[{\"id\":1001,\"name\":\"myItem2\",\"group_id\":2},{\"id\":1002,\"name\":\"myItem3\",\"group_id\":2},{\"id\":1003,\"name\":\"myItem4\",\"group_id\":2}]},{\"id\":3,\"name\":\"Section3\",\"project_id\":100,\"configs\":[{\"id\":1004,\"name\":\"myItem5\",\"group_id\":5},]}]";
            //JArray obj = Newtonsoft.Json.JsonConvert.DeserializeObject<JArray>(inputPayload);
            foreach (var result in array)
            {
                Console.WriteLine(result.ToString());
              
                if (result["LeaveInfo"] != null)
                {
                    foreach (JObject config in result["LeaveInfo"])
                    {
                        Console.WriteLine(config["StartDate"]);
                         Console.WriteLine("-----------------------");
                    }
                }
               
            }


        }
    }
}
