namespace BCM_Migration_Tool.Objects
{
    internal class FindPeople
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
            private FindPeopleResponse findPeopleResponseField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
            public FindPeopleResponse FindPeopleResponse
            {
                get
                {
                    return this.findPeopleResponseField;
                }
                set
                {
                    this.findPeopleResponseField = value;
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
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages", IsNullable = false)]
        public partial class FindPeopleResponse
        {
            private byte firstLoadedRowIndexField;
            private sbyte firstMatchingRowIndexField;
            private Persona[] peopleField;
            private string responseClassField;
            private string responseCodeField;
            private ushort totalNumberOfPeopleInViewField;

            /// <remarks/>
            public byte FirstLoadedRowIndex
            {
                get
                {
                    return this.firstLoadedRowIndexField;
                }
                set
                {
                    this.firstLoadedRowIndexField = value;
                }
            }

            /// <remarks/>
            public sbyte FirstMatchingRowIndex
            {
                get
                {
                    return this.firstMatchingRowIndexField;
                }
                set
                {
                    this.firstMatchingRowIndexField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("Persona", Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
            public Persona[] People
            {
                get
                {
                    return this.peopleField;
                }
                set
                {
                    this.peopleField = value;
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

            /// <remarks/>
            public ushort TotalNumberOfPeopleInView
            {
                get
                {
                    return this.totalNumberOfPeopleInViewField;
                }
                set
                {
                    this.totalNumberOfPeopleInViewField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        public partial class Persona
        {
            private string companyNameField;
            private System.DateTime creationTimeField;
            private string displayNameField;
            private string displayNameFirstLastField;
            private string displayNameLastFirstField;
            private PersonaAddress[] emailAddressesField;
            private PersonaEmailAddress emailAddressField;
            private string fileAsField;
            private string givenNameField;
            private string homeCityField;
            private string imAddressField;
            private PersonaPersonaId personaIdField;
            private string personaTypeField;
            private uint relevanceScoreField;
            private string surnameField;
            private string workCityField;

            /// <remarks/>
            public string CompanyName
            {
                get
                {
                    return this.companyNameField;
                }
                set
                {
                    this.companyNameField = value;
                }
            }

            /// <remarks/>
            public System.DateTime CreationTime
            {
                get
                {
                    return this.creationTimeField;
                }
                set
                {
                    this.creationTimeField = value;
                }
            }

            /// <remarks/>
            public string DisplayName
            {
                get
                {
                    return this.displayNameField;
                }
                set
                {
                    this.displayNameField = value;
                }
            }

            /// <remarks/>
            public string DisplayNameFirstLast
            {
                get
                {
                    return this.displayNameFirstLastField;
                }
                set
                {
                    this.displayNameFirstLastField = value;
                }
            }

            /// <remarks/>
            public string DisplayNameLastFirst
            {
                get
                {
                    return this.displayNameLastFirstField;
                }
                set
                {
                    this.displayNameLastFirstField = value;
                }
            }

            /// <remarks/>
            public PersonaEmailAddress EmailAddress
            {
                get
                {
                    return this.emailAddressField;
                }
                set
                {
                    this.emailAddressField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("Address", IsNullable = false)]
            public PersonaAddress[] EmailAddresses
            {
                get
                {
                    return this.emailAddressesField;
                }
                set
                {
                    this.emailAddressesField = value;
                }
            }

            /// <remarks/>
            public string FileAs
            {
                get
                {
                    return this.fileAsField;
                }
                set
                {
                    this.fileAsField = value;
                }
            }

            /// <remarks/>
            public string GivenName
            {
                get
                {
                    return this.givenNameField;
                }
                set
                {
                    this.givenNameField = value;
                }
            }

            /// <remarks/>
            public string HomeCity
            {
                get
                {
                    return this.homeCityField;
                }
                set
                {
                    this.homeCityField = value;
                }
            }

            /// <remarks/>
            public string ImAddress
            {
                get
                {
                    return this.imAddressField;
                }
                set
                {
                    this.imAddressField = value;
                }
            }

            /// <remarks/>
            public PersonaPersonaId PersonaId
            {
                get
                {
                    return this.personaIdField;
                }
                set
                {
                    this.personaIdField = value;
                }
            }

            /// <remarks/>
            public string PersonaType
            {
                get
                {
                    return this.personaTypeField;
                }
                set
                {
                    this.personaTypeField = value;
                }
            }

            /// <remarks/>
            public uint RelevanceScore
            {
                get
                {
                    return this.relevanceScoreField;
                }
                set
                {
                    this.relevanceScoreField = value;
                }
            }

            /// <remarks/>
            public string Surname
            {
                get
                {
                    return this.surnameField;
                }
                set
                {
                    this.surnameField = value;
                }
            }

            /// <remarks/>
            public string WorkCity
            {
                get
                {
                    return this.workCityField;
                }
                set
                {
                    this.workCityField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
        public partial class PersonaAddress
        {
            private string emailAddressField;
            private PersonaAddressItemId itemIdField;
            private string mailboxTypeField;
            private string nameField;
            private string originalDisplayNameField;
            private string routingTypeField;

            /// <remarks/>
            public string EmailAddress
            {
                get
                {
                    return this.emailAddressField;
                }
                set
                {
                    this.emailAddressField = value;
                }
            }

            /// <remarks/>
            public PersonaAddressItemId ItemId
            {
                get
                {
                    return this.itemIdField;
                }
                set
                {
                    this.itemIdField = value;
                }
            }

            /// <remarks/>
            public string MailboxType
            {
                get
                {
                    return this.mailboxTypeField;
                }
                set
                {
                    this.mailboxTypeField = value;
                }
            }

            /// <remarks/>
            public string Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }

            /// <remarks/>
            public string OriginalDisplayName
            {
                get
                {
                    return this.originalDisplayNameField;
                }
                set
                {
                    this.originalDisplayNameField = value;
                }
            }

            /// <remarks/>
            public string RoutingType
            {
                get
                {
                    return this.routingTypeField;
                }
                set
                {
                    this.routingTypeField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
        public partial class PersonaAddressItemId
        {
            private string changeKeyField;
            private string idField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string ChangeKey
            {
                get
                {
                    return this.changeKeyField;
                }
                set
                {
                    this.changeKeyField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Id
            {
                get
                {
                    return this.idField;
                }
                set
                {
                    this.idField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
        public partial class PersonaEmailAddress
        {
            private string emailAddressField;
            private PersonaEmailAddressItemId itemIdField;
            private string mailboxTypeField;
            private string nameField;
            private string originalDisplayNameField;
            private string routingTypeField;

            /// <remarks/>
            public string EmailAddress
            {
                get
                {
                    return this.emailAddressField;
                }
                set
                {
                    this.emailAddressField = value;
                }
            }

            /// <remarks/>
            public PersonaEmailAddressItemId ItemId
            {
                get
                {
                    return this.itemIdField;
                }
                set
                {
                    this.itemIdField = value;
                }
            }

            /// <remarks/>
            public string MailboxType
            {
                get
                {
                    return this.mailboxTypeField;
                }
                set
                {
                    this.mailboxTypeField = value;
                }
            }

            /// <remarks/>
            public string Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }

            /// <remarks/>
            public string OriginalDisplayName
            {
                get
                {
                    return this.originalDisplayNameField;
                }
                set
                {
                    this.originalDisplayNameField = value;
                }
            }

            /// <remarks/>
            public string RoutingType
            {
                get
                {
                    return this.routingTypeField;
                }
                set
                {
                    this.routingTypeField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
        public partial class PersonaEmailAddressItemId
        {
            private string changeKeyField;
            private string idField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string ChangeKey
            {
                get
                {
                    return this.changeKeyField;
                }
                set
                {
                    this.changeKeyField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Id
            {
                get
                {
                    return this.idField;
                }
                set
                {
                    this.idField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
        public partial class PersonaPersonaId
        {
            private string idField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Id
            {
                get
                {
                    return this.idField;
                }
                set
                {
                    this.idField = value;
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
    }
}
