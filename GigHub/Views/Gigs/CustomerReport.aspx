<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerReport.aspx.cs" Inherits="GigHub.Views.Gigs.CustomerReport" %>
<%@ Register TagPrefix="rsweb" Namespace="Microsoft.Reporting.WebForms" Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" %>

<form id="formCustomerReport" runat="server">  
    <div>  
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>  
        <rsweb:ReportViewer ID="CustomerListReportViewer" runat="server" Width="100%"></rsweb:ReportViewer>  
    </div>  
</form>  