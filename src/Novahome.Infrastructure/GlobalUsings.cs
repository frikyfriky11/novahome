﻿global using System.Diagnostics.CodeAnalysis;
global using System.Net.Http.Headers;
global using System.Reflection;
global using System.Text.Json;
global using EntityFramework.Exceptions.PostgreSQL;
global using JetBrains.Annotations;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.ChangeTracking;
global using Microsoft.EntityFrameworkCore.Diagnostics;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Novahome.Application.Common.Interfaces.Persistence;
global using Novahome.Application.Common.Interfaces.Services;
global using Novahome.Application.Common.Models;
global using Novahome.Domain.Common;
global using Novahome.Domain.Entities;
global using Novahome.Infrastructure.Persistence;
global using Novahome.Infrastructure.Persistence.Converters;
global using Novahome.Infrastructure.Persistence.Extensions;
global using Novahome.Infrastructure.Persistence.Interceptors;
global using Novahome.Infrastructure.Services;
