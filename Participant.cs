using Newtonsoft.Json.Linq;
using SurveyMonkey.Containers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static QuizTrainingReport.ResultPage;

namespace QuizTrainingReport
{
    public partial class Participant : Form
    {
        List<string> id_row;
        public Participant()
        {
            InitializeComponent();
            id_row = static_id_arr.id_ar;
        }
       
        private void btnAdd_temp_Click(object sender, EventArgs e)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("ID");
            

            foreach (var id in static_id_arr.id_ar)
            {
                DataRow row = dataTable.NewRow();
                row["ID"] = id;
                dataTable.Rows.Add(row);
            }


            dataGridView1.DataSource = dataTable;
        }

        private async void btnParticipant_Click(object sender, EventArgs e)
        {

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "fE6kbS5hk-qpNXBhMQm.uqzy9sXLrbwS804TMyqnNxrARydIp4shkZPIZfjHz.orCrc3g.Qj1JcVmQVTOYHB8BPLGTk9N-uu6IGSXMPrv2XKbTcEPqsTIohaok1un5sv");

            try
            {
                HttpResponseMessage response = await client.GetAsync("https://api.surveymonkey.com/v3/surveys/519696776/responses/bulk");
                string responseBody = await response.Content.ReadAsStringAsync();

                // Print the entire JSON response
                Console.WriteLine(responseBody);

                JObject jsonObj = JObject.Parse(responseBody);
                JArray partResults = (JArray)jsonObj["data"];
                List<string> idList = new List<string>();

                // Print the structure of partResults
                Console.WriteLine(partResults.ToString());

                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("ID");
                dataTable.Columns.Add("Name");
                dataTable.Columns.Add("Email");

                foreach (var result in partResults)
                {
                    DataRow row = dataTable.NewRow();
                    row["ID"] = result["id"];

                    var pages = result["pages"];
                    foreach (var page in pages)
                    {
                        var questions = page["questions"];
                        foreach (var question in questions)
                        {
                            var answers = question["answers"];
                            foreach (var answer in answers)
                            {
                                if (answer["text"] != null && !answer["text"].ToString().Contains("@"))
                                {
                                    row["Name"] = answer["text"];
                                }
                                if (answer["text"] != null && answer["text"].ToString().Contains("@"))
                                {
                                    row["Email"] = answer["text"];
                                }
                            }
                        }
                    }
                   
                    dataTable.Rows.Add(row);

                    idList.Add((string)result["id"]);
                }
                static_id_arr.id_ar = idList;


                dataGridView1.DataSource = dataTable;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General error: {ex.Message}");
            }
        }
    }
}
