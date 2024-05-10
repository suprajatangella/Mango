using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;

namespace Mango.Services.CouponAPI.Extensions
{
	public static class WebApplicationBuilderExtensions
	{
		public static WebApplicationBuilder AddAppAuthentication(this WebApplicationBuilder builder) 
		{
			var settingsSection = builder.Configuration.GetSection("ApiSettings");

			var secret = settingsSection.GetValue<string>("Secret");
			var issuer = settingsSection.GetValue<string>("Issuer");
			var audience = settingsSection.GetValue<string>("Audience");


			var key = Encoding.ASCII.GetBytes(secret);

			builder.Services.AddAuthentication(x =>
			{
				x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(x =>
			{
				x.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),

					ValidateIssuer = true,
					ValidIssuer = issuer,
					ValidAudience = audience,
					ValidateAudience = true,
					ClockSkew = new System.TimeSpan(0, 0, 30)
				};
				x.Events = new JwtBearerEvents()
				{
					OnChallenge = context =>
					{
						context.HandleResponse();
						context.Response.StatusCode = StatusCodes.Status401Unauthorized;
						context.Response.ContentType = "application/json";

						// Ensure we always have an error and error description.
						if (string.IsNullOrEmpty(context.Error))
							context.Error = "invalid_token";
						if (string.IsNullOrEmpty(context.ErrorDescription))
							context.ErrorDescription = "This request requires a valid JWT access token to be provided";

						// Add some extra context for expired tokens.
						if (context.AuthenticateFailure != null && context.AuthenticateFailure.GetType() == typeof(SecurityTokenExpiredException))
						{
							var authenticationException = context.AuthenticateFailure as SecurityTokenExpiredException;
							context.Response.Headers.Add("x-token-expired", authenticationException.Expires.ToString("o"));
							context.ErrorDescription = $"The token expired on {authenticationException.Expires.ToString("o")}";
						}

						return context.Response.WriteAsync(JsonSerializer.Serialize(new
						{
							error = context.Error,
							error_description = context.ErrorDescription
						}));
					}
				};

			});

			return builder;
		}
	}
}
