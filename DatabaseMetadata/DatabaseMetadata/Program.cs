namespace DatabaseMetadata
{
    class Program
    {
        
        /// <summary>
        /// Given a Database Name, Generate the XML files with Order Level of Tables and Table with its Immediate parents
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            LevelOrder.generateXML();
            TableParents.generateXML();
            // Console.ReadLine();
        }
    }
}
