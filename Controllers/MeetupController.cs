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
            var meetup = _meetupContext.Meetups
                .Include(m => m.Location)
                .FirstOrDefault(m => m.Name.Replace(" ", "-").ToLower() == meetupName.ToLower());
            if (meetup == null)
                return NotFound();

            var meetupDto = _mapper.Map<MeetupDeatilsDto>(meetup);
            return Ok(meetupDto);
        }

        [HttpPost]
        public ActionResult Post([FromBody] MeetupDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var meetup = _mapper.Map<Meetup>(model);
            _meetupContext.Meetups.Add(meetup);
            _meetupContext.SaveChanges();

            var key = meetup.Name.Replace(" ", "-").ToLower();
            return Created("api/meetup/" + key, null);
        }

        [HttpPut("{meetupName}")]
        public ActionResult Put(string meetupName, [FromBody] MeetupDto model)
        {
            var meetup = _meetupContext.Meetups
                .FirstOrDefault(m => m.Name.Replace(" ", "-").ToLower() == meetupName.ToLower());

            if (meetup == null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            meetup.Name = model.Name;
            meetup.Organizer = model.Organizer;
            meetup.Date = model.Date;
            meetup.IsPrivate = model.IsPrivate;

            _meetupContext.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{meetupName}")]
        public ActionResult Delete(string meetupName)
        {
            var meetup = _meetupContext.Meetups
                .Include(m => m.Location)
                .FirstOrDefault(m => m.Name.Replace(" ", "-").ToLower() == meetupName.ToLower());

            if (meetup == null)
                return NotFound();

            _meetupContext.Remove(meetup);
            _meetupContext.SaveChanges();

            return NoContent();
        }
    }
}
