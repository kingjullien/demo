using System.Web.Optimization;

namespace SBISCCMWeb
{
    public static class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/JQuery/jquery-3.5.1.min.js",
                        "~/Scripts/jquery-migrate-3.3.0.min.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/libs/jquery.validate*", "~/Scripts/jquery.unobtrusive*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/libs/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/layoutcss").Include(
                "~/Content/Template/css/smartadmin-production.min.css",
                "~/Content/Template/css/smartadmin-skins.min.css",
                "~/Content/Site.css",
                "~/Scripts/plugin/bootstarp-select/css/bootstrap-select.min.css",
                "~/Scripts/plugin/bootstarp-select/css/flag-icon.min.css",
                "~/Content/chosen.css",
                "~/Content/filterTable/bootstrap-filterable.css",
                "~/Content/bootstrap-multiselect.css",
                "~/Scripts/daterangepicker/daterangepicker-bs3.css",
                "~/Scripts/plugin/jQuery-DataTable/dataTables/datatables.min.css",
                "~/Content/Custom.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/layoutjs").Include(
                "~/Scripts/jquery-ui-1.12.1.min.js",
                "~/Scripts/popper.min.js",
                "~/Scripts/bootstrap/bootstrap.min.js",
                "~/Scripts/notification/SmartNotification.min.js",
                "~/Scripts/smartwidgets/jarvis.widget.min.js",
                "~/Scripts/JQuery/jquery.validate.min.js",
                "~/Scripts/JQuery/jquery.unobtrusive-ajax.min.js",
                "~/Scripts/plugin/msie-fix/jquery.mb.browser.min.js",
                "~/Scripts/plugin/fastclick/fastclick.min.js",
                "~/Scripts/bootbox.min.js",
                //"~/Scripts/app.js",
                "~/Scripts/idle_timer.js",
                "~/Scripts/bootstrap-notify.js",
                "~/Scripts/moment.min.js",
                "~/Scripts/daterangepicker/daterangepicker.js",
                "~/Scripts/bootstrap-multiselect.js",
                "~/Scripts/plugin/jQuery-DataTable/dataTables/datatables.min.js",
                "~/Scripts/plugin/jQuery-DataTable/dataTables/dataTables.bootstrap4.min.js",
                "~/Scripts/CustomJS/Layout.js",
                "~/Scripts/CustomJS/Common.js",
                "~/Scripts/chosen.jquery.js",
                //"~/Scripts/jquery.contextMenu.js",
                "~/Scripts/jquery.ui.position.min.js",
                "~/Scripts/nouislider.min.js",
                "~/Scripts/plugin/bootstarp-select/js/bootstrap-select.min.js"
                ));

            bundles.Add(new StyleBundle("~/Content/popupcss").Include(
                      "~/Scripts/plugin/magnific/magnific-popup.css",
                      "~/Content/Template/css/bootstrap.min.css",
                      "~/Content/Template/css/font-awesome.min.css",
                      "~/Content/Template/css/smartadmin-production.min.css",
                      "~/Content/Template/css/smartadmin-skins.min.css",
                       "~/Content/Custom.css"));
            bundles.Add(new ScriptBundle("~/bundles/popupjs").Include(
                    //"~/Scripts/modernizr-2.6.2.js",
                    //"~/Scripts/jquery-1.10.2.min.js",
                    "~/Scripts/modernizr-2.8.3.js",
                    "~/Scripts/jquery-2.2.4.min.js",
                    "~/Scripts/plugin/magnific/jquery.magnific-popup.js"));

            bundles.Add(new ScriptBundle("~/bundles/home").Include(
                "~/Scripts/highcharts.js",
                "~/Scripts/exporting.js",
                "~/Scripts/CustomJS/Home.js",
                "~/Scripts/CustomJS/BackgroundProcessList.js",
                "~/Scripts/CustomJS/ImportProcessQueue.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/badinputdata").Include(
                "~/Scripts/CustomJS/BadInputData.js",
                "~/Scripts/CustomJS/RejectAllMatches.js",
                "~/Scripts/CustomJS/RejectPurgeFromFile.js",
                "~/Scripts/CustomJS/CleanSearchPopup.js",
                "~/Scripts/CustomJS/SearchDataPopUp.js",
                "~/Scripts/CustomJS/MatchDetailReviews.js",
                "~/Scripts/CustomJS/Portal.js",
                "~/Scripts/CustomJS/DomainSearch.js",
                "~/Scripts/CustomJS/BingSearch.js",
                "~/Scripts/CustomJS/iResearchInvestigation.js",
                "~/Scripts/CustomJS/ReMatchRecords.js",
                "~/Scripts/CustomJS/AddCompany.js",
                "~/Scripts/Ogma/ogma.min.js",
                "~/Scripts/CustomJS/Benificialownership.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/dandb").Include(
                "~/Scripts/jquery.bootstrap-duallistbox.js",
                "~/Scripts/CustomJS/DandB.js",
                "~/Scripts/CustomJS/DandBMonitoring.js",
                "~/Scripts/CustomJS/DandBDataEnrichment.js",
                "~/Scripts/CustomJS/DandBCleanseMatch.js",
                "~/Scripts/CustomJS/DandBLicence.js",
                "~/Scripts/CustomJS/AddCountryGroupPopup.js",
                "~/Scripts/CustomJS/CleanseMatchSettings.js",
                "~/Scripts/CustomJS/DeleteComment.js",
                "~/Scripts/CustomJS/CacheDataSetting.js",
                "~/Scripts/CustomJS/PopupMonitoringPlusDUNSDetails.js",
                "~/Scripts/CustomJS/CleanseMatchImportData.js",
                "~/Scripts/CustomJs/ResetSystemData.js",
                "~/Scripts/CustomJS/MonitorProfile.js",
                "~/Scripts/CustomJS/AddDUNSMonitoringPlus.js",
                "~/Scripts/CustomJS/RemoveDUNSMonitoringPlus.js",
                "~/Scripts/CustomJS/MultiPassConfig.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/exportview").Include(
                "~/Scripts/plugin/jQuery-DataTable/dataTables/datetime-moment.js",
                "~/Scripts/CustomJS/ExportData.js",
                "~/Scripts/CustomJS/ExportFileName.js",
                "~/Scripts/CustomJS/ReExport.js",
                "~/Scripts/CustomJS/Delimiter.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/familytree").Include(
                "~/Scripts/CustomJS/FamilyTree.js",
                "~/Scripts/CustomJS/AddFamilyTreeNode.js",
                "~/Scripts/CustomJS/DuplicateFamilyTree.js",
                "~/Scripts/CustomJS/SideBySideFamilyTree.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/importdata").Include(
                "~/Scripts/highcharts.js",
                "~/Scripts/CustomJS/ImportDataIndex.js",
                "~/Scripts/CustomJS/BackgroundProcessList.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/portal").Include(
                "~/Scripts/CustomJS/Portal.js",
                "~/Scripts/CustomJS/CommadLine.js",
                "~/Scripts/CustomJS/CountryImportData.js",
                "~/Scripts/CustomJS/BindColumnMapping.js",
                "~/Scripts/CustomJS/CommandDownloadPopup.js",
                "~/Scripts/CustomJS/ImportDataIndex.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/previewmatchdata").Include(
                "~/Scripts/CustomJS/PreviewMatchData.js",
                "~/Scripts/CustomJS/PreviewSearchPopup.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/reportslist").Include(
                "~/Scripts/highcharts.js",
                "~/Scripts/CustomJS/Report.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/researchinvestigation").Include(
                "~/Scripts/CustomJS/iResearchInvestigation.js",
                "~/Scripts/CustomJS/InvestigationColumnMapping.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/review").Include(
                "~/Scripts/CustomJS/Review.js",
                "~/Scripts/CustomJS/AddCountryGroupPopup.js",
                "~/Scripts/CustomJS/CleanseMatchSettings.js",
                "~/Scripts/CustomJS/DandBCleanseMatch.js",
                "~/Scripts/CustomJS/AddCompany.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/searchdata").Include(
                "~/Scripts/CustomJS/SearchData.js",
                "~/Scripts/CustomJS/DomainSearch.js",
                "~/Scripts/CustomJS/BingSearch.js",
                "~/Scripts/CustomJS/iResearchInvestigation.js",
                "~/Scripts/CustomJS/AddCompany.js",
                "~/Scripts/CustomJS/MatchDetailReviews.js",
                "~/Scripts/Ogma/ogma.min.js",
                "~/Scripts/CustomJS/Benificialownership.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/stewardshipportal").Include(
                "~/Scripts/CustomJS/BubbleInfo.js",
                "~/Scripts/CustomJS/StewardshipPortal.js",
                "~/Scripts/CustomJS/RejectAllMatches.js",
                "~/Scripts/CustomJS/RejectPurgeFromFile.js",
                "~/Scripts/CustomJS/MatchDetailReviews.js",
                "~/Scripts/CustomJS/Portal.js",
                "~/Scripts/CustomJS/iResearchInvestigation.js",
                "~/Scripts/CustomJS/CleanseMatchSettings.js",
                "~/Scripts/CustomJS/AddCompany.js",
                "~/Scripts/CustomJS/AcceptFromFile.js",
                "~/Scripts/Ogma/ogma.min.js",
                "~/Scripts/CustomJS/Benificialownership.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/ticket").Include(
                "~/Scripts/CustomJS/Ticket.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/buildlist_search").Include(
                "~/Scripts/CustomJS/BuildSearch.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/home_profiledetails").Include(
                "~/Scripts/CustomJS/ProfileDetails.js",
                "~/Scripts/CustomJS/ResetPassword.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/ticket_create").Include(
                "~/Scripts/plugin/masked-input/jquery.maskedinput.min.js",
                "~/Scripts/CustomJS/AddUpdateTicket.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/ticket_edit").Include(
                "~/Scripts/jquery.validate.unobtrusive.3.2.11.min.js",
               "~/Scripts/plugin/masked-input/jquery.maskedinput.min.js",
               "~/Scripts/CustomJS/AddUpdateTicket.js"
               ));

            bundles.Add(new ScriptBundle("~/bundles/benificiaryownership").Include(
                "~/Scripts/Ogma/ogma.min.js",
               "~/Scripts/CustomJS/Benificialownership.js",
               "~/Scripts/CustomJS/MatchDetailReviews.js",
               "~/Scripts/CustomJS/AddCompany.js",
               "~/Scripts/CustomJS/BeneficiaryOwnershipTypeAhead.js"
               ));

            bundles.Add(new ScriptBundle("~/bundles/emailverification").Include(
                "~/Scripts/CustomJS/EmailVerification.js"
               ));

            bundles.Add(new ScriptBundle("~/bundles/forgetpassword").Include(
                "~/Scripts/CustomJS/Login.js"
               ));

            bundles.Add(new ScriptBundle("~/bundles/login").Include(
                "~/Scripts/CustomJS/Login.js",
                "~/Scripts/CustomJS/Common.js"
               ));

            bundles.Add(new ScriptBundle("~/bundles/resetpassword").Include(
                "~/Scripts/CustomJS/Common.js",
                "~/Scripts/CustomJS/ResetPassword.js"
               ));

            bundles.Add(new ScriptBundle("~/bundles/common").Include(
                "~/Scripts/CustomJS/Common.js"
               ));

            bundles.Add(new ScriptBundle("~/bundles/apiusagestatistics").Include(
                "~/Scripts/CustomJS/APIUsageStatistics.js"
               ));

            bundles.Add(new ScriptBundle("~/bundles/onlyreport").Include(
                "~/Scripts/CustomJS/Report.js"
               ));

            bundles.Add(new ScriptBundle("~/bundles/usersecurity").Include(
                "~/Scripts/CustomJS/UserSecurity.js"
               ));

            bundles.Add(new ScriptBundle("~/bundles/review_index").Include(
                "~/Scripts/CustomJS/table.js"
               ));

            bundles.Add(new ScriptBundle("~/bundles/review_matchitemdetailview").Include(
                "~/Scripts/CustomJS/ReviewMatchDetailViewReview.js"
               ));

            bundles.Add(new ScriptBundle("~/bundles/error").Include(
                "~/Scripts/CustomJS/Error.js"
               ));

            bundles.Add(new ScriptBundle("~/bundles/systemsetting").Include(
                "~/Scripts/CustomJS/SystemSetting.js"
               ));

            bundles.Add(new ScriptBundle("~/bundles/usersessionfilter").Include(
                "~/Scripts/CustomJS/UserSessionFilter.js"
               ));

            bundles.Add(new ScriptBundle("~/bundles/oiviewresolutionmap").Include(
                "~/Scripts/CustomJS/OI/ViewresolutionMap.js"
               ));

            bundles.Add(new ScriptBundle("~/bundles/OIusersessionfilter").Include(
                "~/Scripts/CustomJS/OI/OIUserSessionFilter.js"
               ));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862

            if (System.Configuration.ConfigurationManager.AppSettings["Environment"].ToLower() == "local")
            {
                BundleTable.EnableOptimizations = false;
            }
            else
            {
                BundleTable.EnableOptimizations = true;
            }

        }
    }
}
