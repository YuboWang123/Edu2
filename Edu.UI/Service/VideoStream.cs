using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Edu.UI.Service
{
    public class VideoStream
    {
        private readonly string _filename;
        private string liveHostAdd =ConfigurationManager.AppSettings["liveAddr"];
        public VideoStream(string filename)
        {
            _filename =liveHostAdd+ filename+".sdp/"+ "20190424/";
        }
        private async Task<Stream> getStream()
        {
            var client = new HttpClient();
            var resp = await client.GetAsync(_filename);
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadAsStreamAsync();
        }


        public string GetDirectoryListingRegexForUrl()
        {
            return "<a href=\".*\">(?<name>.*)</a>";
        }

        public void listName()
        {
          
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_filename);
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string html = reader.ReadToEnd();
                    Regex regex = new Regex(GetDirectoryListingRegexForUrl());
                    MatchCollection matches = regex.Matches(html);
                    if (matches.Count > 0)
                    {
                        foreach (Match match in matches)
                        {
                            if (match.Success)
                            {
                                Console.WriteLine(match.Groups["name"]);
                            }
                        }
                    }
                }
            }
        }

      
        public async Task WriteToStream(Stream outputStream, HttpContent content, TransportContext context)
        {
            try
            {
                var buffer = new byte[65536];
                listName();
                outputStream = await getStream();

                await Task.Yield();


            }
            catch (HttpException ex)
            {
                throw new ApplicationException(ex.ToString());
            }
            finally
            {
                
                outputStream.Close();
            }
        }



      



    }
}