// Global using directives

global using System.Data;
global using System.Net;
global using System.Text.Json.Serialization;
global using Dapper;
global using EisoMeter.Api;
global using EisoMeter.Api.Common;
global using EisoMeter.Api.Common.Filters;
global using EisoMeter.Api.Features.Claims;
global using EisoMeter.Api.Features.Claims.ClaimReward;
global using EisoMeter.Api.Features.Claims.GetClaimStatus;
global using EisoMeter.Api.Features.Temperatures;
global using EisoMeter.Api.Infrastructure;
global using EisoMeter.Api.Infrastructure.Persistence;
global using EisoMeter.Api.Infrastructure.Persistence.Claims;
global using EisoMeter.Api.Infrastructure.Persistence.Temperature;
global using EisoMeter.Api.Infrastructure.Persistence.User;
global using FluentValidation;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.Data.Sqlite;
global using ResultType.Core;