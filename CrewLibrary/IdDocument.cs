using Microsoft.Data.Sqlite;

namespace Crewing
{
    class IdDocument
    {
        public int? Id { get; set; }
        public string? Number { get; set;}
        public IdDocumentType? DocumentType { get; set; }
        public Nationality IssuingCountry { get; set; }
        public DateOnly IssueDate { get; set; }
        public DateOnly ExpiryDate { get; set; }
        public Person Owner { get; set; }
        public bool InUse { get; set; }
        public IdDocument()
        {
            Number = "";
            DocumentType = new IdDocumentType();
            IssuingCountry = new Nationality();
            Owner = new Person();
        }
        public static IdDocument CreateIdDocument(Person person, IdDocumentType idDocumentType)
        {
            IdDocument idDocument = new IdDocument();
            idDocument.DocumentType = idDocumentType;
            
            using (SqliteConnection con = new SqliteConnection("data source=" + Statics.GetConfigValue("FILES", "Db")))
            using (SqliteCommand command = con.CreateCommand())
            {
                con.Open();

                command.CommandText = $"INSERT INTO IdDocuments (IdDocumentType_Id, Person_Id) VALUES ({idDocumentType.Id}, {person.Id})";                
                command.ExecuteNonQuery();

                command.CommandText = $"SELECT Id FROM IdDocuments WHERE IdDocumentType_Id='{idDocumentType.Id}' AND Person_Id='{person.Id}' ORDER BY Id DESC LIMIT 1";
                idDocument.Id = Convert.ToInt32(command.ExecuteScalar());
            }

            person.IdDocuments.Add(idDocument);
            Lists.GetLists.IdDocuments.Add(idDocument);

            return idDocument;
        }
        public static IdDocument CreateIdDocument(Person person, string number, IdDocumentType idDocumentType, Nationality issuingCountry, DateOnly issueDate, DateOnly expiryDate)
        {
            IdDocument idDocument = CreateIdDocument(person, idDocumentType);
            EditIdDocumentNumber(idDocument, number);
            EditIdDocumentType(idDocument, idDocumentType);
            EditIssuingCountry(idDocument, issuingCountry);
            EditIssueDate(idDocument, issueDate);
            EditExpiryDate(idDocument, expiryDate);

            return idDocument;
        }
        public static void EditExpiryDate(IdDocument idDocument, DateOnly expiryDate)
        {
            using (SqliteConnection con = new SqliteConnection("data source=" + Statics.GetConfigValue("FILES", "Db")))
            using (SqliteCommand command = con.CreateCommand())
            {
                con.Open();

                command.CommandText = $"UPDATE IdDocuments SET ExpiryDate='{expiryDate.Year,0:D4}-{expiryDate.Month,0:D2}-{expiryDate.Day,0:D2}' WHERE Id={idDocument.Id}";                
                command.ExecuteNonQuery();
            }

            idDocument.ExpiryDate = expiryDate;
        }
        public static void EditIssueDate(IdDocument idDocument, DateOnly issueDate)
        {
            using (SqliteConnection con = new SqliteConnection("data source=" + Statics.GetConfigValue("FILES", "Db")))
            using (SqliteCommand command = con.CreateCommand())
            {
                con.Open();

                command.CommandText = $"UPDATE IdDocuments SET IssueDate='{issueDate.Year,0:D4}-{issueDate.Month,0:D2}-{issueDate.Day,0:D2}' WHERE Id={idDocument.Id}";                
                command.ExecuteNonQuery();
            }

            idDocument.IssueDate = issueDate;
        }
        public static void EditIdDocumentType(IdDocument idDocument, int idDocumentTypeId)
        {
            using (SqliteConnection con = new SqliteConnection("data source=" + Statics.GetConfigValue("FILES", "Db")))
            using (SqliteCommand command = con.CreateCommand())
            {
                con.Open();

                command.CommandText = $"UPDATE IdDocuments SET IdDocumentType_Id={idDocumentTypeId} WHERE Id={idDocument.Id}";                
                command.ExecuteNonQuery();
            }

            foreach (IdDocumentType idDocumentType in Lists.GetLists.IdDocumentTypes)
                if (idDocumentType.Id == idDocumentTypeId)
                    idDocument.DocumentType = idDocumentType;
        }
        public static void EditIdDocumentType(IdDocument idDocument, IdDocumentType idDocumentType)
        {
            EditIdDocumentType(idDocument, idDocumentType.Id);
        }
        public static void EditIdDocumentNumber(IdDocument idDocument, string number)
        {
            using (SqliteConnection con = new SqliteConnection("data source=" + Statics.GetConfigValue("FILES", "Db")))
            using (SqliteCommand command = con.CreateCommand())
            {
                con.Open();

                command.CommandText = $"UPDATE IdDocuments SET Number='{number}' WHERE Id={idDocument.Id}";                
                command.ExecuteNonQuery();
            }

            idDocument.Number = number;
        }
        public static void EditIssuingCountry(IdDocument idDocument, int issuingCountryId)
        {
            using (SqliteConnection con = new SqliteConnection("data source=" + Statics.GetConfigValue("FILES", "Db")))
            using (SqliteCommand command = con.CreateCommand())
            {
                con.Open();

                command.CommandText = $"UPDATE IdDocuments SET IssuingCountry_Id={issuingCountryId} WHERE Id={idDocument.Id}";                
                command.ExecuteNonQuery();
            }

            foreach (Nationality nationality in Lists.GetLists.Nationalities)
                if (nationality.Id == issuingCountryId)
                    idDocument.IssuingCountry = nationality;
        }
        public static void EditIssuingCountry(IdDocument idDocument, Nationality issuingCountry)
        {
            EditIssuingCountry(idDocument, issuingCountry.Id);
        }
        public static void RemoveIdDocument(IdDocument idDocument)
        {
            using (SqliteConnection con = new SqliteConnection("data source=" + Statics.GetConfigValue("FILES", "Db")))
            {                
                using (SqliteCommand del_command = con.CreateCommand())
                {
                    con.Open();

                    del_command.CommandText = $"DELETE FROM IdDocuments WHERE Id={idDocument.Id}";
                    del_command.ExecuteNonQuery();

                    Lists.GetLists.IdDocuments.Remove(idDocument);
                    idDocument = null;
                }
            }
        }
    }
}