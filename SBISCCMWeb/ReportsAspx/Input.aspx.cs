using Microsoft.Reporting.WebForms;
using SBISCCMWeb.Utility;
using SBISCompanyCleanseMatchFacade.Objects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SBISCCMWeb.ReportsAspx
{
    public partial class Input : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                CompanyFacade fac = new CompanyFacade(SessionHelper.GetConnctionstring, Helper.UserName);
                string strReportType = Request.QueryString["ReportType"];
                DataSet dsInput = new DataSet();
                string ReportType = "";
                switch (strReportType)
                {
                    case "InputAndOutput":
                        ReportType = "1";
                        break;
                    case "CompanyProcessAudit":
                        ReportType = "2";
                        break;
                    case "StewardshipStatistics":
                        ReportType = "3";
                        break;
                    case "APIUsage":
                        ReportType = "4";
                        break;
                    case "TopMatchGrades":
                        ReportType = "5";
                        break;
                    case "Investigation":
                        ReportType = "6";
                        break;
                }
                dsInput = fac.GetReport(ReportType);
                this.ReportViewer1.Reset();
                this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.Visible = true;
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportDataSource reportDataSource = new ReportDataSource();
                switch (strReportType)
                {
                    case "InputAndOutput":
                        this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/ReportFiles/InputAndOutput.rdlc");
                        reportDataSource.Name = "Input";
                        reportDataSource.Value = dsInput.Tables[0];
                        ReportViewer1.LocalReport.DataSources.Add(reportDataSource);
                        ReportDataSource reportDataSourceOutput = new ReportDataSource();
                        reportDataSourceOutput.Name = "Output";
                        DataSet dsOutput = new DataSet();
                        dsOutput = fac.GetReport("4");
                        reportDataSourceOutput.Value = dsOutput.Tables[0];
                        ReportViewer1.LocalReport.DataSources.Add(reportDataSourceOutput);
                        break;
                    case "CompanyProcessAudit":
                        this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/ReportFiles/CompanyProcessAudit.rdlc");
                        reportDataSource.Name = "CompanyAudit";
                        reportDataSource.Value = dsInput.Tables[0];
                        ReportViewer1.LocalReport.DataSources.Add(reportDataSource);
                        break;
                    case "StewardshipStatistics":
                        this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/ReportFiles/StewardshipStatistics.rdlc");
                        reportDataSource.Name = "StewardshipStats";
                        reportDataSource.Value = dsInput.Tables[0];
                        ReportViewer1.LocalReport.DataSources.Add(reportDataSource);
                        break;
                    case "APIUsage":
                        this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/ReportFiles/APIUsage.rdlc");
                        DataTable dataTable = new DataTable();
                        dataTable = fac.GetAPIUsage();
                        reportDataSource.Name = "APIUsage";
                        reportDataSource.Value = dataTable;
                        ReportViewer1.LocalReport.DataSources.Add(reportDataSource);
                        break;
                    case "TopMatchGrades":
                        this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/ReportFiles/TopMatchGrades.rdlc");
                        DataTable dtInput = new DataTable();
                        dtInput = fac.GetTopMatchGrades(true);
                        reportDataSource.Name = "MatchStatsTopMatch";
                        reportDataSource.Value = dtInput;

                        ReportDataSource matchStatAll = new ReportDataSource();
                        DataTable dtMatch = fac.GetTopMatchGrades(false);
                        matchStatAll.Name = "MatchStatsAll";
                        matchStatAll.Value = dtMatch;

                        ReportDataSource first = new ReportDataSource();
                        DataTable dtFirst = fac.GetFirstMatchCCMG();
                        first.Name = "FirstMatchCCMG";
                        first.Value = dtFirst;
                        ReportViewer1.LocalReport.DataSources.Add(reportDataSource);
                        ReportViewer1.LocalReport.DataSources.Add(matchStatAll);
                        ReportViewer1.LocalReport.DataSources.Add(first);
                        break;
                    case "Investigation":
                        break;
                }
                ReportViewer1.LocalReport.Refresh();
            }
        }
    }
}