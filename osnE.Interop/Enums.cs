namespace osnE.Interop
{
    public enum MapType : uint
    {
        MAPVK_VK_TO_VSC = 0x0,
        MAPVK_VSC_TO_VK = 0x1,
        MAPVK_VK_TO_CHAR = 0x2,
        MAPVK_VSC_TO_VK_EX = 0x3,
    }

    public enum VerbParsingState
    {
        PendingVerb,
        VerbFound,
        Reset,
        Execute,
        Executed
    }
    public enum SubjectParsingState
    {
        Reset,
        PendingSubject,
        Subject,
        Predicate,
        Execute,
        Executed
    }
    public enum PredicateType
    {
        None,
        List
    }
    public enum SubjectType
    {
        None,
        Bounded,
        ArbitraryWithSuggestions,
        Arbitrary
    }


}