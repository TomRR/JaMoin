// Global using directives

global using System.Data;
global using System.Text.RegularExpressions;
global using System.Threading;
global using System.Threading.Tasks;
global using Dapper;
global using EisoMeter.Api;
global using EisoMeter.Api.Common;
global using EisoMeter.Api.Features.Claims;
global using EisoMeter.Api.Features.Claims.GetClaimStatus;
global using EisoMeter.Api.Features.Temperatures;
global using EisoMeter.Api.Infrastructure.Persistence;
global using EisoMeter.Api.Infrastructure.Persistence.Claims;
global using EisoMeter.Api.Infrastructure.Persistence.Temperature;
global using EisoMeter.Api.Infrastructure.Persistence.User;
global using Microsoft.Data.Sqlite;
global using NSubstitute;
global using Xunit;