using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DatabaseMetadata
{
    class TableParents
    {
        public static Dictionary<string, Table> map = new Dictionary<string, Table>();

        public static string sql = @"
                CREATE TABLE #SEQUENCE ([TYPE] INT, [NAME] NVARCHAR(256), [OWNER] NVARCHAR(256), [LEVEL] INT);
                INSERT INTO #SEQUENCE
                EXEC SP_MSDEPENDENCIES @INTRANS = 1 ,@OBJTYPE=3


                ;WITH CTE AS (
                SELECT ALLTABLES.NAME, 
                (SELECT NAME FROM SYS.TABLES WHERE OBJECT_ID = PARENTS.REFERENCED_OBJECT_ID ) PARENT FROM SYS.TABLES ALLTABLES
                LEFT JOIN SYS.FOREIGN_KEYS PARENTS
                ON ALLTABLES.OBJECT_ID = PARENTS.PARENT_OBJECT_ID 
                ) 
                ,TABLEPARENTS AS (
                SELECT DISTINCT C1.NAME, PARENTS = STUFF(( SELECT	',' + PARENT
				                FROM	CTE AS C2
				                WHERE C2.NAME = C1.NAME
				                FOR XML	PATH('')
				                ), 1, 1, '')
                FROM CTE C1
                )
                SELECT TP.NAME,LO.LEVEL, TP.PARENTS 
                FROM #SEQUENCE LO INNER JOIN TABLEPARENTS TP ON TP.NAME = LO.NAME";

        public static void populate()
        {
            var temp = Query.eachLine(sql);
            foreach (var dataRow in temp)
            {
                var row = new Table(dataRow[0].ToString(), (int)dataRow[1], new List<Table>());
                var parentNames = dataRow[2].ToString().Split(',').ToList();

                /*
                    As the SQL takes the data in the order of Level Ascending, No need to verify the Parents(Table Name) exists in the Map
                    and also the Order of Parents is Ascending Order.
                */
                if (parentNames[0] != "") // If the Table has no Parents
                {
                    if(parentNames.Count == 1) // Only One parent
                    {
                        row.Parents.Add(map[parentNames[0]]);
                    } else
                    {
                        for (int i = 0; i < parentNames.Count; i++)
                        {
                            for (int j = i + 1; j < parentNames.Count; j++)
                            {
                                if (map[parentNames[i]].Dependent(map[parentNames[j]]))
                                {
                                    if (!row.Parents.Contains(map[parentNames[i]]))
                                    {
                                        row.Parents.Add(map[parentNames[i]]);
                                    }
                                    parentNames.RemoveAt(j);
                                }
                                else
                                {
                                    row.Parents.Add(map[parentNames[i]]);
                                }
                            }
                        }
                    }
                }
                map.Add(row.Name, row);
            }
        }

        public static void generateXML()
        {
            populate();
            var levelOrderXML = new XDocument(
                new XElement("Tables")
                );
            foreach (var x in map) // Dictionary<string, Table>
            {
                var element = new XElement("Table");
                element.SetAttributeValue("name", x.Key);
                element.SetAttributeValue("order", x.Value.Level);
                foreach(var parent in x.Value.Parents)
                {
                    var parentElement = new XElement("Parent");
                    parentElement.SetAttributeValue("name", parent.Name);
                    element.Add(parentElement);
                }
                levelOrderXML.Root.Add(element);
            }
            levelOrderXML.Save("tableParents.xml"); // DatabaseMetadata\DatabaseMetadata\bin\Debug
        }
    }

    class Table
    {
        public string Name { get; set; }

        public int Level { get; set; }

        public List<Table> Parents { get; set; }

        public Table(string name, int level, List<Table> parents)
        {
            Name = name;
            Level = level;
            Parents = parents;
        }

        public bool Dependent(Table t)
        {
            if(Parents.Contains(t))
            {
                return true;
            } else
            {
                foreach(var parent in Parents)
                {
                    parent.Dependent(t);
                }
                return false;
            }
        }
    }
}
