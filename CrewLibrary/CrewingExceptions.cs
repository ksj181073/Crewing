namespace Crewing
{
    class PersonException : Exception
    {
        public PersonException() { }
        public PersonException(string? message) : base(message) { }
        public PersonException(string? message, Exception inner) : base(message, inner) { }
    }
    class SignedOnException : PersonException
    {
        public SignedOnException() { }
        public SignedOnException(string? message) : base(message) { }
        public SignedOnException(string? message, Exception inner) : base(message, inner) { }
    }
    class MissingNamesException : PersonException
    {
        public MissingNamesException() { }
        public MissingNamesException(string? message) : base(message) { }
        public MissingNamesException(string? message, Exception inner) : base(message, inner) { }
    }
    class MissingRankException : PersonException
    {
        public MissingRankException() { }
        public MissingRankException(string? message) : base(message) { }
        public MissingRankException(string? message, Exception inner) : base(message, inner) { }
    }
    class BackendDbException : Exception
    {
        public BackendDbException() { }
        public BackendDbException(string? message) : base(message) { }
        public BackendDbException(string? message, Exception inner) : base(message, inner) { }
    }
    class AlreadySignedOnException : BackendDbException
    {
        public AlreadySignedOnException() { }
        public AlreadySignedOnException(string? message) : base(message) { }
        public AlreadySignedOnException(string? message, Exception inner) : base(message, inner) { }
    }
    class CronologyException : BackendDbException
    {
        public CronologyException() { }
        public CronologyException(string? message) : base(message) { }
        public CronologyException(string? message, Exception inner) : base(message, inner) { }
    }
    class LocalCrewListException : Exception
    {
        public LocalCrewListException() { }
        public LocalCrewListException(string? message) : base(message) { }
        public LocalCrewListException(string? message, Exception inner) : base(message, inner) { }
    }
    class AlreadyOnLocalListException : LocalCrewListException
    {
        public AlreadyOnLocalListException() { }
        public AlreadyOnLocalListException(string? message) : base(message) { }
        public AlreadyOnLocalListException(string? message, Exception inner) : base(message, inner) { }
    }
    class IdDocumentException : Exception
    {
        public IdDocumentException() { }
        public IdDocumentException(string? message) : base(message) { }
        public IdDocumentException(string? message, Exception inner) : base(message, inner) { }
    }
    class IdDocumentExpiredException : IdDocumentException
    {
        public IdDocumentExpiredException() { }
        public IdDocumentExpiredException(string? message) : base(message) { }
        public IdDocumentExpiredException(string? message, Exception inner) : base(message, inner) { }
    }
    class SignOnException : Exception
    {
        public SignOnException() { }
        public SignOnException(string? message) : base(message) { }
        public SignOnException(string? message, Exception inner) : base(message, inner) { }
    }
    class NoVacancyException : SignOnException
    {
        public NoVacancyException() { }
        public NoVacancyException(string? message) : base(message) { }
        public NoVacancyException(string? message, Exception inner) : base(message, inner) { }
    }
    class CabinNotLivableException : SignOnException
    {
        public CabinNotLivableException() { }
        public CabinNotLivableException(string? message) : base(message) { }
        public CabinNotLivableException(string? message, Exception inner) : base(message, inner) { }
    }
}