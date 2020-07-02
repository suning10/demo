<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="demo.WebForm1" %>

<html xmlns="www.w3.org/1999/xhtml">
<link href="https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-Vkoo8x4CGsO3+Hhxv8T/Q5PaXtkKtu6ug5TOeNV6gBiFeWPGFN9MuhOf23Q9Ifjh" crossorigin="anonymous">
<head runat="server">
	<title></title>
    <style type="text/css">
        .auto-style1 {
            text-align: center;
        }
        .auto-style2 {
            font-size: x-large;
        }

        
     input[type="text"] {
     width: 100%; 
     height:100%;
     box-sizing: border-box;
     -webkit-box-sizing:border-box;
     -moz-box-sizing: border-box;
}

    </style>
</head>
<body style="height: 738px">
	<form id="form2" runat="server">
	<div>


	</div>
        <div class="auto-style1">
<h style="text-align: center;"><strong><em><span class="auto-style2">Update Milestone Portal</span></em></strong></h>
        </div>
        <p>
            &nbsp;</p>

            &nbsp;</p>
        <p style="text-align: center">
            <asp:PlaceHolder ID="PH2" runat="server"></asp:PlaceHolder>
        </p>
        <p align="center">
            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Get Data" style="text-align: center" CssClass ="btn btn-primary" />
           
            <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="Update" style="text-align: center" CssClass ="btn btn-primary" />
        </p>
        <p style="height: 133px">
            &nbsp;</p>

    <div style="text-align: center;">
        <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click">Go To Update Failure Reason</asp:LinkButton>
    </div>

    <div style="text-align: center;">
        <asp:LinkButton ID="LinkButton2" runat="server" OnClick="LinkButton2_Click">Go To Update Failure Reason Delivery</asp:LinkButton>
    </div>
        

        </form>

</body>
</html>
