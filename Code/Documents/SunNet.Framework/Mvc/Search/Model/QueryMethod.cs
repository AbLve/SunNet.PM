namespace SF.Framework.Mvc.Search.Model
{
    public enum QueryMethod
    {
        Equal = 0,

        LessThan = 1,

        GreaterThan = 2,

        LessThanOrEqual = 3,

        GreaterThanOrEqual = 4,

        Like = 6,

        In = 7,

        DateBlock = 8,

        NotEqual = 9,

        StartsWith = 10,

        EndsWith = 11,

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        Contains = 12,

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        StdIn = 13,

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        DateTimeLessThanOrEqual = 14
    }
}