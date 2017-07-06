namespace BCM_Migration_Tool.Objects
{
    /// <summary>
    /// Represents the JSON returned from /api/me/XrmOrganizationTemplate('CompanyTemplate') AND /api/me/XrmContactTemplate('ContactTemplate')
    /// </summary>
    public partial class OCMCompanyTemplate
    {

        public class Rootobject
        {
            public string odatacontext { get; set; }
            public string odataid { get; set; }
            public string Id { get; set; }
            public Template Template { get; set; }
            public int Version { get; set; }
            public string ChangeKey { get; set; }
        }

        public class Template
        {
            public string _Name { get; set; }
            public string FolderClass { get; set; }
            public int _Version { get; set; }
            public int FieldLimit { get; set; }
            public int StageLimit { get; set; }
            public Customtypelist[] CustomTypeList { get; set; }
            public Fieldlist[] FieldList { get; set; }
        }

        public class Customtypelist
        {
            public string Label { get; set; }
            public string NativeType { get; set; }
        }

        public class Fieldlist
        {
            public string Id { get; set; }
            public string Label { get; set; }
            public _Type _Type { get; set; }
            public bool IsCustom { get; set; }
            public bool Deleted { get; set; }
        }

        public class _Type
        {
            public string Label { get; set; }
            public string NativeType { get; set; }
        }

    }
}
