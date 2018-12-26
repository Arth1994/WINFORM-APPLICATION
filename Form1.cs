using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Asg3_axs175430
{
    public partial class FormEvaluator : Form
    {
        //variable for sum of backspaces
        public int sumBackSpaces = 0;

        //declaring list variables for back space counts, record entry times and record save times
        //list for back space counts
        public ArrayList totalCount = new ArrayList();

        //list to keep track of time when record was entered
        public ArrayList recordEntryTimes = new ArrayList();

        //list to keep track of time when record was saved
        public ArrayList recordSaveTime = new ArrayList();

        //declaring sum of times
        public double sumTime = 0.0;

        //list for time taken to enter a single record
        public ArrayList entryTimes = new ArrayList();

        //list to keep track of inter record time
        public ArrayList interTimes = new ArrayList();

        //variables for Average Entry Time and Average  Inter Time
        public double avgEntryTime = 0, avgInterTime = 0;

        //default constructor
        public FormEvaluator()
        {
            InitializeComponent();
            Controls.Add(listView1);

        }

        //form load
        private void Form1_Load(object sender, EventArgs e)
        {
            //adjusting form position when program runs
            expandScreen();
            //Indicates the mode of operation
            toolStripStatusLabel1.Text = "You are in Browse Mode";
        }

        //on clicking label do nothing
        private void label2_Click(object sender, EventArgs e)
        {
            //do nothing 
        }

        //on clicking label do nothing
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //do nothing
        }

        //on clicking label do nothing
        private void label3_Click(object sender, EventArgs e)
        {
            //do nothing
        }

        //When you click the evalute form
        private void EVALUATORPROGRAM_Click(object sender, EventArgs e)
        {
            //do nothing
        }

        //Adjusting the Working Screen Size according to the resolution programmatically
        private void expandScreen()
        {
            int iHeight = Screen.PrimaryScreen.WorkingArea.Height - Height;
            Point pt = listView1.Location;
            int iWidth = Screen.PrimaryScreen.WorkingArea.Width - Width;
            CenterToScreen();

        }
        private void browse_Click(object sender, EventArgs Exception)
        {
            //try - catch block to prevent any invalid input
            try
            {
                //On Clicking File Dialog to select the file
                DialogResult dialogResult = openFileDialog1.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    //Message to the user
                    toolStripStatusLabel1.Text = "Your File Has Been Selected";
                    textBox1.Text = openFileDialog1.FileName;

                    //stores name of the file stored in the file dialog
                    String nameOfFile = openFileDialog1.FileName;

                    //throws exception on invalid file name
                    try
                    {
                        //reads the selected file
                        StreamReader file = new StreamReader(@nameOfFile);

                        //file is not empty and there is atleast a single line to read
                        while (true)
                        {
                            //scanning file line by line
                            String recordEntry = file.ReadLine();

                            //splits the line according to tab spaces
                            string[] line = recordEntry.Split('\t');

                            //declaring string variables for reading different fields 
                            //from the line
                            string fullName = "", phone = "", email = "", proof = "", startTime = "";

                            fullName = line[0] + " " + line[1] + " " + line[2];
                            phone = line[9];
                            email = line[10];
                            proof = line[11];
                            startTime = line[12];

                            //storing in a string array for further use in ListViewItem instantiation
                            string[] entry = { fullName, phone, email, proof, startTime };

                            //main purpose to initialize listViewItem with string array
                            ListViewItem listViewItem = new ListViewItem(entry);

                            //add row to the listview
                            listView1.Items.Add(listViewItem);

                            //saving record entry time in arraylist
                            recordEntryTimes.Add(line[13]);

                            //adding record save time to the arraylist
                            recordSaveTime.Add(line[14]);

                            //adding backspace counnt to the arraylist
                            int temp = 0;
                            int.TryParse(line[15], out temp);
                            totalCount.Add(temp);
                        }
                    }
                    catch (Exception et)
                    {
                        Debug.WriteLine(et.ToString());
                    }

                    //count total number of backspaces
                    foreach (int item in totalCount)
                    {
                        sumBackSpaces += item;
                    }
                    //toolStripStatusLabel1.Text = sumBackSpaces.ToString();
                }
            }
            catch (Exception e)
            {
                //catching and writing exception
                Debug.WriteLine(e.ToString());
                //Error Message
                toolStripStatusLabel1.Text = "Invalid File";
            }
        }

        //when you click evaluate button
        private void evaluate_Click(object sender, EventArgs e)
        {
            //getting time values
            formatAndGetTime();
            //changing mode to evaluation
            toolStripStatusLabel1.Text = "In Evalutation Mode";
            //max value
            double max = int.MinValue;
            //min value
            double min = int.MaxValue;
            //minimum inter record
            double minInterRecord = int.MaxValue;
            //max inter record
            double maxInterRecord = int.MinValue;

            //calculating minimum entry time, maximum entry time
            for (int i = 0; i < totalCount.Count; i++)
            {
                double temp = (double)entryTimes[i];
                if (temp > max)
                {
                    max = temp;
                }
            }

            for (int k = 0; k < totalCount.Count; k++)
            {
                double temp1 = (double)entryTimes[k];
                if (temp1 < min)
                {
                    min = temp1;
                }
            }


            //calculating minimum time interval for entries and maximum time interval for entries
            for (int j = 0; j < totalCount.Count - 1; j++)
            {
                double temp2 = (double)interTimes[j];
                if (temp2 > maxInterRecord)
                {
                    maxInterRecord = temp2;
                }

            }

            for (int l = 0; l < totalCount.Count - 1; l++)
            {
                double temp3 = (double)interTimes[l];
                if (temp3 < minInterRecord)
                {
                    minInterRecord = temp3;
                }
            }

            //coversion to get in desired format
            TimeSpan iMax = TimeSpan.FromSeconds(maxInterRecord);

            TimeSpan iMin = TimeSpan.FromSeconds(minInterRecord);

            TimeSpan minTime = TimeSpan.FromSeconds(min);

            TimeSpan maxTime = TimeSpan.FromSeconds(max);

            TimeSpan avgEnt = TimeSpan.FromSeconds(avgEntryTime);

            TimeSpan avgInt = TimeSpan.FromSeconds(avgInterTime);

            TimeSpan total = TimeSpan.FromSeconds(sumTime);

            StreamWriter streamWriter = new StreamWriter(@"CS6326Asg3.txt");

            streamWriter.Write("Number of records: " + totalCount.Count + "\n");

            streamWriter.Write("Minimum entry time: " + string.Format("{0}:{1}", minTime.Minutes, minTime.Seconds) + "\n");

            streamWriter.Write("Maximum entry time: " + string.Format("{0}:{1}", maxTime.Minutes, maxTime.Seconds) + "\n");

            streamWriter.Write("Average entry time: " + string.Format("{0}:{1}", avgEnt.Minutes, avgEnt.Seconds) + "\n");

            streamWriter.Write("Minimum inter-record time: " + string.Format("{0}:{1}", iMin.Minutes, iMin.Seconds) + "\n");

            streamWriter.Write("Maximum inter-record time: " + string.Format("{0}:{1}", iMax.Minutes, iMax.Seconds) + "\n");

            streamWriter.Write("Average inter-record time: " + string.Format("{0}:{1}", avgInt.Minutes, avgInt.Seconds) + "\n");

            streamWriter.Write("Total time: " + string.Format("{0}:{1}", total.Minutes, total.Seconds) + "\n");

            streamWriter.Write("Backspace count: " + sumBackSpaces + "\n");

            streamWriter.Close();

            //reading values from the file and displaying results
            StreamReader streamReader = new StreamReader(@"CS6326Asg3.txt");
            while (true)
            {
                String line = streamReader.ReadLine();
                if (line == null)
                {
                    break;
                }
                string row = line;
                var listViewItem1 = new ListViewItem(row);
                //displaying in list
                listView2.Items.Add(listViewItem1);
            }
            streamReader.Close();
        }


        //get the time format in the specified
        public void formatAndGetTime()
        {
            //throw new NotImplementedException();
            double sumEntryTime = 0.0, sumInterTime = 0.0;
            for (int i = 0; i < totalCount.Count; i++)
            {
                //Calculating entry time for each record
                TimeSpan entryTime = DateTime.Parse(recordSaveTime[i].ToString()).Subtract(DateTime.Parse(recordEntryTimes[i].ToString()));
                entryTimes.Add(entryTime.TotalSeconds);
            }

            for (int a = 0; a < totalCount.Count - 1; a++)
            {

                TimeSpan interTime = DateTime.Parse(recordEntryTimes[a + 1].ToString()).Subtract(DateTime.Parse(recordSaveTime[a].ToString()));
                interTimes.Add(interTime.TotalSeconds);

            }

            //calculating the sum of entry and inter times
            foreach (double entry in entryTimes)
            {
                sumEntryTime += entry;
            }
            foreach (double var in interTimes)
            {
                sumInterTime += var;
            }

            //calculating the average of entry and inter time
            avgEntryTime = sumEntryTime / totalCount.Count;
            avgInterTime = sumInterTime / ((totalCount.Count - 1));
            TimeSpan totalTimeTaken = DateTime.Parse(recordSaveTime[totalCount.Count - 1].ToString()).Subtract(DateTime.Parse(recordEntryTimes[0].ToString()));
            sumTime = totalTimeTaken.TotalSeconds;
            toolStripStatusLabel1.Text = avgEntryTime.ToString();
        }
    }
}
