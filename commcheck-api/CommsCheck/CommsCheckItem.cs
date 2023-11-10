public readonly record struct CommsCheckItem(
    DateOnly DateOfBirth, 
    DateOnly DateOfSmsUpdated,
    IReasonForRemoval ReasonForRemoval)
{
    public int DaysOld => 
        DateOnly.FromDateTime(DateTime.Now).DayNumber - DateOfBirth.DayNumber;

    public int DaySinceSmsUpdate => 
        DateOnly.FromDateTime(DateTime.Now).DayNumber - DateOfSmsUpdated.DayNumber;

    public int YearsOld => (int)Math.Floor(DaysOld / (float)365);

    public static CommsCheckItem FromDto(CommsCheckQuestionRequestDto dto)
    {
        return new CommsCheckItem(
            dto.DateOfBirth, 
            dto.DateOfSmsUpdate,
            IReasonForRemoval.FromEnum(dto.RfR));
    }
}
