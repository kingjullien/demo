using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace SBISCCMWeb.Models
{
    public class SalesForce
    {

    }
    public class Urls
    {
        public string rowTemplate { get; set; }
        public string defaultValues { get; set; }
        public string describe { get; set; }
        public string sobject { get; set; }
        public string compactLayouts { get; set; }
        public string approvalLayouts { get; set; }
        public string listviews { get; set; }
        public string quickActions { get; set; }
        public string layouts { get; set; }
        public string caseArticleSuggestions { get; set; }
        public string caseRowArticleSuggestions { get; set; }
        public string push { get; set; }
        public string namedLayouts { get; set; }
        public string passwordUtilities { get; set; }
    }

    public class Sobject
    {
        public bool activateable { get; set; }
        public bool createable { get; set; }
        public bool custom { get; set; }
        public bool customSetting { get; set; }
        public bool deletable { get; set; }
        public bool deprecatedAndHidden { get; set; }
        public bool feedEnabled { get; set; }
        public string keyPrefix { get; set; }
        public string label { get; set; }
        public string labelPlural { get; set; }
        public bool layoutable { get; set; }
        public bool mergeable { get; set; }
        public string name { get; set; }
        public bool queryable { get; set; }
        public bool replicateable { get; set; }
        public bool retrieveable { get; set; }
        public bool searchable { get; set; }
        public bool triggerable { get; set; }
        public bool undeletable { get; set; }
        public bool updateable { get; set; }
        public Urls urls { get; set; }
    }

    public class RootObjects
    {
        public string encoding { get; set; }
        public int maxBatchSize { get; set; }
        public List<Sobject> sobjects { get; set; }
    }


    // for getting fields from the table name
    public class ChildRelationship
    {
        public bool cascadeDelete { get; set; }
        public string childSObject { get; set; }
        public bool deprecatedAndHidden { get; set; }
        public string field { get; set; }
        public object junctionIdListName { get; set; }
        public List<object> junctionReferenceTo { get; set; }
        public string relationshipName { get; set; }
        public bool restrictedDelete { get; set; }
    }

    public class Field
    {
        public bool aggregatable { get; set; }
        public bool autoNumber { get; set; }
        public int byteLength { get; set; }
        public bool calculated { get; set; }
        public object calculatedFormula { get; set; }
        public bool cascadeDelete { get; set; }
        public bool caseSensitive { get; set; }
        public object controllerName { get; set; }
        public bool createable { get; set; }
        public bool custom { get; set; }
        public object defaultValue { get; set; }
        public object defaultValueFormula { get; set; }
        public bool defaultedOnCreate { get; set; }
        public bool dependentPicklist { get; set; }
        public bool deprecatedAndHidden { get; set; }
        public int digits { get; set; }
        public bool displayLocationInDecimal { get; set; }
        public bool encrypted { get; set; }
        public bool externalId { get; set; }
        public string extraTypeInfo { get; set; }
        public bool filterable { get; set; }
        public object filteredLookupInfo { get; set; }
        public bool groupable { get; set; }
        public bool highScaleNumber { get; set; }
        public bool htmlFormatted { get; set; }
        public bool idLookup { get; set; }
        public object inlineHelpText { get; set; }
        public string label { get; set; }
        public int length { get; set; }
        public object mask { get; set; }
        public object maskType { get; set; }
        public string name { get; set; }
        public bool nameField { get; set; }
        public bool namePointing { get; set; }
        public bool nillable { get; set; }
        public bool permissionable { get; set; }
        public List<object> picklistValues { get; set; }
        public int precision { get; set; }
        public bool queryByDistance { get; set; }
        public object referenceTargetField { get; set; }
        public List<object> referenceTo { get; set; }
        public string relationshipName { get; set; }
        public object relationshipOrder { get; set; }
        public bool restrictedDelete { get; set; }
        public bool restrictedPicklist { get; set; }
        public int scale { get; set; }
        public string soapType { get; set; }
        public bool sortable { get; set; }
        public string type { get; set; }
        public bool unique { get; set; }
        public bool updateable { get; set; }
        public bool writeRequiresMasterRead { get; set; }
    }

    public class UrlsField
    {
        public string layout { get; set; }
    }

    public class RecordTypeInfo
    {
        public bool available { get; set; }
        public bool defaultRecordTypeMapping { get; set; }
        public bool master { get; set; }
        public string name { get; set; }
        public string recordTypeId { get; set; }
        public UrlsField urls { get; set; }
    }

    public class SupportedScope
    {
        public string label { get; set; }
        public string name { get; set; }
    }

    public class Urls2
    {
        public string compactLayouts { get; set; }
        public string rowTemplate { get; set; }
        public string approvalLayouts { get; set; }
        public string uiDetailTemplate { get; set; }
        public string uiEditTemplate { get; set; }
        public string defaultValues { get; set; }
        public string listviews { get; set; }
        public string describe { get; set; }
        public string uiNewRecord { get; set; }
        public string quickActions { get; set; }
        public string layouts { get; set; }
        public string sobject { get; set; }
    }

    public class RootObjectField
    {
        public List<object> actionOverrides { get; set; }
        public bool activateable { get; set; }
        public List<ChildRelationship> childRelationships { get; set; }
        public bool compactLayoutable { get; set; }
        public bool createable { get; set; }
        public bool custom { get; set; }
        public bool customSetting { get; set; }
        public bool deletable { get; set; }
        public bool deprecatedAndHidden { get; set; }
        public bool feedEnabled { get; set; }
        public List<Field> fields { get; set; }
        public string keyPrefix { get; set; }
        public string label { get; set; }
        public string labelPlural { get; set; }
        public bool layoutable { get; set; }
        public object listviewable { get; set; }
        public object lookupLayoutable { get; set; }
        public bool mergeable { get; set; }
        public string name { get; set; }
        public List<object> namedLayoutInfos { get; set; }
        public object networkScopeFieldName { get; set; }
        public bool queryable { get; set; }
        public List<RecordTypeInfo> recordTypeInfos { get; set; }
        public bool replicateable { get; set; }
        public bool retrieveable { get; set; }
        public bool searchLayoutable { get; set; }
        public bool searchable { get; set; }
        public List<SupportedScope> supportedScopes { get; set; }
        public bool triggerable { get; set; }
        public bool undeletable { get; set; }
        public bool updateable { get; set; }
        public Urls2 urls { get; set; }
    }
    public class ResultSaleforceTable
    {
        public string describeUrl { get; set; }
        public string developerName { get; set; }
        public string id { get; set; }
        public string label { get; set; }
        public string resultsUrl { get; set; }
        public string soqlCompatible { get; set; }
        public string url { get; set; }

    }


    public class ListtoDataTableConverter
    {
        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
    }




    public class Column
    {
        public string ascendingLabel { get; set; }
        public string descendingLabel { get; set; }
        public string fieldNameOrPath { get; set; }
        public bool hidden { get; set; }
        public string label { get; set; }
        public string selectListItem { get; set; }
        public string sortDirection { get; set; }
        public int? sortIndex { get; set; }
        public bool sortable { get; set; }
        public string type { get; set; }
    }

    public class Column2
    {
        public string fieldNameOrPath { get; set; }
        public string value { get; set; }
    }

    public class Record
    {
        public List<Column2> columns { get; set; }
    }

    public class SalesForcesCustomView
    {
        public List<Column> columns { get; set; }
        public string developerName { get; set; }
        public bool done { get; set; }
        public string id { get; set; }
        public string label { get; set; }
        public List<Record> records { get; set; }
        public int size { get; set; }
    }
}