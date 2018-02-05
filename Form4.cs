using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace WindowsFormsApplication1
{
    public partial class Form4 : Form
    {
        public Button selectedButton;

        public Form4()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            string connectionStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|" + @"\db.mdb";
            string commandStr = "select menu, price from food where button = \"" + selectedButton.Name + "\"";

            // 엑세스 파일 연결;
            OleDbDataAdapter DBAdapter = new OleDbDataAdapter(commandStr, connectionStr);
            OleDbCommandBuilder mycommandBuilder = new OleDbCommandBuilder(DBAdapter);

            // DataSet에 쿼리 결과 연결
            DataSet DS = new DataSet();
            DBAdapter.Fill(DS, "food");

            // DataSet으로부터 food 테이블 가져오기
            DataTable foodTable = DS.Tables["food"];

            // food테이블의 한 행 가져오기
            DataRow currRow = foodTable.Rows[0];

            // 선택된 메뉴와 가격을 텍스트박스에 출력한다.
            textBox1.Text = currRow["menu"].ToString();
            textBox2.Text = currRow["price"].ToString();
        }
        


        /*****************************************************
         * 수정버튼 클릭시
         *****************************************************/
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
                MessageBox.Show("메뉴를 입력해주세요.");
            else if (textBox2.Text == "")
                MessageBox.Show("가격을 입력해주세요.");
            else
            {
                OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|" + @"\db.mdb");
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = con;

                cmd.CommandText = "UPDATE food SET menu = \'" + textBox1.Text + "\', price = " + textBox2.Text +
                    " WHERE button = \'" + selectedButton.Name + "\'";

                // 쿼리 실행
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                selectedButton.Text = textBox1.Text + "\n\\" + textBox2.Text;

                // 편집 완료 후 폼 닫기
                this.Close();
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 엔터키가 입력되면
            if (e.KeyChar == '\r')
            {
                // 수정버튼 클릭 이벤트 발생
                button1_Click(sender, e);
            }
        }


    }
}
