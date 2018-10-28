using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FreedomToIHCM
{
    static class Extensions
    {
        /// <summary>
        /// Convert ArrayList to List.
        /// </summary>
        public static List<T> ToList<T>(this ArrayList arrayList)
        {
            List<T> list = new List<T>(arrayList.Count);
            foreach (T instance in arrayList)
            {
                list.Add(instance);
            }
            return list;
        }
    }

    class Table
    {
        public static List<Table> Tables;

        public string Name { get; set; }

        public int Level { get; set; }

        public List<Table> Parents { get; set; }

        public bool IsCompleted { get; set; }

        private Table(string name, int level, List<Table> parents)
        {
            Name = name;
            Level = level;
            Parents = parents;
            IsCompleted = false;
        }

        public static void FromXML(string path)
        {
            Dictionary<string, Table > map = new Dictionary<string, Table>();
            ArrayList xmlTables = new ArrayList();

            XDocument xml = XDocument.Load(path);

            var tables = xml.Element("Tables").Elements("Table");

            foreach(var table in tables)
            {
                var name = table.Attribute("name").Value;
                var level = int.Parse(table.Attribute("order").Value);
                var parents = table.Elements("Parent");
                if(parents.Count() == 0) // Table with No Parents
                {
                    var t = new Table(name, level, null);
                    xmlTables.Add(t);
                    map.Add(name, t);
                } else // Search for parents in the map
                {
                    var parentsTables = new List<Table>();
                    foreach(var parentsTable in parents)
                    {
                        parentsTables.Add(map[parentsTable.Attribute("name").Value]);
                    }
                    var t = new Table(name, level, parentsTables);
                    xmlTables.Add(t);
                    map.Add(name, t);
                }
            }
            
            Tables =  xmlTables.ToList<Table>();
        }

        public bool isParentsCompleted()
        {
            foreach(var parent in Parents)
            {
                if(!parent.IsCompleted)
                {
                    return false;
                } 
            }
            return true;
        }
    }
}
