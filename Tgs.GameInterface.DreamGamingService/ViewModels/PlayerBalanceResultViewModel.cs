namespace Tgs.GameInterface.DreamGamingService.ViewModels;

public record PlayerBalanceResultViewModel(
	bool IsPlayerExist,
	decimal? Balance);
