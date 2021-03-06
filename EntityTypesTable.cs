//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BcmMigrationTool
{
    using System;
    using System.Collections.Generic;
    
    public partial class EntityTypesTable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EntityTypesTable()
        {
            this.EntityReferences = new HashSet<EntityReference>();
            this.EntitySubTypesTables = new HashSet<EntitySubTypesTable>();
            this.EntityUserFields = new HashSet<EntityUserField>();
            this.FormLayouts = new HashSet<FormLayout>();
            this.UserPropertiesTables = new HashSet<UserPropertiesTable>();
        }
    
        public int EntityTypeID { get; set; }
        public string EntityTypeName { get; set; }
        public string EntityTypeDescrption { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EntityReference> EntityReferences { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EntitySubTypesTable> EntitySubTypesTables { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EntityUserField> EntityUserFields { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FormLayout> FormLayouts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserPropertiesTable> UserPropertiesTables { get; set; }
    }
}
