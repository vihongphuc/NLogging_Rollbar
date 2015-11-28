using Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common.Web
{
    public static class ApiConsumerHelpers
    {
        public static string GetConsumerAddress(HttpRequestBase request)
        {
            foreach (var item in headerItems)
            {
                var ipString = request.Headers[item.Key];
                if (!String.IsNullOrWhiteSpace(ipString))
                {
                    string privateIP = null;
                    bool isPrivate;
                    if (ValidIP(ipString, out isPrivate))
                    {
                        if (!isPrivate)
                        {
                            return ipString;
                        }
                        else if (String.IsNullOrWhiteSpace(privateIP))
                        {
                            privateIP = ipString;
                        }
                    }

                    if (!String.IsNullOrWhiteSpace(privateIP))
                    {
                        return privateIP;
                    }
                }
            }

            return request.UserHostAddress;
        }
        public static string GetConsumerAddress(HttpRequestMessage request)
        {
            foreach (var item in headerItems)
            {
                IEnumerable<string> values;
                if (request.Headers.TryGetValues(item.Key, out values))
                {
                    foreach (var ipString in values.Select(s => s == null ? String.Empty : s.Trim()))
                    {
                        string privateIP = null;
                        bool isPrivate;
                        if (item.Split)
                        {
                            foreach (var ip in ipString.Split(',').Select(s => s.Trim()))
                            {
                                if (ValidIP(ip, out isPrivate))
                                {
                                    if (!isPrivate)
                                    {
                                        return ip;
                                    }
                                    else if (String.IsNullOrWhiteSpace(privateIP))
                                    {
                                        privateIP = ip;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (ValidIP(ipString, out isPrivate))
                            {
                                if (!isPrivate)
                                {
                                    return ipString;
                                }
                                else if (String.IsNullOrWhiteSpace(privateIP))
                                {
                                    privateIP = ipString;
                                }
                            }
                        }

                        if (!String.IsNullOrWhiteSpace(privateIP))
                        {
                            return privateIP;
                        }
                    }
                }
            }

            var wrapper = (HttpContextWrapper)request.Properties.GetOrDefault("MS_HttpContext");
            if (wrapper != null)
                return wrapper.Request.UserHostAddress;

            var remoteEndPointName = (RemoteEndpointMessageProperty)request.Properties.GetOrDefault(RemoteEndpointMessageProperty.Name);
            if (remoteEndPointName != null)
                return remoteEndPointName.Address;

            return null;    //here the user can return whatever they like
        }

        public static string GetEndUserAddress(HttpRequestBase request)
        {
            return request.Headers[ApiConsumerKeys.IPAddress];
        }
        public static string GetEndUserAddress(HttpRequestMessage request)
        {
            IEnumerable<string> values;
            if (request.Headers.TryGetValues(ApiConsumerKeys.IPAddress, out values))
            {
                return values.FirstOrDefault();
            }

            return null;
        }

        #region IP Address
        private static bool ValidIP(string ip, out bool isPrivate)
        {
            IPAddress ipAddr;

            isPrivate = false;
            if (0 == ip.Length
                || false == IPAddress.TryParse(ip, out ipAddr)
                || (ipAddr.AddressFamily != AddressFamily.InterNetwork
                    && ipAddr.AddressFamily != AddressFamily.InterNetworkV6))
                return false;

            var addr = IpRange.AddrToUInt64(ipAddr);
            foreach (var range in privateRanges)
            {
                if (range.Encompasses(addr))
                {
                    isPrivate = true;
                }
            }

            return true;
        }

        private sealed class IpRange
        {
            private readonly UInt64 _start;
            private readonly UInt64 _end;

            public IpRange(string startStr, string endStr)
            {
                _start = ParseToUInt64(startStr);
                _end = ParseToUInt64(endStr);
            }

            public static UInt64 AddrToUInt64(IPAddress ip)
            {
                var ipBytes = ip.GetAddressBytes();
                UInt64 value = 0;

                foreach (var abyte in ipBytes)
                {
                    value <<= 8;    // shift
                    value += abyte;
                }

                return value;
            }

            public static UInt64 ParseToUInt64(string ipStr)
            {
                var ip = IPAddress.Parse(ipStr);
                return AddrToUInt64(ip);
            }

            public bool Encompasses(UInt64 addrValue)
            {
                return _start <= addrValue && addrValue <= _end;
            }

            public bool Encompasses(IPAddress addr)
            {
                var value = AddrToUInt64(addr);
                return Encompasses(value);
            }
        };

        private static readonly IpRange[] privateRanges =
            new IpRange[]
            {
                new IpRange("0.0.0.0", "2.255.255.255"),
                new IpRange("10.0.0.0", "10.255.255.255"),
                new IpRange("127.0.0.0", "127.255.255.255"),
                new IpRange("169.254.0.0", "169.254.255.255"),
                new IpRange("172.16.0.0", "172.31.255.255"),
                new IpRange("192.0.2.0", "192.0.2.255"),
                new IpRange("192.168.0.0", "192.168.255.255"),
                new IpRange("255.255.255.0", "255.255.255.255")
            };

        private sealed class HeaderItem
        {
            public readonly string Key;
            public readonly bool Split;

            public HeaderItem(string key, bool split)
            {
                Key = key;
                Split = split;
            }
        }
        private static readonly HeaderItem[] headerItems =
            new HeaderItem[]
            {
                new HeaderItem("CLIENT-IP", false),
                new HeaderItem("X-FORWARDED-FOR", true),
                new HeaderItem("X-FORWARDED", false),
                new HeaderItem("X-CLUSTER-CLIENT-IP", false),
                new HeaderItem("FORWARDED-FOR", false),
                new HeaderItem("FORWARDED", false),
                new HeaderItem("VIA", false),
                new HeaderItem("REMOTE-ADDR", false)
            };
        #endregion
    }
}
