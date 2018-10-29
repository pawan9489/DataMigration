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

                CREATE TABLE #NAMEPARENT ([NAME] NVARCHAR(256), [PARENT] NVARCHAR(256));
                INSERT INTO #NAMEPARENT
                SELECT T.TABLE_SCHEMA + '.' + ALLTABLES.NAME 'NAME', 
                (SELECT T.TABLE_SCHEMA + '.' + NAME FROM SYS.TABLES S
                INNER JOIN INFORMATION_SCHEMA.TABLES T ON T.TABLE_NAME = S.NAME
                WHERE OBJECT_ID = PARENTS.REFERENCED_OBJECT_ID 
                ) PARENT FROM SYS.TABLES ALLTABLES
                LEFT JOIN SYS.FOREIGN_KEYS PARENTS
                ON ALLTABLES.OBJECT_ID = PARENTS.PARENT_OBJECT_ID 
                INNER JOIN INFORMATION_SCHEMA.TABLES T ON T.TABLE_NAME = ALLTABLES.NAME


                CREATE TABLE #NAMEPARENTS ([NAME] NVARCHAR(256), [PARENTS] NVARCHAR(4000));
                INSERT INTO #NAMEPARENTS
                SELECT DISTINCT C1.NAME, PARENTS = STUFF(( SELECT	',' + PARENT
				                FROM	#NAMEPARENT AS C2
				                WHERE C2.NAME = C1.NAME
				                FOR XML	PATH('')
				                ), 1, 1, '')
                FROM #NAMEPARENT C1

                SELECT TP.NAME,LO.LEVEL, TP.PARENTS 
                FROM #SEQUENCE LO INNER JOIN #NAMEPARENTS TP ON TP.NAME = LO.OWNER + '.' + LO.NAME
                ORDER BY LO.LEVEL";

        public static void populate()
        {
            var temp = Query.eachLine(sql);
            foreach (var dataRow in temp)
            {
                // Ex: dataRow -> ProductModelProductDescriptionCulture | 	2	| ProductDescription,Culture,ProductModel
                var row = new Table(dataRow[0].ToString(), (int)dataRow[1], new List<Table>());
                var parentNames = dataRow[2].ToString().Split(',').ToList(); // List (ProductDescription, Culture, ProductModel)

                /*
                    As the SQL takes the data in the order of Level Ascending, No need to verify the Parents(Table Name) exists in the Map
                    and also the Order of Parents is Ascending Order.
                */
                if (parentNames[0] != "") // If the Table has Parents
                {
                    if (parentNames.Count == 1) // Only One parent
                    {
                        row.Parents.Add(map[parentNames[0]]); // Map will have the parent since the SQL qurey results are in the order of Levels
                    }
                    else // Multiple Parents
                    {
                        for (int i = 0; i < parentNames.Count; i++)
                        {
                            // Some times the table might refer to itself
                            // dbo.OC_TABLEDEFS  |  2  |  dbo.OC_ADMINCONSOLE_FOLDERS,  dbo.OC_TABLEDEFS
                            if (row.Name == parentNames[i])
                            {
                                continue;
                            }
                            for (int j = i + 1; j < parentNames.Count; j++)
                            {
                                ///
                                /// In List of Parents (Sorted by their Levels) if the parents in the starting are dependent on the later then remove the later
                                ///
                                // Some times the table might refer to itself
                                // dbo.OC_TABLEDEFS  |  2  |  dbo.OC_ADMINCONSOLE_FOLDERS,  dbo.OC_TABLEDEFS
                                if(row.Name == parentNames[j])
                                {
                                    continue;
                                }
                                continue;
                                if (map[parentNames[i]].Dependent(map[parentNames[j]]))
                                {
                                    ///
                                    /// Checking for Duplications of parents since the SQL query has duplicate parents because of Multiple same parent keys in the table (like Sales Table with "Shipping Address" and "Departure Address")
                                    ///
                                    if (!(row.Parents.Contains(map[parentNames[i]])))
                                    {
                                        row.Parents.Add(map[parentNames[i]]);
                                    }
                                    parentNames.RemoveAt(j);
                                }
                                else
                                {
                                    ///
                                    /// Checking for Duplications of parents since the SQL query has duplicate parents because of Multiple same parent keys in the table (like Sales Table with "Shipping Address" and "Departure Address")
                                    ///
                                    if (!(row.Parents.Contains(map[parentNames[i]])))
                                    {
                                        row.Parents.Add(map[parentNames[i]]);
                                    }
                                    if (!(row.Parents.Contains(map[parentNames[j]])))
                                    {
                                        row.Parents.Add(map[parentNames[j]]);
                                    }
                                }
                            }
                        }
                    }
                }
                if (!(map.ContainsKey(row.Name)))
                {
                    map.Add(row.Name, row);
                }
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
