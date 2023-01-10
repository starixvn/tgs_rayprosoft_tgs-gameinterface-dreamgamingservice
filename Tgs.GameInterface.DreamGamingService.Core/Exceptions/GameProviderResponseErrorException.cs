namespace Tgs.GameInterface.DreamGamingService.Core.Exceptions;

public class GameProviderResponseErrorException : Exception
{
    public GameProviderResponseErrorException(int? codeId, string? message) : base(
       $"The game manufacturer responds with an error code({codeId}), please contact the engineer or game supplier for assistance.({message})")
    {
    }

    public GameProviderResponseErrorException(string? errorMessage) : base(
       $"The game manufacturer responds with an error , please contact the engineer or game supplier for assistance.({errorMessage})")
    {
    }
}