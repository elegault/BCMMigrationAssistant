namespace BCM_Migration_Tool.Objects
{
    class GetPersonaResponse
    {
        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        public partial class AttributedHasActiveDeals
        {
            private AttributedHasActiveDealsStringAttributedValue stringAttributedValueField;

            /// <remarks/>
            public AttributedHasActiveDealsStringAttributedValue StringAttributedValue
            {
                get
                {
                    return this.stringAttributedValueField;
                }
                set
                {
                    this.stringAttributedValueField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
        public partial class AttributedHasActiveDealsStringAttributedValue
        {
            private byte[] attributionsField;
            private string valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayAttribute()]
            [System.Xml.Serialization.XmlArrayItemAttribute("Attribution", IsNullable = false)]
            public byte[] Attributions
            {
                get
                {
                    return this.attributionsField;
                }
                set
                {
                    this.attributionsField = value;
                }
            }

            /// <remarks/>
            public string Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        public partial class AttributedIsBusinessContact
        {
            private AttributedIsBusinessContactStringAttributedValue[] stringAttributedValueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("StringAttributedValue")]
            public AttributedIsBusinessContactStringAttributedValue[] StringAttributedValue
            {
                get
                {
                    return this.stringAttributedValueField;
                }
                set
                {
                    this.stringAttributedValueField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
        public partial class AttributedIsBusinessContactStringAttributedValue
        {
            private AttributedIsBusinessContactStringAttributedValueAttributions attributionsField;
            private string valueField;

            /// <remarks/>
            public AttributedIsBusinessContactStringAttributedValueAttributions Attributions
            {
                get
                {
                    return this.attributionsField;
                }
                set
                {
                    this.attributionsField = value;
                }
            }

            /// <remarks/>
            public string Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
        public partial class AttributedIsBusinessContactStringAttributedValueAttributions
        {
            private byte attributionField;

            /// <remarks/>
            public byte Attribution
            {
                get
                {
                    return this.attributionField;
                }
                set
                {
                    this.attributionField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        public partial class Attributions
        {
            private AttributionsAttribution[] attributionField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Attribution")]
            public AttributionsAttribution[] Attribution
            {
                get
                {
                    return this.attributionField;
                }
                set
                {
                    this.attributionField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
        public partial class AttributionsAttribution
        {
            private string displayNameField;
            private AttributionsAttributionFolderId folderIdField;
            private byte idField;
            private bool isHiddenField;
            private bool isQuickContactField;
            private bool isWritableField;
            private AttributionsAttributionSourceId sourceIdField;

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
            public AttributionsAttributionFolderId FolderId
            {
                get
                {
                    return this.folderIdField;
                }
                set
                {
                    this.folderIdField = value;
                }
            }

            /// <remarks/>
            public byte Id
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

            /// <remarks/>
            public bool IsHidden
            {
                get
                {
                    return this.isHiddenField;
                }
                set
                {
                    this.isHiddenField = value;
                }
            }

            /// <remarks/>
            public bool IsQuickContact
            {
                get
                {
                    return this.isQuickContactField;
                }
                set
                {
                    this.isQuickContactField = value;
                }
            }

            /// <remarks/>
            public bool IsWritable
            {
                get
                {
                    return this.isWritableField;
                }
                set
                {
                    this.isWritableField = value;
                }
            }

            /// <remarks/>
            public AttributionsAttributionSourceId SourceId
            {
                get
                {
                    return this.sourceIdField;
                }
                set
                {
                    this.sourceIdField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
        public partial class AttributionsAttributionFolderId
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
        public partial class AttributionsAttributionSourceId
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
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        public partial class CompanyNames
        {
            private CompanyNamesStringAttributedValue stringAttributedValueField;

            /// <remarks/>
            public CompanyNamesStringAttributedValue StringAttributedValue
            {
                get
                {
                    return this.stringAttributedValueField;
                }
                set
                {
                    this.stringAttributedValueField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
        public partial class CompanyNamesStringAttributedValue
        {
            private byte[] attributionsField;
            private string valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayAttribute()]
            [System.Xml.Serialization.XmlArrayItemAttribute("Attribution", IsNullable = false)]
            public byte[] Attributions
            {
                get
                {
                    return this.attributionsField;
                }
                set
                {
                    this.attributionsField = value;
                }
            }

            /// <remarks/>
            public string Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        public partial class DisplayNames
        {
            private DisplayNamesStringAttributedValue stringAttributedValueField;

            /// <remarks/>
            public DisplayNamesStringAttributedValue StringAttributedValue
            {
                get
                {
                    return this.stringAttributedValueField;
                }
                set
                {
                    this.stringAttributedValueField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
        public partial class DisplayNamesStringAttributedValue
        {
            private byte[] attributionsField;
            private string valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayAttribute()]
            [System.Xml.Serialization.XmlArrayItemAttribute("Attribution", IsNullable = false)]
            public byte[] Attributions
            {
                get
                {
                    return this.attributionsField;
                }
                set
                {
                    this.attributionsField = value;
                }
            }

            /// <remarks/>
            public string Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        public partial class EmailAddress
        {
            private string emailAddress1Field;
            private EmailAddressItemId itemIdField;
            private string mailboxTypeField;
            private string nameField;
            private string originalDisplayNameField;
            private string routingTypeField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("EmailAddress")]
            public string EmailAddress1
            {
                get
                {
                    return this.emailAddress1Field;
                }
                set
                {
                    this.emailAddress1Field = value;
                }
            }

            /// <remarks/>
            public EmailAddressItemId ItemId
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
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        public partial class EmailAddresses
        {
            private EmailAddressesAddress addressField;

            /// <remarks/>
            public EmailAddressesAddress Address
            {
                get
                {
                    return this.addressField;
                }
                set
                {
                    this.addressField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
        public partial class EmailAddressesAddress
        {
            private string emailAddressField;
            private EmailAddressesAddressItemId itemIdField;
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
            public EmailAddressesAddressItemId ItemId
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
        public partial class EmailAddressesAddressItemId
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
        public partial class EmailAddressItemId
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
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        public partial class Emails1
        {
            private Emails1EmailAddressAttributedValue[] emailAddressAttributedValueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("EmailAddressAttributedValue")]
            public Emails1EmailAddressAttributedValue[] EmailAddressAttributedValue
            {
                get
                {
                    return this.emailAddressAttributedValueField;
                }
                set
                {
                    this.emailAddressAttributedValueField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
        public partial class Emails1EmailAddressAttributedValue
        {
            private Emails1EmailAddressAttributedValueAttributions attributionsField;
            private Emails1EmailAddressAttributedValueValue valueField;

            /// <remarks/>
            public Emails1EmailAddressAttributedValueAttributions Attributions
            {
                get
                {
                    return this.attributionsField;
                }
                set
                {
                    this.attributionsField = value;
                }
            }

            /// <remarks/>
            public Emails1EmailAddressAttributedValueValue Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
        public partial class Emails1EmailAddressAttributedValueAttributions
        {
            private byte attributionField;

            /// <remarks/>
            public byte Attribution
            {
                get
                {
                    return this.attributionField;
                }
                set
                {
                    this.attributionField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
        public partial class Emails1EmailAddressAttributedValueValue
        {
            private string emailAddressField;
            private Emails1EmailAddressAttributedValueValueItemId itemIdField;
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
            public Emails1EmailAddressAttributedValueValueItemId ItemId
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
        public partial class Emails1EmailAddressAttributedValueValueItemId
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
            private GetPersonaResponseMessage getPersonaResponseMessageField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
            public GetPersonaResponseMessage GetPersonaResponseMessage
            {
                get
                {
                    return this.getPersonaResponseMessageField;
                }
                set
                {
                    this.getPersonaResponseMessageField = value;
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
        public partial class FileAses
        {
            private FileAsesStringAttributedValue stringAttributedValueField;

            /// <remarks/>
            public FileAsesStringAttributedValue StringAttributedValue
            {
                get
                {
                    return this.stringAttributedValueField;
                }
                set
                {
                    this.stringAttributedValueField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
        public partial class FileAsesStringAttributedValue
        {
            private FileAsesStringAttributedValueAttributions attributionsField;
            private string valueField;

            /// <remarks/>
            public FileAsesStringAttributedValueAttributions Attributions
            {
                get
                {
                    return this.attributionsField;
                }
                set
                {
                    this.attributionsField = value;
                }
            }

            /// <remarks/>
            public string Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
        public partial class FileAsesStringAttributedValueAttributions
        {
            private byte attributionField;

            /// <remarks/>
            public byte Attribution
            {
                get
                {
                    return this.attributionField;
                }
                set
                {
                    this.attributionField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        public partial class FileAsIds
        {
            private FileAsIdsStringAttributedValue stringAttributedValueField;

            /// <remarks/>
            public FileAsIdsStringAttributedValue StringAttributedValue
            {
                get
                {
                    return this.stringAttributedValueField;
                }
                set
                {
                    this.stringAttributedValueField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
        public partial class FileAsIdsStringAttributedValue
        {
            private byte[] attributionsField;
            private string valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayAttribute()]
            [System.Xml.Serialization.XmlArrayItemAttribute("Attribution", IsNullable = false)]
            public byte[] Attributions
            {
                get
                {
                    return this.attributionsField;
                }
                set
                {
                    this.attributionsField = value;
                }
            }

            /// <remarks/>
            public string Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages", IsNullable = false)]
        public partial class GetPersonaResponseMessage
        {
            private GetPersonaResponseMessagePersona personaField;
            private string responseClassField;
            private string responseCodeField;

            /// <remarks/>
            public GetPersonaResponseMessagePersona Persona
            {
                get
                {
                    return this.personaField;
                }
                set
                {
                    this.personaField = value;
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

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
        public partial class GetPersonaResponseMessagePersona
        {
            private AttributedHasActiveDeals attributedHasActiveDealsField;
            private AttributedIsBusinessContactStringAttributedValue[] attributedIsBusinessContactField;
            private AttributionsAttribution[] attributionsField;
            private string companyNameField;
            private CompanyNames companyNamesField;
            private System.DateTime creationTimeField;
            private object departmentField;
            private string displayNameField;
            private string displayNameFirstLastField;
            private string displayNameLastFirstField;
            private DisplayNames displayNamesField;
            private EmailAddresses emailAddressesField;
            private EmailAddress emailAddressField;
            private Emails1EmailAddressAttributedValue[] emails1Field;
            private FileAses fileAsesField;
            private string fileAsField;
            private string fileAsIdField;
            private FileAsIds fileAsIdsField;
            private string givenNameField;
            private GivenNames givenNamesField;
            private string hasActiveDealsField;
            private object inlineLinksField;
            private string isBusinessContactField;
            private ItemLinkIds itemLinkIdsField;
            private System.DateTime lastContactedDateField;
            private System.DateTime lastModifiedTimeField;
            private PersonaId personaIdField;
            private string personaTypeField;
            private byte relevanceScoreField;
            private object sourceMailboxGuidsField;
            private string surnameField;
            private Surnames surnamesField;
            private object titleField;
            private object workCityField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
            public AttributedHasActiveDeals AttributedHasActiveDeals
            {
                get
                {
                    return this.attributedHasActiveDealsField;
                }
                set
                {
                    this.attributedHasActiveDealsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
            [System.Xml.Serialization.XmlArrayItemAttribute("StringAttributedValue", IsNullable = false)]
            public AttributedIsBusinessContactStringAttributedValue[] AttributedIsBusinessContact
            {
                get
                {
                    return this.attributedIsBusinessContactField;
                }
                set
                {
                    this.attributedIsBusinessContactField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
            [System.Xml.Serialization.XmlArrayItemAttribute("Attribution", IsNullable = false)]
            public AttributionsAttribution[] Attributions
            {
                get
                {
                    return this.attributionsField;
                }
                set
                {
                    this.attributionsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
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
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
            public CompanyNames CompanyNames
            {
                get
                {
                    return this.companyNamesField;
                }
                set
                {
                    this.companyNamesField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
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
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
            public object Department
            {
                get
                {
                    return this.departmentField;
                }
                set
                {
                    this.departmentField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
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
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
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
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
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
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
            public DisplayNames DisplayNames
            {
                get
                {
                    return this.displayNamesField;
                }
                set
                {
                    this.displayNamesField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
            public EmailAddress EmailAddress
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
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
            public EmailAddresses EmailAddresses
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
            [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
            [System.Xml.Serialization.XmlArrayItemAttribute("EmailAddressAttributedValue", IsNullable = false)]
            public Emails1EmailAddressAttributedValue[] Emails1
            {
                get
                {
                    return this.emails1Field;
                }
                set
                {
                    this.emails1Field = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
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
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
            public FileAses FileAses
            {
                get
                {
                    return this.fileAsesField;
                }
                set
                {
                    this.fileAsesField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
            public string FileAsId
            {
                get
                {
                    return this.fileAsIdField;
                }
                set
                {
                    this.fileAsIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
            public FileAsIds FileAsIds
            {
                get
                {
                    return this.fileAsIdsField;
                }
                set
                {
                    this.fileAsIdsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
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
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
            public GivenNames GivenNames
            {
                get
                {
                    return this.givenNamesField;
                }
                set
                {
                    this.givenNamesField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
            public string HasActiveDeals
            {
                get
                {
                    return this.hasActiveDealsField;
                }
                set
                {
                    this.hasActiveDealsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
            public object InlineLinks
            {
                get
                {
                    return this.inlineLinksField;
                }
                set
                {
                    this.inlineLinksField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
            public string IsBusinessContact
            {
                get
                {
                    return this.isBusinessContactField;
                }
                set
                {
                    this.isBusinessContactField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
            public ItemLinkIds ItemLinkIds
            {
                get
                {
                    return this.itemLinkIdsField;
                }
                set
                {
                    this.itemLinkIdsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
            public System.DateTime LastContactedDate
            {
                get
                {
                    return this.lastContactedDateField;
                }
                set
                {
                    this.lastContactedDateField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
            public System.DateTime LastModifiedTime
            {
                get
                {
                    return this.lastModifiedTimeField;
                }
                set
                {
                    this.lastModifiedTimeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
            public PersonaId PersonaId
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
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
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
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
            public byte RelevanceScore
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
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
            public object SourceMailboxGuids
            {
                get
                {
                    return this.sourceMailboxGuidsField;
                }
                set
                {
                    this.sourceMailboxGuidsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
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
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
            public Surnames Surnames
            {
                get
                {
                    return this.surnamesField;
                }
                set
                {
                    this.surnamesField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
            public object Title
            {
                get
                {
                    return this.titleField;
                }
                set
                {
                    this.titleField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
            public object WorkCity
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
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        public partial class GivenNames
        {
            private GivenNamesStringAttributedValue stringAttributedValueField;

            /// <remarks/>
            public GivenNamesStringAttributedValue StringAttributedValue
            {
                get
                {
                    return this.stringAttributedValueField;
                }
                set
                {
                    this.stringAttributedValueField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
        public partial class GivenNamesStringAttributedValue
        {
            private byte[] attributionsField;
            private string valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayAttribute()]
            [System.Xml.Serialization.XmlArrayItemAttribute("Attribution", IsNullable = false)]
            public byte[] Attributions
            {
                get
                {
                    return this.attributionsField;
                }
                set
                {
                    this.attributionsField = value;
                }
            }

            /// <remarks/>
            public string Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        public partial class ItemLinkIds
        {
            private ItemLinkIdsStringArrayAttributedValue stringArrayAttributedValueField;

            /// <remarks/>
            public ItemLinkIdsStringArrayAttributedValue StringArrayAttributedValue
            {
                get
                {
                    return this.stringArrayAttributedValueField;
                }
                set
                {
                    this.stringArrayAttributedValueField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
        public partial class ItemLinkIdsStringArrayAttributedValue
        {
            private ItemLinkIdsStringArrayAttributedValueAttributions attributionsField;
            private ItemLinkIdsStringArrayAttributedValueValues valuesField;

            /// <remarks/>
            public ItemLinkIdsStringArrayAttributedValueAttributions Attributions
            {
                get
                {
                    return this.attributionsField;
                }
                set
                {
                    this.attributionsField = value;
                }
            }

            /// <remarks/>
            public ItemLinkIdsStringArrayAttributedValueValues Values
            {
                get
                {
                    return this.valuesField;
                }
                set
                {
                    this.valuesField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
        public partial class ItemLinkIdsStringArrayAttributedValueAttributions
        {
            private byte attributionField;

            /// <remarks/>
            public byte Attribution
            {
                get
                {
                    return this.attributionField;
                }
                set
                {
                    this.attributionField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
        public partial class ItemLinkIdsStringArrayAttributedValueValues
        {
            private string valueField;

            /// <remarks/>
            public string Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        public partial class PersonaId
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

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        public partial class Surnames
        {
            private SurnamesStringAttributedValue stringAttributedValueField;

            /// <remarks/>
            public SurnamesStringAttributedValue StringAttributedValue
            {
                get
                {
                    return this.stringAttributedValueField;
                }
                set
                {
                    this.stringAttributedValueField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
        public partial class SurnamesStringAttributedValue
        {
            private byte[] attributionsField;
            private string valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayAttribute()]
            [System.Xml.Serialization.XmlArrayItemAttribute("Attribution", IsNullable = false)]
            public byte[] Attributions
            {
                get
                {
                    return this.attributionsField;
                }
                set
                {
                    this.attributionsField = value;
                }
            }

            /// <remarks/>
            public string Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }
    }
}
