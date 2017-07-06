using System;

namespace BCM_Migration_Tool.Objects
{
    public class Relationship
    {
        public bool Bidirectional { get; set; }

        public DateTime CreationTime { get; set; }

        public bool Derived { get; set; }

        public string DisplayName { get; set; }

        public string ItemLinkId { get; set; }

        public string ItemType { get; set; }

        public string LinkId { get; set; }

        public string LinkType { get; set; }

        public string SourceMailboxId { get; set; }
    }
}
