using Microsoft.Data.Sqlite;

namespace Crewing
{
    class Certificate
    {
        public int? Id { get; set; }
        public string? Number { get; set;}
        public CertificateType? CertificateType { get; set; }
        public string IssuingAuthority { get; set; }
        public DateOnly IssueDate { get; set; }
        public DateOnly ExpiryDate { get; set; }
        public Certificate()
        {
            Number = "";
            CertificateType = new CertificateType();
            IssuingAuthority = "";
        }
        public Certificate(string number) : this()
        {
            Number = number;
        }
        public static Certificate CreateCertificate(Person person, CertificateType certificateType)
        {
            Certificate certificate = new Certificate();
            certificate.CertificateType = certificateType;
            
            using (SqliteConnection con = new SqliteConnection("data source=" + Statics.GetConfigValue("FILES", "Db")))
            using (SqliteCommand command = con.CreateCommand())
            {
                con.Open();

                command.CommandText = $"INSERT INTO Certificates (CertificateType_Id, Person_Id) VALUES ({certificateType.Id}, {person.Id})";                
                command.ExecuteNonQuery();

                command.CommandText = $"SELECT Id FROM Certificates WHERE CertificateType_Id='{certificateType.Id}' AND Person_Id='{person.Id}' ORDER BY Id DESC LIMIT 1";
                certificate.Id = Convert.ToInt32(command.ExecuteScalar());
            }

            person.Certificates.Add(certificate);
            Lists.GetLists.Certificates.Add(certificate);

            return certificate;
        }
        public static Certificate CreateCertificate(Person person, string number, CertificateType certificateType, string issuingAuthority, DateOnly issueDate, DateOnly expiryDate)
        {
            Certificate certificate = CreateCertificate(person, certificateType);
            Certificate.EditCertificateNumber(certificate, number);
            Certificate.EditIssuingAuthority(certificate, issuingAuthority);
            Certificate.EditIssueDate(certificate, issueDate);
            Certificate.EditExpiryDate(certificate, expiryDate);

            return certificate;
        }
        public static void EditExpiryDate(Certificate certificate, DateOnly expiryDate)
        {
            using (SqliteConnection con = new SqliteConnection("data source=" + Statics.GetConfigValue("FILES", "Db")))
            using (SqliteCommand command = con.CreateCommand())
            {
                con.Open();

                command.CommandText = $"UPDATE Certificates SET ExpiryDate='{expiryDate.Year,0:D4}-{expiryDate.Month,0:D2}-{expiryDate.Day,0:D2}' WHERE Id={certificate.Id}";                
                command.ExecuteNonQuery();
            }

            certificate.ExpiryDate = expiryDate;
        }
        public static void EditIssueDate(Certificate certificate, DateOnly issueDate)
        {
            using (SqliteConnection con = new SqliteConnection("data source=" + Statics.GetConfigValue("FILES", "Db")))
            using (SqliteCommand command = con.CreateCommand())
            {
                con.Open();

                command.CommandText = $"UPDATE Certificates SET IssueDate='{issueDate.Year,0:D4}-{issueDate.Month,0:D2}-{issueDate.Day,0:D2}' WHERE Id={certificate.Id}";                
                command.ExecuteNonQuery();
            }

            certificate.IssueDate = issueDate;
        }
        public static void EditCertificateType(Certificate certificate, int certificateTypeId)
        {
            using (SqliteConnection con = new SqliteConnection("data source=" + Statics.GetConfigValue("FILES", "Db")))
            using (SqliteCommand command = con.CreateCommand())
            {
                con.Open();

                command.CommandText = $"UPDATE Certificates SET CertificateType_Id={certificateTypeId} WHERE Id={certificate.Id}";                
                command.ExecuteNonQuery();
            }

            foreach (CertificateType certificateType in Lists.GetLists.CertificateTypes)
                if (certificate.Id == certificateTypeId)
                    certificate.CertificateType = certificateType;
        }
        public static void EditCertificateType(Certificate certificate, CertificateType certificateType)
        {
            using (SqliteConnection con = new SqliteConnection("data source=" + Statics.GetConfigValue("FILES", "Db")))
            using (SqliteCommand command = con.CreateCommand())
            {
                con.Open();

                command.CommandText = $"UPDATE Certificates SET CertificateType_Id={certificateType.Id} WHERE Id={certificate.Id}";                
                command.ExecuteNonQuery();
            }

            certificate.CertificateType = certificateType;
        }
        public static void EditCertificateNumber(Certificate certificate, string number)
        {
            using (SqliteConnection con = new SqliteConnection("data source=" + Statics.GetConfigValue("FILES", "Db")))
            using (SqliteCommand command = con.CreateCommand())
            {
                con.Open();

                command.CommandText = $"UPDATE Certificates SET Number='{number}' WHERE Id={certificate.Id}";                
                command.ExecuteNonQuery();
            }

            certificate.Number = number;
        }
        public static void EditIssuingAuthority(Certificate certificate, string issuingAuthority)
        {
            using (SqliteConnection con = new SqliteConnection("data source=" + Statics.GetConfigValue("FILES", "Db")))
            using (SqliteCommand command = con.CreateCommand())
            {
                con.Open();

                command.CommandText = $"UPDATE Certificates SET IssuingAuth='{issuingAuthority}' WHERE Id={certificate.Id}";                
                command.ExecuteNonQuery();
            }

            certificate.IssuingAuthority = issuingAuthority;
        }
        public static void RemoveCertificate(Certificate? certificate)
        {
            if (certificate != null) 
                using (SqliteConnection con = new SqliteConnection("data source=" + Statics.GetConfigValue("FILES", "Db")))
                {                
                    using (SqliteCommand del_command = con.CreateCommand())
                    {
                        con.Open();

                        del_command.CommandText = $"DELETE FROM Certificates WHERE Id={certificate.Id}";
                        del_command.ExecuteNonQuery();

                        Lists.GetLists.Certificates.Remove(certificate);
                        certificate = null;
                    }
                }
        }
    }
}