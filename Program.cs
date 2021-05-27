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
        static  void Main(string[] args)
        {
            Console.WriteLine("JobID1");
            Integration inte = new Integration();
            Console.WriteLine("JobID2");
            string LeaveData = inte.Run().Result;
            inte.TransformJson(LeaveData);

        }
    }
}
