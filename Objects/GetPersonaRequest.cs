namespace BCM_Migration_Tool.Objects
{
    class GetPersonaRequest
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
            private GetPersona getPersonaField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
            public GetPersona GetPersona
            {
                get
                {
                    return this.getPersonaField;
                }
                set
                {
                    this.getPersonaField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public partial class EnvelopeHeader
        {
            private RequestServerVersion requestServerVersionField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
            public RequestServerVersion RequestServerVersion
            {
                get
                {
                    return this.requestServerVersionField;
                }
                set
                {
                    this.requestServerVersionField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages", IsNullable = false)]
        public partial class GetPersona
        {
            private GetPersonaExtendedFieldURI[] additionalPropertiesField;
            private GetPersonaPersonaId personaIdField;

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("ExtendedFieldURI", IsNullable = false)]
            public GetPersonaExtendedFieldURI[] AdditionalProperties
            {
                get
                {
                    return this.additionalPropertiesField;
                }
                set
                {
                    this.additionalPropertiesField = value;
                }
            }

            /// <remarks/>
            public GetPersonaPersonaId PersonaId
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
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
        public partial class GetPersonaExtendedFieldURI
        {
            private string propertyNameField;
            private string propertySetIdField;
            private string propertyTypeField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string PropertyName
            {
                get
                {
                    return this.propertyNameField;
                }
                set
                {
                    this.propertyNameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string PropertySetId
            {
                get
                {
                    return this.propertySetIdField;
                }
                set
                {
                    this.propertySetIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string PropertyType
            {
                get
                {
                    return this.propertyTypeField;
                }
                set
                {
                    this.propertyTypeField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
        public partial class GetPersonaPersonaId
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
        public partial class RequestServerVersion
        {
            private string versionField;

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
