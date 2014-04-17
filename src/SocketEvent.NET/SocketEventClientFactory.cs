using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SocketEvent.Impl;
using AutoMapper;
using SocketEvent.Dto;

namespace SocketEvent
{
    public class SocketEventClientFactory
    {
        static SocketEventClientFactory()
        {
            Mapper.CreateMap<string, RequestResult>()
                .ConvertUsing(new Func<string, RequestResult>((src) =>
                    {
                        RequestResult result;
                        if (Enum.TryParse<RequestResult>(src, true, out result))
                        {
                            return result;
                        }
                        else
                        {
                            return RequestResult.Fail;
                        }
                    }));
            Mapper.CreateMap<RequestResult, string>()
                .ConvertUsing(new Func<RequestResult, string>((src) =>
                    {
                        return src.ToString().ToUpper();
                    }));

            Mapper.CreateMap<ServerResponse, ServerResponseDto>();
            Mapper.CreateMap<ServerResponseDto, ServerResponse>();
        }

        private SocketEventClientFactory()
        {
        }

        /// <summary>
        /// Connect to a socket event URL
        /// </summary>
        /// <param name="url">Target SocketIO URL</param>
        /// <returns>SocketEventClient</returns>
        public static ISocketEventClient CreateInstance(string url)
        {
            return new SocketEventClient(url);
        }
    }
}
