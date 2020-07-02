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
    public partial class AddLateDel : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int count_row = 0;
            int count_column = 0;
            if (!IsPostBack)
            {
                //add a new table to PH1
                PlaceHolder1.Controls.Add(new Literal { Text = "<table border = '1' align = 'center'>" });
                PlaceHolder1.Controls.Add(new Literal { Text = "<tr>" });
                PlaceHolder1.Controls.Add(new Literal { Text = "<th> Location </th> <th> BOL </th> " +
                    "<th> Partner </th><th> ShipTo </th><th> Qty </th><th> Plan delivered DateTime </th><th> Actual Arrive DateTime </th>" +
                    "<th> Reason </th><th> Comment </th>"
                });
                PlaceHolder1.Controls.Add(new Literal { Text = "</tr>" });

                // add five empty row to hold the data
                for (count_row = 0;count_row<=4; count_row++)
                {
                    //add tr
                    PlaceHolder1.Controls.Add(new Literal { Text = "<tr>" });
                    for (count_column = 0; count_column <= 8; count_column++)
                    {
                        if (count_column == 0)// location use drop down list
                        {
                            PlaceHolder1.Controls.Add(new Literal { Text = "<td>" });
                            DropDownList d = new DropDownList();
                            d.Items.Add(new ListItem("Bensenville"));
                            d.Items.Add(new ListItem("Coppell"));
                            d.Items.Add(new ListItem("Bethel"));
                            d.ID = "dropdown_location" + count_row;
                            PlaceHolder1.Controls.Add(d);
                        }

                        else if (count_column == 2)// Partner use drop down list
                        {
                            PlaceHolder1.Controls.Add(new Literal { Text = "<td>" });
                            DropDownList d1 = new DropDownList();
                            d1.Items.Add(new ListItem("Challenger"));
                            d1.Items.Add(new ListItem("Covenant"));
                            d1.Items.Add(new ListItem("Schneider"));
                            d1.Items.Add(new ListItem("US XPRESS"));
                            d1.Items.Add(new ListItem("Werner"));
                            d1.ID = "dropdown_partner" + count_row;
                            PlaceHolder1.Controls.Add(d1);
                        }

                        else if (count_column == 6 || count_column == 5)// plan&ata use datetime textbox
                        {
                            PlaceHolder1.Controls.Add(new Literal { Text = "<td>" });
                            TextBox t = new TextBox();
                            t.TextMode = TextBoxMode.DateTimeLocal;
                            t.ID = "tbox" + count_row + "_" + count_column;
                            PlaceHolder1.Controls.Add(t);
                        }

                        else if (count_column == 7)// reason use drop down list
                        {
                            PlaceHolder1.Controls.Add(new Literal { Text = "<td>" });
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
                            d.Items.Add(new ListItem("Late Pickup"));
                            d.Items.Add(new ListItem("Traffic"));
                            d.Items.Add(new ListItem("Driver Fatigue"));
                            d.Items.Add(new ListItem("Weather"));
                            d.Items.Add(new ListItem("No Issue"));
                            d.ID = "dropdown_reason" + count_row;
                            PlaceHolder1.Controls.Add(d);
                        }
                        else
                        {
                            PlaceHolder1.Controls.Add(new Literal { Text = "<td>" });
                            TextBox t = new TextBox();
                            //t.TextMode = TextBoxMode.SingleLine;
                            t.ID = "tbox" + count_row + "_" + count_column;
                            PlaceHolder1.Controls.Add(t);
                        }


                        PlaceHolder1.Controls.Add(new Literal { Text = "</td>" });//end of a row
                        
                    }
                    PlaceHolder1.Controls.Add(new Literal { Text = "</tr>" });
                }
                

            }
                
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            PlaceHolder1.Visible = false;
            int noofrows = 0;
            //if BOL is not empty add new records
            string connetionString;
            connetionString = @"Data Source=DRTW-SEAQLIK\LOGISTICS_FDA;Initial Catalog=FDA ;User id=sa;Password=Bryant718!;";
            SqlConnection cnn = new SqlConnection(connetionString);
            cnn.Open();
            SqlCommand cmd = new SqlCommand("[insert_new]", cnn);

            //inintilize parameters
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@bol", "test");
            cmd.Parameters.AddWithValue("@reason", "warehouse_date_time");
            cmd.Parameters.AddWithValue("@comment", "test");
            cmd.Parameters.AddWithValue("@partner", "coppell");
            cmd.Parameters.AddWithValue("@ship_to", "coppell");
            cmd.Parameters.AddWithValue("@qty", 1);
            cmd.Parameters.AddWithValue("@plan_date", "1/18/2020  2:00:00 PM'");
            cmd.Parameters.AddWithValue("@ata", "1/18/2020  2:00:00 PM");
            cmd.Parameters.AddWithValue("@location", "test");

            //fill in parameters value
            for (int i = 0; i <= 4; i++)
            {
                if (Request.Form["tbox" + i + "_1"] != "")
                {



                    string location = "";

                    if (Request.Form["dropdown_location" + i] == "Bensenville")
                    {
                        location = "WC20";
                    }
                    else if (Request.Form["dropdown_location" + i] == "Coppell")
                    {
                        location = "WC30";
                    }
                    else if (Request.Form["dropdown_location" + i] == "Bethel")
                    {
                        location = "WCWX";
                    }

                    string carrier = "";

                    if (Request.Form["dropdown_partner" + i] == "Challenger")
                    {
                        carrier = "DJWX";
                    }
                    else if (Request.Form["dropdown_partner" + i] == "Covenant")
                    {
                        carrier = "DN5Q";
                    }
                    else if (Request.Form["dropdown_partner" + i] == "Schneider")
                    {
                        carrier = "DJX2";
                    }
                    else if (Request.Form["dropdown_partner" + i] == "US XPRESS")
                    {
                        carrier = "DMP7";
                    }
                    else if (Request.Form["dropdown_partner" + i] == "Werner")
                    {
                        carrier = "DZN9";
                    }

                    cmd.Parameters["@bol"].Value = Request.Form["tbox" + i + "_1"];
                    cmd.Parameters["@location"].Value = location;
                    cmd.Parameters["@partner"].Value = carrier;
                    cmd.Parameters["@ship_to"].Value = Request.Form["tbox" + i + "_3"];
                    cmd.Parameters["@qty"].Value = Request.Form["tbox" + i + "_4"];
                    DateTime inputDate = DateTime.Parse(Request.Form["tbox" + i + "_5"]);
                    cmd.Parameters["@plan_date"].Value = inputDate.ToString("yyyy - MM - dd HH: mm:ss");
                    DateTime inputDate_ata = DateTime.Parse(Request.Form["tbox" + i + "_6"]);
                    cmd.Parameters["@ata"].Value = inputDate_ata.ToString("yyyy - MM - dd HH: mm:ss");
                    cmd.Parameters["@reason"].Value = Request.Form["dropdown_reason" + i];
                    cmd.Parameters["@comment"].Value = Request.Form["tbox" + i + "_8"];

                   
                    int noofrow = cmd.ExecuteNonQuery();
                    noofrows = noofrows + noofrow;
                }

            }
            cnn.Close();
            Response.Write("Number of Records Updated: " + noofrows/2);
        }

        protected void LinkButton4_Click(object sender, EventArgs e)
        {
            Response.Redirect("https://drtw-seaqlik.sea.samsung.com/hub");
        }
    }
}