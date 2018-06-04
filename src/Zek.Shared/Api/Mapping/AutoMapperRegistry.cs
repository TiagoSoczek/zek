using System;
using AutoMapper;
using AutoMapper.Mappers;

namespace Zek.Shared.Api.Mapping
{
    public class AutoMapperRegistry : Profile
    {
        public AutoMapperRegistry()
        {
            AddConditionalObjectMapper().Where(DtoMapping);
        }

        private bool DtoMapping(Type source, Type destination)
        {
            return source.Name == destination.Name + "Dto" ||
                   source.Name + "Dto" == destination.Name;
        }
    }
}