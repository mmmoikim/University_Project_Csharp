using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


// DB연동을 위한 OleDb 네임스페이스
using System.Data.OleDb;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /* 키 입력 이벤트 */
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 엔터키가 입력되면
            if (e.KeyChar == '\r')
            {
                // 버튼1(로그인버튼)클릭 이벤트 발생
                button1_Click(sender, e);
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 엔터키가 입력되면
            if (e.KeyChar == '\r')
            {
                // 버튼1(로그인버튼)클릭 이벤트 발생
                button1_Click(sender, e);
            }
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 엔터키가 입력되면
            if (e.KeyChar == '\r')
            {
                // 버튼1(로그인버튼)클릭 이벤트 발생
                button1_Click(sender, e);
            }
        }

        /* 로그인 버튼 클릭시*/
        private void button1_Click(object sender, EventArgs e)
        {
            string connectionStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|" + @"\db.mdb";
            string commandStr = "select * from login";
            // 엑세스 파일 연결
            OleDbDataAdapter DBAdapter = new OleDbDataAdapter(commandStr, connectionStr);
            OleDbCommandBuilder mycommandBuilder = new OleDbCommandBuilder(DBAdapter);

            // DataSet에 login 테이블 연결
            DataSet DS = new DataSet();
            DBAdapter.Fill(DS, "login");

            // DataSet으로부터 login 테이블 가져오기
            DataTable aTable = DS.Tables["login"];

            /* 사용자 검증 */
            bool idExist = false; // ID 존재 여부
            bool correctPw = false; // 패스워드 일치 여부
            string currentId = ""; // 현재 로그인한 ID

            // 테이블의 레코드(row)를 한줄씩 가져와서 id와 pw 비교
            foreach (DataRow row in aTable.Rows)
            {
                // id가 존재하는 경우(alba, master)
                if (textBox1.Text == row["id"].ToString())
                {
                    idExist = true;
                }
                else
                {
                    // ID가 없으면 비밀번호를 체크하지 않고 다음단계로 넘어감
                    continue;
                }

                // id가 일치한 상태에서 pw도 일치하는 경우 => 로그인 성공
                if (textBox2.Text == row["pw"].ToString())
                {
                    // 패스워드가 일치함을 표시
                    correctPw = true;

                    // 현재 로그인한 ID 저장
                    currentId = row["id"].ToString();

                    // 로그인에 성공하면 더이상 반복문을 돌지 않고 빠져나감
                    break;
                }
                else // id는 존재하지만 패스워드가 틀린경우 => 로그인 실패
                {
                    correctPw = false;
                    break;
                }
            }
            /* 사용자 검증 끝 */

            if (!idExist)
            {
                MessageBox.Show("ID가 존재하지 않습니다.");
            }
            else if (idExist && !correctPw)
            {
                MessageBox.Show("비밀번호가 올바르지 않습니다.");
            }
            else // 로그인 성공
            {
                // 주문 폼 열기
                Form2 pv = new Form2();
                // 현재 로그인한 사용자 정보를 주문폼(Form2 pv)에 넘겨준다.
                pv.currentId = currentId;
                
                pv.Show();
                
             
                if (currentId != "master")
                {
                    pv.tabControl1.TabPages.RemoveAt(3);
                    pv.tabControl1.TabPages.RemoveAt(2);  
                }

                
                 
                // 로그인 폼 닫기
                this.Visible = false;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

    }
}
