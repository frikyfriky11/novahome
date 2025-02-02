﻿global using System.Security.Claims;
global using EntityFramework.Exceptions.Common;
global using FluentValidation.AspNetCore;
global using JetBrains.Annotations;
global using MediatR;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.Filters;
global using Microsoft.EntityFrameworkCore;
global using Novahome.Application.Common.Exceptions;
global using Novahome.Application.Common.Interfaces.Services;
global using Novahome.Application.Extensions;
global using Novahome.Infrastructure.Extensions;
global using Novahome.Infrastructure.Persistence;
global using Novahome.WebApi.Extensions;
global using Novahome.WebApi.Filters;
global using Novahome.WebApi.Services;
global using Npgsql;
global using NSwag;
global using NSwag.AspNetCore;
global using NSwag.Generation.Processors.Security;
global using OpenTelemetry.Metrics;
global using OpenTelemetry.Resources;
global using OpenTelemetry.Trace;
