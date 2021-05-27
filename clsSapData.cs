using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace CHRS_V1
{
    /// <summary>
    /// Summary description for clsSapData.
    /// </summary>
    public class clsSapData
	{
        //OdbcConnection cn;
        //OdbcCommand cmd;
        //OdbcParameter prm;
        //OdbcDataReader dread;

        SqlConnection cn;
        SqlCommand cmd;
        SqlParameter prm;
        SqlDataReader dread;
        LogCreation objLogFileT = null;



        public clsSapData()
		{
            //
            // TODO: Add constructor logic here
            //objLogFile = new LogCreation();
            //

        }
		public void ProcessCustomerData(DataTable dt, LogCreation objLogFile)

		{
            objLogFileT = objLogFile;
            string ls_Server= ConfigurationManager.AppSettings["SQL_SERVER"].ToString();
            string ls_Database = ConfigurationManager.AppSettings["DB_NAME"].ToString();
            string ls_User = ConfigurationManager.AppSettings["USER"].ToString();
            string ls_Pwd = ConfigurationManager.AppSettings["PWD"].ToString();


            cn = new SqlConnection("Data Source=" + ls_Server + ";Initial Catalog=" + ls_Database + ";Persist Security Info=False;Integrated Security=SSPI;");
            try
            {
                Console.WriteLine("Connection Try");
                cn.Open();
                Console.WriteLine("Connection Ok");
                objLogFile.writeToLogFile("Connection Ok");
               

            }

            catch (Exception e)

            {
                Console.WriteLine(e.Message.ToString());
                throw new System.Exception(e.Message.ToString());

            }

            DateTime MyDateTime;
			MyDateTime = new DateTime();

			if (dt.Rows.Count > 0)

			{

				try
				{

					//s=new SQLClass();        
					cmd = new SqlCommand("delete sapbapit", cn);
					dread = cmd.ExecuteReader();
					dread.Close();
                    objLogFile.writeToLogFile("delete sapbapit ");

                    cmd = new SqlCommand("delete sap_leavet", cn);
                    dread = cmd.ExecuteReader();
                    dread.Close();
                    objLogFile.writeToLogFile("delete sap_leavet ");

                }

				catch (Exception e) 

				{
                    Console.WriteLine(e.Message.ToString());
                    throw new System.Exception(e.Message.ToString());


                }
				
				foreach(DataRow dr in dt.Rows)

				{

					//tw.WriteLine("Processing customer: " + dr["Absatt_Tp"].ToString());

					//Console.WriteLine("Processing customer: " + dr["Lv_From_D"].ToString());
                   // Console.WriteLine(DateTime.ParseExact(dr["Lv_From_D"].ToString(), "yyyyMMdd", null));
                    
                    cmd = new SqlCommand("dbo.insertSAPBAPIT", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    prm = cmd.Parameters.Add("@LeaveType_Code", SqlDbType.VarChar, 4);
					prm.Value = dr["LeaveType_Code"];

					prm = cmd.Parameters.Add("@Dept_C", SqlDbType.VarChar, 8);
					prm.Value = dr["Dept_C"];

					prm = cmd.Parameters.Add("@Lv_From_D", SqlDbType.DateTime);
					//MyDateTime = DateTime.ParseExact(dr["Lv_From_D"].ToString(), "yyyyMMdd",null);
					prm.Value = dr["Lv_From_D"].ToString();
					//Console.WriteLine("Processing customer: " + dr["Lv_From_D"].ToString() +"**"+ MyDateTime.ToString());
					prm = cmd.Parameters.Add("@Fac_C", SqlDbType.VarChar, 8);
					prm.Value = dr["Fac_C"];

					prm = cmd.Parameters.Add("@Duration", SqlDbType.Decimal, 10);
					prm.Value = dr["Duration"];
                 

                    prm = cmd.Parameters.Add("@Dept_T", SqlDbType.VarChar, 60);
					prm.Value = dr["Dept_T"];


					prm = cmd.Parameters.Add("@LeaveType_Desc", SqlDbType.VarChar, 50);
					prm.Value = dr["LeaveType_Desc"];
                   

					prm = cmd.Parameters.Add("@Lv_End_D", SqlDbType.DateTime);
					//MyDateTime = DateTime.ParseExact(dr["Lv_End_D"].ToString(), "yyyyMMdd",null);
					prm.Value = dr["Lv_End_D"].ToString();

					prm = cmd.Parameters.Add("@Work_Sch_C", SqlDbType.VarChar, 8);
					prm.Value = dr["Work_Sch_C"];

					prm = cmd.Parameters.Add("@Staff_No", SqlDbType.VarChar, 8);
					prm.Value = dr["Staff_No"];

                    prm = cmd.Parameters.Add("@LeaveTime", SqlDbType.VarChar, 8);
                    prm.Value = dr["LeaveTime"];

                    dread = cmd.ExecuteReader();
					dread.Close();
					


					//Console.WriteLine("Processed customer: " + dr["Absatt_Tp"].ToString());

					//tw.WriteLine("Processed customer: " + dr["NAME1"].ToString());

				}
                objLogFile.writeToLogFile("data inserted ");
                cmd = new SqlCommand("dbo.updateSAP_LeaveT", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                dread = cmd.ExecuteReader();
                cn.Close();
                objLogFile.writeToLogFile("data updated ");

            }


		}


	}

}
