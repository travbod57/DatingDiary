using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using GoogleWebServiceApi.StaticMaps.Model;

namespace GoogleWebServiceApi.StaticMaps
{


	/// <summary>
	/// Represents Google Static map V2 web service
    /// Please refer to https://developers.google.com/maps/documentation/staticmaps/
	/// </summary>
	public class GoogleStaticMapClient
	{


        /// <summary>
        /// Async retrieve a static map
        /// </summary>
        /// <param name="staticMapRequest"></param>
        /// <param name="callback"></param>
		public void RetrieveStaticMap(GoogleStaticMapRequest staticMapRequest, GoogleStaticMapCompleteDelegate callback)
		{
			var request = (HttpWebRequest)HttpWebRequest.Create(staticMapRequest.GenerateRequestUrl());
			request.Method = "Post";
			try
			{
				request.BeginGetResponse(ResponseCallBack, new List<object> {request, staticMapRequest,callback});
				
			} catch(Exception e)
			{
				callback(null, staticMapRequest,e);
			}

		}


	

		private void ResponseCallBack(IAsyncResult ar)
		{
			var request = (HttpWebRequest)((List<Object>) ar.AsyncState)[0];
				var googleStaticMapRequest = (GoogleStaticMapRequest)((List<Object>)ar.AsyncState)[1];
				var callback = (GoogleStaticMapCompleteDelegate)((List<Object>)ar.AsyncState)[2];
			try
			{
				
				var response = request.EndGetResponse(ar);
				var responseStream = response.GetResponseStream();
				byte[] output;
				using (var reader = new BinaryReader(responseStream))
				{
					output = reader.ReadBytes((int)responseStream.Length);
				}
				callback(output, googleStaticMapRequest, null);

			}catch( Exception e)
			{
				callback(null, googleStaticMapRequest, e);
			}
			
		}
	}

	public delegate void GoogleStaticMapCompleteDelegate(byte[] response, GoogleStaticMapRequest googleStaticMapRequest, Exception e);
	

	
}