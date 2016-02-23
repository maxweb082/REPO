using AutoMapper;
using AutoMapper.Data;
using AutoMapper.Mappers;

namespace LBOM.DataAccess
{
    public static class AutoMapperWebConfiguration
    {


        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                MapperRegistry.Mappers.Add(new DataReaderMapper());

            });
        }
    }
}