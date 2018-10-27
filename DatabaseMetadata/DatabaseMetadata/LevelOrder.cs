using System.Collections.Generic;
using System.Xml.Linq;

namespace DatabaseMetadata
{
    public class LevelOrder
    {
        public static Dictionary<string, int> map = new Dictionary<string, int>();

        public static string sql = @"
                CREATE TABLE #LevelOrdering ([type] INT, [name] NVARCHAR(256), [owner] NVARCHAR(256), [level] INT);
                INSERT INTO #LevelOrdering
                EXEC sp_msdependencies @intrans = 1 ,@objtype=3
                SELECT name, level
                FROM #LevelOrdering;";

        public static Dictionary<string, int> populate()
        {
            foreach(var dataRow in Query.eachLine(sql))
            {
                map.Add(dataRow[0].ToString(), (int)dataRow[1]);
            }
            return map;
        }

        public static void generateXML()
        {
            populate();
            var levelOrderXML = new XDocument(
                new XElement("Tables")
                );
            foreach (var x in map)
            {
                var element = new XElement("Table");
                element.SetAttributeValue("order", x.Value);
                element.SetAttributeValue("name", x.Key);
                levelOrderXML.Root.Add(element);
            }
            levelOrderXML.Save("levels.xml"); // DatabaseMetadata\DatabaseMetadata\bin\Debug
        }
    }
}
