using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Text;

namespace demo
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }


        protected void Button1_Click(object sender, EventArgs e)
        {
            string connetionString;
            SqlConnection cnn;

            //connetionString = @"Data Source=DRTW-SEAQLIK\LOGISTICS_FDA;Initial Catalog=FDA ;User id=sa;Password=Bryant718!;";
            connetionString = @"Data Source=tcp:DRTW-SEAQLIK\LOGISTICS_FDA,49172;Initial Catalog=FDA ;User id=sa;Password=Bryant718!;";
            cnn = new SqlConnection(connetionString);

            cnn.Open();
            //build sql query
            string sqlcmd = "select BOL,[warehouse_date_time] as ArriveWarehouse,[A_ATD_DATE_Time] as LeaveWarehouse,[A_ATA_Date_Time] as ArriveCustomer,[S/Loc] from raw_inter where A_ATA_Date_Time Is Null or A_ATA_Date_Time < \'2019-01-01\';";
            SqlCommand cmd = new SqlCommand(sqlcmd, cnn);
            //initial adpater
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            //create dataset to hold query result
            DataTable ds = new DataTable();
            da.Fill(ds);


            cmd.ExecuteNonQuery();
            cnn.Close();
            
            //add textbox to ph2 according to # of rows
            int count_row = 0;
            int count_column = 0;

            /*
            Label l_bol = new Label();
            l_bol.Text = "BOL";
            //l_bol.Style["display"] = "block";
            //l_bol.Style["width"] = "100px";
            PH2.Controls.Add(l_bol);
            Label l_awhse = new Label();
            l_awhse.Text = "Arrive at Warehouse";
            //l_awhse.Style["display"] = "block";
            //l_awhse.Style["width"] = "100px";
            PH2.Controls.Add(l_awhse);
            Label l_gi = new Label();
            l_gi.Text = "Leave Warehouse";
            //l_gi.Style["display"] = "block";
           // l_gi.Style["width"] = "100px";
            PH2.Controls.Add(l_gi);
            Label l_ata = new Label();
            l_ata.Text = "Arrive at Customer";
            //l_ata.Style["display"] = "block";
            //l_ata.Style["width"] = "100px";
            PH2.Controls.Add(l_ata);


            PH2.Controls.Add(new Literal { Text = "<br></br>" }); */

            //test header with th
            PH2.Controls.Add(new Literal { Text = "<table id = 'tb1' border = '1' align = 'center' class = 'table table-striped'>" });
            PH2.Controls.Add(new Literal { Text = "<tr>" });
            
            foreach (DataColumn column in ds.Columns)
            {
                PH2.Controls.Add(new Literal { Text = "<th>" });
                PH2.Controls.Add(new Literal { Text = column.ColumnName });
                PH2.Controls.Add(new Literal { Text = "</th>" });
            }

            PH2.Controls.Add(new Literal { Text = "</tr>" });

            foreach (DataRow row in ds.Rows)
            {
                PH2.Controls.Add(new Literal { Text = "<tr>" });
                foreach (DataColumn column in ds.Columns)
                {
                    //if missing milestones, use datatime as textboxmode
                    PH2.Controls.Add(new Literal { Text = "<td>" });
                    if (row[column.ColumnName].ToString() != "")
                    {
                        TextBox t = new TextBox();
                        t.ID = "tbox" + count_row + "_" + count_column;
                        t.Text = row[column.ColumnName].ToString();
                        t.TextMode = TextBoxMode.SingleLine;
                        t.ReadOnly = true;
                        t.Columns = 30;
                        t.BorderStyle = BorderStyle.None;
                     
                        //add to PH2
                        PH2.Controls.Add(t);
                    }
                    else
                    {
                        TextBox t = new TextBox();
                        t.ID = "tbox" + count_row + "_" + count_column;
                        t.Text = row[column.ColumnName].ToString();
                        t.TextMode = TextBoxMode.DateTimeLocal;
                        t.Columns = 30;
                        t.BorderStyle = BorderStyle.None;
                        //add to PH2
                        PH2.Controls.Add(t);

                    }

                    count_column++;
                    PH2.Controls.Add(new Literal { Text = "</td>" });
                }
                //add a break
                PH2.Controls.Add(new Literal { Text = "</tr>" });
                count_row++;
                count_column = 0;

            }


            //store number of rows 

            
            PH2.Controls.Add(new Literal { Text = "</table>" });



            /*

                for (int i = 0; i <= ds.Rows.Count; i++)
                {
                TextBox t = new TextBox();
                t.ID = "tbox" + i;
                //t.Text = "test";
                t.TextMode = TextBoxMode.DateTime;
                
                PH2.Controls.Add(t);
                }

            

            Response.Write(ds.Rows.Count); */

        }

        protected void Button2_Click(object sender, EventArgs e)
        {


            //create connection string
            string connetionString;
            //connetionString = @"Data Source=DRTW-SEAQLIK\LOGISTICS_FDA;Initial Catalog=FDA ;User id=sa;Password=Bryant718!;";
            connetionString = @"Data Source=tcp:DRTW-SEAQLIK\LOGISTICS_FDA,49172;Initial Catalog=FDA ;User id=sa;Password=Bryant718!; ";

            SqlConnection cnn = new SqlConnection(connetionString);

            cnn.Open();
            //get no of rows
            SqlCommand cmd_rc = new SqlCommand("select count(1) from raw_inter where A_ATA_Date_Time Is Null or A_ATA_Date_Time < \'2019-01-01\';", cnn);
            int nofrows = Int32.Parse(cmd_rc.ExecuteScalar().ToString());

            if (nofrows == 0)
            {
                PH2.Controls.Add(new Literal { Text = "<h> No Records to update </h>" });
            }
            else
            {


                SqlCommand cmd = new SqlCommand("[dbo].[update_milestone]", cnn);

                //update each entered value
                cmd.CommandType = CommandType.StoredProcedure; //define command type

                cmd.Parameters.AddWithValue("@bol", "test");
                cmd.Parameters.AddWithValue("@column_need_update", "warehouse_date_time");
                cmd.Parameters.AddWithValue("@status_column", "arrive_warehouse_status");
                cmd.Parameters.AddWithValue("@date_need_update", "2020-01-10 18:00:00.000");
                cmd.Parameters.AddWithValue("@p_column", "planned_pickup");
                for (int i = 0; i < nofrows; i++)
                {

                    string arrivewhse = Request.Form["tbox" + i + "_1"];
                    string leavewhse = Request.Form["tbox" + i + "_2"];
                    string ata = Request.Form["tbox" + i + "_3"];
                    if (arrivewhse != "") //if not null then update arrive warehouse
                    {
                        cmd.Parameters["@bol"].Value = Request.Form["tbox" + i + "_0"];
                        cmd.Parameters["@column_need_update"].Value = "warehouse_date_time";
                        cmd.Parameters["@status_column"].Value = "arrive_warehouse_status";
                        DateTime inputDate = DateTime.Parse(arrivewhse);
                        cmd.Parameters["@date_need_update"].Value = inputDate.ToString("yyyy - MM - dd HH: mm:ss");
                        cmd.Parameters["@p_column"].Value = "planned_pickup";
                        cmd.ExecuteNonQuery();
                    }
                    if (leavewhse != "")
                    {
                        cmd.Parameters["@bol"].Value = Request.Form["tbox" + i + "_0"];
                        cmd.Parameters["@column_need_update"].Value = "A_ATD_DATE_Time";
                        cmd.Parameters["@status_column"].Value = "depart_warehouse_status";
                        DateTime inputDate = DateTime.Parse(leavewhse);
                        cmd.Parameters["@date_need_update"].Value = inputDate.ToString("yyyy - MM - dd HH: mm:ss");
                        cmd.Parameters["@p_column"].Value = "Planned_GI_DataTime";
                        cmd.ExecuteNonQuery();
                    }
                    if (ata != "")
                    {
                        cmd.Parameters["@bol"].Value = Request.Form["tbox" + i + "_0"];
                        cmd.Parameters["@column_need_update"].Value = "A_ATA_Date_Time";
                        cmd.Parameters["@status_column"].Value = "ATA_Status";
                        DateTime inputDate = DateTime.Parse(ata);
                        cmd.Parameters["@date_need_update"].Value = inputDate.ToString("yyyy - MM - dd HH: mm:ss");
                        cmd.Parameters["@p_column"].Value = "Last_Del_Time";
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            cnn.Close();


            //create connection string
            string connetionString_local ;
            //connetionString = @"Data Source=DRTW-SEAQLIK\LOGISTICS_FDA;Initial Catalog=FDA ;User id=sa;Password=Bryant718!;";
            connetionString_local = @"Data Source=tcp:DRTW-SEAQLIK\LOGISTICS_FDA,49172;Initial Catalog=FDA ;User id=sa;Password=Bryant718!;";

            SqlConnection cnn_local = new SqlConnection(connetionString_local);

            cnn_local.Open();
            //get no of rows
            SqlCommand cmd_rc_local = new SqlCommand("select count(1) from raw_inter where A_ATA_Date_Time Is Null or A_ATA_Date_Time < \'2019-01-01\';", cnn_local);
            int nofrows_local = Int32.Parse(cmd_rc_local.ExecuteScalar().ToString());

            if (nofrows_local == 0)
            {
                PH2.Controls.Add(new Literal { Text = "<h> No Records to update </h>" });
            }
            else
            {


                SqlCommand cmd_local = new SqlCommand("[dbo].[update_milestone]", cnn_local);

                //update each entered value
                cmd_local.CommandType = CommandType.StoredProcedure; //define command type

                cmd_local.Parameters.AddWithValue("@bol", "test");
                cmd_local.Parameters.AddWithValue("@column_need_update", "warehouse_date_time");
                cmd_local.Parameters.AddWithValue("@status_column", "arrive_warehouse_status");
                cmd_local.Parameters.AddWithValue("@date_need_update", "2020-01-10 18:00:00.000");
                cmd_local.Parameters.AddWithValue("@p_column", "planned_pickup");
                for (int i = 0; i < nofrows; i++)
                {

                    string arrivewhse = Request.Form["tbox" + i + "_1"];
                    string leavewhse = Request.Form["tbox" + i + "_2"];
                    string ata = Request.Form["tbox" + i + "_3"];
                    if (arrivewhse != "") //if not null then update arrive warehouse
                    {
                        cmd_local.Parameters["@bol"].Value = Request.Form["tbox" + i + "_0"];
                        cmd_local.Parameters["@column_need_update"].Value = "warehouse_date_time";
                        cmd_local.Parameters["@status_column"].Value = "arrive_warehouse_status";
                        DateTime inputDate = DateTime.Parse(arrivewhse);
                        cmd_local.Parameters["@date_need_update"].Value = inputDate.ToString("yyyy - MM - dd HH: mm:ss");
                        cmd_local.Parameters["@p_column"].Value = "planned_pickup";
                        cmd_local.ExecuteNonQuery();
                    }
                    if (leavewhse != "")
                    {
                        cmd_local.Parameters["@bol"].Value = Request.Form["tbox" + i + "_0"];
                        cmd_local.Parameters["@column_need_update"].Value = "A_ATD_DATE_Time";
                        cmd_local.Parameters["@status_column"].Value = "depart_warehouse_status";
                        DateTime inputDate = DateTime.Parse(leavewhse);
                        cmd_local.Parameters["@date_need_update"].Value = inputDate.ToString("yyyy - MM - dd HH: mm:ss");
                        cmd_local.Parameters["@p_column"].Value = "Planned_GI_DataTime";
                        cmd_local.ExecuteNonQuery();
                    }
                    if (ata != "")
                    {
                        cmd_local.Parameters["@bol"].Value = Request.Form["tbox" + i + "_0"];
                        cmd_local.Parameters["@column_need_update"].Value = "A_ATA_Date_Time";
                        cmd_local.Parameters["@status_column"].Value = "ATA_Status";
                        DateTime inputDate = DateTime.Parse(ata);
                        cmd_local.Parameters["@date_need_update"].Value = inputDate.ToString("yyyy - MM - dd HH: mm:ss");
                        cmd_local.Parameters["@p_column"].Value = "Last_Del_Time";
                        cmd_local.ExecuteNonQuery();
                    }
                }
            }
            cnn_local.Close();

        }    

        /* if (CheckReadOnly(  PH2.FindControl("tbox0_1")))
         {
             Response.Write("test success");
         }
         */
        /*
       if (PH2.Controls.Cast<Control>().First(item => item.ID == "tbox0_1") )
        {
            Response.Write("test success");
        }

       */
        /*
        cnn = new SqlConnection(connetionString);

        cnn.Open();
        //build sql query
        string sqlcmd = "select BOL,[warehouse_date_time] as ArriveWarehouse,[A_ATD_DATE_Time] as LeaveWarehouse,[A_ATA_Date_Time] as ArriveCustomer from raw_inter where A_ATA_Date_Time Is Null or A_ATA_Date_Time < \'2019-01-01\';";
        //SqlCommand cmd = new SqlCommand(sqlcmd, cnn);
        SqlDataAdapter d = new SqlDataAdapter();

*/

    

        

        private bool CheckReadOnly(Control Ctrl)
        {
            bool isReadOnly = false;
            if (((TextBox)Ctrl).ReadOnly == true)
            {
                isReadOnly = true;
            }
            else
            {
                isReadOnly = false;
            }
            return isReadOnly;
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            Server.Transfer("UpdateReasonAPS.aspx");
        }

        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            Server.Transfer("UpdateReasonAPSDel.aspx");
        }
    }
}
