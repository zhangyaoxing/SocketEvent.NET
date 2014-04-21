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

            Mapper.CreateMap<SocketEventResponse, SocketEventResponseDto>();
            Mapper.CreateMap<SocketEventResponseDto, SocketEventResponse>();
            Mapper.CreateMap<SocketEventRequest, SocketEventRequestDto>();
            Mapper.CreateMap<SocketEventRequestDto, SocketEventRequest>();
        }

        private SocketEventClientFactory()
        {
        }

        /// <summary>
        /// Connect to a socket event URL
        /// </summary>
        /// <param name="url">Server URL</param>
        /// <returns>SocketEventClient</returns>
        public static ISocketEventClient CreateInstance(string url)
        {
            return new SocketEventClient(url);
        }

        /// <summary>
        /// Connect to a socket event URL. Name the client with provided ID.
        /// </summary>
        /// <param name="id">ID of this client</param>
        /// <param name="url">Server URL</param>
        /// <returns>SocketEventClient</returns>
        public static ISocketEventClient CreateInstance(string id, string url)
        {
            return new SocketEventClient(id, url);
        }
    }
}
