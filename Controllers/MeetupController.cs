using AutoMapper;
using MeetupAPI.Entities;
using MeetupAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeetupAPI.Controllers
{
    [Route("api/meetup")]
    public class MeetupController : ControllerBase
    {
        private readonly MeetupContext _meetupContext;
        private readonly IMapper _mapper;

        public MeetupController(MeetupContext meetupContext, IMapper mapper)
        {
            _meetupContext = meetupContext;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<List<MeetupDeatilsDto>> Get()
        {
            var meetups = _meetupContext.Meetups.Include(m => m.Location).ToList();
            var meetupDtos = _mapper.Map<List<MeetupDeatilsDto>>(meetups);
            return Ok(meetupDtos);
        }

        [HttpGet("{meetupName}")]
        public ActionResult<MeetupDeatilsDto> Get(string meetupName)
        {
            var meetup = _meetupContext.Meetups.
                Include(m => m.Location)
                .FirstOrDefault(m => m.Name.Replace(" ", "-").ToLower() == meetupName.ToLower());
            if (meetup == null)
                return NotFound();

            var meetupDto = _mapper.Map<MeetupDeatilsDto>(meetup);
            return Ok(meetupDto);
        }

        [HttpPost]
        public ActionResult Post([FromBody] MeetupDto model)
        {
            var meetup = _mapper.Map<Meetup>(model);
            _meetupContext.Meetups.Add(meetup);
            _meetupContext.SaveChanges();

            var key = meetup.Name.Replace(" ", "-").ToLower();
            return Created("api/meetup/" + key, null);
        }
    }
}
