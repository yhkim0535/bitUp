using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;

namespace bitup
{
    /// <summary>
    /// Access key - 1rZ9cA0JYqzgFgH82JUmmEjPXGTvNwcd5YarExVx
    /// Secret key - OVl6JEbKnhHlTSZLYMkbpDdhs3mY6jytbmxxj71P
    /// </summary>
    class Program
    {
        

        static string ticker_url = "https://api.upbit.com/v1/ticker";
        static string account_url = "https://api.upbit.com/v1/accounts";
        static string UUID = Guid.NewGuid().ToString();
        static string AccessKey = "1rZ9cA0JYqzgFgH82JUmmEjPXGTvNwcd5YarExVx"; //발급받은 AccessKey를 넣어줍니다.
        static string SecretKey = "OVl6JEbKnhHlTSZLYMkbpDdhs3mY6jytbmxxj71P"; //발급받은 SecretKey를 넣어줍니다.
        static string AccountInquiry() //나의 계좌 조회 (전체 계좌 조회)
        {
            var payload = new Dictionary<string, object>
            {
                { "access_key" , AccessKey },
                { "nonce" , UUID },
            };
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm(); //JWT 라이브러리 이용하여 JWT 토큰을 만듭니다.
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
            var token = encoder.Encode(payload, SecretKey);
            var authorize_token = string.Format("Bearer {0}", token);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(account_url); //요청 과정
            request.Method = "GET";
            request.Headers.Add(string.Format("Authorization:{0}", authorize_token));
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string strResult = reader.ReadToEnd();
            reader.Close();
            dataStream.Close();
            response.Close();
            return strResult;
        }
        static string CoinInquiry(string coinName) //특정 코인의 현재 시세 조회 (현재가 정보)
        {
            StringBuilder dataParams = new StringBuilder();//요청 과정
            dataParams.Append("markets=" + coinName);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ticker_url + "?" + dataParams);
            request.Method = "GET";
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string strResult = reader.ReadToEnd();
            reader.Close();
            dataStream.Close();
            response.Close();
            return strResult;
        }

        static void Main(string[] args)
        {
            string strResult1 = CoinInquiry("KRW-BTC");
            //string strResult2 = AccountInquiry();
            Console.WriteLine(strResult1 + "\n");
            //Console.WriteLine(strResult2);



            Console.ReadLine();
        }
    }
}
