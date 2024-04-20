namespace Crewing
{
    class IdDocumentType
    {
        public int Id { get; set; }
        public string? Name { get; set;}
        public char? Code { get; set;}
    
        public static IdDocumentType? GetIdDocumentType(int IdDocumentType_Id)
        {
            foreach (IdDocumentType idDocumentType in Lists.GetLists.IdDocumentTypes)
                if (idDocumentType.Id == IdDocumentType_Id)
                    return idDocumentType;
            
            return null;
        }
        public static IdDocumentType? GetIdDocumentType(string IdDocumentType_Name)
        {
            foreach (IdDocumentType idDocumentType in Lists.GetLists.IdDocumentTypes)
                if (idDocumentType.Name == IdDocumentType_Name)
                    return idDocumentType;
            
            return null;
        }
    }
}