using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


namespace sapleave
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class MainSAP
	{
		static void Main(string[] args)

		{
            DateTime CurrTime = DateTime.Now;
            clsSapData s;
            s = new clsSapData();
            DataTable dt;
            string connstring = ConfigurationManager.AppSettings["SAP_Conn"].ToString();


            // Put user code to initialize the page here
            // build the connection string
            //string connstring="ASHOST=owl.intranet.nus.edu.sg SYSNR=0 CLIENT=130 USER=ISS_TMRFC PASSWD=ohr54321";

            // construct the proxy with connection string
            SAPProxy1 proxy=new SAPProxy1(connstring);

            // create a SAP Table variable
            try
            {
                ZBAPI_EMP_ABSATTTable tbl = new ZBAPI_EMP_ABSATTTable();


               // call the RFM method on the proxy
               BAPIRETURN bapiret;


                CurrTime = CurrTime.AddDays(-90);
                proxy.Z_Bapi_Tmdept_Absatt("99991231", "20081231", out bapiret, ref tbl);
                dt = tbl.ToADODataTable();
                s.ProcessCustomerData(dt);
            }
            catch (Exception e)

            {
                Console.WriteLine(e.Message.ToString());
                throw new System.Exception(e.Message.ToString());

            }
			
		}

		
	}




		



}
