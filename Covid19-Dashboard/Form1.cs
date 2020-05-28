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
using System.Windows;
using System.Windows.Input;
using System.Collections;

namespace Covid19_Dashboard
{
    public partial class Form1 : Form
    {
        
        string remoteUri = "https://info.gesundheitsministerium.at/data/";
        string fileName = "data.zip";
        string dataFolder = @"c:\temp\coviddata";

        //filenames
        string Epikurve = "Epikurve.csv";
        string GenesenTimeline = "GenesenTimeline.csv";
        string IBAuslastung = "IBAuslastung.csv";
        string IBKapazitaeten = "IBKapazitaeten.csv";
        string NBAuslastung = "NBAuslastung.csv";
        string NBKapazitaeten = "NBKapazitaeten.csv";
        string TodesfaelleTimeline = "TodesfaelleTimeline.csv";
        string AllgemeinDaten = "AllgemeinDaten.csv";
        string Altersverteilung = "Altersverteilung.csv";
        string AltersverteilungTodesfaelle = "AltersverteilungTodesfaelle.csv";
        string AltersverteilungTodesfaelleDemogr = "AltersverteilungTodesfaelleDemogr.csv";
        string Bezirke = "Bezirke.csv";
        string Bundesland = "Bundesland.csv";
        string GenesenTodesFalleBL = "GenesenTodesFalleBL.csv";
        string Geschlechtsverteilung = "Geschlechtsverteilung.csv";
        string VerstorbenGeschlechtsverteilung = "VerstorbenGeschlechtsverteilung.csv";

        List<CurrentData> GeneralDataSet = new List<CurrentData>();

        List<GenderData> GenderDataSet = new List<GenderData>();

        public Form1()
        {
            InitializeComponent();
            DownloadData();
            Unzip();
            ReadData();
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
            ReadCurrentData();
        }


        private void ReadCurrentData()
        {
            ReadAllgemeinDaten();
            MessageBox.Show("Finished");
        }

        private void ReadAllgemeinDaten()
        {
            string fullpath = dataFolder + @"\" + AllgemeinDaten;
            int i = 0;

            foreach(string line in File.ReadLines(fullpath))
            {
                if(i != 0)
                {
                    if(!string.IsNullOrEmpty(line))
                    {
                        string[] splitLine = line.Split(';');
                        try
                        {
                            GeneralDataSet.Add(new CurrentData() { current = Convert.ToInt32(splitLine[0]), positive = Convert.ToInt32(splitLine[1]),
                                recoverd = Convert.ToInt32(splitLine[2]), deadConfirmed = Convert.ToInt32(splitLine[3]), deadReported = Convert.ToInt32(splitLine[4]),
                                tested = Convert.ToInt32(splitLine[5]), allconfirmedcases = Convert.ToInt32(splitLine[6]), allNBavailable = Convert.ToInt32(splitLine[7]),
                                allIBavailable = Convert.ToInt32(splitLine[8]), allNBload = Convert.ToInt32(splitLine[9]), allIBload = Convert.ToInt32(splitLine[10]),
                                ccnH = Convert.ToInt32(splitLine[11]), diseases = Convert.ToInt32(splitLine[12]), lastUpdate = Convert.ToDateTime(splitLine[13]),
                                timestamp = Convert.ToDateTime(splitLine[14])});
                        }
                        catch(Exception ex)
                        {

                        }
                    }
                }
            }
        }

        private void ReadGenderData()
        {
            string fullpath1 = dataFolder + @"\" + Geschlechtsverteilung;
            string fullpath2 = dataFolder + @"\" + VerstorbenGeschlechtsverteilung;

            int i = 0;

            foreach(string line in File.ReadLines(fullpath1))
            {
                if(i != 0)
                {
                    if(!string.IsNullOrEmpty(line))
                    {
                        string[] splitLine = line.Split(';');
                        try
                        {
                            GenderDataSet.Add(new GenderData() {gender = splitLine[0], dist = Convert.ToInt32(splitLine[1]), timestamp = Convert.ToDateTime(splitLine[2])});
                        }
                        catch(Exception ex)
                        {

                        }
                    }
                }
            }

            

            foreach (string line in File.ReadLines(fullpath2))
            {
                if (i != 0)
                {
                    if (!string.IsNullOrEmpty(line))
                    {
                        string[] splitLine = line.Split(';');
                        try
                        {
                            GenderDataSet.Add(new GenderData() { gender = splitLine[0], death_dist = Convert.ToInt32(splitLine[1]), timestamp = Convert.ToDateTime(splitLine[2]) });
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }

        }

        private void beendenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        struct TimeData
        {
            public DateTime datatime;

            public int dailyDiseases { get; set; } //new diseases on this day keep in mind the special scheme that is used for the data in the readme
            public int recoverd { get; set; } //recoverd 
            public int IBload { get; set; } //intensive bed load 
            public int IBcap { get; set; } //intensive bed capacity
            public int NBload { get; set; } //normal bed load
            public int NBcap { get; set; } //normal bed capacity
            public int deaths { get; set; } //total deaths
            public DateTime timestamp { get; set; } //timestamp of data getting pulled
        }
        struct CurrentData
        {
            //all values display totals

            //Values inside of AllgemeinDaten.csv
            public int current { get; set; } //cuzrrently Ill
            public int positive { get; set; } //Tested positive
            public int recoverd { get; set; } //recoverd 
            public int deadConfirmed { get; set; } //all deaths that have been confirmed to be caused by covid-19
            public int deadReported { get; set; } //all deaths that have been reported but there could be some that are not yet confirmed to be caused by covid-19
            public int tested { get; set; } //people tested
            public int allconfirmedcases { get; set; } //this value represents all cases including those that intially did not get tested
            public int allNBavailable { get; set; } //all normalbeds currently available
            public int allIBavailable { get; set; } //all intensive beds currently available
            public int allNBload { get; set; } //normal beds currently used
            public int allIBload { get; set; } //intensive beds currently used
            public int ccnH { get; set; } //confirmed cases not hospitalized
            public int diseases { get; set; } //all confirmed diseases
            public DateTime lastUpdate { get; set; } //last time the data was updated
            public DateTime timestamp { get; set; } //timestamp of data getting pulled

        }

        struct AgeData
        {
            public string ageGroup { get; set; } //contains the agegroup
            public int cases { get; set; } //cases related to the age group
            public int deaths { get; set; } //deaths related to the age group
            public int deaths_demo { get; set; } //deaths per 100k population
            public DateTime timestamp { get; set; } //timestamp of data getting pulled
        }

        struct GenderData
        {
            public string gender { get; set; }
            public int dist { get; set; } //Distribiution of cases
            public int death_dist { get; set; } //Distribiution of deaths
            public DateTime timestamp { get; set; } //timestamp of data getting pulled
        }

        struct StateData
        {
            public string state { get; set; } //Name of the state
            public string cases { get; set; } //Cases in that state
            public int GKZ { get; set; } //I honestly don't know why this int exsists
            public DateTime timestamp { get; set; } //timestamp of data getting pulled
        }

        struct CountyData
        {
            public string county { get; set; } //Name of the county
            public int cases { get; set; } //cases in that county
            public double cases_demo { get; set; } //cases per 100k population
            public int GKZ { get; set; } //I honestly don't know why this int exsists
            public DateTime timestamp { get; set; }
        }
    }
}
