using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;


namespace demo
{
    public partial class UpdateReasonAPS : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                for (int i = 1; i <= 52; i++)
                {
                    DropDownList1.Items.Add(new ListItem("Week " + i, i.ToString()));
                }
            }


            
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //Response.Write(DropDownList1.SelectedValue);
            //Create connection
            if (DropDownList1.SelectedValue != "YTD")
            {
                string connetionString;
                SqlConnection cnn;

                connetionString = @"Data Source=DRTW-SEAQLIK\LOGISTICS_FDA;Initial Catalog=FDA ;User id=sa;Password=Bryant718!;";

                cnn = new SqlConnection(connetionString);

                cnn.Open();
                //build sql query ON tTime Pick UP

                string wk = DropDownList1.SelectedValue;
                string sqlcmd = @"select 
                            [week], Carrier, MnsTrnspID,
                            a.[BOL],[QtyByBOL],[Ship-to Party(Text)],
                            [planned_pickup],[warehouse_date_time],[arrive_warehouse_status],iif([arrive_warehouse_status]< -30, 'Late','on time') as warehouse_in_status,b.reason,b.comment
                            from
                            weekly_tracking a
                            left join
                            failure_tracking b
                            on a.BOL = b.BOL
                            where a.arrive_warehouse_status<-30 and b.reason is null and week = @wk;";
                SqlCommand cmd = new SqlCommand(sqlcmd, cnn);
                cmd.Parameters.AddWithValue("@wk", wk);
                //initial adpater
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                //create dataset to hold query result
                DataTable ds = new DataTable();
                da.Fill(ds);


                cmd.ExecuteNonQuery();


                cnn.Close();



                //write table to html
                PlaceHolder1.Controls.Add(new Literal { Text = "<table border = '1' align = 'center' class = 'table table-striped'>" });
                PlaceHolder1.Controls.Add(new Literal { Text = "<tr>" });

                foreach (DataColumn column in ds.Columns)
                {
                    PlaceHolder1.Controls.Add(new Literal { Text = "<th>" });
                    PlaceHolder1.Controls.Add(new Literal { Text = column.ColumnName });
                    PlaceHolder1.Controls.Add(new Literal { Text = "</th>" });
                }

                PlaceHolder1.Controls.Add(new Literal { Text = "</tr>" });
                //fill in the details
                int count_row = 0;
                int count_column = 0;
                foreach (DataRow row in ds.Rows)
                {
                    PlaceHolder1.Controls.Add(new Literal { Text = "<tr>" });
                    foreach (DataColumn column in ds.Columns)
                    {
                        //if missing milestones, use datatime as textboxmode
                        PlaceHolder1.Controls.Add(new Literal { Text = "<td>" });
                        if (column.ColumnName.ToString() == "reason")
                        {
                            DropDownList d = new DropDownList();
                            d.Items.Add(new ListItem("Please select a Reason"));
                            d.Items.Add(new ListItem("Carrier Accident"));
                            d.Items.Add(new ListItem("Carrier CSR Error"));
                            d.Items.Add(new ListItem("Dispatch System Issue"));
                            d.Items.Add(new ListItem("Driver Error"));
                            d.Items.Add(new ListItem("Driver Lost"));
                            d.Items.Add(new ListItem("Driver Performance"));
                            d.Items.Add(new ListItem("Driver Sick"));
                            d.Items.Add(new ListItem("Fueling Delay"));
                            d.Items.Add(new ListItem("HOS Issue"));
                            d.Items.Add(new ListItem("Mechanical"));
                            d.Items.Add(new ListItem("Previous Unload"));
                            d.Items.Add(new ListItem("Traffic"));
                            d.Items.Add(new ListItem("Trailer Search"));
                            d.Items.Add(new ListItem("Weather"));
                            d.Items.Add(new ListItem("No Issue"));
                            d.ID = "dropdown_" + count_row;
                            //add to PH2
                            PlaceHolder1.Controls.Add(d);
                        }

                        if (column.ColumnName.ToString() == "comment")
                        {
                            TextBox t = new TextBox();
                            t.ID = "tbox_comment_" + count_row;
                            t.Text = row[column.ColumnName].ToString();
                            t.TextMode = TextBoxMode.SingleLine;
                            //add to PH2
                            PlaceHolder1.Controls.Add(t);

                        }

                        if (column.ColumnName.ToString() != "reason" && column.ColumnName.ToString() != "comment")
                        {
                            TextBox t = new TextBox();
                            t.ID = "tbox" + count_row + "_" + count_column;
                            t.Text = row[column.ColumnName].ToString();
                            t.TextMode = TextBoxMode.SingleLine;
                            t.ReadOnly = true;
                            //add to PH2
                            PlaceHolder1.Controls.Add(t);

                        }

                        count_column++;
                        PlaceHolder1.Controls.Add(new Literal { Text = "</td>" });
                    }
                    //add a break
                    PlaceHolder1.Controls.Add(new Literal { Text = "</tr>" });
                    count_row++;
                    count_column = 0;

                }

            }
            
            

        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            string connetionString;
            connetionString = @"Data Source=DRTW-SEAQLIK\LOGISTICS_FDA;Initial Catalog=FDA ;User id=sa;Password=Bryant718!;";

            SqlConnection cnn = new SqlConnection(connetionString);

            cnn.Open();
            //get row count
            string wk = DropDownList1.SelectedValue;
            string sqlcmd_rc = @"select 
                            count(1)
                            from
                            weekly_tracking a
                            left join
                            failure_tracking b
                            on a.BOL = b.BOL
                            where a.arrive_warehouse_status<-30 and b.reason is null and week = @wk;";
            SqlCommand cmd_rc = new SqlCommand(sqlcmd_rc, cnn);
            cmd_rc.Parameters.AddWithValue("@wk", wk);
            int noOfRows = Int32.Parse(cmd_rc.ExecuteScalar().ToString());
            //create connection
            if (noOfRows == 0)
            {
                //PlaceHolder1.Visible = false;
                PH2.Controls.Add(new Literal { Text = "<h> No Records to update </h>" });
            }
            else
            {


                //using sql to find if bol exists if not add to failure tracking
                string merge_sql = @" merge failure_tracking  using (select distinct BOL from weekly_tracking where  [arrive_warehouse_status]< -30 and [week] = @wk) x
                                on  failure_tracking.BOL =x.BOL
                                when not matched by target
                                then 
                                insert (BOL,ind_field) values (x.BOL,'p') ;
                                    
                                ";
                SqlCommand cmd_merge = new SqlCommand(merge_sql, cnn);
                cmd_merge.Parameters.AddWithValue("@wk", wk);
                cmd_merge.ExecuteNonQuery();


                // using sp to update failure reason
                SqlCommand cmd = new SqlCommand("[UpdateCarrierReason]", cnn);

                //update each entered value
                cmd.CommandType = CommandType.StoredProcedure; //define command type
                                                               //inintialize parameters
                cmd.Parameters.AddWithValue("@bol", "test");
                cmd.Parameters.AddWithValue("@reason", "warehouse_date_time");
                cmd.Parameters.AddWithValue("@comment", Request.Form["tbox_comment_0"]);
                cmd.Parameters.AddWithValue("@ind_field", "p");


                for (int i = 0; i < noOfRows; i++)
                {
                    cmd.Parameters["@bol"].Value = Request.Form["tbox" + i + "_3"];
                    cmd.Parameters["@reason"].Value = Request.Form["dropdown_" + i];
                    cmd.Parameters["@comment"].Value = Request.Form["tbox_comment_" + i];
                    cmd.Parameters["@ind_field"].Value = "p";
                    cmd.ExecuteNonQuery();

                }
                
            }

            cnn.Close();

            PH2.Controls.Add(new Literal { Text = "<h> Number of Records to Update: " + noOfRows + "</h>" });






        }

        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            Server.Transfer("UpdateReasonAPSDel.aspx");
        }

        protected void LinkButton3_Click(object sender, EventArgs e)
        {
            Response.Redirect("https://drtw-seaqlik.sea.samsung.com/hub");
        }

        protected void LinkButton4_Click(object sender, EventArgs e)
        {
            Server.Transfer("AddLatePU.aspx");
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            string connetionString;
            SqlConnection cnn;
            //connetionString = @"Data Source=tcp:DRTW-SEAQLIK\LOGISTICS_FDA,49172;Initial Catalog=FDA ;User id=sa;Password=Bryant718!;";
            connetionString = @"Data Source=DRTW-SEAQLIK\LOGISTICS_FDA;Initial Catalog=FDA ;User id=sa;Password=Bryant718!;";
            cnn = new SqlConnection(connetionString);

            cnn.Open();
            //build sql query ON tTime Del

            string wk = DropDownList1.SelectedValue;
            string sqlcmd = @"select 
                            [week], Carrier, MnsTrnspID,
                            a.[BOL],[QtyByBOL],[Ship-to Party(Text)],
                            [planned_pickup],[warehouse_date_time],[arrive_warehouse_status],iif([arrive_warehouse_status]< -30, 'Late','on time') as warehouse_in_status,b.reason,b.comment
                            from
                            weekly_tracking a
                            left join
                            failure_tracking b
                            on a.BOL = b.BOL
                            where a.arrive_warehouse_status<-30 and week = @wk;
                            ";
            string sqlcmd_all = @"select 
                            [week], Carrier, MnsTrnspID,
                            a.[BOL],[QtyByBOL],[Ship-to Party(Text)],
                            [planned_pickup],[warehouse_date_time],[arrive_warehouse_status],iif([arrive_warehouse_status]< -30, 'Late','on time') as warehouse_in_status,b.reason,b.comment
                            from
                            weekly_tracking a
                            left join
                            failure_tracking b
                            on a.BOL = b.BOL
                            where a.arrive_warehouse_status<-30;
                            ";
            if (wk == "YTD")
            {
                SqlCommand cmd = new SqlCommand(sqlcmd_all, cnn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                //create dataset to hold query result
                DataTable ds = new DataTable();
                da.Fill(ds);
                cmd.ExecuteNonQuery();
                cnn.Close();
                //create table to Display

                PlaceHolder1.Controls.Add(new Literal { Text = "<table border = '1' align = 'center' class = 'table table-striped'>" });
                PlaceHolder1.Controls.Add(new Literal { Text = "<tr>" });

                foreach (DataColumn column in ds.Columns)
                {
                    PlaceHolder1.Controls.Add(new Literal { Text = "<th>" });
                    PlaceHolder1.Controls.Add(new Literal { Text = column.ColumnName });
                    PlaceHolder1.Controls.Add(new Literal { Text = "</th>" });
                }

                PlaceHolder1.Controls.Add(new Literal { Text = "</tr>" });
                //fill in the details
                foreach (DataRow row in ds.Rows)
                {
                    PlaceHolder1.Controls.Add(new Literal { Text = "<tr>" });
                    foreach (DataColumn column in ds.Columns)
                    {
                        //if missing milestones, use datatime as textboxmode
                        PlaceHolder1.Controls.Add(new Literal { Text = "<td>" });

                        PlaceHolder1.Controls.Add(new Literal { Text = row[column.ColumnName].ToString() });

                        PlaceHolder1.Controls.Add(new Literal { Text = "</td>" });
                    }



                }
                //add a break
                PlaceHolder1.Controls.Add(new Literal { Text = "</tr>" });


            }




            else
            {
                SqlCommand cmd = new SqlCommand(sqlcmd, cnn);
                cmd.Parameters.AddWithValue("@wk", wk);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                //create dataset to hold query result
                DataTable ds = new DataTable();
                da.Fill(ds);


                cmd.ExecuteNonQuery();
                cnn.Close();
                //create table to Display

                PlaceHolder1.Controls.Add(new Literal { Text = "<table border = '1' align = 'center' class = 'table table-striped'>" });
                PlaceHolder1.Controls.Add(new Literal { Text = "<tr>" });

                foreach (DataColumn column in ds.Columns)
                {
                    PlaceHolder1.Controls.Add(new Literal { Text = "<th>" });
                    PlaceHolder1.Controls.Add(new Literal { Text = column.ColumnName });
                    PlaceHolder1.Controls.Add(new Literal { Text = "</th>" });
                }

                PlaceHolder1.Controls.Add(new Literal { Text = "</tr>" });
                //fill in the details
                foreach (DataRow row in ds.Rows)
                {
                    PlaceHolder1.Controls.Add(new Literal { Text = "<tr>" });
                    foreach (DataColumn column in ds.Columns)
                    {
                        //if missing milestones, use datatime as textboxmode
                        PlaceHolder1.Controls.Add(new Literal { Text = "<td>" });

                        PlaceHolder1.Controls.Add(new Literal { Text = row[column.ColumnName].ToString() });

                        PlaceHolder1.Controls.Add(new Literal { Text = "</td>" });
                    }



                }
                //add a break
                PlaceHolder1.Controls.Add(new Literal { Text = "</tr>" });
            }
        }
        /*
public List<string> FetchWeeks(int year)
{
List<string> weeks = new List<string>();
DateTime startDate = new DateTime(year, 1, 1);
startDate = startDate.AddDays(1 - (int)startDate.DayOfWeek);
DateTime endDate = startDate.AddDays(6);
while (startDate.Year < 1 + year)
{
weeks.Add(string.Format("Week: From {0:dd/MM/yyyy}to {1:dd/MM/yyyy}", startDate, endDate));
startDate = startDate.AddDays(7);
endDate = endDate.AddDays(7);
}
return weeks;
}
*/
    }

}