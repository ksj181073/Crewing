using Microsoft.Data.Sqlite;

namespace Crewing
{
    class CertificateType
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public CertificateType() { }
        public CertificateType(int id, string name)
        {
            Id = id;
            Name = name;
        }
        public static void New(string name)
        {
            if (name == "")
                throw new Exception("Cannot create a certificate type with an empty string.");
            
            else
            {
                foreach (CertificateType certificateType in Lists.GetLists.CertificateTypes)
                {
                    if (certificateType.Name == name)
                        throw new Exception($"Certificate Type '{name}' already exists in the local list.");
                }

                using (SqliteConnection con = new SqliteConnection("data source=" + Statics.GetConfigValue("FILES", "Db")))
                using (SqliteCommand command = con.CreateCommand())
                {
                    con.Open();

                    command.CommandText = $"SELECT * FROM CertificateTypes WHERE Name='{name}'";

                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            throw new Exception($"Certificate Type '{name}' already exists in the backend database.");
                        
                        else
                        {
                            reader.Close();
                            command.CommandText = $"INSERT INTO CertificateTypes (Name) VALUES ('{name}')";
                            command.ExecuteNonQuery();

                            command.CommandText = $"SELECT Id FROM CertificateTypes WHERE Name='{name}'";

                            int newId = Convert.ToInt32(command.ExecuteScalar());
                            
                            Lists.GetLists.CertificateTypes.Add(new CertificateType(newId, name));
                        }
                    }
                }
            }
        }
        public void Rename(string name)
        {
            if (name == "")
                throw new Exception("Cannot rename a certificate type to an empty string.");
            
            else
            {
                if (!Lists.GetLists.CertificateTypes.Contains(this))
                    throw new Exception($"Certificate Type '{name}' already exists in the local list.");
                
                using (SqliteConnection con = new SqliteConnection("data source=" + Statics.GetConfigValue("FILES", "Db")))
                using (SqliteCommand command = con.CreateCommand())
                {
                    con.Open();

                    command.CommandText = $"SELECT * FROM CertificateTypes WHERE Name='{name}'";

                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            throw new Exception($"Certificate Type '{name}' already exists in the backend database.");
                        
                        else
                        {
                            reader.Close();
                            command.CommandText = $"UPDATE CertificateTypes SET Name='{name}' WHERE Id={this.Id}";
                            command.ExecuteNonQuery();

                            this.Name = name;
                        }
                    }
                }
            }
        }
        public void Delete()
        {
            if (!Lists.GetLists.CertificateTypes.Contains(this))
                throw new Exception($"Certificate Type '{this.Name}' does not exist in the local list.");

            using (SqliteConnection con = new SqliteConnection("data source=" + Statics.GetConfigValue("FILES", "Db")))
            using (SqliteCommand command = con.CreateCommand())
            {
                con.Open();

                command.CommandText = $"SELECT * FROM CertificateTypes WHERE Name='{this.Name}'";

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                        throw new Exception($"Certificate Type '{this.Name}' does not exist in the backend database.");
                    
                    else
                    {
                        reader.Close();
                        command.CommandText = $"DELETE FROM CertificateTypes WHERE Id={this.Id}";
                        command.ExecuteNonQuery();

                        Lists.GetLists.CertificateTypes.Remove(this);
                    }
                }
            }
        
        }
        public static CertificateType GetCertificateType(int CertificateType_Id)
        {
            foreach (CertificateType certificateType in Lists.GetLists.CertificateTypes)
                if (certificateType.Id == CertificateType_Id)
                    return certificateType;
            
            return null;
        }
        public static CertificateType GetCertificateType(string CertificateType_Name)
        {
            foreach (CertificateType certificateType in Lists.GetLists.CertificateTypes)
                if (certificateType.Name == CertificateType_Name)
                    return certificateType;
            
            return null;
        }
    }
}