using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.IO;
using System.Collections;
using System.Configuration;

namespace CHRS_V1
{
    public class LogCreation
    {
        private string strLogFile;
        private string strLogFolder;
        string dirName = "";//Commented by KP Path.GetDirectoryName(Application.ExecutablePath);

        // property to get and set the course name
        public string StrLogFile
        {
            get
            {
                return strLogFile;
            }  // end get 
            set
            {
                strLogFile = value;
            }  // end set 
        }

        public string StrLogFolder
        {
            get
            {
                strLogFolder = ConfigurationManager.AppSettings["LOG"].ToString();
                return strLogFolder;
            }  // end get 

        }

        public void Init()
        {
            DateTime now = DateTime.Now;
            StrLogFile = ConfigurationManager.AppSettings["LOG_FILENAME"].ToString();
            StrLogFile = (dirName.Length > 0 ? (dirName + "\\" + StrLogFolder + StrLogFile + now.ToString("yyyy") + now.ToString("MM") + now.ToString("dd") + now.ToString("hh") + now.ToString("mm") + now.ToString("ss") + ".txt") : StrLogFolder + StrLogFile + now.ToString("yyyy") + now.ToString("MM") + now.ToString("dd") + now.ToString("hh") + now.ToString("mm") + now.ToString("ss") + ".txt");

        }

        public void writeToLogFile(string logMessage)
        {
            string strLogMessage = string.Empty;
            StreamWriter swLog;

            strLogMessage = string.Format("{0}: {1}", DateTime.Now, logMessage);

            if (string.IsNullOrEmpty(StrLogFile))
            {
                Init();
            }

            if (!Directory.Exists(Path.GetDirectoryName(StrLogFile)))
            {
                return;
            }


            if (!File.Exists(StrLogFile))
            {
                swLog = new StreamWriter(StrLogFile);
            }
            else
            {
                swLog = File.AppendText(StrLogFile);
            }

            swLog.WriteLine(strLogMessage);

            swLog.Close();

        }


    }
}
