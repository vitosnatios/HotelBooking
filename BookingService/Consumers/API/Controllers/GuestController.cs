﻿using Application;
using Application.Guest.DTO;
using Application.Guest.Ports;
using Application.Guest.Requests;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GuestController : ControllerBase
    {
        private readonly ILogger<GuestController> _logger;
        private readonly IGuestManager _gestManager;

        public GuestController(ILogger<GuestController> logger, IGuestManager guestManager)
        {
            _logger = logger;
            _gestManager = guestManager;
        }

        [HttpPost]
        public async Task<ActionResult<GuestDTO>> Post(GuestDTO guest)
        {
            var request = new CreateGuestRequest { Data = guest };
            var res = await _gestManager.CreateGuest(request);
            if (res.Success) return Created("", res.Data);
            if (res.ErrorCode == ErrorCodes.NOT_FOUND) { return NotFound(res); }
            if (res.ErrorCode == ErrorCodes.INVALID_PERSON_ID) { return BadRequest(res); }
            if (res.ErrorCode == ErrorCodes.MISSING_REQUIRED_INFORMATION) { return BadRequest(res); }
            if (res.ErrorCode == ErrorCodes.INVALID_EMAIL) { return BadRequest(res); }
            if (res.ErrorCode == ErrorCodes.COULD_NOT_STORE_DATA) { return BadRequest(res); }
            _logger.LogError("Response with unknown ErrorCode Returned", res);
            return BadRequest(500);
        }

        [HttpGet]
        public async Task<ActionResult<GuestDTO>> Get(int guestId)
        {
            var res = await _gestManager.GetGuest(guestId);
            if (res.Success) return Created("", res.Data);
            return NotFound(res);
        }
    }
}
