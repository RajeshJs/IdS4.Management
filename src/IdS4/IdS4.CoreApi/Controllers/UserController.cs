﻿using AutoMapper;
using IdS4.CoreApi.Extensions;
using IdS4.CoreApi.Models.Paging;
using IdS4.CoreApi.Models.User;
using IdS4.DbContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using IdS4.CoreApi.Models.Results;

namespace IdS4.CoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [BearerAuthorize]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IdS4IdentityDbContext _identityDb;
        private readonly IMapper _mapper;

        public UserController(
            ILogger<UserController> logger, 
            IdS4IdentityDbContext identityDb, 
            IMapper mapper)
        {
            _logger = logger;
            _identityDb = identityDb;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<Paged<VmUser>> Get([FromQuery]PageQuery query)
        {
            var users = await _identityDb.Users.AsNoTracking()
                .OrderBy(query.Sort ?? "Id")
                .Skip(query.Skip)
                .Take(query.Limit)
                .ToListAsync();

            return Paged<VmUser>.From(
                _mapper.Map<List<VmUser>>(users),
                await _identityDb.Users.AsNoTracking().CountAsync()
            );
        }

        [HttpGet("{id}")]
        public async Task<ApiResult> Get([FromRoute] string id)
        {
            if (string.IsNullOrEmpty(id)) return ApiResult.NotFound(id);

            var user = await _identityDb.Users.FindAsync(id);
            if (user == null) return  ApiResult.NotFound(id);

            var userClaims = await _identityDb.UserClaims.AsNoTracking().Where(s => s.UserId.Equals(id)).ToListAsync();

            var vmUser = _mapper.Map<VmUser>(user);
            vmUser.UserClaims = _mapper.Map<List<VmUserClaim>>(userClaims);
            return ApiResult.Success(vmUser);
        }
    }
}
