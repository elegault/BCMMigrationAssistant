using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using BCM_Migration_Tool.Properties;
using TracerX;

namespace BCM_Migration_Tool.Objects
{
    public partial class FieldMappings
    {
        #region Constructors
        public FieldMappings()
        {
            //InitializeFieldMappings();
        }
        #endregion
        #region Fields
        public const string BCMIDPropertyGUID = "{BC013BA3-3A6D-4826-B0EC-CB703A722B09}";
        private static readonly Logger Log = Logger.GetLogger("FieldMappings");
        public enum BCMDataSetTypes
        {
            Accounts,
            BusinessContacts,
            Opportunities
        }
        public enum FieldMappingTypes
        {
            Automatic,
            Manual,
            OptionalAutomatic
        }
        public enum OCMDataSetTypes
        {
            Accounts,
            BusinessContacts,
            Deals
        }
        #endregion
        #region Properties
        public static string ConnectionString { get; set; }
#endregion
        #region Methods
        public static void InitializeFieldMappings(string conn)
        {
            ConnectionString = conn;
            BCMAccountFields.BCMFields = new BCMAccountFieldMappings();
            BCMAccountFields.BCMFields.UnMappedFields = BCMAccountFields.ManualFields;
            OCMAccountFields.OCMFields = new OCMAccountFieldMappings();
            OCMAccountFields.OCMFields.UnMappedFields = OCMAccountFields.ManualFields;

            BCMBusinessContactFields.BCMFields = new BCMBusinessContactFieldMappings();
            BCMBusinessContactFields.BCMFields.UnMappedFields = BCMBusinessContactFields.ManualFields;        
            OCMBusinessContactFields.OCMFields = new OCMBusinessContactFieldMappings();
            OCMBusinessContactFields.OCMFields.UnMappedFields = OCMBusinessContactFields.ManualFields;
            
            BCMOpportunityFields.BCMFields = new BCMOpportunityFieldMappings();
            BCMOpportunityFields.BCMFields.UnMappedFields = BCMOpportunityFields.ManualFields;

            OCMDealFields.OCMFields = new OCMDealFieldMappings();
            OCMDealFields.OCMFields.UnMappedFields = OCMDealFields.ManualFields;

        }
        #endregion
        public class BCMField : DataField
        {
            private OCMField _ocmFieldMapping;
            #region Constructors
            public BCMField(BCMDataSetTypes bcmDataSetType, BCMFieldTypes bcmFieldType, FieldMappingTypes fieldMappingType, string name, bool isCustom = false)
            {
                BCMDataSetType = bcmDataSetType;
                FieldType = bcmFieldType;
                FieldMappingType = fieldMappingType;
                Name = name; //NOTE This MUST match the field name in the database tables //REVIEW Maybe add a display name prop for the display values in the mapping combo boxes
                IsCustom = isCustom;
            }
            public BCMField(BCMFieldTypes bcmFieldType, FieldMappingTypes fieldMappingType, string name, OCMField mappedOCMField, bool isHidden = false)
            {
                FieldType = bcmFieldType;
                FieldMappingType = fieldMappingType;
                Name = name;
                OCMFieldMapping = mappedOCMField;
                IsHidden = isHidden;
            }
            #endregion
            #region Fields
            public enum BCMFieldTypes
            {
                Text,
                Number,
                Percent,
                Currency,
                YesNo,
                DateTime,
                Integer,
                DropDownList, //aka Picklist
                URL,
                Relationship //aka Reference
            }
            #endregion
            #region Properties
            public BCMDataSetTypes BCMDataSetType { get; set; }
            public new string DataTypeLabel
            {
                get
                {
                    switch (FieldType)
                    {
                        case BCMFieldTypes.Text:
                        case BCMFieldTypes.YesNo:
                        case BCMFieldTypes.DropDownList:
                        case BCMFieldTypes.URL:
                        case BCMFieldTypes.Relationship:
                            return "Text";
                        case BCMFieldTypes.Number:
                        case BCMFieldTypes.Percent:
                        case BCMFieldTypes.Currency:
                        case BCMFieldTypes.Integer:
                            return "Number/Text";
                        case BCMFieldTypes.DateTime:
                            return "Date";
                        default:
                            return "Text";

                    }
                    //return "";
                }
            }
            public BCMFieldTypes FieldType { get; set; }
            public string Label { get { return String.Format("{0} ({1})", Name, DataTypeLabel); } }

            public OCMField OCMFieldMapping
            {
                get { return _ocmFieldMapping; }
                set { _ocmFieldMapping = value; }
            }
            #endregion
        }
        public class DataField
        {
            #region Properties
            public FieldMappingTypes FieldMappingType { get; set; }
            public string Id { get; set; }
            public bool IsCustom { get; set; }
            public bool IsHidden { get; set; }
            //public string Label { get { return String.Format("{0} ({1})", Name, DataTypeLabel); } }
            public string Name { get; set; }
#endregion
        }
        public class OCMField: DataField
        {
            #region Constructors
            public OCMField(OCMDataSetTypes ocmDataSetType, OCMFieldTypes ocmFieldTypes, FieldMappingTypes fieldMappingType, string name, bool isArray, bool isHidden = false)
            {
                OCMDataSetType = ocmDataSetType;
                FieldMappingType = fieldMappingType;
                FieldType = ocmFieldTypes;
                Name = name;
                IsArray = isArray;
                IsHidden = isHidden;
            }
            #endregion
            #region Fields
            public enum OCMFieldTypes
            {
                Text,
                NumberOrText,
                Date
            }
            #endregion
            #region Properties
            public new string DataTypeLabel
            {
                get
                {
                    switch (FieldType)
                    {
                        case OCMFieldTypes.Date:
                            return "Date";
                        case OCMFieldTypes.NumberOrText:
                            return "Number/Text";
                        default:
                            return "Text";

                    }
                }
            }
            public new string DataTypeLabelForJSON
            {
                get
                {
                    switch (FieldType)
                    {
                        case OCMFieldTypes.Date:
                            return "SystemTime"; //Was Date
                        case OCMFieldTypes.NumberOrText:
                            return "Double"; //Was Numeric
                        default:
                            return "String"; //Was Text
                    }
                }
            }
            //public FieldMappingTypes FieldMappingType { get; set; }
            public OCMFieldTypes FieldType { get; set; }
            //public string Id { get; set; }
            public bool IsArray { get; set; }
            //public bool IsHidden { get; set; }
            public string Label { get { return String.Format("{0} ({1})", Name, DataTypeLabel); } }
            //public List<OCMField> MappedFields { get; set; } //REVIEW What is MappedFields for? Not used
            public OCMDataSetTypes OCMDataSetType { get; set; }
            public string PropertyID { get; set; }

            public string PropertyIDForJSON
            {
                get { return PropertyID?.Replace("{", "").Replace("}" ,"") ?? ""; }
            }
            #endregion
            #region Methods
            //public void SetValueFromOpportunity(object val, OCMCompany.Rootobject ocmCompany)
            //{
            //    PropertyInfo prop = ocmCompany.GetType().GetProperty(BCMAccountFields.BusinessFax.OCMFieldMapping.Name, BindingFlags.Public | BindingFlags.Instance);
            //    if (null != prop && prop.CanWrite)
            //    {
            //        prop.SetValue(ocmCompany, val, null);
            //    }
            //}
            //public void SetValue(object val, OCMDeal2.Rootobject ocmObject)
            //{
            //    PropertyInfo prop = ocmObject.GetType().GetProperty(BCMAccountFields.BusinessFax.OCMFieldMapping.Name, BindingFlags.Public | BindingFlags.Instance);
            //    if (null != prop && prop.CanWrite)
            //    {
            //        prop.SetValue(ocmCompany, val, null);
            //    }
            //}
            //public void SetValue(object val, OCMCompany.Rootobject ocmCompany)
            //{
            //    PropertyInfo prop = ocmCompany.GetType().GetProperty(BCMAccountFields.BusinessFax.OCMFieldMapping.Name, BindingFlags.Public | BindingFlags.Instance);
            //    if (null != prop && prop.CanWrite)
            //    {
            //        prop.SetValue(ocmCompany, val, null);
            //    }
            //}
            #endregion
        }
        #region OCM Fields
        public class OCMAccountFieldMappings: OCMDataSet
        {
        }
        public static class OCMAccountFields
        {
            #region Properties
            public static OCMCompanyTemplate.Rootobject CompanyTemplate { get; set; }
            public static OCMAccountFieldMappings OCMFields  { get; set; }
            public static List<OCMField> ManualFields
            {
                get
                {
                    List<OCMField> result = new List<OCMField>();
                    //Return all manual fields as default collection of unmapped fields
                    //NOTE Update manual fields here if they change
                    result.AddRange(GetCustomFields());
                    return result;
                }
            }
            public static List<OCMField> UnMappedDateFields
            {
                get
                {
                    List<OCMField> result = new List<OCMField>();
                    var fields = from field in OCMFields.UnMappedFields
                                 where field.FieldType == OCMField.OCMFieldTypes.Date
                                 select field;
                    result.AddRange(fields);
                    return result;
                }
            }
            public static List<OCMField> UnMappedNumberOrTextFields
            {
                get
                {
                    List<OCMField> result = new List<OCMField>();
                    var fields = from field in OCMFields.UnMappedFields
                                 where field.FieldType == OCMField.OCMFieldTypes.NumberOrText
                                 select field;
                    result.AddRange(fields);
                    return result;
                }
            }
            public static List<OCMField> UnMappedTextFields
            {
                get
                {
                    List<OCMField> result = new List<OCMField>();
                    var fields = from field in OCMFields.UnMappedFields
                                 where field.FieldType == OCMField.OCMFieldTypes.Text
                                 select field;
                    result.AddRange(fields);
                    return result;
                }
            }
            #region Automatic Mappings
            public static OCMField BCMID { get { return new OCMField(OCMDataSetTypes.Accounts, OCMField.OCMFieldTypes.Text, FieldMappingTypes.Automatic, "BCM ID", false, true); } }
            public static OCMField BusinessAddress { get { return new OCMField(OCMDataSetTypes.Accounts, OCMField.OCMFieldTypes.Text, FieldMappingTypes.Automatic, "Business Address", false); } }
            public static OCMField BusinessPhone { get { return new OCMField(OCMDataSetTypes.Accounts, OCMField.OCMFieldTypes.Text, FieldMappingTypes.Automatic, "Business Phone", true); } }
            public static OCMField BusinessHomePage { get { return new OCMField(OCMDataSetTypes.Accounts, OCMField.OCMFieldTypes.Text, FieldMappingTypes.Automatic, "Business Web Page", false); } }
            public static OCMField Email { get { return new OCMField(OCMDataSetTypes.Accounts, OCMField.OCMFieldTypes.Text, FieldMappingTypes.Automatic, "Email Address", false); } }
            public static OCMField Name { get { return new OCMField(OCMDataSetTypes.Accounts, OCMField.OCMFieldTypes.Text, FieldMappingTypes.Automatic, "Name (Company)", false); } }
            public static OCMField Notes { get { return new OCMField(OCMDataSetTypes.Accounts, OCMField.OCMFieldTypes.Text, FieldMappingTypes.Automatic, "Notes", false); } }
            #endregion
            #region Manual Mappings
            //public static OCMField BusinessFax { get { return new OCMField(OCMField.OCMFieldTypes.Text, FieldMappingTypes.Manual, "Business Fax", false); } }
            //public static OCMField CompanyPeople { get { return new OCMField(OCMField.OCMFieldTypes.Text, FieldMappingTypes.Manual, "Company People", false); } }
            //public static OCMField CompanySize { get { return new OCMField(OCMField.OCMFieldTypes.NumberOrText, FieldMappingTypes.Manual, "Company Size", false); } }
            //public static OCMField ContactType { get { return new OCMField(OCMField.OCMFieldTypes.NumberOrText, FieldMappingTypes.Manual, "Contact Type", false); } }
            //public static OCMField CustomerBit { get { return new OCMField(OCMField.OCMFieldTypes.NumberOrText, FieldMappingTypes.Manual, "Customer Bit", false); } } 
            //public static OCMField CustomerStatus { get { return new OCMField(OCMField.OCMFieldTypes.NumberOrText, FieldMappingTypes.Manual, "Customer Status", false); } } //REVIEW Customer Status?? Of type Microsoft.OutlookServices.XrmCustomerStatus; None = 0; WithActiveDeals = 1; NoActiveDeals = 2
            //public static OCMField DisplayAs { get { return new OCMField(OCMField.OCMFieldTypes.Text, FieldMappingTypes.Manual, "Display Name", false); } }
            //public static OCMField Employees { get; set; }
            //public static OCMField MailingAddress { get; set; }
            //public static OCMField Manager { get { return new OCMField(OCMField.OCMFieldTypes.Text, FieldMappingTypes.Manual, "Manager", false); } }
            //public static OCMField DoNotCall { get { return new OCMField(OCMField.OCMFieldTypes.Text, FieldMappingTypes.Manual, "Do Not Call", false); } }
            //public static OCMField DoNotEmail { get { return new OCMField(OCMField.OCMFieldTypes.Text, FieldMappingTypes.Manual, "Do Not Email", false); } }
            //public static OCMField DoNotMail { get { return new OCMField(OCMField.OCMFieldTypes.Text, FieldMappingTypes.Manual, "Do Not Mail", false); } }
            //public static OCMField Notes { get { return new OCMField(OCMDataSetTypes.Accounts, OCMField.OCMFieldTypes.Text, FieldMappingTypes.OptionalAutomatic, "Notes", false); } }
            //public static OCMField Revenue { get; set; }
            //public static OCMField Profession { get { return new OCMField(OCMField.OCMFieldTypes.Text, FieldMappingTypes.Manual, "Profession", false); } }
            //public static OCMField TickerSymbol { get { return new OCMField(OCMField.OCMFieldTypes.Text, FieldMappingTypes.Manual, "Ticker Symbol", false); } }
            #endregion
            #endregion
            #region Methods
            public static List<OCMField> GetCustomFields()
            {               
                //Get OCM custom fields

                List<OCMField> result = new List<OCMField>();
                if (CompanyTemplate == null)
                    return result;

                OCMField.OCMFieldTypes ocmFieldType;
                foreach (var field in CompanyTemplate.Template.FieldList)
                {
                    if (!field.IsCustom || field.Deleted) //REVIEW Is IsCustom ever false???
                        continue;
                    switch (field._Type.Label)
                    {
                        case "Text":
                            ocmFieldType = OCMField.OCMFieldTypes.Text;
                            break;
                        case "Date":
                            ocmFieldType = OCMField.OCMFieldTypes.Date;
                            break;
                        case "Numeric":
                            ocmFieldType = OCMField.OCMFieldTypes.NumberOrText;
                            break;
                        default:
                            ocmFieldType = OCMField.OCMFieldTypes.Text;
                            break;
                    }
                    OCMField ocmField = new OCMField(OCMDataSetTypes.Accounts, ocmFieldType,FieldMappingTypes.Manual, field.Label, false, false);
                    ocmField.PropertyID = field.Id;
                    ocmField.IsCustom = true;
                    result.Add(ocmField);
                }
                return result;
            }
            #endregion
        }
        public class OCMBusinessContactFieldMappings : OCMDataSet
        {
        }
        public static class OCMBusinessContactFields
        {
            public static OCMCompanyTemplate.Rootobject ContactTemplate { get; set; }
            public static OCMBusinessContactFieldMappings OCMFields { get; set; }
            public static List<OCMField> ManualFields
            {
                get
                {
                    List<OCMField> result = new List<OCMField>();
                    //Return all manual fields as default collection of unmapped fields
                    //NOTE Update manual fields here if they change
                    result.AddRange(GetCustomFields());
                    return result;
                }
            }
            public static List<OCMField> UnMappedDateFields
            {
                get
                {
                    List<OCMField> result = new List<OCMField>();
                    var fields = from field in OCMFields.UnMappedFields
                                 where field.FieldType == OCMField.OCMFieldTypes.Date
                                 select field;
                    result.AddRange(fields);
                    return result;
                }
            }
            public static List<OCMField> UnMappedNumberOrTextFields
            {
                get
                {
                    List<OCMField> result = new List<OCMField>();
                    var fields = from field in OCMFields.UnMappedFields
                                 where field.FieldType == OCMField.OCMFieldTypes.NumberOrText
                                 select field;
                    result.AddRange(fields);
                    return result;
                }
            }
            public static List<OCMField> UnMappedTextFields
            {
                get
                {
                    List<OCMField> result = new List<OCMField>();
                    var fields = from field in OCMFields.UnMappedFields
                                 where field.FieldType == OCMField.OCMFieldTypes.Text
                                 select field;
                    result.AddRange(fields);
                    return result;
                }
            }
            #region Automatic Mappings
            public static OCMField CompanyName { get { return new OCMField(OCMDataSetTypes.BusinessContacts, OCMField.OCMFieldTypes.Text, FieldMappingTypes.Automatic, "Name (Company)", false); } }
            public static OCMField BCMID { get { return new OCMField(OCMDataSetTypes.BusinessContacts, OCMField.OCMFieldTypes.Text, FieldMappingTypes.Automatic, "BCM ID", false, true); } }
            public static OCMField Birthday { get { return new OCMField(OCMDataSetTypes.BusinessContacts, OCMField.OCMFieldTypes.Date, FieldMappingTypes.Automatic, "Birthday", false); } }
            public static OCMField BusinessAddress { get { return new OCMField(OCMDataSetTypes.BusinessContacts, OCMField.OCMFieldTypes.Text, FieldMappingTypes.Automatic, "Business Address", false); } }
            public static OCMField BusinessPhone { get { return new OCMField(OCMDataSetTypes.BusinessContacts, OCMField.OCMFieldTypes.Text, FieldMappingTypes.Automatic, "Business Phone", true); } }            
            public static OCMField Email { get { return new OCMField(OCMDataSetTypes.BusinessContacts, OCMField.OCMFieldTypes.Text, FieldMappingTypes.Automatic, "Email Address", false); } }
            public static OCMField HomePhone { get { return new OCMField(OCMDataSetTypes.BusinessContacts, OCMField.OCMFieldTypes.Text, FieldMappingTypes.Automatic, "Home Phone", true); } }
            public static OCMField JobTitle { get { return new OCMField(OCMDataSetTypes.BusinessContacts, OCMField.OCMFieldTypes.Text, FieldMappingTypes.Automatic, "Job Title", false); } }
            public static OCMField MobilePhone { get { return new OCMField(OCMDataSetTypes.BusinessContacts, OCMField.OCMFieldTypes.Text, FieldMappingTypes.Automatic, "Mobile Phone", true); } }
            public static OCMField Name { get { return new OCMField(OCMDataSetTypes.BusinessContacts, OCMField.OCMFieldTypes.Text, FieldMappingTypes.Automatic, "Name", false); } }
            public static OCMField Notes { get { return new OCMField(OCMDataSetTypes.BusinessContacts, OCMField.OCMFieldTypes.Text, FieldMappingTypes.Automatic, "Notes", false); } }
            #endregion
            #region Manual Mappings

            #endregion
            #region Methods
            public static List<OCMField> GetCustomFields()
            {                
                //Get OCM custom fields

                List<OCMField> result = new List<OCMField>();
                if (ContactTemplate == null)
                    return result;

                OCMField.OCMFieldTypes ocmFieldType;
                foreach (var field in ContactTemplate.Template.FieldList)
                {
                    if (!field.IsCustom || field.Deleted) //REVIEW Is IsCustom ever false???
                        continue;
                    switch (field._Type.Label)
                    {
                        case "Text":
                            ocmFieldType = OCMField.OCMFieldTypes.Text;
                            break;
                        case "Date":
                            ocmFieldType = OCMField.OCMFieldTypes.Date;
                            break;
                        case "Numeric": 
                            ocmFieldType = OCMField.OCMFieldTypes.NumberOrText;
                            break;
                        default:
                            ocmFieldType = OCMField.OCMFieldTypes.Text;
                            break;
                    }
                    OCMField ocmField = new OCMField(OCMDataSetTypes.BusinessContacts, ocmFieldType, FieldMappingTypes.Manual, field.Label, false, false);
                    ocmField.IsCustom = true;
                    ocmField.PropertyID = field.Id;
                    result.Add(ocmField);
                }
                return result;
            }
            #endregion
        }
        public class OCMDealFieldMappings : OCMDataSet
        {
        }
        public class OCMDealFields
        {
            public static OCMDealTemplate.Rootobject DealTemplate { get; set; }
            public static OCMDealFieldMappings OCMFields { get; set; }

            public static List<OCMField> ManualFields
            {
                get
                {
                    List<OCMField> result = new List<OCMField>();
                    //Return all manual fields as default collection of unmapped fields
                    //NOTE Update manual fields here if they change
                    //result.Add(Probability);
                    result.AddRange(GetCustomFields());
                    return result;
                }
            }
            public static List<OCMField> UnMappedDateFields
            {
                get
                {
                    List<OCMField> result = new List<OCMField>();
                    var fields = from field in OCMFields.UnMappedFields
                                 where field.FieldType == OCMField.OCMFieldTypes.Date
                                 select field;
                    result.AddRange(fields);
                    return result;
                }
            }
            public static List<OCMField> UnMappedNumberOrTextFields
            {
                get
                {
                    List<OCMField> result = new List<OCMField>();
                    var fields = from field in OCMFields.UnMappedFields
                                 where field.FieldType == OCMField.OCMFieldTypes.NumberOrText
                                 select field;
                    result.AddRange(fields);
                    return result;
                }
            }
            public static List<OCMField> UnMappedTextFields
            {
                get
                {
                    List<OCMField> result = new List<OCMField>();
                    var fields = from field in OCMFields.UnMappedFields
                                 where field.FieldType == OCMField.OCMFieldTypes.Text 
                                 select field;
                    result.AddRange(fields);
                    return result;
                }
            }
            #region Automatic Mappings
            public static OCMField Amount { get { return new OCMField(OCMDataSetTypes.Deals, OCMField.OCMFieldTypes.NumberOrText, FieldMappingTypes.Automatic, "Amount", false); } }
            public static OCMField BCMID { get { return new OCMField(OCMDataSetTypes.Deals, OCMField.OCMFieldTypes.Text, FieldMappingTypes.Automatic, "BCM ID", false, true); } }
            public static OCMField CloseDate { get { return new OCMField(OCMDataSetTypes.Deals, OCMField.OCMFieldTypes.Date, FieldMappingTypes.Automatic, "Close Date", false); } }
            public static OCMField CompanyName { get { return new OCMField(OCMDataSetTypes.Deals, OCMField.OCMFieldTypes.Text, FieldMappingTypes.Automatic, "Company Name", false); } }
            public static OCMField ContactEmail { get { return new OCMField(OCMDataSetTypes.Deals, OCMField.OCMFieldTypes.Text, FieldMappingTypes.Automatic, "Contact Email", false); } }
            public static OCMField Name { get { return new OCMField(OCMDataSetTypes.Deals, OCMField.OCMFieldTypes.Text, FieldMappingTypes.Automatic, "Name (Deal)", false); } }
            #endregion
            #region Manual Mappings
            //public static OCMField Probability { get { return new OCMField(OCMDataSetTypes.Deals, OCMField.OCMFieldTypes.NumberOrText, FieldMappingTypes.Manual, "Probability", false); } }
            public static OCMField Stage { get { return new OCMField(OCMDataSetTypes.Deals, OCMField.OCMFieldTypes.Text, FieldMappingTypes.Automatic, "Stage", false); } }
            #endregion
            #region Methods
            public static List<OCMField> GetCustomFields()
            {
                List<OCMField> result = new List<OCMField>();
                //Get OCM custom fields

                if (DealTemplate == null)
                    return result;

                OCMField.OCMFieldTypes ocmFieldType;
                foreach (var field in DealTemplate.Template.FieldList)
                {
                    if (!field.IsCustom || field.Deleted) //REVIEW Is IsCustom ever false???
                        continue;
                    switch (field._Type.Label)
                    {
                        case "Text":
                            ocmFieldType = OCMField.OCMFieldTypes.Text;
                            break;
                        case "Date":
                            ocmFieldType = OCMField.OCMFieldTypes.Date;
                            break;
                        case "Numeric":
                            ocmFieldType = OCMField.OCMFieldTypes.NumberOrText;
                            break;
                        default:
                            ocmFieldType = OCMField.OCMFieldTypes.Text;
                            break;
                    }
                    OCMField ocmField = new OCMField(OCMDataSetTypes.Deals, ocmFieldType, FieldMappingTypes.Manual, field.Label, false, false);
                    ocmField.IsCustom = true;
                    ocmField.PropertyID = field.Id;
                    result.Add(ocmField);
                }
                return result;
            }
            #endregion
        }
        public class OCMDataSet
        {
            public enum OCMDataSetTypes
            {
                Account,
                BusinessContact,
                Deal
            }
            public OCMDataSet()
            {
                MappedFields = new List<OCMField>();
                UnMappedFields = new List<OCMField>();
            }
            public List<OCMField> MappedFields { get; set; }
            public List<OCMField> UnMappedFields { get; set; }
        }
        #endregion
        #region BCM Fields
        public class BCMAccountFieldMappings: BCMDataSet
        {
            #region Constructors
            public BCMAccountFieldMappings()
            {
                MappedFields = new List<BCMField>();
                UnMappedFields = new List<BCMField>();
                BCMDataSetType = BCMDataSetTypes.Accounts;
            }
            #endregion
        }
        public class BCMBusinessContactFieldMappings : BCMDataSet
        {
            #region Constructors
            public BCMBusinessContactFieldMappings()
            {
                MappedFields = new List<BCMField>();
                UnMappedFields = new List<BCMField>();
                BCMDataSetType = BCMDataSetTypes.BusinessContacts;
            }
            #endregion
        }
        public class BCMOpportunityFieldMappings : BCMDataSet
        {
            #region Constructors
            public BCMOpportunityFieldMappings()
            {
                MappedFields = new List<BCMField>();
                UnMappedFields = new List<BCMField>();
                BCMDataSetType = BCMDataSetTypes.Accounts;
            }
            #endregion
        }
        public class BCMDataSet
        {
            public BCMDataSetTypes BCMDataSetType { get; set; }
            public List<BCMField> MappedFields { get; set; }
            public List<BCMField> UnMappedFields { get; set; }
        }
        public static class BCMAccountFields
        {
            #region Methods
            static List<BCMField> GetCustomFields()
            {
                List<BCMField> result = new List<BCMField>();

                try
                {
                    using (SqlConnection con = new SqlConnection(ConnectionString))
                    {
                        using (SqlCommand com = new SqlCommand(String.Format("{0} AND EntityTypeName = 'Account'", Resources.BCM_UserFields), con))
                        {
                            con.Open();

                            using (DbDataReader reader = com.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    BCMField.BCMFieldTypes fieldType;
                                    try
                                    {
                                        string type = Convert.ToString(reader["DataTypeID"]);
                                        int typeNum = Convert.ToInt16(type) - 1; //Must subtract one as base DataTypeID is 1, but base enum is 0 (all enums match the DataTypeID values in dbo.UserFieldDataTypes
                                        fieldType = (BCMField.BCMFieldTypes) Enum.Parse(typeof(BCMField.BCMFieldTypes), typeNum.ToString());
                                        BCMField field = new BCMField(BCMDataSetTypes.Accounts, fieldType, FieldMappingTypes.Manual, Convert.ToString(reader["FieldName"]), true);
                                        result.Add(field);
                                    }
                                    catch (Exception ex)
                                    {
                                        Log.Error(ex);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    Log.Error(ex);
                }

                return result;
            }
            #endregion
            #region Properties
            public static BCMAccountFieldMappings BCMFields { get; set; }
            public static List<BCMField> ManualFields
            {
                get
                {
                    List<BCMField> result = new List<BCMField>();
                    //Return all manual fields as default collection of unmapped fields 
                    //HIGH NOTE Update manual fields here if they change
                    result.Add(DoNotCall);
                    result.Add(DoNotEmail);
                    result.Add(DoNotMail);
                    result.Add(BusinessFax);
                    //result.Add(Notes);
                    result.AddRange(GetCustomFields());
                                                             
                    return result;
                }
            }
            public static List<BCMField> UnMappedDateFields
            {
                get
                {
                    List<BCMField> result = new List<BCMField>();
                    var fields = from field in BCMFields.UnMappedFields
                        where field.FieldType == BCMField.BCMFieldTypes.DateTime
                        select field;
                    result.AddRange(fields);
                    
                    return result;
                }
            }
            public static List<BCMField> UnMappedNumberOrTextFields
            {
                get
                {
                    List<BCMField> result = new List<BCMField>();
                    var fields = from field in BCMFields.UnMappedFields
                        where
                        field.FieldType == BCMField.BCMFieldTypes.Number ||
                        field.FieldType == BCMField.BCMFieldTypes.Percent ||
                        field.FieldType == BCMField.BCMFieldTypes.Currency ||
                        field.FieldType == BCMField.BCMFieldTypes.Integer                
                                 select field;
                    result.AddRange(fields);
                    return result;
                }
            }
            public static List<BCMField> UnMappedTextFields
            {
                get
                {
                    List<BCMField> result = new List<BCMField>();
                    var fields = from field in BCMFields.UnMappedFields
                                 where field.FieldType == BCMField.BCMFieldTypes.Text || field.FieldType == BCMField.BCMFieldTypes.YesNo || field.FieldType == BCMField.BCMFieldTypes.DropDownList || field.FieldType == BCMField.BCMFieldTypes.URL || field.FieldType == BCMField.BCMFieldTypes.Relationship
                                 select field;
                    result.AddRange(fields);                    
                    return result;
                }
            }
            #region Automatic Mappings
            public static BCMField BusinessAddress { get { return new BCMField(BCMField.BCMFieldTypes.Text, FieldMappingTypes.Automatic, "Business Address", OCMAccountFields.BusinessAddress); } }
            public static BCMField Email { get { return new BCMField(BCMField.BCMFieldTypes.Text, FieldMappingTypes.Automatic, "Email", OCMAccountFields.Email); } }
            public static BCMField FullName { get { return new BCMField(BCMField.BCMFieldTypes.Text, FieldMappingTypes.Automatic, "FullName", OCMAccountFields.Name); } }
            public static BCMField Notes { get { return new BCMField(BCMField.BCMFieldTypes.Text, FieldMappingTypes.Automatic, "Notes", OCMAccountFields.Notes); } }
            public static BCMField WebPageAddress { get { return new BCMField(BCMField.BCMFieldTypes.Text, FieldMappingTypes.Automatic, "Web Address", OCMAccountFields.BusinessHomePage); } }
            public static BCMField WorkPhoneNum { get { return new BCMField(BCMField.BCMFieldTypes.Text, FieldMappingTypes.Automatic, "Business Phone", OCMAccountFields.BusinessPhone); } }
            #endregion
            #region Manual Mappings
            //public static BCMField AccountNumber { get { return new BCMField(BCMField.BCMFieldTypes.Text, "Account Number"); } }
            //public static BCMField BusinessContact { get; set; }
            public static BCMField BusinessFax { get { return new BCMField(BCMDataSetTypes.Accounts, BCMField.BCMFieldTypes.Text, FieldMappingTypes.Manual, "BusinessFaxNum"); } }
            ////public static BCMField DetailsOrNotes { get; set; }
            //public static BCMField DisplayAs { get { return new BCMField(BCMDataSetTypes.Accounts, BCMField.BCMFieldTypes.Text, "Display As"); } }
            //public static BCMField Employees { get { return new BCMField(BCMDataSetTypes.Accounts, BCMField.BCMFieldTypes.Number, "Employees"); } }
            //public static BCMField MailingAddress { get; set; }// What field in AccountsFullView does MailingAddress map to? It's apparently boolean! Entity Field Mappings spec lists it as "Mail Address (Y/N)"              
            public static BCMField DoNotCall { get { return new BCMField(BCMDataSetTypes.Accounts, BCMField.BCMFieldTypes.Text, FieldMappingTypes.Manual, "DoNotCall"); } }
            public static BCMField DoNotEmail { get { return new BCMField(BCMDataSetTypes.Accounts, BCMField.BCMFieldTypes.Text, FieldMappingTypes.Manual, "DoNotEmail"); } }
            public static BCMField DoNotMail { get { return new BCMField(BCMDataSetTypes.Accounts, BCMField.BCMFieldTypes.Text, FieldMappingTypes.Manual, "DoNotMail"); } }
            //public static BCMField Notes { get { return new BCMField(BCMDataSetTypes.Accounts, BCMField.BCMFieldTypes.Text, FieldMappingTypes.OptionalAutomatic, "Notes"); } }
            //public static BCMField OutstandingBalance { get; set; }
            //public static BCMField Revenue { get { return new BCMField(BCMField.BCMFieldTypes.Number, "Revenue"); } }
            //public static BCMField Specialty { get; set; }
            #endregion
            #endregion
        }

        public static class BCMBusinessContactFields
        {
            #region Methods
            static List<BCMField> GetCustomFields()
            {
                List<BCMField> result = new List<BCMField>();

                try
                {
                    using (SqlConnection con = new SqlConnection(ConnectionString))
                    {
                        using (SqlCommand com = new SqlCommand(String.Format("{0} AND EntityTypeName = 'Contact'", Resources.BCM_UserFields), con))
                        {
                            con.Open();

                            using (DbDataReader reader = com.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    BCMField.BCMFieldTypes fieldType;
                                    try
                                    {
                                        string type = Convert.ToString(reader["DataTypeID"]);
                                        int typeNum = Convert.ToInt16(type) - 1; //Must subtract one as base DataTypeID is 1, but base enum is 0 (all enums match the DataTypeID values in dbo.UserFieldDataTypes
                                        fieldType = (BCMField.BCMFieldTypes) Enum.Parse(typeof(BCMField.BCMFieldTypes), typeNum.ToString());
                                        BCMField field = new BCMField(BCMDataSetTypes.BusinessContacts, fieldType, FieldMappingTypes.Manual, Convert.ToString(reader["FieldName"]), true);
                                        result.Add(field);
                                    }
                                    catch (Exception ex)
                                    {
                                        Log.Error(ex);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    Log.Error(ex);
                }

                return result;
            }
#endregion
            #region Properties
            public static BCMBusinessContactFieldMappings BCMFields { get; set; }
            public static List<BCMField> ManualFields
            {
                get
                {
                    List<BCMField> result = new List<BCMField>();
                    //Return all manual fields as default collection of unmapped fields 
                    //HIGH NOTE Update manual fields here if they change

                    //result.Add(AccountNumber);//No longer mapped
                    result.Add(Active);
                    result.Add(AreaOfInterest);//
                    result.Add(AssignedTo);//
                    result.Add(AssistantName);//
                    result.Add(Children);//
                    //result.Add(ContactNotes); //Automatic
                    result.Add(Department);//
                    result.Add(DoNotCall);//
                    result.Add(DoNotEmail);//
                    result.Add(DoNotFax);//
                    result.Add(DoNotSendLetter);
                    //result.Add(Employees);//No longer mapped
                    result.Add(Hobby);//
                    //result.Add(IMAddress);//No longer mapped
                    //result.Add(LastAccessTime);//No longer mapped
                    result.Add(LeadScore);//
                    //result.Add(LeadSource);//No longer mapped
                    result.Add(ManagerName);//
                    //result.Add(Nickname);//No longer mapped
                    result.Add(OfficeLocation);
                    result.Add(PaymentStatus);//
                    result.Add(PrefContactMethod);//
                    //result.Add(PrimaryPhoneNum);//No longer mapped
                    result.Add(Profession);//
                    result.Add(Rating);//
                    result.Add(ReferredBy);//
                    //result.Add(Revenue);//No longer mapped
                    result.Add(Spouse);//
                    //result.Add(Territory);//No longer mapped
                    //result.Add(TickerSymbol);//No longer mapped
                    //result.Add(TypeOfEntity);//No longer mapped
                    //result.Add(WebAddress);//No longer mapped
                    result.Add(WeddingAnniversary);//
                    result.AddRange(GetCustomFields());                    

                    return result;
                }
            }            
            public static List<BCMField> UnMappedDateFields
            {
                get
                {
                    List<BCMField> result = new List<BCMField>();
                    var fields = from field in BCMFields.UnMappedFields
                                 where field.FieldType == BCMField.BCMFieldTypes.DateTime
                                 select field;
                    result.AddRange(fields);
                    return result;
                }
            }
            public static List<BCMField> UnMappedNumberOrTextFields
            {
                get
                {
                    List<BCMField> result = new List<BCMField>();
                    var fields = from field in BCMFields.UnMappedFields
                                 where field.FieldType == BCMField.BCMFieldTypes.Number || field.FieldType == BCMField.BCMFieldTypes.Percent || field.FieldType == BCMField.BCMFieldTypes.Currency || field.FieldType == BCMField.BCMFieldTypes.Integer 
                                 select field;
                    result.AddRange(fields);
                    return result;
                }
            }
            public static List<BCMField> UnMappedTextFields
            {
                get
                {
                    List<BCMField> result = new List<BCMField>();
                    var fields = from field in BCMFields.UnMappedFields
                                 where field.FieldType == BCMField.BCMFieldTypes.Text || field.FieldType == BCMField.BCMFieldTypes.YesNo || field.FieldType == BCMField.BCMFieldTypes.DropDownList || field.FieldType == BCMField.BCMFieldTypes.URL || field.FieldType == BCMField.BCMFieldTypes.Relationship
                                 select field;
                    result.AddRange(fields);
                    return result;
                }
            }
            #region Automatic Mappings 
            //public static BCMField BCMID { get { return new BCMField(BCMField.BCMFieldTypes.Text, FieldMappingTypes.Automatic, "BCM ID", OCMBusinessContactFields.BCMID, true); } }
            public static BCMField Birthday { get { return new BCMField(BCMField.BCMFieldTypes.DateTime, FieldMappingTypes.Automatic, "Birthday", OCMBusinessContactFields.Birthday); } }
            //Changed from BusinessPhone
            //public static BCMField BusinessPhone { get { return new BCMField(BCMField.BCMFieldTypes.Text, FieldMappingTypes.Automatic, "Business Phone", OCMBusinessContactFields.BusinessPhone); } }
            public static BCMField CompanyName { get { return new BCMField(BCMField.BCMFieldTypes.Text, FieldMappingTypes.Automatic, "Company Name", OCMBusinessContactFields.Name); } }
            public static BCMField ContactNotes { get { return new BCMField(BCMField.BCMFieldTypes.Text, FieldMappingTypes.Automatic, "Contact Notes", OCMBusinessContactFields.Notes); } }
            public static BCMField Email1Address { get { return new BCMField(BCMField.BCMFieldTypes.Text, FieldMappingTypes.Automatic, "Email Address", OCMBusinessContactFields.Email); } }
            public static BCMField FullName { get { return new BCMField(BCMField.BCMFieldTypes.Text, FieldMappingTypes.Automatic, "FullName", OCMBusinessContactFields.Name); } }
            public static BCMField HomePhoneNum { get { return new BCMField(BCMField.BCMFieldTypes.Text, FieldMappingTypes.Automatic, "Home Phone", OCMBusinessContactFields.HomePhone); } }
            public static BCMField JobTitle { get { return new BCMField(BCMField.BCMFieldTypes.Text, FieldMappingTypes.Automatic, "Job Title", OCMBusinessContactFields.JobTitle); } }
            public static BCMField MobilePhoneNum { get { return new BCMField(BCMField.BCMFieldTypes.Text, FieldMappingTypes.Automatic, "Mobile Phone", OCMBusinessContactFields.MobilePhone); } }
            public static BCMField WorkPhoneNum { get { return new BCMField(BCMField.BCMFieldTypes.Text, FieldMappingTypes.Automatic, "Business Phone", OCMBusinessContactFields.BusinessPhone); } }
            public static BCMField WorkAddressStreet { get { return new BCMField(BCMField.BCMFieldTypes.Text, FieldMappingTypes.Automatic, "Business Address", OCMBusinessContactFields.BusinessAddress); } }
            #endregion
            #region Manual Mappings
            //public static BCMField AccountNumber { get {return new BCMField(BCMDataSetTypes.BusinessContacts, BCMField.BCMFieldTypes.Text, FieldMappingTypes.Manual, "AccountNumber"); } }
            public static BCMField Active { get { return new BCMField(BCMDataSetTypes.BusinessContacts, BCMField.BCMFieldTypes.YesNo, FieldMappingTypes.Manual, "Active"); } }
            public static BCMField AreaOfInterest { get { return new BCMField(BCMDataSetTypes.BusinessContacts, BCMField.BCMFieldTypes.Text, FieldMappingTypes.Manual, "AreaOfInterest"); } }
            public static BCMField AssignedTo { get { return new BCMField(BCMDataSetTypes.BusinessContacts, BCMField.BCMFieldTypes.Text, FieldMappingTypes.Manual, "AssignedTo"); } }
            public static BCMField AssistantName { get { return new BCMField(BCMDataSetTypes.BusinessContacts, BCMField.BCMFieldTypes.Text, FieldMappingTypes.Manual, "AssistantName"); } }
            public static BCMField Children { get { return new BCMField(BCMDataSetTypes.BusinessContacts, BCMField.BCMFieldTypes.Text, FieldMappingTypes.Manual, "Children"); } }
            public static BCMField Department { get { return new BCMField(BCMDataSetTypes.BusinessContacts, BCMField.BCMFieldTypes.Text, FieldMappingTypes.Manual, "Department"); } }
            public static BCMField DoNotCall { get { return new BCMField(BCMDataSetTypes.BusinessContacts, BCMField.BCMFieldTypes.YesNo, FieldMappingTypes.Manual, "DoNotCall"); } }
            public static BCMField DoNotEmail { get { return new BCMField(BCMDataSetTypes.BusinessContacts, BCMField.BCMFieldTypes.YesNo, FieldMappingTypes.Manual, "DoNotEmail"); } }
            public static BCMField DoNotFax { get { return new BCMField(BCMDataSetTypes.BusinessContacts, BCMField.BCMFieldTypes.YesNo, FieldMappingTypes.Manual, "DoNotFax"); } }
            public static BCMField DoNotSendLetter { get { return new BCMField(BCMDataSetTypes.BusinessContacts, BCMField.BCMFieldTypes.YesNo, FieldMappingTypes.Manual, "DoNotSendLetter"); } }
            //public static BCMField Employees { get { return new BCMField(BCMDataSetTypes.BusinessContacts, BCMField.BCMFieldTypes.Text, FieldMappingTypes.Manual, "Employees"); } }
            public static BCMField Hobby { get { return new BCMField(BCMDataSetTypes.BusinessContacts, BCMField.BCMFieldTypes.Text, FieldMappingTypes.Manual, "Hobby"); } }
            //public static BCMField IMAddress { get { return new BCMField(BCMDataSetTypes.BusinessContacts, BCMField.BCMFieldTypes.Text, FieldMappingTypes.Manual, "IMAddress"); } }
            //public static BCMField LastAccessTime { get { return new BCMField(BCMDataSetTypes.BusinessContacts, BCMField.BCMFieldTypes.DateTime, FieldMappingTypes.Manual, "LastAccessTime"); } }
            public static BCMField LeadScore { get { return new BCMField(BCMDataSetTypes.BusinessContacts, BCMField.BCMFieldTypes.Text, FieldMappingTypes.Manual, "LeadScore"); } }
            //public static BCMField LeadSource { get { return new BCMField(BCMDataSetTypes.BusinessContacts, BCMField.BCMFieldTypes.Text, FieldMappingTypes.Manual, "LeadSource"); } }
            public static BCMField ManagerName { get { return new BCMField(BCMDataSetTypes.BusinessContacts, BCMField.BCMFieldTypes.Text, FieldMappingTypes.Manual, "ManagerName"); } }
            //public static BCMField Nickname { get { return new BCMField(BCMDataSetTypes.BusinessContacts, BCMField.BCMFieldTypes.Text, FieldMappingTypes.Manual, "Nickname"); } }
            public static BCMField OfficeLocation { get { return new BCMField(BCMDataSetTypes.BusinessContacts, BCMField.BCMFieldTypes.Text, FieldMappingTypes.Manual, "OfficeLocation"); } }
            public static BCMField PaymentStatus { get { return new BCMField(BCMDataSetTypes.BusinessContacts, BCMField.BCMFieldTypes.Text, FieldMappingTypes.Manual, "PaymentStatus"); } }
            public static BCMField PrefContactMethod { get { return new BCMField(BCMDataSetTypes.BusinessContacts, BCMField.BCMFieldTypes.Text, FieldMappingTypes.Manual, "PrefContactMethod"); } }
            //public static BCMField PrimaryPhoneNum { get { return new BCMField(BCMDataSetTypes.BusinessContacts, BCMField.BCMFieldTypes.Text, FieldMappingTypes.Manual, "PrimaryPhoneNum"); } }
            public static BCMField Profession { get { return new BCMField(BCMDataSetTypes.BusinessContacts, BCMField.BCMFieldTypes.Text, FieldMappingTypes.Manual, "Profession"); } }
            public static BCMField Rating { get { return new BCMField(BCMDataSetTypes.BusinessContacts, BCMField.BCMFieldTypes.Text, FieldMappingTypes.Manual, "Rating"); } }
            public static BCMField ReferredBy { get { return new BCMField(BCMDataSetTypes.BusinessContacts, BCMField.BCMFieldTypes.Text, FieldMappingTypes.Manual, "ReferredBy"); } }
            //public static BCMField Revenue { get { return new BCMField(BCMDataSetTypes.BusinessContacts, BCMField.BCMFieldTypes.Currency, FieldMappingTypes.Manual, "Revenue"); } }
            public static BCMField Spouse { get { return new BCMField(BCMDataSetTypes.BusinessContacts, BCMField.BCMFieldTypes.Text, FieldMappingTypes.Manual, "Spouse"); } }
            //public static BCMField Territory { get { return new BCMField(BCMDataSetTypes.BusinessContacts, BCMField.BCMFieldTypes.Text, FieldMappingTypes.Manual, "Territory"); } }
            //public static BCMField TickerSymbol { get { return new BCMField(BCMDataSetTypes.BusinessContacts, BCMField.BCMFieldTypes.Text, FieldMappingTypes.Manual, "TickerSymbol"); } }
            //public static BCMField TypeOfEntity { get { return new BCMField(BCMDataSetTypes.BusinessContacts, BCMField.BCMFieldTypes.Text, FieldMappingTypes.Manual, "TypeOfEntity"); } }
            //public static BCMField WebAddress { get { return new BCMField(BCMDataSetTypes.BusinessContacts, BCMField.BCMFieldTypes.Text, FieldMappingTypes.Manual, "WebAddress"); } }
            public static BCMField WeddingAnniversary { get { return new BCMField(BCMDataSetTypes.BusinessContacts, BCMField.BCMFieldTypes.Text, FieldMappingTypes.Manual, "WeddingAnniversary"); } }
            public static BCMField WorkAddressPOB { get { return new BCMField(BCMDataSetTypes.BusinessContacts, BCMField.BCMFieldTypes.Text, FieldMappingTypes.Manual, "WorkAddressPOB"); } }
            #endregion
            #endregion
        }
        public static class BCMOpportunityFields
        {
            #region Methods
            static List<BCMField> GetCustomFields()
            {
                List<BCMField> result = new List<BCMField>();

                try
                {
                    using (SqlConnection con = new SqlConnection(ConnectionString))
                    {
                        using (SqlCommand com = new SqlCommand(String.Format("{0} AND EntityTypeName = 'Opportunity'", Resources.BCM_UserFields), con))
                        {
                            con.Open();

                            using (DbDataReader reader = com.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    BCMField.BCMFieldTypes fieldType;
                                    try
                                    {
                                        string type = Convert.ToString(reader["DataTypeID"]);
                                        int typeNum = Convert.ToInt16(type) - 1; //Must subtract one as base DataTypeID is 1, but base enum is 0 (all enums match the DataTypeID values in dbo.UserFieldDataTypes
                                        fieldType = (BCMField.BCMFieldTypes) Enum.Parse(typeof(BCMField.BCMFieldTypes), typeNum.ToString());
                                        BCMField field = new BCMField(BCMDataSetTypes.Opportunities, fieldType, FieldMappingTypes.Manual, Convert.ToString(reader["FieldName"]), true);
                                        result.Add(field);
                                    }
                                    catch (Exception ex)
                                    {
                                        Log.Error(ex);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    Log.Error(ex);
                }

                return result;
            }
            #endregion
            #region Properties
            public static BCMOpportunityFieldMappings BCMFields { get; set; }
            public static List<BCMField> ManualFields
            {
                get
                {
                    List<BCMField> result = new List<BCMField>();
                    //Return all manual fields as default collection of unmapped fields 
                    //HIGH NOTE Update manual fields here if they change
                    result.Add(AreaOfInterest);
                    result.Add(AssignedTo);
                    result.Add(Competition);
                    result.Add(ExpectedGrossMargin);
                    result.Add(ExpectedRevenue);
                    result.Add(GrossMargin);
                    result.Add(LeadSource);                    
                    result.Add(Probability);
                    result.Add(ReferredBy);
                    result.Add(Status);
                    result.Add(TotalDiscount);
                    result.AddRange(GetCustomFields());
                    return result;
                }
            }
            public static List<BCMField> UnMappedDateFields
            {
                get
                {
                    List<BCMField> result = new List<BCMField>();
                    var fields = from field in BCMFields.UnMappedFields
                                 where field.FieldType == BCMField.BCMFieldTypes.DateTime
                                 select field;
                    result.AddRange(fields);                    
                    return result;
                }
            }

            public static List<BCMField> UnMappedNumberOrTextFields
            {
                get
                {
                    List<BCMField> result = new List<BCMField>();
                    var fields = from field in BCMFields.UnMappedFields
                                 where field.FieldType == BCMField.BCMFieldTypes.Number || field.FieldType == BCMField.BCMFieldTypes.Percent || field.FieldType == BCMField.BCMFieldTypes.Currency || field.FieldType == BCMField.BCMFieldTypes.Integer 
                                 select field;
                    result.AddRange(fields);
                    return result;
                }
            }

            public static List<BCMField> UnMappedTextFields
            {
                get
                {
                    List<BCMField> result = new List<BCMField>();
                    var fields = from field in BCMFields.UnMappedFields
                                 where field.FieldType == BCMField.BCMFieldTypes.Text || field.FieldType == BCMField.BCMFieldTypes.YesNo || field.FieldType == BCMField.BCMFieldTypes.DropDownList || field.FieldType == BCMField.BCMFieldTypes.URL || field.FieldType == BCMField.BCMFieldTypes.Relationship
                                 select field;
                    result.AddRange(fields);
                    return result;
                }
            }
            #region Automatic Mappings 
            public static BCMField BCMID { get { return new BCMField(BCMField.BCMFieldTypes.Text, FieldMappingTypes.Automatic, "BCM ID", OCMDealFields.BCMID, true); } }
            public static BCMField Account { get { return new BCMField(BCMField.BCMFieldTypes.Text, FieldMappingTypes.Automatic, "Company Name", OCMDealFields.CloseDate); } }
            public static BCMField CloseDate { get { return new BCMField(BCMField.BCMFieldTypes.DateTime, FieldMappingTypes.Automatic, "Close Date", OCMDealFields.CloseDate); } }
            public static BCMField Contact { get { return new BCMField(BCMField.BCMFieldTypes.Text, FieldMappingTypes.Automatic, "Contact", OCMDealFields.ContactEmail); } }
            //public static BCMField Probability { get { return new BCMField(BCMField.BCMFieldTypes.Number, FieldMappingTypes.Automatic, "Probability", OCMDealFields.Probability); } }
            public static BCMField Stage { get { return new BCMField(BCMField.BCMFieldTypes.Text, FieldMappingTypes.Automatic, "Stage", OCMDealFields.Stage); } }
            public static BCMField Title { get { return new BCMField(BCMField.BCMFieldTypes.Text, FieldMappingTypes.Automatic, "Deal (Name)", OCMDealFields.Name); } }
            public static BCMField TotalAmount { get { return new BCMField(BCMField.BCMFieldTypes.Number, FieldMappingTypes.Automatic, "Total Amount", OCMDealFields.Amount); } }
            #endregion
            #region Manual Mappings
            public static BCMField AreaOfInterest
            {
                get
                {
                    //BCMField newField;
                    //newField = new BCMField(BCMDataSetTypes.Opportunities, BCMField.BCMFieldTypes.Text,
                    //    FieldMappingTypes.Manual, "AreaOfInterest");
                    //newField.Label = "Area Of Interest";
                    return new BCMField(BCMDataSetTypes.Opportunities, BCMField.BCMFieldTypes.Text,
                        FieldMappingTypes.Manual, "AreaOfInterest");
                }
            }

            public static BCMField AssignedTo { get { return new BCMField(BCMDataSetTypes.Opportunities, BCMField.BCMFieldTypes.Text, FieldMappingTypes.Manual, "AssignedTo"); } }
            public static BCMField Competition { get { return new BCMField(BCMDataSetTypes.Opportunities, BCMField.BCMFieldTypes.Text, FieldMappingTypes.Manual, "Competition"); } }
            public static BCMField ExpectedGrossMargin { get { return new BCMField(BCMDataSetTypes.Opportunities, BCMField.BCMFieldTypes.Number, FieldMappingTypes.Manual, "OpportunityExpectedGrossMargin"); } }
            public static BCMField ExpectedRevenue { get { return new BCMField(BCMDataSetTypes.Opportunities, BCMField.BCMFieldTypes.Number, FieldMappingTypes.Manual, "OpportunityExpectedRevenue"); } }
            public static BCMField GrossMargin { get { return new BCMField(BCMDataSetTypes.Opportunities, BCMField.BCMFieldTypes.Number, FieldMappingTypes.Manual, "OpportunityGrossMargin"); } }
            public static BCMField LeadSource { get { return new BCMField(BCMDataSetTypes.Opportunities, BCMField.BCMFieldTypes.Text, FieldMappingTypes.Manual, "LeadSource"); } }
            public static BCMField Probability { get { return new BCMField(BCMDataSetTypes.Opportunities, BCMField.BCMFieldTypes.Number, FieldMappingTypes.Manual, "Probability"); } }
            public static BCMField ReferredBy { get { return new BCMField(BCMDataSetTypes.Opportunities, BCMField.BCMFieldTypes.Text, FieldMappingTypes.Manual, "ReferredBy"); } }
            public static BCMField Status { get { return new BCMField(BCMDataSetTypes.Opportunities, BCMField.BCMFieldTypes.Text, FieldMappingTypes.Manual, "Status"); } }
            public static BCMField TotalDiscount { get { return new BCMField(BCMDataSetTypes.Opportunities, BCMField.BCMFieldTypes.Number, FieldMappingTypes.Manual, "OpportunityTotalDiscount"); } }           
            #endregion
            #endregion
        }
        #endregion
    }
}
