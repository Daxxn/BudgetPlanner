namespace PaystubLibrary
{
    public enum CalcType
    {
        CalcGross = 1,
        CalcNet = 2,
        NotEnoughInfo = 3,
        NeedOneCompletePaystub = 4,
        NoEmptyPaystubs = 5
    }

    public enum Warning
    {
        NoWarning = 0,
        LowAccuracy = 1,
        LowCompletePaystubs = 2
    }
}
