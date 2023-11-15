namespace tests;

public class CheckCommsCommandTests
{
    [Fact]
    public void TestCommsCheckQuestionRequestDtoIsNotMutated_Empty()
    {
        //arrange
        var dto = new CommsCheckQuestionRequestDto();

        //Act
        var x = new CheckCommsCommand(dto);

        //assert
        var expected = dto;
        var actual = x.Dto;

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TestCommsCheckQuestionRequestDtoIsNotMutated_NotEmpty()
    {
        //arrange
        var dto = new CommsCheckQuestionRequestDto(
            DateOnly.FromDateTime(new DateTime(2000,1,1)),
            DateOnly.FromDateTime(new DateTime(2010,1,1)),
            ReasonForRemoval.DEA);

        //Act
        var x = new CheckCommsCommand(dto);

        //assert
        var expected = dto;
        var actual = x.Dto;

        Assert.Equal(expected, actual);
    }
}