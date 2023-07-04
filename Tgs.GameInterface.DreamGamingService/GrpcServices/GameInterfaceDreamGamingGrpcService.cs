using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Tgs.GameInterface.DreamGamingService.Core.ApplicationServices;
using Tgs.GameInterface.DreamGamingService.Protos;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.DreamGamingService.GrpcServices;

internal class GameInterfaceDreamGamingGrpcService : GameInterfaceDreamGamingService.GameInterfaceDreamGamingServiceBase
{
    private readonly ILogger<GameInterfaceDreamGamingGrpcService> _logger;
    private readonly IGetService<FundInQuery, FundResult> _fundInService;
    private readonly IGetService<FundOutQuery, FundResult> _fundOutService;
    private readonly IGetService<FundStatusQuery, bool> _fundStatusService;
    private readonly ICommandService<PlayerCreateCommand> _playerCreateService;
    private readonly IGetService<PlayerBalanceQuery, PlayerBalanceResult> _playerBalanceService;
    private readonly ICommandService<PlayerKickCommand> _playerKickService;
    private readonly IGetService<PlayerTableLimitCommand, bool> _playerTableLimitService;
    private readonly IQueryService<PlayerOnlineQuery, PlayerOnlineResult> _playerOnlineService;
    private readonly IGetService<GameUrlQuery, string?> _gameUrlService;
    private readonly IQueryService<GameHistorySettledQuery, GameHistorySettledResult> _gameHistorySettledService;
    private readonly IGetService<GameHistoryMarkQuery, bool> _gameHistoryMarkService;
    private readonly IGetService<GameRoundDetailQuery, string> _gameRoundDetailSerivce;

    public GameInterfaceDreamGamingGrpcService(
        ILogger<GameInterfaceDreamGamingGrpcService> logger,
        IGetService<FundInQuery, FundResult> fundInService,
        IGetService<FundOutQuery, FundResult> fundOutService,
        IGetService<FundStatusQuery, bool> fundStatusService,
        ICommandService<PlayerCreateCommand> playerCreateService,
        IGetService<PlayerBalanceQuery, PlayerBalanceResult> playerBalanceService,
        ICommandService<PlayerKickCommand> playerKickService,
        IGetService<PlayerTableLimitCommand, bool> playerTableLimitService,
        IQueryService<PlayerOnlineQuery, PlayerOnlineResult> playerOnlineService,
        IGetService<GameUrlQuery, string?> gameUrlService,
        IQueryService<GameHistorySettledQuery, GameHistorySettledResult> gameHistorySettledService,
        IGetService<GameHistoryMarkQuery, bool> gameHistoryMarkService,
        IGetService<GameRoundDetailQuery, string> gameRoundDetailSerivce)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _fundInService = fundInService ?? throw new ArgumentNullException(nameof(fundInService));
        _fundOutService = fundOutService ?? throw new ArgumentNullException(nameof(fundOutService));
        _fundStatusService = fundStatusService ?? throw new ArgumentNullException(nameof(fundStatusService));
        _playerCreateService = playerCreateService ?? throw new ArgumentNullException(nameof(playerCreateService));
        _playerBalanceService = playerBalanceService ?? throw new ArgumentNullException(nameof(playerBalanceService));
        _playerKickService = playerKickService ?? throw new ArgumentNullException(nameof(playerKickService));
        _playerTableLimitService = playerTableLimitService ?? throw new ArgumentNullException(nameof(playerTableLimitService));
        _playerOnlineService = playerOnlineService ?? throw new ArgumentNullException(nameof(playerOnlineService));
        _gameUrlService = gameUrlService ?? throw new ArgumentNullException(nameof(gameUrlService));
        _gameHistorySettledService = gameHistorySettledService ?? throw new ArgumentNullException(nameof(gameHistorySettledService));
        _gameHistoryMarkService = gameHistoryMarkService ?? throw new ArgumentNullException(nameof(gameHistoryMarkService));
        _gameRoundDetailSerivce = gameRoundDetailSerivce ?? throw new ArgumentNullException(nameof(gameRoundDetailSerivce));
    }

    public override Task<CheckingServiceReply> CheckingService(CheckingServiceRequest request, ServerCallContext context)
    {
        _logger.LogWarning("LogOn:{logOn} Message: Create a check connection.({machineName})", DateTime.Now.ToString("o"), request.MachineName);

        return Task.FromResult(new CheckingServiceReply
        {
            MachineName = Environment.MachineName
        });
    }

    public override async Task<FundReply> FundIn(FundRequest request, ServerCallContext context)
    {
        var query = new FundInQuery(
            ApiUrl: new(request.ApiUrl),
            ApiKey: request.ApiKey,
            AgentName: request.AgentName,
            Username: request.Username,
            RefTransactionCode: request.RefTransactionCode,
            Amount: request.Amount.ToDecimal().GetValueOrDefault(0));

        var result = await _fundInService
            .GetAsync(
                query: query,
                cancellationToken: context.CancellationToken)
            .ConfigureAwait(false);

        return new FundReply()
        {
            RefTransactionCode = result.RefTransactionCode,
            TransactionId = result.TransactionId,
            Balance = result.Balance
        };
    }

    public override async Task<FundReply> FundOut(FundRequest request, ServerCallContext context)
    {
        var query = new FundOutQuery(
            ApiUrl: new(request.ApiUrl),
            ApiKey: request.ApiKey,
            AgentName: request.AgentName,
            Username: request.Username,
            RefTransactionCode: request.RefTransactionCode,
            Amount: request.Amount.ToDecimal().GetValueOrDefault(0));

        var result = await _fundOutService
            .GetAsync(
                query: query,
                cancellationToken: context.CancellationToken)
            .ConfigureAwait(false);

        return new FundReply()
        {
            RefTransactionCode = result.RefTransactionCode,
            TransactionId = result.TransactionId,
            Balance = result.Balance
        };
    }

    public override async Task<FundStatusReply> FundStatus(FundStatusRequest request, ServerCallContext context)
    {
        var query = new FundStatusQuery(
           ApiUrl: new(request.ApiUrl),
           ApiKey: request.ApiKey,
           AgentName: request.AgentName,
           RefTransactionCode: request.RefTransactionCode);

        var result = await _fundStatusService
            .GetAsync(
                query: query,
                cancellationToken: context.CancellationToken)
            .ConfigureAwait(false);

        return new FundStatusReply()
        {
            IsOrderExist = result
        };
    }

    public override async Task<BaseReply> PlayerCreate(PlayerCreateRequest request, ServerCallContext context)
    {
        var command = new PlayerCreateCommand(
           ApiUrl: new(request.ApiUrl),
           ApiKey: request.ApiKey,
           AgentName: request.AgentName,
           Username: request.Username,
           Password: request.Password,
           CurrencyName: request.CurrencyName,
           TableLimit: request.TableLimit,
           WinLimit: request.WinLimit);

        await _playerCreateService
            .ExecuteAsync(
                command: command,
                cancellationToken: context.CancellationToken)
            .ConfigureAwait(false);

        return new BaseReply()
        {
            Status = true
        };
    }

    public override async Task<PlayerBalanceReply> PlayerBalance(PlayerBalanceRequest request, ServerCallContext context)
    {
        var query = new PlayerBalanceQuery(
           ApiUrl: new(request.ApiUrl),
           ApiKey: request.ApiKey,
           AgentName: request.AgentName,
           Username: request.Username);

        var result = await _playerBalanceService
            .GetAsync(
                query: query,
                cancellationToken: context.CancellationToken)
            .ConfigureAwait(false);

        return new PlayerBalanceReply()
        {
            Balance = result.Balance,
            IsPlayerExist = result.IsPlayerExist,
        };
    }

    public override async Task<BaseReply> PlayerKick(PlayerKickRequest request, ServerCallContext context)
    {
        var command = new PlayerKickCommand(
           ApiUrl: new(request.ApiUrl),
           ApiKey: request.ApiKey,
           AgentName: request.AgentName,
           MemberId: request.MemberId);

        await _playerKickService
            .ExecuteAsync(
                command: command,
                cancellationToken: context.CancellationToken)
            .ConfigureAwait(false);

        return new BaseReply()
        {
            Status = true
        };
    }

    public override async Task<BaseReply> PlayerTableLimit(PlayerTableLimitRequest request, ServerCallContext context)
    {
        var command = new PlayerTableLimitCommand(
            ApiUrl: new(request.ApiUrl),
            ApiKey: request.ApiKey,
            AgentName: request.AgentName,
            Username: request.Username,
            TableLimit: request.TableLimit);

        var result = await _playerTableLimitService
            .GetAsync(command, cancellationToken: context.CancellationToken)
            .ConfigureAwait(false);

        return new BaseReply()
        {
            Status = result
        };
    }

    public override async Task<PlayerOnlineReply> PlayerOnline(PlayerOnlineRequest request, ServerCallContext context)
    {
        var query = new PlayerOnlineQuery(
            ApiUrl: new(request.ApiUrl),
            ApiKey: request.ApiKey,
            AgentName: request.AgentName);

        var playerOnlines = await _playerOnlineService
            .QueryAsync(
                query: query,
                cancellationToken: context.CancellationToken)
            .ToArrayAsync(context.CancellationToken)
            .ConfigureAwait(false);

        var reply = new PlayerOnlineReply();

        if (playerOnlines.Any())
        {
            foreach (var playerOnline in playerOnlines)
            {
                reply.Items.Add(new PlayerOnline()
                {
                    PlayerCode = playerOnline.PlayerCode,
                    MemberId = playerOnline.MemberId,
                });
            }
        }

        return reply;
    }

    public override async Task<GameUrlReply> GameUrl(GameUrlRequest request, ServerCallContext context)
    {
        var query = new GameUrlQuery(
           ApiUrl: new(request.ApiUrl),
           ApiKey: request.ApiKey,
           AgentName: request.AgentName,
           Username: request.Username,
           Password: request.Password,
           IsMobile: request.IsMobile,
           Lang: request.Lang);

        var result = await _gameUrlService
            .GetAsync(
                query: query,
                cancellationToken: context.CancellationToken)
            .ConfigureAwait(false);

        return new GameUrlReply()
        {
            GameUrl = result
        };
    }

    public override async Task<GameHistorySettledReply> GameHistorySettled(GameHistorySettledRequest request, ServerCallContext context)
    {
        var query = new GameHistorySettledQuery(
            ApiUrl: new(request.ApiUrl),
            ApiKey: request.ApiKey,
            AgentName: request.AgentName);

        var gameHistorySettleds = await _gameHistorySettledService
            .QueryAsync(
                query: query,
                cancellationToken: context.CancellationToken)
            .ToArrayAsync(context.CancellationToken)
            .ConfigureAwait(false);

        var reply = new GameHistorySettledReply();

        if (gameHistorySettleds.Any())
        {
            foreach (var gameHistorySettled in gameHistorySettleds)
            {
                reply.Items.Add(new GameHistorySettled()
                {
                    PlayerCode = gameHistorySettled.PlayerCode,
                    ExternalRoundId = gameHistorySettled.ExternalRoundId,
                    IsClosed = gameHistorySettled.IsClosed,
                    IsVoid = gameHistorySettled.IsVoid,
                    ExternalCreatedOn = gameHistorySettled.ExternalCreatedOn.ToTimestamp(),
                    ExternalClosedOn = gameHistorySettled.ExternalClosedOn.ToTimestamp(),
                    Bet = gameHistorySettled.Bet,
                    ValidBet = gameHistorySettled.ValidBet,
                    PlayerWinLoss = gameHistorySettled.PlayerWinLoss,
                    Turnover = gameHistorySettled.Turnover,
                    Settle = gameHistorySettled.Settle,
                    AdditionalData = gameHistorySettled.AdditionalData,
                    ExternalGameId = gameHistorySettled.ExternalGameId,
                });
            }
        }

        return reply;
    }

    public override async Task<BaseReply> GameHistoryMark(GameHistoryMarkRequest request, ServerCallContext context)
    {
        var query = new GameHistoryMarkQuery(
           ApiUrl: new(request.ApiUrl),
           ApiKey: request.ApiKey,
           AgentName: request.AgentName,
           ExternalRoundIds: request.ExternalRoundIds.ToArray());

        var result = await _gameHistoryMarkService
            .GetAsync(
                query: query,
                cancellationToken: context.CancellationToken)
            .ConfigureAwait(false);

        return new BaseReply()
        {
            Status = result
        };
    }

    public override async Task<GameHistoryDetailReply> GameHistoryDetail(GameHistoryDetailRequest request, ServerCallContext context)
    {
        var query = new GameRoundDetailQuery(
           VideoGameApiUrl: new(request.VideoGameApiUrl),
           AgentName: request.AgentName,
           Language: request.Language,
           Id: request.Id);

        var result = await _gameRoundDetailSerivce
            .GetAsync(
                query: query,
                cancellationToken: context.CancellationToken)
            .ConfigureAwait(false);

        return new GameHistoryDetailReply()
        {
            GameResult = result
        };
    }
}
