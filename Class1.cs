using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace sapleave
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class mainSAP
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			//
			// TODO: Add code to start application here
			//
			string connstring="ASHOST=owl.intranet.nus.edu.sg SYSNR=0 CLIENT=130 USER=ISS_TMRFC PASSWD=ohr54321";
			
			// construct the proxy with connection string
			SAPProxy1 proxy=new SAPProxy1(connstring);

			// create a SAP Table variable
			ZBAPI_EMP_ABSATTTable tbl=new ZBAPI_EMP_ABSATTTable();

			// call the RFM method on the proxy
			BAPIRETURN bapiret;
			proxy.Z_Bapi_Tmfac_Absatt("99991231", "19000101", out bapiret, ref tbl);

			DataGrid1.DataSource=tbl;
			DataGrid1.DataBind();
				
		
			}

		

		}
		
	}


}
