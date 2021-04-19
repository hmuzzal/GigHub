using GigHub.Models;
using Microsoft.Reporting.WebForms;
using System;
using System.Data.Entity;
using System.Web.UI;

namespace GigHub.Views.Gigs
{
    public partial class CustomerReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //string searchText = string.Empty;

                //if (Request.QueryString["searchText"] != null)
                //{
                //    searchText = Request.QueryString["searchText"].ToString();
                //}

                DbSet<Gig> gigs = null;
                using (var _context = new ApplicationDbContext())
                {
                    gigs = _context.Gigs;
                    CustomerListReportViewer.LocalReport.ReportPath = Server.MapPath("~/Report/GigsReport.rdl");
                    CustomerListReportViewer.LocalReport.DataSources.Clear();
                    ReportDataSource rdc = new ReportDataSource("GigDataSet", gigs);
                    CustomerListReportViewer.LocalReport.DataSources.Add(rdc);
                    CustomerListReportViewer.LocalReport.Refresh();
                    CustomerListReportViewer.DataBind();
                }
            }
        }
    }
}