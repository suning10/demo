<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UpdateReasonAPS.aspx.cs" Inherits="demo.UpdateReasonAPS" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<link href="https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-Vkoo8x4CGsO3+Hhxv8T/Q5PaXtkKtu6ug5TOeNV6gBiFeWPGFN9MuhOf23Q9Ifjh" crossorigin="anonymous">
        <style type="text/css">
        .auto-style1 {
            text-align: center;
        }
        .auto-style2 {
            font-size: x-large;
        }
    </style>

<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
                <div class="auto-style1">
<h style="text-align: center;"><strong><em><span class="auto-style2">Update Failure Reason On Time Pick UP</span></em></strong></h>
        </div>
        <div style="text-align: center;">
            <label>
                Please select the Week
            </label>
        <asp:DropDownList ID="DropDownList1" runat="server">
        </asp:DropDownList>
        </div>


        <div style="text-align: center;">
        <asp:PlaceHolder ID="PlaceHolder1" runat="server" EnableTheming="True"></asp:PlaceHolder>

        </div>



        <div style="text-align: center;">
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Get Late Trucks" CssClass ="btn btn-primary"/>
        <asp:Button ID="Button2" runat="server" Text="Update Reason" OnClick="Button2_Click" CssClass ="btn btn-primary"/>
        <asp:Button ID="Button3" runat="server" Text="View All        " OnClick="Button3_Click" CssClass ="btn btn-primary" />
         </div>

        <div style="text-align: center;">
        <asp:PlaceHolder ID="PH2" runat="server"></asp:PlaceHolder>
        </div>

        
    <div style="text-align: center;">
        <asp:LinkButton ID="LinkButton2" runat="server" OnClick="LinkButton2_Click">Go To Update Failure Reason Delivery</asp:LinkButton>
    </div>

                       <div style="text-align: center;">
        <asp:LinkButton ID="LinkButton4" runat="server" OnClick="LinkButton4_Click">Add a new On time Pick Up Failure Record</asp:LinkButton>
    </div>

               <div style="text-align: center;">
        <asp:LinkButton ID="LinkButton3" runat="server" OnClick="LinkButton3_Click">Go To Qlik Sense Dashboard</asp:LinkButton>
    </div>



    </form>
</body>
</html>
