using System;
using AutoMapper;
using Contracts;
using SearchService.Models;

namespace SearchService.RequestHelpers;

public class MappingProifles : Profile
{
    public MappingProifles()
    {
        CreateMap<AuctionCreated, Item>();
    }
}
