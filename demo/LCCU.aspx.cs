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
    public partial class LCCU : System.Web.UI.Page
    {

        private bool checkReadOnly(Control Ctrl)
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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //DropDownList1.Items.Add(new ListItem("YTD"));

                for (int i = 1; i <= 52; i++)
                {
                    DropDownList1.Items.Add(new ListItem("Week " + i, i.ToString()));
                }
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            PlaceHolder1.Visible = true;
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Data Source=DRTW-SEAQLIK\LOGISTICS_FDA;Initial Catalog=FDA ;User id=sa;Password=Bryant718!;";
            cnn = new SqlConnection(connetionString);
            cnn.Open();

            string wk = DropDownList1.SelectedValue;
            string sqlcmd = @"[dbo].[find_not_lccu]";
            SqlCommand cmd = new SqlCommand(sqlcmd, cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@wk", wk);
            //initial adpater
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            //create dataset to hold query result
            DataTable ds = new DataTable();
            da.Fill(ds);
            cmd.ExecuteNonQuery();

            cnn.Close();
            //Build Table at PH1
            //write table to html
            PlaceHolder1.Controls.Add(new Literal { Text = "<table border = '1' align = 'center'>" });
            PlaceHolder1.Controls.Add(new Literal { Text = "<tr>" });
            //add header
            foreach (DataColumn column in ds.Columns)
            {
                PlaceHolder1.Controls.Add(new Literal { Text = "<th>" });
                PlaceHolder1.Controls.Add(new Literal { Text = column.ColumnName });
                PlaceHolder1.Controls.Add(new Literal { Text = "</th>" });
            }
            // add Reason and comment
           // PlaceHolder1.Controls.Add(new Literal { Text = "<th>Reason</th>" });

           // PlaceHolder1.Controls.Add(new Literal { Text = "<th> Comment </th>" });
            //Fill in Data
            PlaceHolder1.Controls.Add(new Literal { Text = "</tr>" });
            //fill in the details
            int count_row = 0;
            //int count_column = 0;
            foreach (DataRow row in ds.Rows)
            {
                PlaceHolder1.Controls.Add(new Literal { Text = "<tr>" });
                foreach (DataColumn column in ds.Columns)
                {
                    //if BOL,Reason and comment use textbox 
                    PlaceHolder1.Controls.Add(new Literal { Text = "<td>" });
                    if(column.ColumnName.ToString() == "BOL")
                    {
                        TextBox t = new TextBox();
                        t.ID = "BOL_" + count_row; 
                        t.Text = row[column.ColumnName].ToString();
                        t.TextMode = TextBoxMode.SingleLine;
                        t.ReadOnly = true;
                        //add to PH2
                        PlaceHolder1.Controls.Add(t);
                    }
                    
                    else if(column.ColumnName.ToString() != "Reason" && column.ColumnName.ToString() != "Comment")
                    {
                        //PlaceHolder1.Controls.Add(new Literal { Text = "<td>" });

                        PlaceHolder1.Controls.Add(new Literal { Text = row[column.ColumnName].ToString() });

                    }

                    //check if column name =reason and is not null,add as text
                    else if (column.ColumnName.ToString() == "Reason" && row[column.ColumnName].ToString() != "" )
                    {
                        //PlaceHolder1.Controls.Add(new Literal { Text = "<td>" });

                        PlaceHolder1.Controls.Add(new Literal { Text = row[column.ColumnName].ToString() });
                        PlaceHolder1.Controls.Add(new Literal { Text = "</td>" });
                        PlaceHolder1.Controls.Add(new Literal { Text = "<td>" });
                        PlaceHolder1.Controls.Add(new Literal { Text = row["Comment"].ToString() });

                    }
                    //add Reason if reason is  null
                    else if (column.ColumnName.ToString() == "Reason" && row[column.ColumnName].ToString() == "")
                    {
                        //PlaceHolder1.Controls.Add(new Literal { Text = "<td>" });

                        //PlaceHolder1.Controls.Add(new Literal { Text = row[column.ColumnName].ToString() });
                        //add 
                        DropDownList d = new DropDownList();
                        d.Items.Add(new ListItem("Please select a Reason"));
                        d.Items.Add(new ListItem("BBY - SCN"));
                        d.Items.Add(new ListItem("Capacity"));
                        d.Items.Add(new ListItem("Destination Change"));
                        d.Items.Add(new ListItem("DF Order Cancellation"));
                        d.Items.Add(new ListItem("Late Order Request"));
                        d.Items.Add(new ListItem("Multi-Stop"));
                        d.Items.Add(new ListItem("No Show"));
                        d.Items.Add(new ListItem("Shift Charges"));
                        d.Items.Add(new ListItem("Spot Rate"));
                        d.Items.Add(new ListItem("All Other Reasons"));
                        d.Items.Add(new ListItem("Tender Rejected"));
                        d.ID = "Reason_" + count_row;
                        PlaceHolder1.Controls.Add(d);
                        //add coment
                        PlaceHolder1.Controls.Add(new Literal { Text = "</td>" });
                        PlaceHolder1.Controls.Add(new Literal { Text = "<td>" });
                        TextBox t1 = new TextBox();
                        t1.ID = "comment_" + count_row;
                        t1.TextMode = TextBoxMode.SingleLine;

                        //add to PH2
                        PlaceHolder1.Controls.Add(t1);

                    }


                    PlaceHolder1.Controls.Add(new Literal { Text = "</td>" });

                }
                //add Reason and Comment at the before of the tr
               // PlaceHolder1.Controls.Add(new Literal { Text = "<td>" });
                //Reason here, use dropdown list
                /*
                DropDownList d = new DropDownList();
                d.Items.Add(new ListItem("Please select a Reason"));
                d.Items.Add(new ListItem("BBY - SCN"));
                d.Items.Add(new ListItem("Capacity"));
                d.Items.Add(new ListItem("Destination Change"));
                d.Items.Add(new ListItem("DF Order Cancellation"));
                d.Items.Add(new ListItem("Late Order Request"));
                d.Items.Add(new ListItem("Multi-Stop"));
                d.Items.Add(new ListItem("No Show"));
                d.Items.Add(new ListItem("Shift Charges"));
                d.Items.Add(new ListItem("Spot Rate"));
                d.Items.Add(new ListItem("All Other Reasons"));
                d.Items.Add(new ListItem("Tender Rejected"));
                d.ID = "Reason_" + count_row;

                PlaceHolder1.Controls.Add(d);
                PlaceHolder1.Controls.Add(new Literal { Text = "</td>" });

                PlaceHolder1.Controls.Add(new Literal { Text = "<td>" });
                //add comment
                TextBox t1 = new TextBox();
                t1.ID = "comment_" + count_row;
                t1.TextMode = TextBoxMode.SingleLine;

                //add to PH2
                PlaceHolder1.Controls.Add(t1);
                PlaceHolder1.Controls.Add(new Literal { Text = "</td>" });
                */

                PlaceHolder1.Controls.Add(new Literal { Text = "</tr>" });

                count_row++;
            }









        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            string connetionString;
            SqlConnection cnn;

            connetionString = @"Data Source=DRTW-SEAQLIK\LOGISTICS_FDA;Initial Catalog=FDA ;User id=sa;Password=Bryant718!;";

            cnn = new SqlConnection(connetionString);

            cnn.Open();
            
            string sqlcmd = @" if not exists(select 1 from LCCU_Failure_Tracking where BOL = @bol )
                                
                                insert into LCCU_Failure_Tracking(BOL,Reason,Comment) values (@bol,@reason,@comment)
                                
                                else
                                
                                update[LCCU_Failure_Tracking]set Reason = @reason, Comment = @comment where BOL = @bol


                               ";
            //        if not exists(select 1 from LCCU_Failure_Tracking where BOL = @bol                        else                               update[LCCU_Failure_Tracking]set Reason = @reason, Comment = @commentwhere BOL = @bol;
            SqlCommand cmd = new SqlCommand(sqlcmd, cnn);
            cmd.Parameters.AddWithValue("@bol", "test");
            cmd.Parameters.AddWithValue("@reason", "test");
            cmd.Parameters.AddWithValue("@comment", "test");


            int rowsupdated = 0;
            //get value from user input
            for (int i = 0; i < 20; i++)
            {
                if(Request.Form["BOL_" +i] !=null && Request.Form["Reason_" + i] != null&& Request.Form["Reason_" + i] != "Please select a Reason")
                {
                    cmd.Parameters["@bol"].Value = Request.Form["BOL_" + i];
                    cmd.Parameters["@reason"].Value = Request.Form["Reason_" + i];
                    cmd.Parameters["@comment"].Value = Request.Form["comment_" + i];                
                    cmd.ExecuteNonQuery();
                    rowsupdated++;

                }
                /*
                else if (Request.Form["BOL_" + i] != null && Request.Form["Reason_" + i] != null && Request.Form["Reason_" + i] == "Please select a Reason")
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "ErrorAlert", "alert('Please select a reason');", true);

                }
                */
            }

            PlaceHolder1.Visible = false;
            PH2.Controls.Add(new Literal { Text = "<h> Number of Records to Update: " + rowsupdated + "</h>" });





        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            PlaceHolder1.Visible = true;
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Data Source=DRTW-SEAQLIK\LOGISTICS_FDA;Initial Catalog=FDA ;User id=sa;Password=Bryant718!;";
            cnn = new SqlConnection(connetionString);
            cnn.Open();

            string wk = DropDownList1.SelectedValue;
            string sqlcmd = @"[dbo].[find_not_lccu]";
            SqlCommand cmd = new SqlCommand(sqlcmd, cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@wk", 0);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            //create dataset to hold query result
            DataTable ds = new DataTable();
            da.Fill(ds);
            cmd.ExecuteNonQuery();

            cnn.Close();
            //Build Table at PH1
            //write table to html
            PlaceHolder1.Controls.Add(new Literal { Text = "<table border = '1' align = 'center'>" });
            PlaceHolder1.Controls.Add(new Literal { Text = "<tr>" });
            //add header
            foreach (DataColumn column in ds.Columns)
            {
                PlaceHolder1.Controls.Add(new Literal { Text = "<th>" });
                PlaceHolder1.Controls.Add(new Literal { Text = column.ColumnName });
                PlaceHolder1.Controls.Add(new Literal { Text = "</th>" });
            }

            PlaceHolder1.Controls.Add(new Literal { Text = "</tr>" });
            //fill in the details
            
            //int count_column = 0;
            foreach (DataRow row in ds.Rows)
            {
                PlaceHolder1.Controls.Add(new Literal { Text = "<tr>" });
                foreach (DataColumn column in ds.Columns)
                {
                    //if BOL,Reason and comment use textbox 
                    PlaceHolder1.Controls.Add(new Literal { Text = "<td>" });
                    
                        //PlaceHolder1.Controls.Add(new Literal { Text = "<td>" });

                        PlaceHolder1.Controls.Add(new Literal { Text = row[column.ColumnName].ToString() });


                    


                    PlaceHolder1.Controls.Add(new Literal { Text = "</td>" });

                }
                

                PlaceHolder1.Controls.Add(new Literal { Text = "</tr>" });

                
            }




        }

        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            Response.Redirect("https://drtw-seaqlik.sea.samsung.com/sense/app/d2ad9844-5c9c-4299-b54d-058fe7e4a859/sheet/64221d93-4899-4223-a2b1-91f131fab5b5/state/analysis");
        }
    }
}