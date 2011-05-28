using System.Collections.Generic;

namespace TeamManager
{
    public class ModuleMapper
    {
        public static Dictionary<string, List<string>> ModuleMap { get; set; }

        static ModuleMapper()
        {
            ModuleMap = new Dictionary<string, List<string>>
                            {
                                {"/Projects", new List<string> {"ProjectModule"}}
                            };
        }
    }
}