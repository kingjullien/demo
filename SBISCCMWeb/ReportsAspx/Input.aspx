<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Input.aspx.cs" Inherits="SBISCCMWeb.ReportsAspx.Input" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0000, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>

<body>
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager ID="scriptmanager1" runat="server"></asp:ScriptManager>
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" AsyncRendering="false" SizeToReportContent="true">
            </rsweb:ReportViewer>
        </div>
    </form>
</body>
</html>
