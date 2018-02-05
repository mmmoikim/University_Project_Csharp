using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Collections;
using System.Data.OleDb;


namespace WindowsFormsApplication1
{
    public partial class Form2 : Form
    {
        string orderDate;//주문내역 주문중테이블 임시 저장 변수들
        string orderNumber;
        string tel;
        string address;
        string content;
        string price1;
        string remarks;
        string card;
        string state;

        bool renew = false;//주문내역 수정 여부확인 변수

        int pay;//해당 주문내역 수정할시 각 메뉴들의 단일가격을 가져오는 변수

        int n; //saveOrderList 주문번호
        int x; //orderList의 주문번호

        public string currentId;

        public ArrayList tabs = new ArrayList();

        // 주소등록 폼에서 입력한 주소를 현재 폼으로 가져오기 위한 변수
        internal string temp;

        // 주문 받은 메뉴에대한 변수
        int selectedRowIndex;
        int sumOfCurrentOrder = 0;


        public Form2()
        {
            InitializeComponent();

            // TabPage 3개를 tabs 변수에 지정한다.
            for (int i = 0; i < 3; i++)
                tabs.Add(tabControl1.Controls[i]);

            

        }

        private void tabControl1_SelectedIndexChanged(Object sender, EventArgs e)
        {

            MessageBox.Show("You are in the TabControl.SelectedIndexChanged event.");

        }
        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            //종료될때
            Application.Exit();

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dbDataSet5.finishedOrderList' table. You can move, or remove it, as needed.
            this.finishedOrderListTableAdapter1.Fill(this.dbDataSet5.finishedOrderList);
            // TODO: 이 코드는 데이터를 'dbDataSet4.login' 테이블에 로드합니다. 필요한 경우 이 코드를 이동하거나 제거할 수 있습니다.
            this.loginTableAdapter1.Fill(this.dbDataSet4.login);
            // TODO: 이 코드는 데이터를 'dbDataSet3.saveOrderList' 테이블에 로드합니다. 필요한 경우 이 코드를 이동하거나 제거할 수 있습니다.
            this.saveOrderListTableAdapter1.Fill(this.dbDataSet3.saveOrderList);
            // TODO: 이 코드는 데이터를 'dbDataSet2.saveOrderList' 테이블에 로드합니다. 필요한 경우 이 코드를 이동하거나 제거할 수 있습니다.
            this.saveOrderListTableAdapter.Fill(this.dbDataSet2.saveOrderList);
            // TODO: This line of code loads data into the 'dbDataSet1.login' table. You can move, or remove it, as needed.
            this.loginTableAdapter.Fill(this.dbDataSet1.login);
            // TODO: 이 코드는 데이터를 'dbDataSet.finishedOrderList' 테이블에 로드합니다. 필요한 경우 이 코드를 이동하거나 제거할 수 있습니다.
            this.finishedOrderListTableAdapter.Fill(this.dbDataSet.finishedOrderList);
            // TODO: 이 코드는 데이터를 'dbDataSet_주문내역.orderList' 테이블에 로드합니다. 필요한 경우 이 코드를 이동하거나 제거할 수 있습니다.
            this.orderListTableAdapter.Fill(this.dbDataSet_주문내역.orderList);

            // 폼이 로드될 때 전화번호 검색창에 포커스를 맞춘다.
            textBox1.Select();
            textBox3.Text = sumOfCurrentOrder.ToString("C0");

            // orderList 주문번호 동기화
            initOrderNumber();

            // saveOrderList 주문번호 동기화
            initSaveListOrderNumber();

            //판매현황 설정
            road_name();
            comboBox_init();

            //메뉴버튼에 메뉴 연결
            bindingMenuToButton(menuButton1);
            bindingMenuToButton(menuButton2);
            bindingMenuToButton(menuButton3);
            bindingMenuToButton(menuButton4);
            bindingMenuToButton(menuButton5);
            bindingMenuToButton(menuButton6);
            bindingMenuToButton(menuButton7);
            bindingMenuToButton(menuButton8);
            bindingMenuToButton(menuButton9);

            bindingMenuToButton(advMenuButton1);
            bindingMenuToButton(advMenuButton2);
            bindingMenuToButton(advMenuButton3);
            bindingMenuToButton(advMenuButton4);
            bindingMenuToButton(advMenuButton5);

            bindingMenuToButton(setMenuButton1);
            bindingMenuToButton(setMenuButton2);
            bindingMenuToButton(setMenuButton3);
            bindingMenuToButton(setMenuButton4);

            bindingMenuToButton(drinkButton1);
            bindingMenuToButton(drinkButton2);
            bindingMenuToButton(drinkButton3);
            bindingMenuToButton(drinkButton4);
            bindingMenuToButton(drinkButton5);

            // 관리자가 아니면 메뉴편집 불가능
            if (currentId != "master")
                contextMenuStrip1.Enabled = false;


            

        }

        // 전화번호 검색 버튼
        private void button10_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("전화번호를 입력해주세요!");
            }
            else
            {
                string connectionStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|" + @"\db.mdb";
                string commandSelectAddress = "select distinct address from saveOrderList where tel = \"" + textBox1.Text + "\"";
                string commandSelectAll = "select orderDate, address, content, price from saveOrderList where tel = \"" + textBox1.Text + "\"";

                // 엑세스 파일 연결
                OleDbDataAdapter DBAdapter = new OleDbDataAdapter(commandSelectAddress, connectionStr);
                OleDbCommandBuilder mycommandBuilder = new OleDbCommandBuilder(DBAdapter);

                // DataSet에 쿼리 결과 연결
                DataSet DS = new DataSet();
                DBAdapter.Fill(DS, "addressList");

                // DataSet으로부터 saveOrderList 테이블 가져오기
                DataTable addressListTable = DS.Tables["addressList"];

                if (addressListTable.Rows.Count != 0) // 주소가 있을경우
                {
                    // 콤보박스에 주소 목록 추가
                    comboBox1.Items.Clear();
                    foreach (DataRow row in addressListTable.Rows)
                    {
                        comboBox1.Items.Add(row["address"]);
                    }
                    comboBox1.SelectedIndex = 0;
                    comboBox1.SelectAll();
                }
                else // 주소가 없을경우
                {
                    comboBox1.Items.Clear();
                    Form3 addressRegisterForm = new Form3();
                    addressRegisterForm.ShowDialog(this);

                    // 입력받은 주소가 없으면 주소목록에 추가시키지 않는다.
                    if (temp != null)
                    {
                        comboBox1.Items.Add(temp);
                        comboBox1.SelectedIndex = 0;
                    }
                }

                DBAdapter = new OleDbDataAdapter(commandSelectAll, connectionStr);
                mycommandBuilder = new OleDbCommandBuilder(DBAdapter);

                DBAdapter.Fill(DS, "saveOrderLIst");
                DataTable saveOrderListTable = DS.Tables["saveOrderList"];


                // 데이터그리드뷰에 과거 주문기록 출력
                // dataSource 바인딩.
                dataGridView3.DataSource = saveOrderListTable;
                dataGridView3.Columns["orderDate"].HeaderText = "날짜";
                dataGridView3.Columns["address"].HeaderText = "주소";
                dataGridView3.Columns["content"].HeaderText = "주문내역";
                dataGridView3.Columns["price"].HeaderText = "가격";
                dataGridView3.Columns["price"].DefaultCellStyle.Format = "C0";
                dataGridView3.Columns["price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridView3.Columns["price"].FillWeight = 60;
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 엔터키가 입력되면
            if (e.KeyChar == '\r')
            {
                button10_Click(sender, e);
            }
        }

        private void dataGridView3_DataSourceChanged(object sender, EventArgs e)
        {
            // 기본적으로 출력되는 열을 없앤다.
            dataGridView3.Columns["Column1"].Visible = false;
            dataGridView3.Columns["Column2"].Visible = false;
            dataGridView3.Columns["Column3"].Visible = false;
            dataGridView3.Columns["Column4"].Visible = false;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }



        // 주문탭의 주문내역 셀 클릭시
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) //클릭한 행이 범위 초과시
            {
                return;
            }

            // 해당하는 row의 수량을 텍스트박스로 가져온다.
            selectedRowIndex = e.RowIndex;
            textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells["count"].Value.ToString();

        }

        // 현재 주문내역의 합계를 구하는 함수
        private void CalSumOfCurrentOrder()
        {
            sumOfCurrentOrder = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["price"].Value.ToString() == "서비스")
                    continue;
                sumOfCurrentOrder += int.Parse(row.Cells["price"].Value.ToString());
            }
            textBox3.Text = sumOfCurrentOrder.ToString("C0");
        }

        // 주문내역 셀값이 변경되면 합계를 새로 구한다.
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            CalSumOfCurrentOrder();
        }

        // 주문내역에 한 행이 추가되면 합계를 새로 구한다.
        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            CalSumOfCurrentOrder();
        }

        // 주문내역에 한 행이 삭제되면 합계를 새로 구한다.
        private void dataGridView1_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            CalSumOfCurrentOrder();
        }

        // 수량변경
        private void button18_Click(object sender, EventArgs e)
        {
            // 선택한 셀의 인덴스가 범위를 벗어나면 리턴
            if (selectedRowIndex < 0 || selectedRowIndex > dataGridView1.Rows.Count - 1)
            {
                return;
            }

            // 변경할 수량을 적지 않은 경우 리턴
            if (textBox2.Text == "")
            {
                return;
            }

            string connectionStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|" + @"\db.mdb";
            string menu = dataGridView1.Rows[selectedRowIndex].Cells["menu"].Value.ToString();
            string commandStr = "select menu, price from food where menu = \"" + menu + "\"";

            // 엑세스 파일 연결
            OleDbDataAdapter DBAdapter = new OleDbDataAdapter(commandStr, connectionStr);
            OleDbCommandBuilder mycommandBuilder = new OleDbCommandBuilder(DBAdapter);

            // DataSet에 쿼리 결과 연결
            DataSet DS = new DataSet();
            DBAdapter.Fill(DS, "food");

            // DataSet으로부터 food 테이블 가져오기
            DataTable foodTable = DS.Tables["food"];

            // 테이블의 한 행 가져오기
            DataRow currRow = foodTable.Rows[0];

            // 선택된 셀의 수량을 변경
            dataGridView1.Rows[selectedRowIndex].Cells["count"].Value = int.Parse(textBox2.Text);

            // 변경된 수량과 해당 메뉴의 가격을 곱함
            int price = int.Parse(currRow["price"].ToString()) * int.Parse(textBox2.Text);

            // 곱한값을 셀에 출력
            dataGridView1.Rows[selectedRowIndex].Cells["price"].Value = price;

            // 합계를 새로 구한다.
            CalSumOfCurrentOrder();
        }

        // 다른 주소 선택
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox11.Text = comboBox1.SelectedItem.ToString();
        }

        //////////////////////////////////////////////////////////////////////
        // 메뉴버튼 클릭시(모든 메뉴에 공통!)
        private void menuButton_Click(object sender, EventArgs e)
        {
            Button menuButton = (Button)sender;
            if (menuButton.Text == "")
            {
                MessageBox.Show("등록되지 않은 메뉴입니다.");
                return;
            }

            string connectionStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|" + @"\db.mdb";
            string commandStr = "select menu, price from food where button = \"" + menuButton.Name + "\"";

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

            // 행이 추가되거나 셀이 변경되었는지 여부를 나타낸다.
            bool isUpdated = false;

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                // 클릭한 메뉴가 주문내역에 들어있으면 행을 새로 추가하지 않고 수량과 가격만 업데이트한다.
                if (currRow["menu"].ToString() == dataGridView1.Rows[i].Cells["menu"].Value.ToString())
                {
                    int count = int.Parse(dataGridView1.Rows[i].Cells["count"].Value.ToString());

                    count++;
                    int price = int.Parse(currRow["price"].ToString()) * count;

                    dataGridView1.Rows[i].Cells["count"].Value = count;
                    dataGridView1.Rows[i].Cells["price"].Value = price;

                    isUpdated = true;

                    break;
                }
            }

            // 클릭한 메뉴가 주문내역에 없으면 새 행을 추가한다.
            if (!isUpdated)
            {
                string menu = currRow["menu"].ToString();
                int price = (int)currRow["price"];

                dataGridView1.Rows.Add(menu, 1, price);
            }
        }

        // 주문취소
        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
        }

        // 선택취소
        private void button14_Click(object sender, EventArgs e)
        {

            if (selectedRowIndex < 0 || selectedRowIndex > dataGridView1.Rows.Count - 1)
            {
                return;
            }

            dataGridView1.Rows.RemoveAt(selectedRowIndex);

        }

        // orderList 주문번호 동기화 함수
        private void initOrderNumber()
        {
            string connectionStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|" + @"\db.mdb";
            string commandStr = "select count(*) from orderList";
            string commandStr2 = "select count(*) from finishedOrderList";

            OleDbDataAdapter DBAdapter = new OleDbDataAdapter(commandStr, connectionStr);
            OleDbDataAdapter DBAdapter2 = new OleDbDataAdapter(commandStr2, connectionStr);

            OleDbCommandBuilder mycommandBuilder = new OleDbCommandBuilder(DBAdapter);
            OleDbCommandBuilder mycommandBuilder2 = new OleDbCommandBuilder(DBAdapter2);

            DataSet DS = new DataSet();
            DBAdapter.Fill(DS, "orderListCount");
            DBAdapter2.Fill(DS, "finishedOrderListCount");

            DataTable orderListCount = DS.Tables["orderListCount"];
            DataTable finishedOrderListCount = DS.Tables["finishedOrderListCount"];

            // orderList 테이블과 finishedOrderList 테이블의 행 개수를 더한값에 1을 증가시켜 x(orderNumber)를 설정한다.
            x = int.Parse(orderListCount.Rows[0][0].ToString()) + int.Parse(finishedOrderListCount.Rows[0][0].ToString()) + 1;
        }

        // saveOrderList 주문번호 동기화 함수
        private void initSaveListOrderNumber()
        {
            string connectionStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|" + @"\db.mdb";
            string commandStr = "select count(*) from saveOrderList";

            OleDbDataAdapter DBAdapter = new OleDbDataAdapter(commandStr, connectionStr);


            OleDbCommandBuilder mycommandBuilder = new OleDbCommandBuilder(DBAdapter);


            DataSet DS = new DataSet();
            DBAdapter.Fill(DS, "saveOrderListCount");

            DataTable saveOrderListCount = DS.Tables["saveOrderListCount"];

            // saveList의 행 개수에 1을 더해 orderNumber를 구한다.
            n = int.Parse(saveOrderListCount.Rows[0][0].ToString()) + 1;
        }


        // 주문하기
        private void button1_Click(object sender, EventArgs e)
        {
            // 주문한 메뉴들을 하나의 문자열로 만듦
            string currMenu = "";
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                currMenu += dataGridView1.Rows[i].Cells["menu"].Value.ToString();
                if (dataGridView1.Rows[i].Cells["price"].Value.ToString() == "서비스")
                {
                    currMenu += "(서)";
                }
                currMenu += "/" + dataGridView1.Rows[i].Cells["count"].Value.ToString();
                if (i != dataGridView1.Rows.Count - 1)
                {
                    currMenu += ", ";
                }
            }

            if (renew == false)
            {

                if (textBox1.Text == "")
                {
                    MessageBox.Show("전화번호를 입력해주세요!");
                }
                else if (textBox11.Text == "")
                {
                    MessageBox.Show("주소를 입력해주세요!");
                }
                else if (currMenu == "")
                {
                    MessageBox.Show("메뉴를 선택해주세요!");
                }


                else
                {
                    initOrderNumber();
                    //DB연결후 orderList 테이블에 주문정보 INSERT
                    OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|" + @"\db.mdb");
                    OleDbCommand cmd = new OleDbCommand();
                    cmd.Connection = con;

                    cmd.CommandText = "INSERT INTO orderList (orderNumber ,tel, address, content, price, card, remarks, state) VALUES("
                        + "\'" + x + "\',"
                        + "\'" + textBox1.Text + "\', "
                        + "\'" + textBox11.Text + "\', "
                        + "\'" + currMenu + "\', "
                        + sumOfCurrentOrder + ", "
                        + checkBox1.Checked + ", "
                        + "\'" + textBox5.Text + "\', "
                        + "\'준비중\')";

                    // 주문번호 증가
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    MessageBox.Show("주문이 완료되었습니다");

                    // 주문 창의 입력 내용을 지운다
                    textBox1.Text = "";
                    comboBox1.Items.Clear();
                    textBox11.Text = "";
                    dataGridView3.DataSource = null;
                    dataGridView3.Columns["Column1"].Visible = true;
                    dataGridView3.Columns["Column2"].Visible = true;
                    dataGridView3.Columns["Column3"].Visible = true;
                    dataGridView3.Columns["Column4"].Visible = true;
                    dataGridView1.Rows.Clear();
                    textBox5.Text = "";

                    // 주문내역 그리드뷰 갱신
                    string connectionStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|" + @"\db.mdb";
                    string commandStr = "select * from orderList order by orderNumber";

                    // 엑세스 파일 연결;
                    OleDbDataAdapter DBAdapter = new OleDbDataAdapter(commandStr, connectionStr);
                    OleDbCommandBuilder mycommandBuilder = new OleDbCommandBuilder(DBAdapter);

                    // DataSet에 쿼리 결과 연결
                    DataSet DS = new DataSet();
                    DBAdapter.Fill(DS, "orderList");

                    // DataSet으로부터 orderList 테이블 가져오기
                    DataTable orderListTable = DS.Tables["orderList"];

                    dataGridView2.DataSource = orderListTable.DefaultView;

                }
            }
            else if (renew == true)
            {
                if (textBox1.Text == "")
                {
                    MessageBox.Show("전화번호를 입력해주세요!");
                }
                else if (textBox11.Text == "")
                {
                    MessageBox.Show("주소를 입력해주세요!");
                }
                else if (currMenu == "")
                {
                    MessageBox.Show("메뉴를 선택해주세요!");
                }


                else
                {
                    initOrderNumber();
                    //DB연결후 orderList 테이블에 주문정보 INSERT
                    OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|" + @"\db.mdb");
                    OleDbCommand cmd = new OleDbCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "UPDATE orderList SET tel = " + "\'" + textBox1.Text
                                       + "\',address = " + "\'" + textBox11.Text
                                       + "\',content = " + "\'" + currMenu
                                       + "\',price = " + sumOfCurrentOrder
                                       + ",card = " + checkBox1.Checked
                                       + ",remarks = " + "\'" + textBox5.Text
                                       + "\',state = " + "\'" + state
                                       + "\' where orderNumber = " + orderNumber;

                    // 주문번호 증가
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    MessageBox.Show("주문이 완료되었습니다");

                    // 주문 창의 입력 내용을 지운다
                    textBox1.Text = "";
                    comboBox1.Items.Clear();
                    textBox11.Text = "";
                    dataGridView3.DataSource = null;
                    dataGridView3.Columns["Column1"].Visible = true;
                    dataGridView3.Columns["Column2"].Visible = true;
                    dataGridView3.Columns["Column3"].Visible = true;
                    dataGridView3.Columns["Column4"].Visible = true;
                    dataGridView1.Rows.Clear();
                    textBox5.Text = "";

                    // 주문내역 그리드뷰 갱신
                    string connectionStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|" + @"\db.mdb";
                    string commandStr = "select * from orderList order by orderNumber";

                    // 엑세스 파일 연결;
                    OleDbDataAdapter DBAdapter = new OleDbDataAdapter(commandStr, connectionStr);
                    OleDbCommandBuilder mycommandBuilder = new OleDbCommandBuilder(DBAdapter);

                    // DataSet에 쿼리 결과 연결
                    DataSet DS = new DataSet();
                    DBAdapter.Fill(DS, "orderList");

                    // DataSet으로부터 orderList 테이블 가져오기
                    DataTable orderListTable = DS.Tables["orderList"];

                    dataGridView2.DataSource = orderListTable.DefaultView;

                    renew = false;//수정후 다시 false상태로 바꿔줘야지요^^

                }
            }
        }

        private void dataGridView6_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }
        private void tabPage6_Click(object sender, EventArgs e)
        {
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        // 주문내역 셀 클릭
        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string connectionStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|" + @"\db.mdb";
            string commandStr = "select * from orderList";

            OleDbDataAdapter DBAdapter = new OleDbDataAdapter(commandStr, connectionStr);

            OleDbCommandBuilder mycommandBuilder = new OleDbCommandBuilder(DBAdapter);

            DataSet DS = new DataSet();

            DBAdapter.Fill(DS, "orderList");

            DataTable leftTable = DS.Tables["orderList"];

            if (e.RowIndex < 0) //클릭한 행이 범위 초과시
            {
                return;
            }
            else if (e.RowIndex > leftTable.Rows.Count - 1)
            {
                MessageBox.Show("잘못 클릭하셨습니다.");
                return;
            }

            DataRow currentRow = leftTable.Rows[e.RowIndex];

            orderDate = currentRow["orderDate"].ToString();
            orderNumber = currentRow["orderNumber"].ToString();
            tel = currentRow["tel"].ToString();
            address = currentRow["address"].ToString();
            content = currentRow["content"].ToString();
            price1 = currentRow["price"].ToString();
            remarks = currentRow["remarks"].ToString();
            card = currentRow["card"].ToString();
            state = currentRow["state"].ToString();

        }

        // 배달완료 버튼 클릭
        private void button34_Click(object sender, EventArgs e)
        {
            try
            {
                if (state == "준비중")
                {
                    MessageBox.Show("아직 준비중입니다.");
                    return;
                }

                string connectionStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|" + @"\db.mdb";
                string commandStrLeft = "select * from orderList";
                string commandStrRight = "select * from finishedOrderList";
                string commandStrMiddle = "select * from saveOrderList";

                OleDbDataAdapter DBAdapterLeft = new OleDbDataAdapter(commandStrLeft, connectionStr);
                OleDbDataAdapter DBAdapterRight = new OleDbDataAdapter(commandStrRight, connectionStr);
                OleDbDataAdapter DBAdapterMiddle = new OleDbDataAdapter(commandStrMiddle, connectionStr);
                OleDbCommandBuilder mycommandBuilderLeft = new OleDbCommandBuilder(DBAdapterLeft);
                OleDbCommandBuilder mycommandBuilderRight = new OleDbCommandBuilder(DBAdapterRight);
                OleDbCommandBuilder mycommandBuilderMiddle = new OleDbCommandBuilder(DBAdapterMiddle);

                DataSet DS = new DataSet();

                DBAdapterLeft.Fill(DS, "orderList");
                DBAdapterRight.Fill(DS, "finishedOrderList");
                DBAdapterMiddle.Fill(DS, "saveOrderList");
                DataTable leftTable = DS.Tables["orderList"];
                DataTable rightTable = DS.Tables["finishedOrderList"];
                DataTable middleTable = DS.Tables["saveOrderList"];

                /////////////////////////////////////////////////////////////////////////////////
                // 배달중 테이블(orderList)에서 삭제
                DataColumn[] Primarykey = new DataColumn[1];
                Primarykey[0] = leftTable.Columns["orderNumber"];
                leftTable.PrimaryKey = Primarykey;

                DataRow currRow = leftTable.Rows.Find(orderNumber);
                currRow.Delete();

                DBAdapterLeft.Update(DS.GetChanges(DataRowState.Deleted), "orderList");
                dataGridView2.DataSource = DS.Tables["orderList"].DefaultView;
                //////////////////////////////////////////////////////////////////////////////////

                //////////////////////////////////////////////////////////////////////////////////
                // 배달완료 테이블(finishedOrderList)에 저장
                DataRow newRow = rightTable.NewRow();

                newRow["orderDate"] = orderDate;
                newRow["orderNumber"] = orderNumber;
                newRow["tel"] = tel;
                newRow["address"] = address;
                newRow["content"] = content;
                newRow["price"] = price1;
                newRow["remarks"] = remarks;
                newRow["card"] = card;
                newRow["state"] = "배달완료";

                rightTable.Rows.Add(newRow);

                DBAdapterRight.Update(DS, "finishedOrderList");
                dataGridView6.DataSource = DS.Tables["finishedOrderList"].DefaultView;
                dataGridView7.DataSource = DS.Tables["finishedOrderList"].DefaultView;
                /////////////////////////////////////////////////////////////////////////////////
                // saveOrderList 테이블에 저장
                DataRow newRow2 = middleTable.NewRow();

                newRow2["orderDate"] = orderDate;
                newRow2["orderNumber"] = n;
                newRow2["tel"] = tel;
                newRow2["address"] = address;
                newRow2["content"] = content;
                newRow2["price"] = price1;
                newRow2["remarks"] = remarks;
                newRow2["card"] = card;
                newRow2["state"] = "배달완료";

                middleTable.Rows.Add(newRow2);
                n++;
                DBAdapterMiddle.Update(DS, "saveOrderList");

            }
            catch (DataException DE)
            {
                MessageBox.Show(DE.Message);
            }
            catch (Exception DE)
            {
                MessageBox.Show(DE.Message);
            }
        }

        // 배달중 버튼 클릭
        private void button35_Click(object sender, EventArgs e)
        {
            try
            {

                string connectionString = "provider=Microsoft.JET.OLEDB.4.0;"
                       + "data source = "
                       + Application.StartupPath
                       + @"\db.mdb";

                string commandString = "select * from orderList";

                OleDbDataAdapter DBAdapter = new OleDbDataAdapter(commandString, connectionString);

                OleDbCommandBuilder myCommandBuilder = new OleDbCommandBuilder(DBAdapter);

                DataSet DS = new DataSet("orderList");
                DBAdapter.Fill(DS, "orderList");

                DataTable aTable = DS.Tables["orderList"];

                DataColumn[] PrimaryKey = new DataColumn[1];
                PrimaryKey[0] = aTable.Columns["orderNumber"];
                aTable.PrimaryKey = PrimaryKey;

                DataRow currRow = aTable.Rows.Find(orderNumber);

                currRow.BeginEdit(); // 편집의 시작


                currRow["state"] = "배달중";


                currRow.EndEdit(); // 편집의 끝

                DataSet UpdatedSet = DS.GetChanges(DataRowState.Modified);
                if (UpdatedSet.HasErrors)
                {
                    MessageBox.Show("변경된 데이터에 문제가 있습니다.");
                }
                else
                {
                    DBAdapter.Update(UpdatedSet, "orderList");
                    DS.AcceptChanges();
                }

                dataGridView2.DataSource = DS.Tables["orderList"].DefaultView;

            }
            catch (DataException DE)
            {
                MessageBox.Show(DE.Message);
            }
            catch (Exception DE)
            {
                MessageBox.Show(DE.Message);
            }
        }

        private void dataGridView4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tabControl1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 3)
                calc_DB();

        }

        private void calc_rest()//비용
        {
            int rest = 0;
            try
            {
                for (int i = 0; i < dataGridView4.Rows.Count; ++i)
                {
                    rest += Convert.ToInt32(dataGridView4.Rows[i].Cells[1].Value);
                }
                textBox8.Text = rest.ToString("");
            }
            catch (DataException DE)
            {
                MessageBox.Show(DE.Message);
            }
        }
        private void calc_netincome()//순이익
        {
            try
            {
                textBox10.Text = (Convert.ToInt32(textBox4.Text) - Convert.ToInt32(textBox8.Text)).ToString();
            }
            catch (DataException DE)
            {
                MessageBox.Show(DE.Message);
            }
        }

        private void calc_outstanding()//미수액
        {
            try
            {
                int sum = 0;
                for (int i = 0; i < dataGridView4.Rows.Count; ++i)
                {
                    sum += Convert.ToInt32(dataGridView4.Rows[i].Cells[2].Value);
                }
                int temp = int.Parse(textBox4.Text);
                textBox9.Text = (sum - temp).ToString("");
            }
            catch (DataException DE)
            {
                MessageBox.Show(DE.Message);
            }
        }
        
        private void calc_DB()
        {
            int cash = 0, card = 0;
            
            try
            {
                for (int i = 0; i < dataGridView7.Rows.Count; ++i)
                {
                    string temp = dataGridView7.Rows[i].Cells[4].Value.ToString();

                    if (temp == "True")
                    {
                        card += Convert.ToInt32(dataGridView7.Rows[i].Cells[3].Value);
                    }
                    else cash += Convert.ToInt32(dataGridView7.Rows[i].Cells[3].Value);
                }
                textBox4.Text = cash.ToString("");
                textBox6.Text = card.ToString("");
                textBox7.Text = (card + cash).ToString("");
            }
            catch (DataException DE)
            {
                MessageBox.Show(DE.Message);
            }
        }
        private void road_name()
        {

            string connectionString = "provider=Microsoft.JET.OLEDB.4.0;"
             + "data source = "
             + Application.StartupPath
             + @"\db.mdb";

            string commandString = "select * from login";

            OleDbDataAdapter DBAdapter = new OleDbDataAdapter(commandString, connectionString);

            OleDbCommandBuilder myCommandBuilder = new OleDbCommandBuilder(DBAdapter);

            DataSet DS = new DataSet("login");
            DBAdapter.Fill(DS, "login");

            DataTable aTable = DS.Tables["login"];

            dataGridView4.ColumnCount = 4;
            dataGridView4.Columns[0].Name = "이름";
            dataGridView4.Columns[1].Name = "비용";
            dataGridView4.Columns[2].Name = "현금";
            dataGridView4.Columns[3].Name = "카드";


            for (int i = 0; i < aTable.Rows.Count; i++)
            {
                DataRow currentRow = aTable.Rows[i];
                string temp = currentRow["sname"].ToString();
                dataGridView4.Rows.Add(temp, 0, 0, 0);
            }


        }
        private void comboBox_init()//날짜 콤보박스 설정
        {

            comboBox2.Items.Add("년");
            for (int i = -5; i < 5; i++)
            {
                comboBox2.Items.Add(DateTime.Now.Year - i);
            }
            comboBox2.SelectedIndex = 0;

            comboBox3.Items.Add("월");
            for (int i = 1; i < 13; i++)
            {
                comboBox3.Items.Add(i);
            }
            comboBox3.SelectedIndex = 0;

            comboBox4.Items.Add("일");
            for (int i = 1; i < 32; i++)
            {
                comboBox4.Items.Add(i);
            }
            comboBox4.SelectedIndex = 0;

        }

        private void button19_Click(object sender, EventArgs e)//정산
        {
            //정산 버튼을 계속 눌러도 그대로 계산
            calc_DB();
            calc_rest();
            calc_netincome();
            calc_outstanding();

        }

        private void button37_Click(object sender, EventArgs e)//제출
        {
            try
            {
                string connectionStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|" + @"\db.mdb";
                string commandStr2 = "select count(*) from orderList";
                OleDbDataAdapter DBAdapter2 = new OleDbDataAdapter(commandStr2, connectionStr);
                OleDbCommandBuilder mycommandBuilder2 = new OleDbCommandBuilder(DBAdapter2);

                DataSet DS2 = new DataSet();
                DBAdapter2.Fill(DS2, "orderListCount");
                DataTable orderListCount = DS2.Tables["orderListCount"];
                // orderList 테이블과 finishedOrderList 테이블의 행 개수를 더한값에 1을 증가시켜 x(orderNumber)를 설정한다.
   
                if (comboBox2.SelectedIndex == 0 && comboBox3.SelectedIndex == 0 && comboBox4.SelectedIndex == 0)
                {
                    MessageBox.Show("날짜를 지정하세요.");
                }
                else if (textBox8.Text == "")
                {
                    MessageBox.Show("정산이 되지 않았습니다.");
                }
                else
                {
                    string connectionString = "provider=Microsoft.JET.OLEDB.4.0;"
           + "data source = "
           + Application.StartupPath
           + @"\db.mdb";

                    string commandString = "select * from sales";

                    OleDbDataAdapter DBAdapter = new OleDbDataAdapter(commandString, connectionString);

                    OleDbCommandBuilder myCommandBuilder = new OleDbCommandBuilder(DBAdapter);

                    DataSet DS = new DataSet("sales");
                    DBAdapter.Fill(DS, "sales");

                    DataTable aTable = DS.Tables["sales"];

                    DataRow newRow = aTable.NewRow();

                    newRow["syear"] = comboBox2.Text;
                    newRow["smonth"] = comboBox3.Text;
                    newRow["sday"] = comboBox4.Text;

                    newRow["total"] = textBox7.Text;
                    newRow["cash"] = textBox4.Text;
                    newRow["card"] = textBox6.Text;
                    newRow["realcash"] = textBox10.Text;
                    newRow["rest"] = textBox8.Text;
                    newRow["outstanding"] = textBox9.Text;

                    aTable.Rows.Add(newRow);

                    DBAdapter.Update(DS, "sales");
                    MessageBox.Show("제출되었습니다");


                    //DB연결후 finishedOrderList 테이블에 전체 레코드 삭제
                    OleDbConnection pick = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|" + @"\db.mdb");
                    OleDbCommand cmd2 = new OleDbCommand();

                    cmd2.Connection = pick;

                    cmd2.CommandText = "DELETE FROM finishedOrderList";

                    pick.Open();
                    cmd2.ExecuteNonQuery();
                    pick.Close();
                    //주문내역 배달완료 그리드뷰 갱신
                    string commandStr = "select * from finishedOrderList";

                    // 엑세스 파일 연결;
                    OleDbDataAdapter DBAdapter3 = new OleDbDataAdapter(commandStr, connectionStr);
                    OleDbCommandBuilder mycommandBuilder = new OleDbCommandBuilder(DBAdapter);

                    // DataSet에 쿼리 결과 연결
                    DBAdapter3.Fill(DS2, "finishedOrderList");

                    // DataSet으로부터 orderList 테이블 가져오기
                    DataTable orderListTable = DS2.Tables["finishedOrderList"];

                    dataGridView6.DataSource = orderListTable.DefaultView;
                    dataGridView7.DataSource = orderListTable.DefaultView;
                    initOrderNumber();
                }


            }
            catch (DataException DE)
            {
                MessageBox.Show(DE.Message);
            }
            catch (Exception DE)
            {
                MessageBox.Show(DE.Message);
            }
        }


        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void chart()//차트
        {
            foreach (var series in chart1.Series)
            {
                series.Points.Clear();
            }

            this.chart1.Series["total"].Points.AddXY(" ", int.Parse(textBox7.Text));
            this.chart1.Series["cash"].Points.AddXY(" ", int.Parse(textBox4.Text));
            this.chart1.Series["card"].Points.AddXY(" ", int.Parse(textBox6.Text));
            this.chart1.Series["realcash"].Points.AddXY(" ", int.Parse(textBox10.Text));
            this.chart1.Series["rest"].Points.AddXY(" ", int.Parse(textBox8.Text));
            this.chart1.Series["outstanding"].Points.AddXY(" ", int.Parse(textBox9.Text));
        }

        private void button38_Click(object sender, EventArgs e)//조회
        {


            textBox7.Text = "0";
            textBox4.Text = "0";
            textBox6.Text = "0";
            textBox10.Text = "0";
            textBox8.Text = "0";
            textBox9.Text = "0";

            try
            {
                string connectionString = "provider=Microsoft.JET.OLEDB.4.0;"
           + "data source = "
           + Application.StartupPath
           + @"\db.mdb";

                string commandString = "select * from sales";

                OleDbDataAdapter DBAdapter = new OleDbDataAdapter(commandString, connectionString);

                OleDbCommandBuilder myCommandBuilder = new OleDbCommandBuilder(DBAdapter);

                DataSet DS = new DataSet("sales");
                DBAdapter.Fill(DS, "sales");

                DataTable aTable = DS.Tables["sales"];

                DataRow[] ResultRow = null;



                if (comboBox2.SelectedIndex == 0 && comboBox3.SelectedIndex == 0 && comboBox4.SelectedIndex == 0)
                {
                    MessageBox.Show("날짜를 지정하세요");
                }


                else if (comboBox2.SelectedIndex != 0 && comboBox3.SelectedIndex == 0 && comboBox4.SelectedIndex == 0)//년
                {
                    MessageBox.Show("'" + comboBox2.Text + "년 ");

                    int total = 0, cash = 0, card = 0, realcash = 0, rest = 0, outstanding = 0;

                    ResultRow = null;

                    ResultRow = aTable.Select("  syear = '" + comboBox2.Text + "'");

                    foreach (DataRow currRow in ResultRow)
                    {

                        total += Convert.ToInt32(currRow["total"]);
                        cash += Convert.ToInt32(currRow["cash"]);
                        card += Convert.ToInt32(currRow["card"]);
                        realcash += Convert.ToInt32(currRow["realcash"]);
                        rest += Convert.ToInt32(currRow["rest"]);
                        outstanding += Convert.ToInt32(currRow["outstanding"]);

                    }

                    textBox7.Text = total.ToString();
                    textBox4.Text = cash.ToString();
                    textBox6.Text = card.ToString();
                    textBox10.Text = realcash.ToString();
                    textBox8.Text = rest.ToString();
                    textBox9.Text = outstanding.ToString();

                    total = 0; cash = 0; card = 0; realcash = 0; rest = 0; outstanding = 0;
                }
                else if (comboBox2.SelectedIndex != 0 && comboBox3.SelectedIndex != 0 && comboBox4.SelectedIndex == 0)//월
                {
                    MessageBox.Show("'" + comboBox2.Text + "'년 '" + comboBox3.Text + "'월'");

                    ResultRow = null;
                    int total = 0, cash = 0, card = 0, realcash = 0, rest = 0, outstanding = 0;

                    ResultRow = aTable.Select("  syear = '" + comboBox2.Text + "' AND  smonth = '" + comboBox3.Text + "' ");

                    foreach (DataRow currRow in ResultRow)
                    {

                        total += Convert.ToInt32(currRow["total"]);
                        cash += Convert.ToInt32(currRow["cash"]);
                        card += Convert.ToInt32(currRow["card"]);
                        realcash += Convert.ToInt32(currRow["realcash"]);
                        rest += Convert.ToInt32(currRow["rest"]);
                        outstanding += Convert.ToInt32(currRow["outstanding"]);

                    }

                    textBox7.Text = total.ToString();
                    textBox4.Text = cash.ToString();
                    textBox6.Text = card.ToString();
                    textBox10.Text = realcash.ToString();
                    textBox8.Text = rest.ToString();
                    textBox9.Text = outstanding.ToString();

                    total = 0; cash = 0; card = 0; realcash = 0; rest = 0; outstanding = 0;

                }
                else //일
                {
                    MessageBox.Show("'" + comboBox2.Text + "'년 '" + comboBox3.Text + "'월'" + comboBox4.Text + "' 일");


                    ResultRow = null;

                    ResultRow = aTable.Select("  syear = '" + comboBox2.Text + "' AND  smonth = '" + comboBox3.Text + "' AND sday = '" + comboBox4.Text + "'");

                    foreach (DataRow currRow in ResultRow)
                    {

                        textBox7.Text = currRow["total"].ToString();
                        textBox4.Text = currRow["cash"].ToString();
                        textBox6.Text = currRow["card"].ToString();
                        textBox10.Text = currRow["realcash"].ToString();
                        textBox8.Text = currRow["rest"].ToString();
                        textBox9.Text = currRow["outstanding"].ToString();
                    }
                }

                chart();

            }
            catch (DataException DE)
            {
                MessageBox.Show(DE.Message);
            }
            catch (Exception DE)
            {
                MessageBox.Show(DE.Message);
            }
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView7_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {

                string connectionString = "provider=Microsoft.JET.OLEDB.4.0;"
                       + "data source = "
                       + Application.StartupPath
                       + @"\db.mdb";

                string commandString = "select * from orderList";

                OleDbDataAdapter DBAdapter = new OleDbDataAdapter(commandString, connectionString);

                OleDbCommandBuilder myCommandBuilder = new OleDbCommandBuilder(DBAdapter);

                DataSet DS = new DataSet("orderList");
                DBAdapter.Fill(DS, "orderList");

                DataTable aTable = DS.Tables["orderList"];

                DataColumn[] PrimaryKey = new DataColumn[1];
                PrimaryKey[0] = aTable.Columns["orderNumber"];
                aTable.PrimaryKey = PrimaryKey;

                DataRow currRow = aTable.Rows.Find(orderNumber);

                if (currRow["state"].ToString() == "준비중")
                {
                    currRow.BeginEdit(); // 편집의 시작


                    currRow["state"] = "배달중";


                    currRow.EndEdit(); // 편집의 끝
                    DataSet UpdatedSet = DS.GetChanges(DataRowState.Modified);
                    if (UpdatedSet.HasErrors)
                    {
                        MessageBox.Show("변경된 데이터에 문제가 있습니다.");
                    }
                    else
                    {
                        DBAdapter.Update(UpdatedSet, "orderList");
                        DS.AcceptChanges();
                    }

                    dataGridView2.DataSource = DS.Tables["orderList"].DefaultView;
                    return;
                }
                else if (currRow["state"].ToString() == "배달중")
                {
                    button34_Click(sender, e);
                    return;
                }

            }
            catch (DataException DE)
            {
                MessageBox.Show(DE.Message);
            }
            catch (Exception DE)
            {
                MessageBox.Show(DE.Message);
            }
        }

        private void button33_Click(object sender, EventArgs e)
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

            foreach (DataRow row in aTable.Rows)
            {
                // id가 존재하는 경우(alba, master)
                if (textBox15.Text == row["id"].ToString())
                {
                    idExist = true;
                }
            }

            if (!idExist && textBox15.Text != "")
            {
                MessageBox.Show("사용가능한 ID입니다.");
            }
            else
            {
                MessageBox.Show("사용가능한 ID가 아닙니다.");
            }

        }

        private void button39_Click(object sender, EventArgs e)
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

            foreach (DataRow row in aTable.Rows)
            {
                // id가 존재하는 경우(alba, master)
                if (textBox15.Text == row["id"].ToString())
                {
                    idExist = true;
                }
            }

            if (!idExist)
            {
                if (textBox15.Text == "")
                {
                    MessageBox.Show("ID를 입력해주세요!");
                }
                else
                    MessageBox.Show("사용가능한 ID입니다.");
            }
            else
            {
                MessageBox.Show("사용가능한 ID가 아닙니다.");
            }


        }

        private void button32_Click(object sender, EventArgs e)
        {
            try
            {

                string connectionString = "provider=Microsoft.JET.OLEDB.4.0;"
                       + "data source = "
                       + Application.StartupPath
                       + @"\db.mdb";

                string commandString = "select * from login";

                OleDbDataAdapter DBAdapter = new OleDbDataAdapter(commandString, connectionString);

                OleDbCommandBuilder myCommandBuilder = new OleDbCommandBuilder(DBAdapter);

                DataSet DS = new DataSet("login");
                DBAdapter.Fill(DS, "login");

                DataTable aTable = DS.Tables["login"];

                DataColumn[] PrimaryKey = new DataColumn[1];
                PrimaryKey[0] = aTable.Columns["id"];
                aTable.PrimaryKey = PrimaryKey;

                DataRow currRow = aTable.Rows.Find(textBox15.Text);

                currRow.BeginEdit();


                currRow["pw"] = textBox16.Text;
                currRow["sname"] = textBox14.Text;
                currRow["age"] = textBox17.Text;
                currRow["tel"] = textBox18.Text;
                currRow["address"] = textBox19.Text;
                currRow["email"] = textBox12.Text;
                if (radioButton1.Checked == true)
                    currRow["gender"] = "남";
                if (radioButton2.Checked == true)
                    currRow["gender"] = "여";

                currRow.EndEdit();

                DataSet UpdatedSet = DS.GetChanges(DataRowState.Modified);
                if (UpdatedSet.HasErrors)
                {
                    MessageBox.Show("변경된 데이터에 문제가 있습니다.");
                }
                else
                {
                    DBAdapter.Update(UpdatedSet, "login");
                    DS.AcceptChanges();
                }

                dataGridView5.DataSource = DS.Tables["login"].DefaultView;

            }
            catch (DataException DE)
            {
                MessageBox.Show(DE.Message);
            }
            catch (Exception DE)
            {
                MessageBox.Show(DE.Message);
            }
        }

        private void button36_Click(object sender, EventArgs e)
        {
            bool idExist = false;

            try
            {
                if (textBox20.Text == "")
                {
                    MessageBox.Show("ID를 입력해주세요!");
                }
                else
                {
                    string connectionStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|" + @"\db.mdb";
                    string commandStr = "select * from login where id = \"" + textBox20.Text + "\"";


                    // 엑세스 파일 연결
                    OleDbDataAdapter DBAdapter = new OleDbDataAdapter(commandStr, connectionStr);
                    OleDbCommandBuilder mycommandBuilder = new OleDbCommandBuilder(DBAdapter);

                    // DataSet에 쿼리 결과 연결
                    DataSet DS = new DataSet();
                    DBAdapter.Fill(DS, "login");

                    DataTable aTable = DS.Tables["login"];

                    foreach (DataRow row in aTable.Rows)
                    {
                        if (textBox20.Text == row["id"].ToString())
                        {
                            idExist = true;

                            dataGridView5.DataSource = DS.Tables["login"].DefaultView;

                        }
                    }

                    if (!idExist)
                    {
                        MessageBox.Show("ID가 존재하지 않습니다.");
                    }

                }

            }
            catch (DataException DE)
            {
                MessageBox.Show(DE.Message);
            }
            catch (Exception DE)
            {
                MessageBox.Show(DE.Message);
            }
        }

        private void textBox20_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                // 버튼1(로그인버튼)클릭 이벤트 발생
                button36_Click(sender, e);
            }
        }

        private void button31_Click(object sender, EventArgs e)
        {
            try
            {
                string connectionStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|"
                                                       + @"\db.mdb";
                string commandStr = "select * from login";

                OleDbDataAdapter DBAdapter = new OleDbDataAdapter(commandStr, connectionStr);

                OleDbCommandBuilder mycommandBuilder = new OleDbCommandBuilder(DBAdapter);

                DataSet DS = new DataSet();

                DBAdapter.Fill(DS, "login");

                DataTable aTable = DS.Tables["login"];
                //////////////////////////////////////////////////////////////////////////////

                DataColumn[] Primarykey = new DataColumn[1];
                Primarykey[0] = aTable.Columns["id"];
                aTable.PrimaryKey = Primarykey;

                DataRow currRow = aTable.Rows.Find(textBox20.Text);
                if (textBox20.Text != "master")
                    currRow.Delete();
                else
                    MessageBox.Show("관리자는 삭제하실 수 없습니다.");

                DBAdapter.Update(DS.GetChanges(DataRowState.Deleted), "login");
                dataGridView5.DataSource = DS.Tables["login"].DefaultView;
            }
            catch (DataException)
            {
              
            }
            catch (Exception)
            {
                
            }
        }

        private void button33_Click_1(object sender, EventArgs e)
        {
            try
            {
                string connectionStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|"
                                                   + @"\db.mdb";
                // 쿼리문
                string commandStr = "select * from login";

                OleDbDataAdapter DBAdapter = new OleDbDataAdapter(commandStr, connectionStr);

                OleDbCommandBuilder mycommandBuilder = new OleDbCommandBuilder(DBAdapter);

                DataSet DS = new DataSet();

                // 엑세스 파일 연결
                DBAdapter.Fill(DS, "login");

                // 테이블 연결
                DataTable aTable = DS.Tables["login"];

                // 
                DataRow newRow = aTable.NewRow();

                if (textBox15.Text == "")
                    MessageBox.Show("ID를 입력해주세요.");
                else if (textBox16.Text == "")
                    MessageBox.Show("PASSWORD를 입력해주세요.");
                else if (textBox14.Text == "")
                    MessageBox.Show("이름를 입력해주세요.");
                else if (textBox17.Text == "")
                    MessageBox.Show("나이를 입력해주세요.");
                else if (textBox18.Text == "")
                    MessageBox.Show("전화번호를 입력해주세요.");
                else if (textBox19.Text == "")
                    MessageBox.Show("주소를 입력해주세요.");
                else if (textBox12.Text == "")
                    MessageBox.Show("e-mail를 입력해주세요.");
                else
                {
                    // 텍스트박스에 있는 값을 Row에 넣어준다.
                    newRow["id"] = textBox15.Text;
                    newRow["pw"] = textBox16.Text;
                    newRow["sname"] = textBox14.Text;
                    newRow["age"] = textBox17.Text;
                    newRow["tel"] = textBox18.Text;
                    newRow["address"] = textBox19.Text;
                    newRow["email"] = textBox12.Text;
                    if (radioButton1.Checked == true)
                        newRow["gender"] = "남";
                    if (radioButton2.Checked == true)
                        newRow["gender"] = "여";

                    // 테이블에 새로운 행 추가
                    aTable.Rows.Add(newRow);
            
                }

                // 세종 테이블을 업데이트
                DBAdapter.Update(DS, "login");

                // 데이터 그리드에 정보 출력
                dataGridView5.DataSource = DS.Tables["login"].DefaultView;

            }
            catch (DataException DE)
            {
                MessageBox.Show(DE.Message);
            }
            catch (Exception DE)
            {
                MessageBox.Show(DE.Message);
            }
        }

        private void dataGridView5_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string connectionStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|"
                                                  + @"\db.mdb";
            string commandStr = "select * from login";

            OleDbDataAdapter DBAdapter = new OleDbDataAdapter(commandStr, connectionStr);

            OleDbCommandBuilder mycommandBuilder = new OleDbCommandBuilder(DBAdapter);

            DataSet DS = new DataSet();

            DBAdapter.Fill(DS, "login");

            DataTable aTable = DS.Tables["login"];

            if (e.RowIndex < 0) //클릭한 행이 범위 초과시
            {
                return;
            }
            else if (e.RowIndex > aTable.Rows.Count - 1)
            {
                MessageBox.Show("잘못 클릭하셨습니다.");
                return;
            }

            DataRow currentRow = aTable.Rows[e.RowIndex];

            textBox20.Text = currentRow["id"].ToString();
            textBox15.Text = currentRow["id"].ToString();
            textBox16.Text = currentRow["pw"].ToString();
            textBox14.Text = currentRow["sname"].ToString();
            textBox17.Text = currentRow["age"].ToString();
            textBox18.Text = currentRow["tel"].ToString();
            textBox19.Text = currentRow["address"].ToString();
            textBox12.Text = currentRow["email"].ToString();
            //임시 성별
            if (currentRow["gender"].ToString() == "남")
                radioButton1.Checked = true;
            if (currentRow["gender"].ToString() == "여")
                radioButton2.Checked = true;
        }

        private void bringprice(string k)//각 메뉴의 가격을 pay변수에 저장시키는 함수
        {
            string tax = k;
            string connectionStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|" + @"\db.mdb";
            string commandStr = "select menu, price from food where menu = \"" + tax + "\"";

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

            pay = int.Parse(currRow["price"].ToString());//각 메뉴의 단일 가격 저장
        }

        // 주문내역 수정 버튼
        private void button17_Click(object sender, EventArgs e)
        {
            // 기존에 선택되어 있던 메뉴를 지운다.
            dataGridView1.Rows.Clear();

            string connectionStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|" + @"\db.mdb";
            string commandSelectAddress = "select distinct address from saveOrderList where tel = \"" + textBox1.Text + "\"";
            string commandSelectAll = "select orderDate, address, content, price from saveOrderList where tel = \"" + textBox1.Text + "\"";
            OleDbDataAdapter DBAdapter = new OleDbDataAdapter(commandSelectAddress, connectionStr);
            OleDbCommandBuilder mycommandBuilder = new OleDbCommandBuilder(DBAdapter);

            // DataSet에 쿼리 결과 연결
            DataSet DS = new DataSet();
            tabControl1.SelectedIndex = 0;

            textBox1.Text = tel;
            comboBox1.Items.Add(address);
            comboBox1.SelectedIndex = 0;
            comboBox1.SelectAll();
            DBAdapter = new OleDbDataAdapter(commandSelectAll, connectionStr);
            mycommandBuilder = new OleDbCommandBuilder(DBAdapter);

            DBAdapter.Fill(DS, "saveOrderLIst");
            DataTable saveOrderListTable = DS.Tables["saveOrderList"];


            // 데이터그리드뷰에 과거 주문기록 출력
            // dataSource 바인딩.
            dataGridView3.DataSource = saveOrderListTable;
            dataGridView3.Columns["orderDate"].HeaderText = "날짜";
            dataGridView3.Columns["address"].HeaderText = "주소";
            dataGridView3.Columns["content"].HeaderText = "주문내역";
            dataGridView3.Columns["price"].HeaderText = "가격";

            textBox5.Text = remarks;

            // 주문내역을 ','와 '/'로 구분하여 각각의 메뉴와 수량으로 다시 나눈다.
            String[] temp = content.Split(',', '/');

            for (int i = 0; i < temp.Length; i++)
            {
                temp[i] = temp[i].Trim();
            }

            for (int i = 0; i < temp.Length - 1; i += 2)
            {
                if (temp[i].Contains("(서)"))
                {
                    string menu = temp[i].Remove( temp[i].Length - 3 );
                    dataGridView1.Rows.Add(menu, temp[i + 1], "서비스");
                }
                else
                {
                    bringprice(temp[i]);
                    dataGridView1.Rows.Add(temp[i], temp[i + 1], pay * int.Parse(temp[i + 1]));
                }
            }
            renew = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            label23.Text = DateTime.Now.ToString("HH");
            label24.Text = DateTime.Now.ToString("mm");
            label28.Text = DateTime.Now.ToString("ss");
            //날짜
            label25.Text = DateTime.Now.ToString("yyyy년 MM월 dd일");
            
        }

        //서비스 버튼 클릭
        private void button40_Click(object sender, EventArgs e)
        {
            // 선택한 셀의 인덴스가 범위를 벗어나면 리턴
            if (selectedRowIndex < 0 || selectedRowIndex > dataGridView1.Rows.Count - 1)
            {
                return;
            }

            // 가격대신 서비스 출력
            dataGridView1.Rows[selectedRowIndex].Cells["price"].Value = "서비스";

            // 합계를 새로 구한다.
            CalSumOfCurrentOrder();
        }

        // 메뉴버튼의 컨텍스트 메뉴의 항목을 클릭했을 떄
        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ContextMenuStrip cms = (ContextMenuStrip)sender;

            if (e.ClickedItem.ToString() == "메뉴 편집")
            {
                Form4 menuEditForm = new Form4();
                // 편집하고자 하는 버튼을 넘겨준다.
                menuEditForm.selectedButton = (Button)cms.SourceControl;
                menuEditForm.ShowDialog();

                return;
            }

            if (e.ClickedItem.ToString() == "메뉴 삭제")
            {
                OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|" + @"\db.mdb");
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = con;

                cmd.CommandText = "UPDATE food SET menu =\'\' , price = 0 WHERE button = \'" + cms.SourceControl.Name + "\'";

                // 쿼리 실행
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                cms.SourceControl.Text = "";
            }
        }

        // DB에 저장된 메뉴를 메뉴버튼에 연결시켜주는 함수
        void bindingMenuToButton(Button b)
        {
            //메뉴버튼 텍스트 가져오기
            string connectionStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|" + @"\db.mdb";
            string commandStr = "select menu, price from food where button = \'" + b.Name + "\'";

            // 엑세스 파일 연결;
            OleDbDataAdapter DBAdapter = new OleDbDataAdapter(commandStr, connectionStr);
            OleDbCommandBuilder mycommandBuilder = new OleDbCommandBuilder(DBAdapter);

            // DataSet에 쿼리 결과 연결
            DataSet DS = new DataSet();
            DBAdapter.Fill(DS, "food");

            // DataSet으로부터 food 테이블 가져오기
            DataTable foodTable = DS.Tables["food"];

            if (foodTable.Rows[0]["menu"].ToString() == "") // 메뉴가 등록되어 있지 않은 경우
                b.Text = "";
            else
                // 버튼Text값 설정 (메뉴명 + 가격)
                b.Text = foodTable.Rows[0]["menu"].ToString() + "\n\\" + foodTable.Rows[0]["price"].ToString();
        }

        private void label29_Click(object sender, EventArgs e)
        {
            Form1 pv = new Form1();

            pv.ShowDialog();
            this.Visible = false;
        }

    }

}



