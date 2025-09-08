namespace Payment.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<CashOutRequestValidator>();
        services.AddHealthChecks();

        return services;
    }

    public static IServiceCollection AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RedisConfiguration>(configuration.GetSection("Redis"));
        
        var redisConfig = configuration.GetSection("Redis").Get<RedisConfiguration>();
        var connectionString = redisConfig?.ConnectionString ?? "localhost:6379,defaultDatabase=0,connectTimeout=5000,syncTimeout=250,asyncTimeout=250,connectRetry=3,abortConnect=false,keepAlive=180";
        
        services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(connectionString));
        services.AddScoped<IRedisContext, RedisContext>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        
        services.AddHealthChecks().AddRedis(connectionString);

        return services;
    }

    public static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {
        services.AddScoped<IPaymentService, PaymentService>();

        return services;
    }

    public static IServiceCollection AddExternalServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }
}
