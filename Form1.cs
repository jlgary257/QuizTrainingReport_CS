using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32.SafeHandles;
using Newtonsoft.Json.Linq;
using SurveyMonkey;
using SurveyMonkey.Containers;

namespace QuizTrainingReport
{
    public partial class ResultPage : Form
    {
        public ResultPage()
        {
            InitializeComponent();
        }
        DataTable table = new DataTable();
        
        private void lblTo_Click(object sender, EventArgs e)
        {

        }

        private void ResultPage_Load(object sender, EventArgs e)
        {
            //table.Columns.Add("ID", typeof(int));
            //table.Columns.Add("Name", typeof(String));
            //table.Columns.Add("Age", typeof(int));

            //dataGridView1.DataSource = table;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {

            dataGridView1.SelectAll();
            DataObject cpydata = dataGridView1.GetClipboardContent();
            if (cpydata != null) Clipboard.SetDataObject(cpydata);
            Microsoft.Office.Interop.Excel.Application xlapp = new Microsoft.Office.Interop.Excel.Application();
            xlapp.Visible = true;
            Microsoft.Office.Interop.Excel.Workbook workbook;
            Microsoft.Office.Interop.Excel.Worksheet worksheet;
            object miseddata = System.Reflection.Missing.Value;
            workbook = xlapp.Workbooks.Add(miseddata);
            worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets.get_Item(1);

            // Write column headers to the Excel worksheet
            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                worksheet.Cells[1, i+2] = dataGridView1.Columns[i].HeaderText;
            }

            worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets.get_Item(1);
            Microsoft.Office.Interop.Excel.Range xlr = (Microsoft.Office.Interop.Excel.Range)worksheet.Cells[2, 1];
            xlr.Select();

            worksheet.PasteSpecial(xlr, Type.Missing, Type.Missing, Type.Missing, true);
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "fE6kbS5hk-qpNXBhMQm.uqzy9sXLrbwS804TMyqnNxrARydIp4shkZPIZfjHz.orCrc3g.Qj1JcVmQVTOYHB8BPLGTk9N-uu6IGSXMPrv2XKbTcEPqsTIohaok1un5sv");

            try
            {
                HttpResponseMessage response = await client.GetAsync("https://api.surveymonkey.com/v3/surveys/");
                string responseBody = await response.Content.ReadAsStringAsync();

                // Print the entire JSON response
                Console.WriteLine(responseBody);

                JObject jsonObj = JObject.Parse(responseBody);
                JArray quizResults = (JArray)jsonObj["data"];

                // Print the structure of quizResults
                Console.WriteLine(quizResults.ToString());

                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("ID");
                dataTable.Columns.Add("Title");
                dataTable.Columns.Add("Survey Monkey Link");

                foreach (var result in quizResults)
                {
                    DataRow row = dataTable.NewRow();
                    row["ID"] = result["id"];
                    row["Title"] = result["title"];
                    row["Survey Monkey Link"] = result["href"];
                    dataTable.Rows.Add(row);
                }

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
