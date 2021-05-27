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
using System.Dynamic;

namespace CHRS_V1
{
    public class Integration
    {
        HttpClient client = new HttpClient();
        string staff_api_key = ConfigurationManager.AppSettings["X-STAFF-API"];
        string app_api_key = ConfigurationManager.AppSettings["X-APP-API"];
        LogCreation objLogFile = null;

        public async Task<string> Run()
        {
            //try
            //{
            objLogFile = new LogCreation();
            client.DefaultRequestHeaders.Add("X-STAFF-API", staff_api_key);
            client.DefaultRequestHeaders.Add("X-APP-API", app_api_key);
            string JobID = await this.GetJobID();    
            objLogFile.writeToLogFile("JobID Retrieved: " + JobID);

            string status = string.Empty;
            while (status != "COMPLETED")
            {
                status = await this.GetPollStatus(JobID);
            }
            string LeaveData = await GetBatchData(JobID);
            await DeleteBatchData(JobID);
            return LeaveData;
            // }
            // catch (Exception ex)
            //{

            // objLogFile.writeToLogFile("Error:" + ex.Message);

            // }
            //return "Error";
            //TransformJson(LeaveData);

        }

        public async Task<string> GetJobID() {
            
            string CHRS_BatchSubmit = ConfigurationManager.AppSettings["CHRS_BatchSubmit"];
            var content = new StringContent(string.Empty, Encoding.UTF8, "application/json");
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            var result = client.PutAsync(CHRS_BatchSubmit, content).Result;
            var stream = result.Content.ReadAsStreamAsync().Result;
            string JobID= this.GetResponseData(this.GetReponse(stream));
            Console.WriteLine("Job ID Retrieved: " + JobID);
            objLogFile.writeToLogFile("Request batch Submit. Job ID Retrieved: " + JobID);
            return JobID;
        }

        public async Task<string> GetPollStatus(string jobID)
        {
            string CHRS_BatchSubmitCheckStatus = ConfigurationManager.AppSettings["CHRS_BatchSubmitCheckStatus"];
            CHRS_BatchSubmitCheckStatus += jobID;
            var content = new StringContent(string.Empty, Encoding.UTF8, "application/json");
            var result = client.PutAsync(CHRS_BatchSubmitCheckStatus, content).Result;
            var stream = result.Content.ReadAsStreamAsync().Result;
            string status = GetResponseData(GetReponse(stream));
            objLogFile.writeToLogFile("Batch Poll Status: " + status);
            return status;
        }

        public async Task<string> GetBatchData(string jobID)
        {
            string CHRS_BatchGetData = ConfigurationManager.AppSettings["CHRS_BatchGetData"];
            CHRS_BatchGetData += jobID;
            var content = new StringContent(string.Empty, Encoding.UTF8, "application/json");
            var result = client.PutAsync(CHRS_BatchGetData, content).Result;
            var stream = result.Content.ReadAsStreamAsync().Result;
            string data = (GetReponse(stream));
            objLogFile.writeToLogFile("Batch Poll StatusLeave Data Retirived: " + data);
            return data;
        }

        public async Task DeleteBatchData(string jobID)
        {
            string CHRS_BatchGetData = ConfigurationManager.AppSettings["CHRS_BatchDelete"];
            CHRS_BatchGetData += jobID;
            var content = new StringContent(string.Empty, Encoding.UTF8, "application/json");
            var result = client.PutAsync(CHRS_BatchGetData, content).Result;
            var stream = result.Content.ReadAsStreamAsync().Result;
            string data = (GetReponse(stream));
            objLogFile.writeToLogFile("Dalete Batch Job.");
           
        }
        public string GetReponse(Stream stream)
        {
            string currentLine = "";
            using (var reader = new StreamReader(stream))
            {

                while (!reader.EndOfStream)
                {
                    currentLine = reader.ReadLine();
                }
            }
            return currentLine;
        }
        public string GetResponseData(string inputPayload)
        {
            JObject obj = JObject.Parse(inputPayload);
            Console.WriteLine(obj["data"]);
            return obj["data"].ToString();
        }

        public string GetResponseMessage(string inputPayload)
        {
            JObject obj = JObject.Parse(inputPayload);
            Console.WriteLine(obj["data"]);
            return obj["msg"].ToString() + "," + obj["data"].ToString();
        }

        /// <summary
        ///  nm.,/l,m
        /// </summary>
        /// <param name="inputPayload"></param>
        public void TransformJson(string inputPayload)
        {
            objLogFile.writeToLogFile("Json Data string: " + inputPayload);
            DataTable dt = new DataTable();
            dt.Columns.Add("LeaveType_Code", typeof(string));
            dt.Columns.Add("Dept_C", typeof(string));
            dt.Columns.Add("Lv_From_D", typeof(DateTime));
            dt.Columns.Add("Fac_C", typeof(string));
            dt.Columns.Add("Duration", typeof(Decimal));
            dt.Columns.Add("Dept_T", typeof(string));
            dt.Columns.Add("LeaveType_Desc", typeof(string));
            dt.Columns.Add("Lv_End_D", typeof(DateTime));
            dt.Columns.Add("Work_Sch_C", typeof(string));
            dt.Columns.Add("Staff_No", typeof(string));
            dt.Columns.Add("LeaveTime", typeof(string));

            JObject obj = JObject.Parse(inputPayload);
            JArray array = (JArray)obj["data"];

            foreach (var result in array)
            {
                var WorkScheduleCode = result["JobInfo"][0][0]["WorkScheduleCode"];
                var Dept_C = result["SAPDeptCode"];
                var Fac_C = result["SAPFacCode"];
                var Dept_T = result["SapDept"];
                var StaffNo = result["StaffNo"];
                if (result["LeaveInfo"] != null)
                {
                    foreach (JObject config in result["LeaveInfo"])
                    {
                            dt.Rows.Add(config["LeaveTypeCode"], Dept_C, Convert.ToDateTime(config["StartDate"]).ToShortDateString(), Fac_C, Convert.ToDecimal(config["NumberOfDays"]), Dept_T, config["LeaveType"], Convert.ToDateTime(config["EndDate"]).ToShortDateString(), WorkScheduleCode, StaffNo, config["AmPm"]);                          
                    }
                }

            }

           clsSapData cls = new clsSapData();
           cls.ProcessCustomerData(dt, objLogFile);

        }
    }
}
