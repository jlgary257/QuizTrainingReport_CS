using Newtonsoft.Json.Linq;
using SurveyMonkey.Containers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
    }
}
