using System.Reflection;
using System.Data.SqlClient;
using System.Diagnostics;

namespace PROG260_Week6
{

    /// <summary>
    /// 
    /// I used the inclass examples availiable from the powerpoint as base to start accessing the database
    /// I improved upon them by dynamically reading the header of the txt file to generate the SQL column heads
    /// 
    /// I used ChatGPT to generate the comments to this code
    /// 
    /// I also used ChatGPT to correct my filter statements as they were wrong originally and threw errors
    /// 
    /// the originals:
    /// DELETE FROM Produce WHERE Sell_by_Date < CURRENT_DATE;
    /// UPDATE Produce WHERE Location = REPLACE(Location, 'F', 'Z') WHERE Location LIKE '%F%
    /// 
    /// the ai enchanced:
    /// WHERE Sell_by_Date < GETDATE();
    /// SET Location = replaced_location FROM ( SELECT Location, REPLACE(Location, 'F', 'Z') AS replaced_location FROM Produce WHERE Location LIKE '%F%' ) AS subquery WHERE Produce.Location LIKE '%F%'
    /// 
    /// </summary>
    internal class Program
    {
        // Class-level variable to store the database connection string
        public static string ConnectionString { get; set; }

        static void Main(string[] args)
        {
            // Output a simple greeting message
            Console.WriteLine("Hello, World!");

            // Initialize and configure a SQL Server connection string
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();
            sqlBuilder["Server"] = @"(localdb)\MSSQLLocalDB";
            sqlBuilder["Trusted_Connection"] = true;
            sqlBuilder["Integrated Security"] = "SSPI";
            sqlBuilder["Initial Catalog"] = "PROG260FA23";
            ConnectionString = sqlBuilder.ToString();

            // Create an InputFile object to work with file input
            FileInfo f = new FileInfo("Produce.txt");
            InputFile InF = new InputFile(f.FullName, f.Name, f.Extension);

            // Write data from the input file to the database
            WriteToDataBase(InF);

            // Apply a SQL filter and update operation on the database
            FilterAndUpdate(InF, "SET Location = replaced_location FROM ( SELECT Location, REPLACE(Location, 'F', 'Z') AS replaced_location FROM Produce WHERE Location LIKE '%F%' ) AS subquery WHERE Produce.Location LIKE '%F%'");

            // Apply a SQL filter and delete operation on the database
            FilterAndDelete(InF, "WHERE Sell_by_Date < GETDATE();");

            // Apply a SQL update operation on the database
            FilterAndUpdate(InF, "SET Price = Price + 1;");

            // Extract data from the database and write it to a file
            DatabaseToFile(InF);

            // Create an InputFile object to work with the output file
            FileInfo fo = new FileInfo($"{InF.Name}_out.txt");
            InputFile InO = new InputFile(fo.FullName, fo.Name, fo.Extension);

            // Display the contents of the output file
            foreach (var line in File.ReadLines(InO.Path))
            {
                Console.WriteLine(line);
            }
        }

        // Method to write data from a file to the database
        public static void WriteToDataBase(IFile file)
        {
            using (StreamReader reader = new StreamReader(file.Path))
            {
                // Read the first line to get column names
                var table_cols = reader.ReadLine().Split(",");

                string inlineSQL_cols = "";
                foreach (var col in table_cols)
                {
                    if (col == table_cols.Last())
                    {
                        inlineSQL_cols += $"[{col}]";
                    }
                    else
                    {
                        inlineSQL_cols += $"[{col}], ";
                    }
                }

                // Read and process data lines
                var line = reader.ReadLine();
                while (line != null)
                {
                    var fields = line.Split("|");

                    string inlineSQL_values = "";
                    foreach (var field in fields)
                    {
                        if (field == fields.Last())
                        {
                            inlineSQL_values += $"'{field}'";
                        }
                        else
                        {
                            inlineSQL_values += $"'{field}', ";
                        }
                    }

                    // Generate the SQL INSERT statement and execute it
                    string inlineSQL = $@"INSERT [dbo].[{file.Name}] ({inlineSQL_cols}) VALUES ({inlineSQL_values})";

                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand(inlineSQL, conn))
                        {
                            var query = cmd.ExecuteNonQuery();
                        }
                        conn.Close();
                    }

                    line = reader.ReadLine();
                }
                reader.Close();
                reader.Dispose();
            }
        }

        // Method to filter and update data in the database
        public static void FilterAndUpdate(IFile file, string filterSQl)
        {
            string inlineSQL = $@"UPDATE {file.Name} {filterSQl}";
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(inlineSQL, conn))
                {
                    var query = cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
        }

        // Method to filter and delete data from the database
        public static void FilterAndDelete(IFile file, string filterSQl)
        {
            string inlineSQL = $@"DELETE FROM {file.Name} {filterSQl}";
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(inlineSQL, conn))
                {
                    var query = cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
        }

        // Method to extract data from the database and write it to a file
        public static void DatabaseToFile(IFile file)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                FileStream fs = new FileStream($"{file.Name}_out.txt", FileMode.OpenOrCreate, FileAccess.Write);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    // Construct the SQL SELECT statement
                    string inlineSql = $"Select * from {file.Name}";

                    using (SqlCommand comm = new SqlCommand(inlineSql, conn))
                    {
                        var reader = comm.ExecuteReader();

                        while (reader.Read())
                        {
                            // Write data to the output file
                            writer.WriteLine($"Name: {reader.GetValue(0)} Location: {reader.GetValue(1)} Price: {reader.GetValue(2)} Uom: {reader.GetValue(3)} Sell_by_Date: {reader.GetValue(4)}");
                        }
                    }
                    writer.Close();
                }
                conn.Close();
            }
        }
    }
}