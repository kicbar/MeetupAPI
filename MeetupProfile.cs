﻿using AutoMapper;
using MeetupAPI.Entities;
using MeetupAPI.Models;

namespace MeetupAPI
{
    public class MeetupProfile : Profile
    {
        public MeetupProfile()
        {
            CreateMap<Meetup, MeetupDeatilsDto>()
                .ForMember(m => m.City, map => map.MapFrom(meetup => meetup.Location.City))
                .ForMember(m => m.Street, map => map.MapFrom(meetup => meetup.Location.Street))
                .ForMember(m => m.PostCode, map => map.MapFrom(meetup => meetup.Location.PostCode));

            CreateMap<MeetupDto, Meetup>();
        }
    }
}
