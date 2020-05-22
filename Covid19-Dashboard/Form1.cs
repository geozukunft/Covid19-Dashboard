using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.IO.Compression;

namespace Covid19_Dashboard
{
    public partial class Form1 : Form
    {
        
        string remoteUri = "https://info.gesundheitsministerium.at/data/";
        string fileName = "data.zip";
        string dataFolder = @"c:\temp\coviddata";
        public Form1()
        {
            InitializeComponent();
            DownloadData();
            Unzip();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void DownloadData()
        {
            using (var client = new WebClient())
            {

                //Download most up to date data
                try
                {
                    client.DownloadFile(new System.Uri(remoteUri + fileName), @"c:\temp\data.zip");
                }
                catch(WebException e)
                {
                    MessageBox.Show("Beim Updaten ist ein Fehler aufgetreten");
                }
            }
        }

        private void Unzip()
        {
            try
            {
                //Get current directory and unzip files to the temp directoy
                string path = Directory.GetCurrentDirectory();
                if(!Directory.Exists(dataFolder))
                {
                    Directory.CreateDirectory(dataFolder);
                }

                ZipFile.ExtractToDirectory(@"c:\temp\data.zip", dataFolder);
                
            }
            catch(Exception e)
            {
                MessageBox.Show("Beim extrahieren der Daten ist ein Fehler aufgetreten. {0}" + e.ToString());
            }
        }


        private void ReadData()
        {

        }







        private void beendenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        struct TimeData
        {
            public DateTime datatime;

            public int dailyDiseases; //new diseases on this day keep in mind the special scheme that is used for the data in the readme
            public int recoverd; //recoverd 
            public int IBload; //intensive bed load 
            public int IBcap; //intensive bed capacity
            public int NBload; //normal bed load
            public int NBcap; //normal bed capacity
            public int deaths; //total deaths
            public DateTime timestamp; //timestamp of data getting pulled
        }

        struct CurrentData
        {
            //all values display totals

            //Values inside of AllgemeinDaten.csv
            public int current; //cuzrrently Ill
            public int positive; //Tested positive
            public int recoverd; //recoverd 
            public int deadConfirmed; //all deaths that have been confirmed to be caused by covid-19
            public int deadReported; //all deaths that have been reported but there could be some that are not yet confirmed to be caused by covid-19
            public int tested; //people tested
            public int allconfirmedcases; //this value represents all cases including those that intially did not get tested
            public int allNBavailable; //all normalbeds currently available
            public int allIBavailable; //all intensive beds currently available
            public int allNBload; //normal beds currently used
            public int allIBload; //intensive beds currently used
            public int ccnH; //confirmed cases not hospitalized
            public int diseases; //all confirmed diseases
            public DateTime lastUpdate; //last time the data was updated
            public DateTime timestamp; //timestamp of data getting pulled

        }

        struct AgeData
        {
            public string ageGroup; //contains the agegroup
            public int cases; //cases related to the age group
            public int deaths; //deaths related to the age group
            public int deaths_demo; //deaths per 100k population
            public DateTime timestamp; //timestamp of data getting pulled
        }

        struct GenderData
        {
            public string sender;
            public int dist; //Distribiution of cases
            public int death_dist; //Distribiution of deaths
        }

        struct StateData
        {
            public string state; //Name of the state
            public string cases; //Cases in that state
            public int GKZ; //I honestly don't know why this int exsists
            public DateTime timestamp; //timestamp of data getting pulled
        }

        struct CountyData
        {
            public string county; //Name of the county
            public int cases; //cases in that county
            public double cases_demo; //cases per 100k population
            public int GKZ; //I honestly don't know why this int exsists
            public DateTime timestamp;
        }
    }
}
