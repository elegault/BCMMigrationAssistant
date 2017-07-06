using System;
using System.Diagnostics;
using System.Linq;
using TracerX;

namespace BCM_Migration_Tool.Objects
{
    public partial class OCMDealTemplate
    {
        private static readonly Logger Log = Logger.GetLogger("OCMDealTemplate");
        public class Rootobject
        {
            public string GetStageIDByName(string stageName)
            {
                string result = "";

                try
                {
                    foreach (var stageGroup in Template.StatusList)
                    {
                        var stage = stageGroup.Stages.FirstOrDefault(stages => stages.Label == stageName && stages.Deleted == false);
                        if (stage != null)
                        {
                            result = stage.Id;
                            break;
                        }
                    }

                }
                catch (System.Exception ex)
                {
                    Log.Error(ex);
                }

                return result;
            }
            public string GetStageStatusByStageID(string stageID)
            {
                string result = "";

                try
                {
                    foreach (var stageGroup in Template.StatusList)
                    {
                        var stage = stageGroup.Stages.FirstOrDefault(stages => stages.Id == stageID);
                        if (stage != null)
                        {
                            result = stageGroup.Label;
                            break;
                        }
                    }

                }
                catch (System.Exception ex)
                {
                    Log.Error(ex);
                }

                return result;
            }
            public string Id { get; set; }
            public Template Template { get; set; }
            public int Version { get; set; }             
        }

        public class Template
        {
            public string _Name { get; set; }
            public int _Version { get; set; }
            public int FieldLimit { get; set; }
            public int StageLimit { get; set; }
            public string CurrencyCode { get; set; }
            public Customtypelist[] CustomTypeList { get; set; }
            public Fieldlist[] FieldList { get; set; }
            public Statuslist[] StatusList { get; set; }
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

        public class Statuslist
        {
            public string Id { get; set; }
            public string Label { get; set; }
            public Stage[] Stages { get; set; }
        }

        public class Stage
        {
            public string Id { get; set; }
            public string Label { get; set; }
            public bool Deleted { get; set; }
        }
    }

}
