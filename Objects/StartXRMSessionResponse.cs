namespace BCM_Migration_Tool.Objects
{
    internal class StartXRMSessionResponse
    {
        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
        public partial class Envelope
        {
            private EnvelopeBody bodyField;
            private EnvelopeHeader headerField;

            /// <remarks/>
            public EnvelopeBody Body
            {
                get
                {
                    return this.bodyField;
                }
                set
                {
                    this.bodyField = value;
                }
            }

            /// <remarks/>
            public EnvelopeHeader Header
            {
                get
                {
                    return this.headerField;
                }
                set
                {
                    this.headerField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public partial class EnvelopeBody
        {
            private StartXrmSessionResponse startXrmSessionResponseField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
            public StartXrmSessionResponse StartXrmSessionResponse
            {
                get
                {
                    return this.startXrmSessionResponseField;
                }
                set
                {
                    this.startXrmSessionResponseField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public partial class EnvelopeHeader
        {
            private ServerVersionInfo serverVersionInfoField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
            public ServerVersionInfo ServerVersionInfo
            {
                get
                {
                    return this.serverVersionInfoField;
                }
                set
                {
                    this.serverVersionInfoField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        public partial class ServerVersionInfo
        {
            private ushort majorBuildNumberField;
            private byte majorVersionField;
            private byte minorBuildNumberField;
            private byte minorVersionField;
            private string versionField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public ushort MajorBuildNumber
            {
                get
                {
                    return this.majorBuildNumberField;
                }
                set
                {
                    this.majorBuildNumberField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte MajorVersion
            {
                get
                {
                    return this.majorVersionField;
                }
                set
                {
                    this.majorVersionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte MinorBuildNumber
            {
                get
                {
                    return this.minorBuildNumberField;
                }
                set
                {
                    this.minorBuildNumberField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte MinorVersion
            {
                get
                {
                    return this.minorVersionField;
                }
                set
                {
                    this.minorVersionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Version
            {
                get
                {
                    return this.versionField;
                }
                set
                {
                    this.versionField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages", IsNullable = false)]
        public partial class StartXrmSessionResponse
        {
            private string allSalesTeamGroupMailboxGuidField;
            private string allSalesTeamGroupObjectIdField;
            private bool isAllSalesTeamGroupOneDriveProvisionedField;
            private bool isMySalesGroupOneDriveProvisionedField;
            private string mySalesGroupMailboxGuidField;
            private string mySalesGroupObjectIdField;
            private string responseClassField;
            private string responseCodeField;

            /// <remarks/>
            public string AllSalesTeamGroupMailboxGuid
            {
                get
                {
                    return this.allSalesTeamGroupMailboxGuidField;
                }
                set
                {
                    this.allSalesTeamGroupMailboxGuidField = value;
                }
            }

            /// <remarks/>
            public string AllSalesTeamGroupObjectId
            {
                get
                {
                    return this.allSalesTeamGroupObjectIdField;
                }
                set
                {
                    this.allSalesTeamGroupObjectIdField = value;
                }
            }

            /// <remarks/>
            public bool IsAllSalesTeamGroupOneDriveProvisioned
            {
                get
                {
                    return this.isAllSalesTeamGroupOneDriveProvisionedField;
                }
                set
                {
                    this.isAllSalesTeamGroupOneDriveProvisionedField = value;
                }
            }

            /// <remarks/>
            public bool IsMySalesGroupOneDriveProvisioned
            {
                get
                {
                    return this.isMySalesGroupOneDriveProvisionedField;
                }
                set
                {
                    this.isMySalesGroupOneDriveProvisionedField = value;
                }
            }

            /// <remarks/>
            public string MySalesGroupMailboxGuid
            {
                get
                {
                    return this.mySalesGroupMailboxGuidField;
                }
                set
                {
                    this.mySalesGroupMailboxGuidField = value;
                }
            }

            /// <remarks/>
            public string MySalesGroupObjectId
            {
                get
                {
                    return this.mySalesGroupObjectIdField;
                }
                set
                {
                    this.mySalesGroupObjectIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string ResponseClass
            {
                get
                {
                    return this.responseClassField;
                }
                set
                {
                    this.responseClassField = value;
                }
            }

            /// <remarks/>
            public string ResponseCode
            {
                get
                {
                    return this.responseCodeField;
                }
                set
                {
                    this.responseCodeField = value;
                }
            }
        }
    }
}
