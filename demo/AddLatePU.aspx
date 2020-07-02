<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddLatePU.aspx.cs" Inherits="demo.AddLatePU" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
        <div style="text-align: center;">
        <H style="text-align: center;">
            Add New Record for Late Pick Up</H>
        </div>
        <div style="text-align: center;">
        <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
        </div>
        <br></br>
        <div style="text-align: center;">
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Update" />
        </div>
          <div style="text-align: center;">
        <asp:LinkButton ID="LinkButton4" runat="server" OnClick="LinkButton4_Click">GO to QlikSense Dashboard</asp:LinkButton>
    </div>

    </form>
</body>
</html>